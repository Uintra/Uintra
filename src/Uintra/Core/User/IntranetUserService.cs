﻿using System;
using System.Threading.Tasks;
using Uintra.Core.User.Models;
using Uintra.Infrastructure.ApplicationSettings;
using Uintra.Infrastructure.Extensions;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace Uintra.Core.User
{
    public class IntranetUserService<T> : IIntranetUserService<T> where T : class, IIntranetUser, new()
    {
        private readonly IUserService _userService;
        private readonly IApplicationSettings _applicationSettings;

        public IntranetUserService(IUserService userService, IApplicationSettings applicationSettings)
        {
            _userService = userService;
            _applicationSettings = applicationSettings;
        }

        public Task<T> GetByIdAsync(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                var uintraUser = Map(user);
                return Task.FromResult(uintraUser);
            }

            return Task.FromResult<T>(null);
        }

        public void Disable(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                user.IsLockedOut = true;
                _userService.Save(user);
            }
        }

        public void Enable(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                user.IsLockedOut = false;
                user.IsApproved = true;
                _userService.Save(user);
            }
        }

        public Task<T> GetByEmailAsync(string email)
        {
            var user = _userService.GetByEmail(email);
            if (user != null)
            {
                var uintraUser = Map(user);
                return Task.FromResult(uintraUser);
            }

            return Task.FromResult<T>(null);
        }

        public T GetByEmail(string email)
        {
            var user = _userService.GetByEmail(email);
            if (user != null)
            {
                var uintraUser = Map(user);
                return uintraUser;
            }

            return null;
        }

        public T GetById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user != null)
            {
                var uintraUser = Map(user);
                return uintraUser;
            }

            return null;
        }

        protected virtual T Map(IUser umbracoUser)
        {
            var user = new T
            {
                IsSuperUser = _applicationSettings.UintraSuperUsers.Contains(umbracoUser.Email,
                    StringComparison.InvariantCultureIgnoreCase),
                DisplayName = umbracoUser.Name,
                Email = umbracoUser.Email,
                Id = umbracoUser.Id,
                IsApproved = umbracoUser.IsApproved,
                IsLockedOut = umbracoUser.IsLockedOut
            };

            return user;
        }
    }
}