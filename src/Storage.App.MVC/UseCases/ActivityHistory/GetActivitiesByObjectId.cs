using AutoMapper;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.UseCases.ActivityHistory
{
    public class GetActivitiesByObjectId : IGetActivitiesByObjectId
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetActivitiesByObjectId> _logger;
        private readonly IMapper _mapper;

        public GetActivitiesByObjectId(IUnitOfWork uow, 
                                       ILogger<GetActivitiesByObjectId> logger, 
                                       IMapper mapper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityHistoryViewModel>> RunAsync(Guid objectId,
                                                                          CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [GetActivitiesByObjectId.RunAsync]");

            var activityHistory = await _uow.ActivityHistory.GetByObjectIdAsync(objectId, cancellationToken);

            if (!activityHistory.Any())
            {
                _logger.LogDebug("End - [GetActivitiesByObjectId.RunAsync] - Any activity found");

                return Enumerable.Empty<ActivityHistoryViewModel>();
            }

            _logger.LogDebug("End - [GetActivitiesByObjectId.RunAsync] - Activity found", new { activityHistory });

            return _mapper.Map<IEnumerable<ActivityHistoryViewModel>>(activityHistory);
        }
    }
}
