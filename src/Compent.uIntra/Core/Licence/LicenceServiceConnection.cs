using uIntra.LicenceService.ApiClient.Interfaces;

namespace Compent.Uintra.Core.Licence
{
    public class LicenceServiceConnection : IWebApiConnection
    {
        public string Endpoint => "http://uintra-licence.compent2.dk/api/validation/validate?licenceKey=";
    }
}