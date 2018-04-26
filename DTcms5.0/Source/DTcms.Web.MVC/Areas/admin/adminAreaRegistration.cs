using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.admin {
   public class adminAreaRegistration : AreaRegistration {
      public override string AreaName {
         get {
            return "admin";
         }
      }

      public override void RegisterArea(AreaRegistrationContext context) {
         context.MapRoute(
             "admin_default",
             "admin/{controller}/{action}",
             new { controller = "index", action = "index" }
         );
         context.MapRoute(
           "admin_default1",
           "admin/{arear}/{controller}/{action}",
           new {controller = "index", action = "index" }
        );
      }
   }
}
