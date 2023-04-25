﻿using Storage.App.MVC.Domain.Core;
using Storage.App.MVC.Models;

namespace Storage.App.MVC.Domain.Enterprise.UseCases
{
    public interface ICreateAdminIfNeeded
    {
        Task<BaseResult> RunAsync(EnterpriseViewModel enterpriseViewModel, CancellationToken cancellationToken);
    }
}
