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
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Infrastructure.Database;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Controllers
{
    public sealed class EnterprisesController : Controller
    {
        private const ActivityType ACTIVITY_TYPE = ActivityType.Enterprise;

        private readonly SqlServerContext _context;
        private readonly IGetActivity _getActivity;

        public EnterprisesController(SqlServerContext context, IGetActivity getActivity)
        {
            _context = context;
            _getActivity = getActivity;
        }

        // GET: Enterprises
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var enterprises = await _context.Enterprises.ToListAsync(cancellationToken);

            var activity = await _getActivity.RunAsync(Guid.Empty, ACTIVITY_TYPE, cancellationToken);

            return View(new EnterprisesPageViewModel { ActivityHistory = activity.ToList(), Enterprises = enterprises });
        }

        // GET: Enterprises/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Enterprises == null)
            {
                return NotFound();
            }

            var enterpriseEntity = await _context.Enterprises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enterpriseEntity == null)
            {
                return NotFound();
            }

            return View(enterpriseEntity);
        }

        // GET: Enterprises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enterprises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] EnterpriseEntity enterpriseEntity)
        {
            if (ModelState.IsValid)
            {
                enterpriseEntity.Id = Guid.NewGuid();
                _context.Add(enterpriseEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enterpriseEntity);
        }

        // GET: Enterprises/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Enterprises == null)
            {
                return NotFound();
            }

            var enterpriseEntity = await _context.Enterprises.FindAsync(id);
            if (enterpriseEntity == null)
            {
                return NotFound();
            }
            return View(enterpriseEntity);
        }

        // POST: Enterprises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] EnterpriseEntity enterpriseEntity)
        {
            if (id != enterpriseEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enterpriseEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnterpriseEntityExists(enterpriseEntity.Id))
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
            return View(enterpriseEntity);
        }

        // GET: Enterprises/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Enterprises == null)
            {
                return NotFound();
            }

            var enterpriseEntity = await _context.Enterprises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enterpriseEntity == null)
            {
                return NotFound();
            }

            return View(enterpriseEntity);
        }

        // POST: Enterprises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Enterprises == null)
            {
                return Problem("Entity set 'SqlServerContext.Enterprises'  is null.");
            }
            var enterpriseEntity = await _context.Enterprises.FindAsync(id);
            if (enterpriseEntity != null)
            {
                _context.Enterprises.Remove(enterpriseEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnterpriseEntityExists(Guid id)
        {
          return _context.Enterprises.Any(e => e.Id == id);
        }
    }
}
