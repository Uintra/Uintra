using System;
using System.Configuration;
using uIntra.LicenceService.ApiClient.Interfaces;
using Umbraco.Core.Services;

namespace Compent.Uintra.Core.Licence
{
    public sealed class ValidateLicenceService : IValidateLicenceService
    {
        private const int MaxAllowedTrialUsers = 30;
        private const string CompentLicenceKey = "CompentLicenceKey";

        private readonly IMemberService _memberService;
        private readonly ILicenceValidationServiceClient _validationServiceClient;

        private Lazy<bool> IsLicenceValid { get; }


        public ValidateLicenceService(IMemberService memberService, ILicenceValidationServiceClient validationServiceClient)
        {
            _memberService = memberService;
            _validationServiceClient = validationServiceClient;
            IsLicenceValid = new Lazy<bool>(ValidateLicence);
        }

        public bool GetValidationResult()
        {
            return IsMemberCountAllowed || IsLicenceValid.Value;
        }

        private bool IsMemberCountAllowed => _memberService.Count() <= MaxAllowedTrialUsers;

        private bool ValidateLicence()
        {
            var licenceKey = ConfigurationManager.AppSettings.Get(CompentLicenceKey);
            return !String.IsNullOrEmpty(licenceKey) && _validationServiceClient.Validate(licenceKey);
        }
    }
}