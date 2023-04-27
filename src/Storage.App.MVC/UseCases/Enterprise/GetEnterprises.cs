using AutoMapper;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Domain.Enterprise.UseCases;
using Storage.App.MVC.Models.Enterprise;

namespace Storage.App.MVC.UseCases.Enterprise
{
    public class GetEnterprises : IGetEnterprises
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEnterprises> _logger;

        public GetEnterprises(IUnitOfWork uow, 
                              IMapper mapper, 
                              ILogger<GetEnterprises> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EnterpriseViewModel>> RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [GetEnterprises.RunAsync]");

            var enterprises = await _uow.Enterprises.GetAllAsync(Guid.Empty, cancellationToken);

            if (!enterprises.Any())
            {
                _logger.LogDebug("End - [GetEnterprises.RunAsync] - Any activity found");

                return Enumerable.Empty<EnterpriseViewModel>();
            }
            _logger.LogDebug("End - [GetEnterprises.RunAsync]");

            return _mapper.Map<IEnumerable<EnterpriseViewModel>>(enterprises);
        }
    }
}
