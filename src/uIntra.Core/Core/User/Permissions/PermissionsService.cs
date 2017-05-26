using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Exceptions;

namespace uIntra.Core.User.Permissions
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IPermissionsConfiguration _configuration;
        private readonly IExceptionLogger _exceptionLogger;

        public PermissionsService(IPermissionsConfiguration configuration, 
            IExceptionLogger exceptionLogger)
        {
            _configuration = configuration;
            _exceptionLogger = exceptionLogger;
        }
        
        public virtual bool IsRoleHasPermissions<T>(T role, params string[] permissions) where T : struct
        {
            if (permissions.Any())
            {
                var rolePermissions = GetRolePermission(role);
                return rolePermissions.Intersect(permissions).Any();
            }

            var defaultValue = false;
            _exceptionLogger.Log(new Exception($"Tryed check role permissions but no permissions was passed into method! Return {defaultValue}!!"));
            
            return defaultValue;
        }

        public virtual IEnumerable<string> GetRolePermission<T>(T role) where T : struct
        {
            var roleConfiguration = _configuration.Roles.FirstOrDefault(s => s.Key == role.ToString());

            if (roleConfiguration == null)
            {
                throw new Exception($"Can't find permissions for role {role}. Please check permissions config!");
            }

            return roleConfiguration.Permissions.Select(s => s.Key);
        }

        public virtual string GetPermissionFromTypeAndAction(IntranetActivityTypeEnum activityType, IntranetActivityActionEnum action)
        {
            return $"{activityType}{action}";
        }
    }
}