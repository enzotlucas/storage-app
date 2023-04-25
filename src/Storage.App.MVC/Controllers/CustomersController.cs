using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Customer;
using Storage.App.MVC.Domain.Authorization;
using Storage.App.MVC.Infrastructure.Database;
using Storage.App.MVC.Infrastructure.Identity;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Controllers
{
    public sealed class CustomersController : Controller
    {
        private const ActivityType ACTIVITY_TYPE = ActivityType.Customer;

        private readonly SqlServerContext _context;
        private readonly IdentityContext _identityContext;
        private readonly IGetActivity _getActivity;

        public CustomersController(SqlServerContext context, IdentityContext identityContext, IGetActivity getActivity)
        {
            _context = context;
            _identityContext = identityContext;
            _getActivity = getActivity;
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var customers = await _context.Customers.Include(s => s.Enterprise).ToListAsync(cancellationToken);

            var activity = await _getActivity.RunAsync(Guid.Empty, ACTIVITY_TYPE, cancellationToken);

            return View(new CustomersPageViewModel { ActivityHistory = activity.ToList(), Customers = customers });
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customerEntity = await _context.Customers
                .Include(c => c.Enterprise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerEntity == null)
            {
                return NotFound();
            }

            return View(customerEntity);
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public IActionResult Create()
        {
            //ViewData["EnterpriseId"] = new SelectList(_identityContext., "Id", "Name");
            return View();
        }

        [HttpPost]
        [ClaimsAuthorize("UserType", "Enterprise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,EnterpriseId")] CustomerEntity customerEntity)
        {
            if (ModelState.IsValid)
            {
                customerEntity.Id = Guid.NewGuid();
                _context.Add(customerEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", customerEntity.EnterpriseId);
            return View(customerEntity);
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customerEntity = await _context.Customers.FindAsync(id);
            if (customerEntity == null)
            {
                return NotFound();
            }
            //ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", customerEntity.EnterpriseId);
            return View(customerEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,EnterpriseId")] CustomerEntity customerEntity)
        {
            if (id != customerEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerEntityExists(customerEntity.Id))
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
            //ViewData["EnterpriseId"] = new SelectList(_context.Enterprises, "Id", "Name", customerEntity.EnterpriseId);
            return View(customerEntity);
        }

        [ClaimsAuthorize("UserType", "Enterprise")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customerEntity = await _context.Customers
                .Include(c => c.Enterprise)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerEntity == null)
            {
                return NotFound();
            }

            return View(customerEntity);
        }

        [HttpPost, ActionName("Delete")]
        [ClaimsAuthorize("UserType", "Enterprise")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'SqlServerContext.Customers'  is null.");
            }
            var customerEntity = await _context.Customers.FindAsync(id);
            if (customerEntity != null)
            {
                _context.Customers.Remove(customerEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerEntityExists(Guid id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
