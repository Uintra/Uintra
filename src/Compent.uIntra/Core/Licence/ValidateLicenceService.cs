using System;
using System.Configuration;
using uIntra.LicenceService.ApiClient.Interfaces;
using Umbraco.Core.Services;

namespace Compent.uIntra.Core
{
    public sealed class ValidateLicenceService : IValidateLicenceService
    {
        private const int MaxAllowedTrialUsers = 30;
        private const string CompentLicenceKey = "CompentLicenceKey";

        private readonly IMemberService _memberService;
        private readonly ILicenceValidationServiceClient _validationServiceClient;

        public ValidateLicenceService(IMemberService memberService, ILicenceValidationServiceClient validationServiceClient)
        {
            _memberService = memberService;
            _validationServiceClient = validationServiceClient;
        }

        public bool Validate()
        {
            var membersCount = _memberService.Count();
            var licenceKey = new Lazy<string>(() => ConfigurationManager.AppSettings.Get(CompentLicenceKey));
            var isLicenceKeyValid = new Lazy<bool>(() => _validationServiceClient.Validate(licenceKey.Value));

            bool isUserCountGraterThanAllowed = IsUserCountGraterThanAllowed(membersCount, MaxAllowedTrialUsers);
            bool result = Validate(isUserCountGraterThanAllowed, licenceKey, isLicenceKeyValid);
            return result;
        }

        private bool Validate(bool isUserCountGraterThanAllowed, Lazy<string> licenceKey, Lazy<bool> isLicenceKeyValid)
        {
            return isUserCountGraterThanAllowed || (!String.IsNullOrEmpty(licenceKey.Value) && isLicenceKeyValid.Value);
        }

        private bool IsUserCountGraterThanAllowed(int actualCount, int allowedCount)
        {
            return actualCount > allowedCount;
        }
    }
}