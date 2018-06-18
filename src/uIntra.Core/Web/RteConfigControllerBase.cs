using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;
using Umbraco.Web.WebApi;

namespace uIntra.Core.Web
{
    public abstract class RteConfigControllerBase: UmbracoAuthorizedApiController
    {
        protected virtual string ConfigFolderPath => "~/App_Plugins/BaseControls/TinyMce/Configs";

        public virtual JsonResult GetConfig(string rteAlias)
        {
            var path = $"{ConfigFolderPath}/{rteAlias}-config.json";
            var configFilePath = HostingEnvironment.MapPath(path);
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException($"RTE config file not found:{path}");
            }

            string result;
            using (var r = new StreamReader(configFilePath))
            {
                result = r.ReadToEnd();
            }

            return new JsonResult
            {
                Data = JsonConvert.DeserializeObject(result)
            };
        }
    }
}
