namespace NodeMVC.Controllers
{
    using System.Web;
    using System.Web.Mvc;

    public class UnloadAppDomainController : Controller
    {
        public ActionResult Index()
        {
            HttpRuntime.UnloadAppDomain();

            return Content(string.Empty);
        }
    }
}