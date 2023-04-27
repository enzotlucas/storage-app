using Microsoft.AspNetCore.Mvc;
using Storage.App.MVC.Core.ActivityHistory.UseCases;

namespace Storage.App.MVC.Controllers
{
    public class ActivityHistoryController : Controller
    {
        private readonly IGetActivityById _getActivityById;
        private readonly ILogger<ActivityHistoryController> _logger;

        public ActivityHistoryController(IGetActivityById getActivityById, ILogger<ActivityHistoryController> logger)
        {
            _getActivityById = getActivityById;
            _logger = logger;
        }

        public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [ActivityHistoryController.Details]", new { id });

            if(id is null)
                return NotFound();

            var activity = await _getActivityById.RunAsync(id.Value, cancellationToken);

            if (!activity.Exists())
            {
                _logger.LogDebug("End - [ActivityHistoryController.Details] - Activity not found", new { id });

                return NotFound();
            }

            _logger.LogDebug("End - [ActivityHistoryController.Details] - Activity found", new { id, activity });

            return View(activity);
        }
    }
}
