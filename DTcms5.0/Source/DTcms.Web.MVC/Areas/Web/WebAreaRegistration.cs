using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Web {
   public class WebAreaRegistration : AreaRegistration {
      public override string AreaName {
         get {
            return "Web";
         }
      }

      public override void RegisterArea(AreaRegistrationContext context) {
         context.MapRoute(
             "Web_default",
             "Web/{controller}/{action}",
             defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
         );
      }
   }
}
