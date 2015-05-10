namespace LanderTest.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    public class ConversionController : Controller
    {
        public ActionResult Index(int fid, decimal? com = null)
        {
            using (var wc = new WebClient())
            {
                wc.DownloadString(new Uri(string.Format("http://127.0.0.3:81/postback?fid={0}&com={1}", fid, com)));
            }

            return View();
        }
    }
}