// The MIT License (MIT)
// 
// Copyright (c) 2015 Daniel Franklin. http://blazedsp.com/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace Frontend
{
    using System.Web.Optimization;

    using HandlebarsHelper;

    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /***************************************************************
                      _____   _____   _____    _   _____   _____   _____  
                    /  ___/ /  ___| |  _  \  | | |  _  \ |_   _| /  ___/ 
                    | |___  | |     | |_| |  | | | |_| |   | |   | |___  
                    \___  \ | |     |  _  /  | | |  ___/   | |   \___  \ 
                     ___| | | |___  | | \ \  | | | |       | |    ___| | 
                    /_____/ \_____| |_|  \_\ |_| |_|       |_|   /_____/ 
            
             ***************************************************************/

            Scripts.DefaultTagFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";

            // LAB
            bundles.Add(new ScriptBundle("~/js/lab")
                            .Include("~/Scripts/LAB.js"));

            // Plugins
            bundles.Add(new ScriptBundle("~/js/plugins")
                            .Include("~/Scripts/underscore.js")
                            .Include("~/Scripts/underscore.string.js")
                            .Include("~/Scripts/moment.js")
                            .Include("~/Scripts/dropzone.js")
                            .Include("~/Scripts/autosize.js")
#if DEBUG
                            .Include("~/Scripts/fakes/faker.js")
#endif
                            .Include("~/Scripts/numeral.js"));

            // QRCode JS
            bundles.Add(new ScriptBundle("~/js/qr")
                            //.Include("~/Scripts/qr.js")
                            .Include("~/Scripts/qrcode.js"));

            // jQuery
            bundles.Add(new ScriptBundle("~/js/jquery")
                            .Include("~/Scripts/jquery-{version}.js")
                            .Include("~/Scripts/jquery-ui-{version}.js")
                            // Plugins
                            .Include("~/Scripts/jquery.signalR-2.2.0.js")
                            // -
                            .Include("~/Scripts/jquery.uniform.js")
                            // -
                            .Include("~/Scripts/jquery-scrolltofixed.js")
                            // -
                            // TODO: Re-enable this...
                            //.Include("~/Scripts/gridstack.js")
                            // -
                            .Include("~/Scripts/jquery.blockUI.js")
                            .Include("~/App/misc/jquery.blockUI.config.js")
                            // -
                            .Include("~/Scripts/jquery.dataTables.js")
                            .Include("~/Scripts/dataTables.bootstrap.min.js")
                            // -
                            .Include("~/Scripts/jquery.slimscroll.js")
                            // -
                            .Include("~/Scripts/jquery.tagsinput.js")
                            // -
                            .Include("~/Scripts/bootstrap-datetimepicker.min.js")
                            // -
                            .Include("~/Scripts/daterangepicker.js")
                            // -
                            //.Include("~/Scripts/velocity.min.js") // Dependency for Liquid Fire
                            // -
                            .Include("~/Scripts/toastr.js")
                            .Include("~/App/misc/toastr.options.js"));

            bundles.Add(new ScriptBundle("~/js/jqueryval")
                            .Include("~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr")
            //                .Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/js/bootstrap")
                            .Include("~/Scripts/bootstrap.js")
                            .Include("~/Scripts/respond.js"));

            // EmberJS
            bundles.Add(new ScriptBundle("~/js/app")
                            // Framework
            #region Ember 1.9.1
                            .Include("~/Scripts/handlebars-v2.0.0.js")
                            .Include("~/Scripts/ember-v1.9.1.js")
            #endregion
            #region Ember 1.10.0
//#if DEBUG
//                            .Include("~/Scripts/ember-template-compiler.js")
//                            .Include("~/Scripts/ember.debug.js")
//#else
//                            .Include("~/Scripts/ember.prod.js")
//#endif
            #endregion
                            .Include("~/Scripts/ember-data.js")

                            // Helpers, Plugins, Mixins, etc
                            .Include("~/App/misc/helper-formatters.js")
                            .Include("~/App/misc/helper-menu-li.js")
                            .Include("~/App/misc/helper-tab.js")
                            .Include("~/App/misc/helper-node.js")
                            .Include("~/App/misc/helper-amchart.js")
                            // - Ember Animate
                            .Include("~/Scripts/ember-animate.js")
                            // - Liquid Fire
                            // TODO: Build liquid fire
                            // - Ember EasyForm
                            .Include("~/Scripts/ember-easyform/core.js")
                            .Include("~/Scripts/ember-easyform/config.js")
                            .Include("~/Scripts/ember-easyform/utilities.js")
                            .IncludeDirectory("~/Scripts/ember-easyform/helpers", "*.js")
                            .IncludeDirectory("~/Scripts/ember-easyform/templates", "*.js")
                            .IncludeDirectory("~/Scripts/ember-easyform/views", "*.js")
                            // - Ember EasyForm w/ Bootstrap v3, Cancel Button, rows for textareas
                            .Include("~/App/misc/ember.easyform.bootstrap3.js")
                            .Include("~/App/misc/ember.easyform.cancel-button.js")
                            .Include("~/App/misc/ember.easyform.extend.js")
                            // - Ember Validations
                            .Include("~/Scripts/ember-validations.js")

                            // App
                            .Include("~/App/app.js")
                            .Include("~/App/router.js")
                            .Include("~/App/transitions.js")
                            // Routes
                            .IncludeDirectory("~/App/routes", "*.js", true)
                            // Models
                            .IncludeDirectory("~/App/models", "*.js", true)
                            // Controller
                            .IncludeDirectory("~/App/controllers", "*.js", true)
                            // Views
                            .IncludeDirectory("~/App/views", "*.js", true));

                            // Templates
            #region Ember 1.9.1
            bundles.Add(new Bundle("~/js/templates", new HandlebarsTransformer())
            //bundles.Add(new Bundle("~/js/templates", new HtmlMinimizerHandlebarsTransformer())
                            .IncludeDirectory("~/App/templates", "*.hbs", true));
            #endregion
            #region Ember 1.10.0
            //bundles.Add(new Bundle("~/js/templates", new HTMLBarsTransformer())
            //                .IncludeDirectory("~/App/templates", "*.hbs", true));
            #endregion

            /***************************************************************
                _____   _____  __    __  _       _____   _____  
               /  ___/ |_   _| \ \  / / | |     | ____| /  ___/ 
               | |___    | |    \ \/ /  | |     | |__   | |___  
               \___  \   | |     \  /   | |     |  __|  \___  \ 
                ___| |   | |     / /    | |___  | |___   ___| | 
               /_____/   |_|    /_/     |_____| |_____| /_____/ 
            
           ***************************************************************/

            Styles.DefaultTagFormat = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";

            bundles.Add(new StyleBundle("~/style/css")
                            // GLOBAL MANDATORY STYLES
                            .Include("~/Content/bootstrap.css")
                            .Include("~/Content/font-awesome.css")
                            .Include("~/Content/icomoon.css")
                            .Include("~/Content/loading.css")
                            .Include("~/Content/uniform.default.css")
                            .Include("~/Content/toastr.css")
                            .Include("~/Content/dropzone.css")
                            .Include("~/Content/simple-line-icons.css")
                            .Include("~/Content/firelogo.css")
                            .Include("~/Content/dataTables.bootstrap.css")
                            .Include("~/Content/jquery.tagsinput.css")
                            .Include("~/Content/bootstrap-datetimepicker.css")
                            .Include("~/Content/daterangepicker-bs3.css")
                            // TODO: Re-enable this...
                            //.Include("~/Content/gridstack.css")
                            .Include("~/Content/sass/override-misc.css")
                            .Include("~/Content/sass/font-awesome-extend.css")
                            
                            // THEME STYLES (SASS)
                            .Include("~/Theme/sass/global/components-rounded.css")
                            .Include("~/Theme/sass/global/plugins.css")
                            .Include("~/Theme/sass/admin/layout4/layout.css")
                            .Include("~/Theme/sass/admin/layout4/themes/default.css")
                            .Include("~/Theme/sass/admin/pages/error.css")
                            .Include("~/Theme/sass/admin/pages/login.css")
                            // THEME PAGE STYLES (SASS)
                            .Include("~/Theme/sass/admin/pages/profile.css")
                            .Include("~/Theme/sass/admin/pages/tasks.css")
                            .Include("~/Theme/sass/admin/pages/todo.css")
                            .Include("~/Theme/sass/admin/pages/timeline.css")

                            // THEME EXTEND
                            .Include("~/Content/sass/override-theme.css")

                            // SITE
                            .Include("~/Content/sass/site.css"));

            // PAGE SPECIFIC STYLES
            bundles.Add(new StyleBundle("~/style/login")
                            .Include("~/Theme/sass/admin/pages/login.css"));

            /***************************************************************
              
                       |¯| ¯|¯ | | |¯ |¯|   |¯  ¯|¯ | | |¯ |¯ 
                       |_|  |  |¯| |¯ |¯\    ¯|  |  |_| |¯ |¯ 
                                    ¯        ¯                                     
             ***************************************************************/

//#if !DEBUG
//            BundleTable.EnableOptimizations = true;
//#endif

            BundleTable.EnableOptimizations = !System.Web.HttpContext.Current.IsDebuggingEnabled;
        }
    }
}