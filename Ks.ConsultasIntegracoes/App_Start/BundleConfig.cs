
using System.Web.Optimization;

namespace Ks.ConsultasIntegracoes
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-2.1.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include("~/Scripts/app/inspinia.js"));
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include("~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/metsiMenu").Include("~/Scripts/plugins/metisMenu/metisMenu.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/pace").Include("~/Scripts/plugins/pace/pace.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/mask").Include("~/Scripts/plugins/Mask/jquery.mask.js"));
            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include("~/Scripts/DataTables/datatables.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/flot").Include("~/Scripts/plugins/flot/jquery.flot.js", "~/Scripts/plugins/flot/jquery.flot.tooltip.min.js", "~/Scripts/plugins/flot/jquery.flot.resize.js", "~/Scripts/plugins/flot/jquery.flot.pie.js", "~/Scripts/plugins/flot/jquery.flot.time.js", "~/Scripts/plugins/flot/jquery.flot.spline.js"));
            bundles.Add(new ScriptBundle("~/plugins/vectorMap").Include("~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js", "~/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));
            bundles.Add(new ScriptBundle("~/plugins/wizardSteps").Include("~/Scripts/plugins/staps/jquery.steps.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/iCheck").Include("~/Scripts/plugins/iCheck/icheck.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/validate").Include("~/Scripts/plugins/validate/jquery.validate.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/dataPicker").Include("~/Scripts/plugins/datapicker/bootstrap-datepicker.js"));
            bundles.Add(new ScriptBundle("~/plugins/slick").Include("~/Scripts/plugins/slick/slick.min.js"));
            bundles.Add(new StyleBundle("~/Content/DataTables").Include("~/Scripts/DataTables/datatables.min.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.min.css", "~/Content/animate.css", "~/Content/style.css"));
            bundles.Add(new StyleBundle("~/font-awesome/css").Include("~/fonts/font-awesome/css/font-awesome.min.css"));
            bundles.Add(new StyleBundle("~/Content/plugins/iCheck/iCheckStyles").Include("~/Content/plugins/iCheck/custom.css"));
            bundles.Add(new StyleBundle("~/plugins/wizardStepsStyles").Include("~/Content/plugins/steps/jquery.steps.css"));
            bundles.Add(new StyleBundle("~/plugins/dataPickerStyles").Include("~/Content/plugins/datapicker/datepicker3.css"));
            bundles.Add(new StyleBundle("~/plugins/slickStyles").Include("~/Content/plugins/slick/slick.css", (IItemTransform)new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/plugins/slickThemeStyles").Include("~/Content/plugins/slick/slick-theme.css", (IItemTransform)new CssRewriteUrlTransform()));
        }
    }
}
