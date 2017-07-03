[assembly: WebActivator.PreApplicationStartMethod(typeof(Compent.uIntra.App_Start.ElmahMvc), "Start")]
namespace Compent.uIntra.App_Start
{
    public class ElmahMvc
    {
        public static void Start()
        {
            Elmah.Mvc.Bootstrap.Initialize();
        }
    }
}