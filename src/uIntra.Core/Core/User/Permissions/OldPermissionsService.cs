using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Exceptions;
using Umbraco.Core.Models;

namespace Uintra.Core.User.Permissions
{
    public class OldPermissionsService : IOldPermissionsService
    {
        private readonly IPermissionsConfiguration _configuration;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly IActivitiesServiceFactory _activitiesServiceFactory;

        public OldPermissionsService(
            IPermissionsConfiguration configuration,
            IExceptionLogger exceptionLogger,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            IActivitiesServiceFactory activitiesServiceFactory)
        {
            _configuration = configuration;
            _exceptionLogger = exceptionLogger;
            _intranetMemberService = intranetMemberService;
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

        public virtual string GetPermissionFromTypeAndAction(Enum activityType, IntranetActionEnum action)
        {
            return $"{activityType.ToString()}{action}";
        }

        public virtual bool IsCurrentMemberHasAccess(Enum activityType, IntranetActionEnum action, Guid? activityId = null)
        {
            var currentMember = _intranetMemberService.GetCurrentMember();

            if (currentMember == null)
            {
                return false;
            }

            var result = IsUserHasAccess(currentMember, activityType, action, activityId);
            return result;
        }

        public virtual bool IsUserHasAccess(IIntranetMember member, Enum activityType, IntranetActionEnum action, Guid? activityId = null)
        {
            if (member == null)
            {
                return false;
            }

            if (IsUserWebmaster(member))
            {
                return true;
            }

            var permission = $"{activityType.ToString()}{action}";
            var userHasPermissions = IsRoleHasPermissions(member.Role, permission);

            if (userHasPermissions && activityId.HasValue)
            {
                var service = _activitiesServiceFactory.GetService<IIntranetActivityService<IIntranetActivity>>(activityType);
                var activity = service.Get(activityId.Value);

                if (activity is IHaveOwner owner)
                {
                    return owner.OwnerId == member.Id;
                }
            }

            return userHasPermissions;
        }

        public virtual bool IsUserWebmaster(IIntranetMember member)
        {
            return member.Role.Name == IntranetRolesEnum.WebMaster.ToString();
        }

        public virtual bool IsUserHasAccessToContent(IIntranetMember member, IPublishedContent content)
        {
            return member.RelatedUser != null;
        }
    }
}