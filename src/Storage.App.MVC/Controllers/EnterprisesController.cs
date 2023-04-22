using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Infrastructure.Database;

namespace Storage.App.MVC.Controllers
{
    public sealed class EnterprisesController : Controller
    {
        private readonly SqlServerContext _context;

        public EnterprisesController(SqlServerContext context)
        {
            _context = context;
        }

        // GET: Enterprises
        public async Task<IActionResult> Index()
        {
              return View(await _context.Enterprises.ToListAsync());
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
