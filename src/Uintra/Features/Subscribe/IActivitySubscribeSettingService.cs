﻿using System;
using System.Threading.Tasks;
using Uintra.Features.Subscribe.Models;
using Uintra.Features.Subscribe.Sql;

namespace Uintra.Features.Subscribe
{
    public interface IActivitySubscribeSettingService
    {
        ActivitySubscribeSetting Get(Guid activityId);
        Task<ActivitySubscribeSetting> GetAsync(Guid activityId);
        ActivitySubscribeSetting Create(ActivitySubscribeSettingDto setting);
        Task<ActivitySubscribeSetting> CreateAsync(ActivitySubscribeSettingDto setting);
        void Save(ActivitySubscribeSettingDto setting);
        Task SaveAsync(ActivitySubscribeSettingDto setting);
        void Delete(Guid activityId);
        Task DeleteAsync(Guid activityId);
        void FillSubscribeSettings(ISubscribeSettings activity);
        Task FillSubscribeSettingsAsync(ISubscribeSettings activity);
    }
}
