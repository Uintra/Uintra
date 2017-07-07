[assembly: WebActivator.PreApplicationStartMethod(typeof(uIntra.App_Start.ElmahMvc), "Start")]
namespace uIntra.App_Start
{
    public class ElmahMvc
    {
        public static void Start()
        {
            Elmah.Mvc.Bootstrap.Initialize();
        }
    }
}