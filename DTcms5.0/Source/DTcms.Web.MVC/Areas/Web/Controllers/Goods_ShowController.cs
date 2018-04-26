﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class Goods_ShowController : DTcms.Web.MVC.UI.Controllers.Article_ShowController {
      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "goods";
      }

      //
      // GET: /Web/Goods_Show/
      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
