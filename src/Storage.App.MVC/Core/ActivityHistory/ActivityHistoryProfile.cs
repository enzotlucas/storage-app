﻿using AutoMapper;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Core.ActivityHistory
{
    public sealed class ActivityHistoryProfile : Profile
    {
        public ActivityHistoryProfile()
        {
            CreateMap<ActivityHistoryEntity, ActivityHistoryViewModel>().ReverseMap();
        }
    }
}
