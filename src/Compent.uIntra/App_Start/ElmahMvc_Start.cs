[assembly: WebActivator.PreApplicationStartMethod(typeof(Compent.Uintra.App_Start.ElmahMvc), "Start")]
namespace Compent.Uintra.App_Start
{
    public class ElmahMvc
    {
        public static void Start()
        {
            Elmah.Mvc.Bootstrap.Initialize();
        }
    }
}