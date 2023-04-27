using Microsoft.AspNetCore.Identity;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Domain.ActivityHistory;
using Storage.App.MVC.Domain.Core;
using Storage.App.MVC.Domain.Enterprise.UseCases;

namespace Storage.App.MVC.UseCases.Enterprise
{
    public class DeleteEnterprise : IDeleteEnterprise
    {
        private readonly UserManager<EnterpriseEntity> _userManager;
        private readonly IUserStore<EnterpriseEntity> _userStore;
        private readonly IUnitOfWork _uow;
        private readonly ISaveActivity _saveActivity;
        private readonly ILogger<DeleteEnterprise> _logger;
        private readonly BaseResult _response;

        public DeleteEnterprise(UserManager<EnterpriseEntity> userManager,
                                IUserStore<EnterpriseEntity> userStore,
                                IUnitOfWork uow,
                                ISaveActivity saveActivity,
                                ILogger<DeleteEnterprise> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _uow = uow;
            _saveActivity = saveActivity;
            _logger = logger;
            _response = new();
        }

        public async Task<BaseResult> RunAsync(Guid id, Guid enterpriseId, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [DeleteEnterprise.RunAsync]");

            var enterprise = await _uow.Enterprises.GetByIdAsync(id, cancellationToken);

            if(!enterprise.Exists())
            {
                _logger.LogWarning("End - [DeleteEnterprise.RunAsync] - User don't exists", new { id });

                return _response.AsError("User don't exists");
            }

            await _saveActivity.RunAsync(enterpriseId, id, ActivityType.Enterprise, ActivityAction.Delete, $"User {enterprise.Name} excluded", cancellationToken);

            await _uow.Enterprises.DeleteAsync(enterprise, cancellationToken);

            if (!await _uow.SaveChangesAsync())
            {
                _logger.LogError("End - [DeleteEnterprise.RunAsync] - Error saving on database", new { id });

                return _response.AsError("Error saving on database");
            }

            var user = await _userStore.FindByNameAsync(enterprise.Email, cancellationToken);

            var claims = await _userManager.GetClaimsAsync(user);

            var deletedClaimsResult = await _userManager.RemoveClaimsAsync(user, claims);

            if (!deletedClaimsResult.Succeeded)
            {
                var errors = deletedClaimsResult.Errors.Select(error => error.Description);

                _logger.LogError("End - [DeleteEnterprise.RunAsync] - Error deleting claims on identity manager", new { id, errors });

                return _response.AsError(errors.ToList());
            }

            var deletedUserResult = await _userManager.DeleteAsync(user);

            if (!deletedUserResult.Succeeded)
            {
                var errors = deletedUserResult.Errors.Select(error => error.Description);

                _logger.LogError("End - [DeleteEnterprise.RunAsync] - Error deleting account on identity manager", new { id, errors });

                await _userManager.AddClaimsAsync(user, claims);

                return _response.AsError(errors.ToList());
            }

            _logger.LogInformation("User deleted", new { id });

            _logger.LogDebug("End - [DeleteEnterprise.RunAsync]");

            return _response.AsSuccess();
        }
    }
}
