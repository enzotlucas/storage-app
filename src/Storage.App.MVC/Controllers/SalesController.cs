using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Sale;
using Storage.App.MVC.Infrastructure.Database;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Controllers
{
    public sealed class SalesController : Controller
    {
        private const ActivityType ACTIVITY_TYPE = ActivityType.Sale;

        private readonly SqlServerContext _context;
        private readonly IGetActivity _getActivity;

        public SalesController(SqlServerContext context, IGetActivity getActivity)
        {
            _context = context;
            _getActivity = getActivity;
        }

        // GET: Sales
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var sales = await _context.Sales.Include(s => s.Customer).Include(s => s.Enterprise).ToListAsync(cancellationToken);

            var activity = await _getActivity.RunAsync(Guid.Empty, ACTIVITY_TYPE, cancellationToken);

            return View(new SalesPageViewModel { ActivityHistory = activity.ToList(), Sales = sales});
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleEntity = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Enterprise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleEntity == null)
            {
                return NotFound();
            }

            return View(saleEntity);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName");
            ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name");
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Note,TotalPrice,CustomerId,EnterpriseId")] SaleEntity saleEntity)
        {
            if (ModelState.IsValid)
            {
                saleEntity.Id = Guid.NewGuid();
                _context.Add(saleEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName", saleEntity.CustomerId);
            ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", saleEntity.EnterpriseId);
            return View(saleEntity);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleEntity = await _context.Sales.FindAsync(id);
            if (saleEntity == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName", saleEntity.CustomerId);
            ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", saleEntity.EnterpriseId);
            return View(saleEntity);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Note,TotalPrice,CustomerId,EnterpriseId")] SaleEntity saleEntity)
        {
            if (id != saleEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saleEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleEntityExists(saleEntity.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "FirstName", saleEntity.CustomerId);
            ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", saleEntity.EnterpriseId);
            return View(saleEntity);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var saleEntity = await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.Enterprise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saleEntity == null)
            {
                return NotFound();
            }

            return View(saleEntity);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Sales == null)
            {
                return Problem("Entity set 'SqlServerContext.Sales'  is null.");
            }
            var saleEntity = await _context.Sales.FindAsync(id);
            if (saleEntity != null)
            {
                _context.Sales.Remove(saleEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaleEntityExists(Guid id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
