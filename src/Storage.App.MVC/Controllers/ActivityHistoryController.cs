using Microsoft.AspNetCore.Mvc;

namespace Storage.App.MVC.Controllers
{
    public class ActivityHistoryController : Controller
    {
        public ActivityHistoryController()
        {

        }

        public async Task<IActionResult> Details(Guid id)
        {
            return await Task.Run(() => NotFound());
        }
    }
}
