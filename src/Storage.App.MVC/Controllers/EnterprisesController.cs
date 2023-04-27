using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Domain.Authorization;
using Storage.App.MVC.Domain.Customer.UseCases;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.Infrastructure.Database;
using Storage.App.MVC.Models.Enterprise;
using Storage.App.MVC.UseCases.Enterprise;
using System.Security.Claims;

namespace Storage.App.MVC.Controllers
{
    public sealed class EnterprisesController : Controller
    {
        private const ActivityType ACTIVITY_TYPE = ActivityType.Enterprise;

        private readonly SqlServerContext _context;
        private readonly IGetActivity _getActivity;
        private readonly IGetActivitiesByObjectId _getActivitiesByObjectId;

        private readonly IGetEnterpriseById _getEnterpriseById;
        private readonly IGetEnterprises _getEnterprises;
        private readonly ICreateEnterprise _createEnterprise;
        private readonly IDeleteEnterprise _deleteEnterprise;

        private readonly ILogger<ActivityHistoryController> _logger;

        public EnterprisesController(SqlServerContext context,
                                     IGetActivity getActivity,
                                     IGetActivitiesByObjectId getActivitiesByObjectId,
                                     IGetEnterprises getEnterprises,
                                     ICreateEnterprise createEnterprise,
                                     IDeleteEnterprise deleteEnterprise,
                                     IGetEnterpriseById getEnterpriseById,
                                     ILogger<ActivityHistoryController> logger)
        {
            _context = context;
            _getActivity = getActivity;
            _getActivitiesByObjectId = getActivitiesByObjectId;

            _getEnterprises = getEnterprises;
            _createEnterprise = createEnterprise;
            _deleteEnterprise = deleteEnterprise;
            _getEnterpriseById = getEnterpriseById;

            _logger = logger;
        }

        [ClaimsAuthorize("UserType", "Admin")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [EnterprisesController.Index]");

            var enterprises = await _getEnterprises.RunAsync(cancellationToken);

            var activity = await _getActivity.RunAsync(Guid.Empty, ACTIVITY_TYPE, cancellationToken);

            _logger.LogDebug("End - [EnterprisesController.Index]");

            return View(new EnterprisesPageViewModel { ActivityHistory = activity.ToList(), Enterprises = enterprises.ToList() });
        }

        [ClaimsAuthorize("UserType", "Admin")]
        public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [EnterprisesController.Details]", new { id });

            if (id == null)            
                return NotFound();

            var enterprise = await _getEnterpriseById.RunAsync(id.Value, cancellationToken);

            if (!enterprise.Exists())
            {
                _logger.LogDebug("End - [EnterprisesController.Details] - Enterprise not found", new { id });

                return NotFound();
            }

            var activities = await _getActivitiesByObjectId.RunAsync(id.Value, cancellationToken);

            _logger.LogDebug("End - [EnterprisesController.Details] - Enterprise found", new { id });

            return View(new EnterprisePageDetailsViewModel { ActivityHistory = activities.ToList(), Enterprise = enterprise});
        }

        [ClaimsAuthorize("UserType", "Admin")]
        public IActionResult Create()
        {
            _logger.LogDebug("[EnterprisesController.Create]");

            return View();
        }

        [HttpPost]
        [ClaimsAuthorize("UserType", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,Password")] EnterpriseViewModel enterprise, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [EnterprisesController.Create]");

            if (ModelState.IsValid)
            {
                var enterpriseId = HttpContext.GetClaimValue(ClaimTypes.NameIdentifier);

                var response = await _createEnterprise.RunAsync(enterprise, new Guid(enterpriseId), cancellationToken);

                if (response.Success)
                {
                    _logger.LogDebug("End - [EnterprisesController.Create] - Success");

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in response.Errors)
                    ModelState.AddModelError(string.Empty, error);
            }

            _logger.LogDebug("End - [EnterprisesController.Create] - Error");

            return View(enterprise);
        }

        [ClaimsAuthorize("UserType", "Admin")]
        public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [EnterprisesController.Edit]", new { id });

            if (id == null)            
                return NotFound();            

            var enterprise = await _getEnterpriseById.RunAsync(id.Value, cancellationToken);

            if (enterprise.Exists())
            {
                _logger.LogDebug("End - [EnterprisesController.Edit]", new { id });

                return NotFound();
            }

            _logger.LogDebug("End - [EnterprisesController.Edit]", new { id });

            return View(enterprise);
        }

        [HttpPost]
        [ClaimsAuthorize("UserType", "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditConfirmed(Guid id, [Bind("Id,Name")] EnterpriseEntity enterpriseEntity)
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
        public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [EnterprisesController.Delete]", new { id });

            if (id == null)            
                return NotFound();            

            var enterprise = await _getEnterpriseById.RunAsync(id.Value, cancellationToken);

            if (!enterprise.Exists())
            {
                _logger.LogDebug("End - [EnterprisesController.Delete] - Enterprise does't exists", new { id });

                return NotFound();
            }

            _logger.LogDebug("End - [EnterprisesController.Delete]- Enterprise exists", new { id });

            return View(enterprise);
        }

        [ClaimsAuthorize("UserType", "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [EnterprisesController.DeleteConfirmed]", new { id });

            var enterpriseId = HttpContext.GetClaimValue(ClaimTypes.NameIdentifier);

            var response = await _deleteEnterprise.RunAsync(id, new Guid(enterpriseId), cancellationToken);

            if (response.Success)
            {
                _logger.LogDebug("End - [EnterprisesController.DeleteConfirmed] - Success", new { id, response });

                return RedirectToAction(nameof(Index));
            }

            foreach (var error in response.Errors)
                ModelState.AddModelError(string.Empty, error);

            _logger.LogDebug("End - [EnterprisesController.DeleteConfirmed] - Error", new { id, response });

            return View();
        }

        private bool EnterpriseEntityExists(Guid id)
        {
            return _context.Enterprises.Any(e => e.Id == id);
        }
    }
}
