using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Domain.Authorization;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.Infrastructure.Database;
using Storage.App.MVC.Models;
using Storage.App.MVC.UseCases.Enterprise;
using System.Security.Claims;

namespace Storage.App.MVC.Controllers
{
    public sealed class EnterprisesController : Controller
    {
        private const ActivityType ACTIVITY_TYPE = ActivityType.Enterprise;

        private readonly SqlServerContext _context;
        private readonly IGetActivity _getActivity;
        private readonly ICreateEnterprise _createEnterprise;
        private readonly IDeleteEnterprise _deleteEnterprise;

        public EnterprisesController(SqlServerContext context,
                                     IGetActivity getActivity,
                                     ICreateEnterprise createEnterprise,
                                     IDeleteEnterprise deleteEnterprise)
        {
            _context = context;
            _getActivity = getActivity;
            _createEnterprise = createEnterprise;
            _deleteEnterprise = deleteEnterprise;
        }

        [ClaimsAuthorize("UserType", "Admin")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var enterprises = await _context.Enterprises.Where(e => e.Name != "Admin").ToListAsync(cancellationToken);

            var activity = await _getActivity.RunAsync(Guid.Empty, ACTIVITY_TYPE, cancellationToken);

            return View(new EnterprisesPageViewModel { ActivityHistory = activity.ToList(), Enterprises = enterprises });
        }

        [ClaimsAuthorize("UserType", "Admin")]
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

        [ClaimsAuthorize("UserType", "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ClaimsAuthorize("UserType", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,Password")] EnterpriseViewModel enterprise, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var enterpriseId = HttpContext.GetClaimValue(ClaimTypes.NameIdentifier);

                var response = await _createEnterprise.RunAsync(enterprise, new Guid(enterpriseId), cancellationToken);

                if (response.Success)
                    return RedirectToAction(nameof(Index));

                foreach (var error in response.Errors)
                    ModelState.AddModelError(string.Empty, error);
            }

            return View(enterprise);
        }

        [ClaimsAuthorize("UserType", "Admin")]
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

        [HttpPost]
        [ClaimsAuthorize("UserType", "Admin")]
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

        [ClaimsAuthorize("UserType", "Admin")]
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

        [ClaimsAuthorize("UserType", "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var enterpriseId = HttpContext.GetClaimValue(ClaimTypes.NameIdentifier);

            var response = await _deleteEnterprise.RunAsync(id, new Guid(enterpriseId), cancellationToken);

            if (response.Success)
                return RedirectToAction(nameof(Index));

            foreach (var error in response.Errors)
                ModelState.AddModelError(string.Empty, error);

            return View();
        }

        private bool EnterpriseEntityExists(Guid id)
        {
            return _context.Enterprises.Any(e => e.Id == id);
        }
    }
}
