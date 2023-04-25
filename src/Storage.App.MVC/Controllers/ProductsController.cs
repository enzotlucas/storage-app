using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Product;
using Storage.App.MVC.Domain.Authorization;
using Storage.App.MVC.Infrastructure.Database;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Controllers
{
    public sealed class ProductsController : Controller
    {
        private const ActivityType ACTIVITY_TYPE = ActivityType.Product;

        private readonly SqlServerContext _context;
        private readonly IGetActivity _getActivity;

        public ProductsController(SqlServerContext context, IGetActivity getActivity)
        {
            _context = context;
            _getActivity = getActivity;
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var products = await _context.Products.Include(s => s.Enterprise).ToListAsync(cancellationToken);

            var activity = await _getActivity.RunAsync(Guid.Empty, ACTIVITY_TYPE, cancellationToken);

            return View(new ProductsPageViewModel { ActivityHistory = activity.ToList(), Products = products });
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productEntity = await _context.Products
                .Include(p => p.Enterprise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productEntity == null)
            {
                return NotFound();
            }

            return View(productEntity);
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public IActionResult Create()
        {
            //ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ClaimsAuthorize("UserType", "Enterprise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count,Price,Cost,Brand,EnterpriseId")] ProductEntity productEntity)
        {
            if (ModelState.IsValid)
            {
                productEntity.Id = Guid.NewGuid();
                _context.Add(productEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", productEntity.EnterpriseId);
            return View(productEntity);
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productEntity = await _context.Products.FindAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }
            //ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", productEntity.EnterpriseId);
            return View(productEntity);
        }

        [HttpPost]
        [ClaimsAuthorize("UserType", "Enterprise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Count,Price,Cost,Brand,EnterpriseId")] ProductEntity productEntity)
        {
            if (id != productEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductEntityExists(productEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", productEntity.EnterpriseId);
            return View(productEntity);
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var productEntity = await _context.Products
                .Include(p => p.Enterprise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productEntity == null)
            {
                return NotFound();
            }

            return View(productEntity);
        }

        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize("UserType", "Enterprise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'SqlServerContext.Products'  is null.");
            }
            var productEntity = await _context.Products.FindAsync(id);
            if (productEntity != null)
            {
                _context.Products.Remove(productEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductEntityExists(Guid id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
