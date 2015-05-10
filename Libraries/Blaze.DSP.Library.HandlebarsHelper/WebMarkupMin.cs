//using System.Text;

//using WebMarkupMin.Core;
//using WebMarkupMin.Core.Loggers;
//using WebMarkupMin.Core.Minifiers;
//using WebMarkupMin.Core.Settings;

//namespace HandlebarsHelper
//{
//    public static class WebMarkupMin
//    {
//        public static string Minimize(string html)
//        {
//            // TODO: Update to read from config file
//            var settings = new HtmlMinificationSettings
//            {
//                AttributeQuotesRemovalMode = HtmlAttributeQuotesRemovalMode.Html5,
//                CollapseBooleanAttributes = true,
//                CustomAngularDirectiveList = string.Empty,
//                EmptyTagRenderMode = HtmlEmptyTagRenderMode.NoSlash,
//                MinifyAngularBindingExpressions = false,
//                MinifyEmbeddedCssCode = true,
//                MinifyEmbeddedJsCode = true,
//                MinifyInlineCssCode = true,
//                MinifyInlineJsCode = true,
//                MinifyKnockoutBindingExpressions = false,
//                ProcessableScriptTypeList = string.Empty,
//                RemoveCdataSectionsFromScriptsAndStyles = true,
//                RemoveCssTypeAttributes = true,
//                RemoveEmptyAttributes = false, //
//                RemoveHtmlComments = true,
//                RemoveHtmlCommentsFromScriptsAndStyles = true,
//                RemoveHttpProtocolFromAttributes = true,
//                RemoveHttpsProtocolFromAttributes = true,
//                RemoveJsProtocolFromAttributes = true,
//                RemoveJsTypeAttributes = true,
//                RemoveOptionalEndTags = true,
//                RemoveRedundantAttributes = true,
//                RemoveTagsWithoutContent = false, //
//                UseMetaCharsetTag = true,
//                UseShortDoctype = true,
//                WhitespaceMinificationMode = WhitespaceMinificationMode.Safe
//            };

//            //var cssMinifier = new KristensenCssMinifier();
//            var cssMinifier = new NullCssMinifier();

//            //var jsMinifier = new CrockfordJsMinifier();
//            var jsMinifier = new NullJsMinifier();

//            var logger = new NullLogger();

//            var htmlMinifier = new HtmlMinifier(settings, cssMinifier, jsMinifier, logger);

//            var result = htmlMinifier.Minify(html, string.Empty, Encoding.Default, false);

//            // BUG: Throws errors on {{}} (shouldn't but does)

//            return result.MinifiedContent;

//            //if (result.Errors.Count == 0)
//            //{
//            //    //Console.WriteLine("Minified content:{0}{0}{1}", Environment.NewLine, result.MinifiedContent);
//            //}
//            //else
//            //{
//            //    //IList<MinificationErrorInfo> errors = result.Errors;

//            //    //Console.WriteLine("Found {0:N0} error(s):", errors.Count);
//            //    //Console.WriteLine();

//            //    //foreach (var error in errors)
//            //    //{
//            //    //    Console.WriteLine("Line {0}, Column {1}: {2}", error.LineNumber, error.ColumnNumber, error.Message);
//            //    //    Console.WriteLine();
//            //    //}
//            //}
//        }
//    }
//}