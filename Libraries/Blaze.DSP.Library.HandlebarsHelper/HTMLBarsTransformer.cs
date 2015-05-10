//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Web;
//using System.Web.Optimization;

//using HandlebarsHelper;

//using Yahoo.Yui.Compressor;

//namespace HandlebarsHelper
//{
//    // ReSharper disable once InconsistentNaming
//    public class HTMLBarsTransformer : IBundleTransform
//    {
//        private readonly ITemplateNamer _namer;

//        public HTMLBarsTransformer()
//        {
//            _namer = new TemplateNamer();
//        }

//        public HTMLBarsTransformer(ITemplateNamer templateNamer)
//        {
//            _namer = templateNamer;
//        }

//        public void Process(BundleContext context, BundleResponse response)
//        {
//            var compiler = new HTMLBarsCompiler();

//            var templates = new Dictionary<string, string>();

//            var server = context.HttpContext.Server;

//            foreach (var bundleFile in response.Files)
//            {
//                var filePath = server.MapPath(bundleFile.VirtualFile.VirtualPath);

//                var bundleRelativePath = GetRelativePath(server, bundleFile, filePath);

//                var templateName = _namer.GenerateName(bundleRelativePath, bundleFile.VirtualFile.Name);

//                var template = File.ReadAllText(filePath);

//                var minz = WebMarkupMin.Minimize(template);

//                var compiled = compiler.Precompile(minz, false);

//                templates[templateName] = compiled;
//            }

//            var javascript = new StringBuilder();

//            foreach (var templateName in templates.Keys)
//            {
//                javascript.AppendFormat("Ember.TEMPLATES['{0}']=", templateName);

//                javascript.AppendFormat("Ember.HTMLBars.template({0});", templates[templateName]);
//            }

//            var compressor = new JavaScriptCompressor();

//            var compressed = compressor.Compress(javascript.ToString());

//            response.ContentType = "text/javascript";

//            response.Cacheability = HttpCacheability.Public;

//            response.Content = compressed;
//        }

//        private static string GetRelativePath(HttpServerUtilityBase server, BundleFile bundleFile, string filePath)
//        {
//            var relativeBundlePath = bundleFile.IncludedVirtualPath.Remove(bundleFile.IncludedVirtualPath.IndexOf(@"\", StringComparison.Ordinal));

//            var bundlePath = server.MapPath(relativeBundlePath);

//            return FileToolkit.PathDifference(filePath, bundlePath);
//        }
//    }
//}