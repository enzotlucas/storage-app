using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Core.Domain;
using AutoMapper;
using Storage.App.MVC.Domain.Core;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.ActivityHistory;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Storage.App.MVC.Domain.Enterprise;
using Storage.App.MVC.Domain.ActivityHistory;
using Storage.App.MVC.Models.Enterprise;

namespace Storage.App.MVC.UseCases.Enterprise
{
    public class CreateEnterprise : ICreateEnterprise
    {
        private readonly UserManager<EnterpriseEntity> _userManager;
        private readonly ILogger<CreateEnterprise> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ISaveActivity _saveActivity;
        private readonly BaseResult _response;

        public CreateEnterprise(UserManager<EnterpriseEntity> userManager,
                                ILogger<CreateEnterprise> logger,
                                IEmailSender emailSender,
                                IUnitOfWork uow,
                                IMapper mapper,
                                ISaveActivity saveActivity)
        {
            if (!userManager.SupportsUserEmail)
                throw new NotSupportedException("The default UI requires a user store with email support.");

            _userManager = userManager;
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _saveActivity = saveActivity;
            _response = new();
        }

        public async Task<BaseResult> RunAsync(EnterpriseViewModel enterpriseViewModel, Guid enterpriseId, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [CreateEnterprise.RunAsync]");

            var enterprise = _mapper.Map<EnterpriseEntity>(enterpriseViewModel);

            var userManagerResult = await _userManager.CreateAsync(enterprise, enterpriseViewModel.Password);

            if (!userManagerResult.Succeeded)
            {
                var errors = userManagerResult.Errors.Select(error => error.Description);

                _logger.LogWarning("End - [CreateEnterprise.RunAsync] - Error creating account", new { errors });

                return _response.AsError(errors.ToList());
            }

            var userClaims = enterprise.GenerateClaims("Enterprise");

            await _userManager.AddClaimsAsync(enterprise, userClaims);

            if (!userManagerResult.Succeeded)
            {
                var errors = userManagerResult.Errors.Select(error => error.Description);

                await _userManager.DeleteAsync(enterprise);

                _logger.LogWarning("End - [CreateEnterprise.RunAsync] - Error creating account", new { errors });

                return _response.AsError(errors.ToList());
            }

            await _uow.Enterprises.CreateAsync(enterprise, cancellationToken);

            await _saveActivity.RunAsync(enterpriseId, enterprise.Id, ActivityType.Enterprise, ActivityAction.Create, $"Enterprise {enterprise.Name} created", cancellationToken);

            if (!await _uow.SaveChangesAsync())
            {
                _logger.LogError("End - [CreateEnterprise.RunAsync] - Error saving on database");

                await _userManager.RemoveClaimsAsync(enterprise, userClaims);

                await _userManager.DeleteAsync(enterprise);

                return _response.AsError("Error saving on database");
            }

            _logger.LogInformation("User created");

            _logger.LogDebug("End - [CreateEnterprise.RunAsync]");

            return _response.AsSuccess();
        }
    }
}
