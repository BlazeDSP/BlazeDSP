namespace LanderTest
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/style/css").Include("~/Content/site.css"));
        }
    }
}