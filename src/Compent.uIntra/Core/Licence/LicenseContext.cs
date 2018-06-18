using System.Linq;
using uIntra.Core.User;
using UIntra.License.Validator;
using UIntra.License.Validator.Context;
using UIntra.License.Validator.Services;

namespace Compent.uIntra.Core.Licence
{
    public class LicenseContext : ILicenseContext
    {
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly ILicenseValidatorService _licenseValidatorService;
        private readonly bool _isValid;
        private readonly string _domain;
        private readonly string _name;
        private readonly LicenseTypeEnum _licenseType;

        public LicenseContext(IIntranetUserService<IIntranetUser> intranetUserService, ILicenseValidatorService licenseValidatorService)
        {
            _intranetUserService = intranetUserService;
            _licenseValidatorService = licenseValidatorService;
            _domain = "TestDomain";
            _name = "TestName";
            _licenseType = LicenseTypeEnum.Large;
            //if (_licenseType != LicenseTypeEnum.Free)
            //{
            //    _isValid = ValidateLicense();
            //}
            //else
            //{
            //    _isValid = true;
            //}
            _isValid = ValidateLicense();
        }

        public bool ValidateLicense()
        {
            return _licenseValidatorService.Validate(_name, _domain, _licenseType);
        }

        public bool IsValid => _isValid;
        public string Domain => _domain;
        public string Name => _name;
        public LicenseTypeEnum LicenseType => _licenseType;

        public LicenseTypeEnum ResolveLicenseType()
        {
            var usersCount = _intranetUserService.GetAll().Count();

            if (usersCount <= 30)
            {
                return LicenseTypeEnum.Free;
            }

            if (usersCount > 30 && usersCount < 300)
            {
                return LicenseTypeEnum.Medium;
            }

            return usersCount > 300 ? LicenseTypeEnum.Large : LicenseTypeEnum.Free;
        }

    }
}