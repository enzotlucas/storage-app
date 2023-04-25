using Microsoft.AspNetCore.Mvc;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.Models;
using System.Diagnostics;

namespace Storage.App.MVC.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, 
                              IConfiguration configuration, 
                              ICreateAdminIfNeeded createAdminIfNeeded)
        {
            _logger = logger;

            var enterprise = new EnterpriseViewModel
            {
                Name = configuration["Admin:Name"],
                Email = configuration["Admin:Email"],
                PhoneNumber = configuration["Admin:PhoneNumber"],
                Password = configuration["Admin:Password"],
            };

            
            if (createAdminIfNeeded.RunAsync(enterprise, CancellationToken.None).Result.Success)
                _logger.LogInformation("Admin user created");
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            var modelErro = new ErrorViewModel();

            if (id == 500)
            {
                modelErro.Message = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Title = "Ocorreu um erro!";
                modelErro.ErrorCode = id;
            }
            else if (id == 404)
            {
                modelErro.Message = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Title = "Ops! Página não encontrada.";
                modelErro.ErrorCode = id;
            }
            else if (id == 403)
            {
                modelErro.Message = "Você não tem permissão para fazer isto.";
                modelErro.Title = "Acesso Negado";
                modelErro.ErrorCode = id;
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", modelErro);
        }
    }
}