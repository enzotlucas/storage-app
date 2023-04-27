using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Domain.Core;
using Storage.App.MVC.Domain.Enterprise;
using System.Security.Claims;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.Models.Enterprise;

namespace Storage.App.MVC.UseCases.Enterprise
{
    public class CreateAdminIfNeeded : ICreateAdminIfNeeded
    {
        private readonly UserManager<EnterpriseEntity> _userManager;
        private readonly IUserStore<EnterpriseEntity> _userStore;
        private readonly ILogger<CreateAdminIfNeeded> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly BaseResult _response;

        public CreateAdminIfNeeded(UserManager<EnterpriseEntity> userManager,
                                IUserStore<EnterpriseEntity> userStore,
                                ILogger<CreateAdminIfNeeded> logger,
                                IUnitOfWork uow,
                                IMapper mapper)
        {
            if (!userManager.SupportsUserEmail)
                throw new NotSupportedException("The default UI requires a user store with email support.");

            _userManager = userManager;
            _userStore = userStore;
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _response = new();
        }

        public async Task<BaseResult> RunAsync(EnterpriseViewModel enterpriseViewModel, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [CreateAdminIfNeeded.RunAsync]");

            var enterprise = _mapper.Map<EnterpriseEntity>(enterpriseViewModel);

            if (await _userStore.FindByNameAsync(enterpriseViewModel.Email, cancellationToken) is not null)
            {
                _logger.LogDebug("End - [CreateAdminIfNeeded.RunAsync] - User exists");

                return _response.AsError("User exists");
            }

            await _userStore.SetUserNameAsync(enterprise, enterpriseViewModel.Email, cancellationToken);

            var userManagerResult = await _userManager.CreateAsync(enterprise, enterpriseViewModel.Password);

            if (!userManagerResult.Succeeded)
            {
                var errors = userManagerResult.Errors.Select(error => error.Description);

                _logger.LogWarning("End - [CreateAdminIfNeeded.RunAsync] - Error creating account", new { errors });

                return _response.AsError(errors.ToList());
            }

            var userClaims = enterprise.GenerateClaims("Admin").ToList();

            await _userManager.AddClaimsAsync(enterprise, userClaims);

            if (!userManagerResult.Succeeded)
            {
                var errors = userManagerResult.Errors.Select(error => error.Description);

                await _userManager.DeleteAsync(enterprise);

                _logger.LogWarning("End - [CreateAdminIfNeeded.RunAsync] - Error creating account", new { errors });

                return _response.AsError(errors.ToList());
            }

            await _uow.Enterprises.CreateAsync(enterprise, cancellationToken);

            if (!await _uow.SaveChangesAsync())
            {
                _logger.LogError("End - [CreateAdminIfNeeded.RunAsync] - Error saving on database");

                await _userManager.RemoveClaimsAsync(enterprise, userClaims);

                await _userManager.DeleteAsync(enterprise);

                return _response.AsError("Error saving on database");
            }

            _logger.LogInformation("User created");

            _logger.LogDebug("End - [CreateAdminIfNeeded.RunAsync]");

            return _response.AsSuccess();
        }
    }
}
