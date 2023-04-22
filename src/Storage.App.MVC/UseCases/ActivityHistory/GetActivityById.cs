using AutoMapper;
using Storage.App.MVC.Core.ActivityHistory.UseCases;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.UseCases.ActivityHistory
{
    public sealed class GetActivityById : IGetActivityById
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetActivity> _logger;
        private readonly IMapper _mapper;

        public GetActivityById(IUnitOfWork uow, ILogger<GetActivity> logger, IMapper mapper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActivityHistoryViewModel> RunAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [GetActivityById.RunAsync]");

            var activityHistory = await _uow.ActivityHistoryRepository.GetByIdAsync(id, cancellationToken);

            if (activityHistory.Id == Guid.Empty)
            {
                _logger.LogDebug("End - [GetActivityById.RunAsync] - Any activity found");

                return new ActivityHistoryViewModel();
            }

            _logger.LogDebug("End - [GetActivityById.RunAsync] - Activity found", new { activityHistory });

            return _mapper.Map<ActivityHistoryViewModel>(activityHistory);
        }
    }
}
