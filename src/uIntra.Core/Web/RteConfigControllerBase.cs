using System.IO;
using System.Web.Hosting;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace uIntra.Core.Web
{
    public abstract class RteConfigControllerBase
    {
        protected string DefaultConfigFolder => "~/App_Plugins/BaseControls/TinyMce/Configs";

        public virtual JsonResult GetConfig(string rteAlias)
        {
            var path = $"{DefaultConfigFolder}/{rteAlias}-config.json";
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
