using System.Web.Optimization;

namespace CableCo.Accounts.WebApp.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
	        BundleTable.EnableOptimizations = false;

			bundles.Add(new ScriptBundle("~/js/lib").Include(
                "~/scripts/jquery-1.*",
                "~/scripts/bootstrap.js",
                "~/scripts/underscore.js"
                ));

            bundles.Add(new ScriptBundle("~/js/app").Include(
                "~/scripts/shell.js"
                ));
            
            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/content/bootstrap.css",
                "~/content/form-signin.css"
                ));
            bundles.Add(new StyleBundle("~/content/css-responsive").Include(
                "~/content/bootstrap-responsive.css"
                ));
        }
    }
}