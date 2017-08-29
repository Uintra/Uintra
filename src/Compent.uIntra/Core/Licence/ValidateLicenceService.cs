using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using uIntra.Core.User;
using uIntra.LicenceService.ApiClient.Interfaces;

namespace Compent.uIntra.Core
{
    public sealed class ValidateLicenceService : IValidateLicenceService
    {
        private const int MaxAllowedTrialUsers = 30;
        private const string CompentLicenceKey = "CompentLicenceKey";

        private readonly IIntranetUserService<IIntranetUser> _userService;
        private readonly ILicenceValidationServiceClient _validationServiceClient;

        public ValidateLicenceService(IIntranetUserService<IIntranetUser> userService, ILicenceValidationServiceClient validationServiceClient)
        {
            _userService = userService;
            _validationServiceClient = validationServiceClient;
        }

        public bool Validate()
        {
            IEnumerable<IIntranetUser> allUsers = _userService.GetAll();
            var licenceKey = new Lazy<string>(() => ConfigurationManager.AppSettings.Get(CompentLicenceKey));
            var isLicenceKeyValid = new Lazy<bool>(() => _validationServiceClient.Validate(licenceKey.Value));

            bool isUserCountGraterThanAllowed = IsUserCountGraterThanAllowed(allUsers, MaxAllowedTrialUsers);
            bool result = Validate(isUserCountGraterThanAllowed, licenceKey, isLicenceKeyValid);
            return result;
        }

        private bool Validate(bool isUserCountGraterThanAllowed, Lazy<string> licenceKey, Lazy<bool> isLicenceKeyValid)
        {
            return isUserCountGraterThanAllowed || (!String.IsNullOrEmpty(licenceKey.Value) && isLicenceKeyValid.Value);
        }

        private bool IsUserCountGraterThanAllowed(IEnumerable<IIntranetUser> allUsers, int allowedCount)
        {
            return allUsers.Count() > allowedCount;
        }
    }
}