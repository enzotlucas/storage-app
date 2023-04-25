using AutoMapper;
using Storage.App.MVC.Core.ActivityHistory;
using Storage.App.MVC.Core.Enterprise;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Domain.Enterprise
{
    public class EnterpriseMapperProfile : Profile
    {
        public EnterpriseMapperProfile()
        {
            CreateMap<EnterpriseViewModel, EnterpriseEntity>().ForMember(entity => entity.Id, mapper => mapper.MapFrom(vm => Guid.NewGuid()))
                                                              .ForMember(entity => entity.EmailConfirmed, mapper => mapper.MapFrom(vm => true))
                                                              .ForMember(entity => entity.UserName, mapper => mapper.MapFrom(vm => vm.Email));

            CreateMap<EnterpriseEntity, EnterpriseViewModel>();
        }
    }
}
