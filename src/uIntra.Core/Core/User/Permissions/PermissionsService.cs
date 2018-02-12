using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Exceptions;
using Umbraco.Core.Models;

namespace Uintra.Core.User.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IPermissionsConfiguration _configuration;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public PermissionsService(
            IPermissionsConfiguration configuration,
            IExceptionLogger exceptionLogger,
            IIntranetUserService<IIntranetUser> intranetUserService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _configuration = configuration;
            _exceptionLogger = exceptionLogger;
            _intranetUserService = intranetUserService;
            _activitiesServiceFactory = activitiesServiceFactory;
        }

        public virtual bool IsRoleHasPermissions(IRole role, params string[] permissions)
        {
            if (permissions.Any())
            {
                var rolePermissions = GetRolePermission(role);
                return rolePermissions.Intersect(permissions).Any();
            }

            var defaultValue = false;
            _exceptionLogger.Log(new Exception($"Tried check role permissions but no permissions was passed into method! Return {defaultValue}!!"));

            return defaultValue;
        }

        public virtual IEnumerable<string> GetRolePermission(IRole role)
        {
            var roleConfiguration = _configuration.Roles.FirstOrDefault(s => s.Key == role.Name);

            if (roleConfiguration == null)
            {
                throw new Exception($"Can't find permissions for role {role.Name}. Please check permissions config!");
            }

            return roleConfiguration.Permissions.Select(s => s.Key);
        }

        public virtual string GetPermissionFromTypeAndAction(Enum activityType, IntranetActivityActionEnum action)
        {
            return $"{activityType.ToString()}{action}";
        }

        public virtual bool IsCurrentUserHasAccess(Enum activityType, IntranetActivityActionEnum action, Guid? activityId = null)
        {
            var currentUser = _intranetUserService.GetCurrentUser();

            if (currentUser == null)
            {
                return false;
            }

            var result = IsUserHasAccess(currentUser, activityType, action, activityId);
            return result;
        }

        public virtual bool IsUserHasAccess(IIntranetUser user, Enum activityType, IntranetActivityActionEnum action, Guid? activityId = null)
        {
            if (user == null)
            {
                return false;
            }

            if (IsUserWebmaster(user))
            {
                return true;
            }

            var permission = $"{activityType.ToString()}{action}";
            var userHasPermissions = IsRoleHasPermissions(user.Role, permission);

            if (userHasPermissions && activityId.HasValue)
            {
                var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityType);
                var activity = service.Get(activityId.Value);

                if (activity is IHaveOwner owner)
                {
                    return owner.OwnerId == user.Id;
                }
            }

            return userHasPermissions;
        }

        public virtual bool IsUserWebmaster(IIntranetUser user)
        {
            return user.Role.Name == IntranetRolesEnum.WebMaster.ToString();
        }

        public virtual bool IsUserHasAccessToContent(IIntranetUser user, IPublishedContent content)
        {
            return user.UmbracoId.HasValue;
        }
    }
}