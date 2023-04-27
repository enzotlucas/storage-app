using AutoMapper;
using Storage.App.MVC.Core.Domain;
using Storage.App.MVC.Domain.Customer.UseCases;
using Storage.App.MVC.Models.Enterprise;

namespace Storage.App.MVC.UseCases.Enterprise
{
    public class GetEnterpriseById : IGetEnterpriseById
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEnterpriseById> _logger;

        public GetEnterpriseById(IUnitOfWork uow,
                                 IMapper mapper,
                                 ILogger<GetEnterpriseById> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EnterpriseViewModel> RunAsync(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Begin - [GetEnterpriseById.RunAsync]");

            var enterprises = await _uow.Enterprises.GetByIdAsync(id, cancellationToken);

            if (!enterprises.Exists())
            {
                _logger.LogDebug("End - [GetEnterpriseById.RunAsync] - Any activity found");

                return new EnterpriseViewModel { Id = Guid.Empty };
            }

            _logger.LogDebug("End - [GetEnterpriseById.RunAsync]");

            return _mapper.Map<EnterpriseViewModel>(enterprises);
        }
    }
}
