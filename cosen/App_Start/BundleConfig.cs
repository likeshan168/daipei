using System.Web;
using System.Web.Optimization;

namespace cosen
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            #region 我定义的加载js文件
            //cosen da pei
            bundles.Add(new ScriptBundle("~/bundles/dp").Include(
                "~/Scripts/jquery.autocomplete.pack.js",
                "~/static/myjs/form.js",
                "~/static/myjs/dp.js"
                ));
            //cosen's upload img
            bundles.Add(new ScriptBundle("~/bundles/uploadimg").Include(
                //"~/static/fileupload/js/jquery.2_1_0.js",
                "~/static/fileupload/js/vendor/jquery.ui.widget.js",
                "~/static/fileupload/js/tmp.js",
                "~/static/fileupload/js/load-image.js",
                "~/static/fileupload/js/canvas-to-blob.js",
                "~/static/fileupload/js/bootstrap.js",
                "~/static/fileupload/js/jquery.blueimp-gallery.js",
                "~/static/fileupload/js/jquery.iframe-transport.js",
                "~/static/fileupload/js/jquery.fileupload.js",
                "~/static/fileupload/js/jquery.fileupload-process.js",
                "~/static/fileupload/js/jquery.fileupload-image.js",
                "~/static/fileupload/js/jquery.fileupload-audio.js",
                "~/static/fileupload/js/jquery.fileupload-video.js",
                "~/static/fileupload/js/jquery.fileupload-validate.js",
                "~/static/fileupload/js/jquery.fileupload-ui.js",
                "~/static/fileupload/js/main.js",
                "~/static/fileupload/js/myimgupload.js"
                ));
            //cosen's tjdp
            bundles.Add(new ScriptBundle("~/bundles/tjdp").Include(
                "~/static/fileupload/js/bootstrap.js",
                "~/static/myjs/tjdp.js"
                
                ));
            //cosen's alldp
            bundles.Add(new ScriptBundle("~/bundles/alltj").Include(
                "~/static/fileupload/js/bootstrap.js",
                "~/static/myjs/alltj.js"
                ));
            //cosen'test
            bundles.Add(new ScriptBundle("~/bundles/test").Include(
                "~/Scripts/test.js"
                ));
            //knockout js
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-3.1.0.js"
                ));
            #endregion


            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

        }
    }
}