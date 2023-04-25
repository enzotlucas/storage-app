using AutoMapper;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Core.ActivityHistory
{
    public sealed class ActivityHistoryMapperProfile : Profile
    {
        public ActivityHistoryMapperProfile()
        {
            CreateMap<ActivityHistoryEntity, ActivityHistoryViewModel>().ReverseMap();
        }
    }
}
