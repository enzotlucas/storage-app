using AutoMapper;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.UseCases.ActivityHistory
{
    public sealed class GetActivity : IGetActivity
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetActivity> _logger;
        private readonly IMapper _mapper;

        public GetActivity(IUnitOfWork uow, ILogger<GetActivity> logger, IMapper mapper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityHistoryViewModel>> RunAsync(Guid enterpriseId, 
                                                                          ActivityType activityType, 
                                                                          CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [GetActivity.RunAsync]");

            var activityHistory = await _uow.ActivityHistoryRepository.GetByAcitivityTypeAsync(enterpriseId, activityType, cancellationToken);

            if (!activityHistory.Any())
            {
                _logger.LogDebug("End - [GetActivity.RunAsync] - Any activity found");

                return Enumerable.Empty<ActivityHistoryViewModel>();
            }

            _logger.LogDebug("End - [GetActivity.RunAsync] - Activity found", new { activityHistory });

            return _mapper.Map<IEnumerable<ActivityHistoryViewModel>>(activityHistory);
        }
    }
}
