using System;
using System.Configuration;
using uIntra.LicenceService.ApiClient.Interfaces;
using Umbraco.Core.Services;

namespace Compent.uIntra.Core.Licence
{
    public sealed class ValidateLicenceService : IValidateLicenceService
    {
        private const int MaxAllowedTrialUsers = 30;
        private const string CompentLicenceKey = "CompentLicenceKey";

        private readonly IMemberService _memberService;
        private readonly ILicenceValidationServiceClient _validationServiceClient;

        public Lazy<bool> IsLicenceValid { get; }

        public ValidateLicenceService(IMemberService memberService, ILicenceValidationServiceClient validationServiceClient)
        {
            _memberService = memberService;
            _validationServiceClient = validationServiceClient;
            IsLicenceValid = new Lazy<bool>(GetValidationResult);
        }

        private bool GetValidationResult()
        {
            var membersCount = _memberService.Count();
            var licenceKey = ConfigurationManager.AppSettings.Get(CompentLicenceKey);

            bool isUserCountGraterThanAllowed = membersCount > MaxAllowedTrialUsers;
            bool result = Validate(isUserCountGraterThanAllowed, licenceKey);
            return result;
        }

        private bool Validate(bool isUserCountGraterThanAllowed, string licenceKey)
        {
            return !isUserCountGraterThanAllowed || !String.IsNullOrEmpty(licenceKey) && _validationServiceClient.Validate(licenceKey);
        }
    }
}