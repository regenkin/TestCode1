using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class UserOrder_ShowController : DTcms.Web.MVC.UI.Controllers.UserController {
      public int id;
      protected string expressdetail = string.Empty; //物流跟踪信息
      public DTcms.Model.orders model;
      public DTcms.Model.payment payModel;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         id = DTRequest.GetQueryInt("id");
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /Web/Article/

      public ActionResult Index() {
         DTcms.BLL.orders bll = new DTcms.BLL.orders();
         if (!bll.Exists(id)) {
            return Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错了，您要浏览的页面不存在或已删除！")));
         }
         model = bll.GetModel(id);
         if (model.user_id != userModel.id) {
            return Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错了，您所查看的并非自己的订单信息！")));
         }
         payModel = new DTcms.BLL.payment().GetModel(model.payment_id);
         if (payModel == null) {
            payModel = new DTcms.Model.payment();
         }
         //根据订单状态和物流单号跟踪物流信息
         if (model.status > 1 && model.status < 4 && model.express_status == 2 && model.express_no.Trim().Length > 0) {
            DTcms.Model.express modelt = new DTcms.BLL.express().GetModel(model.express_id);
            DTcms.Model.orderconfig orderConfig = new DTcms.BLL.orderconfig().loadConfig();
            if (modelt != null && modelt.express_code.Trim().Length > 0 && orderConfig.kuaidiapi != "") {
               string apiurl = orderConfig.kuaidiapi + "?id=" + orderConfig.kuaidikey + "&com=" + modelt.express_code + "&nu=" + model.express_no + "&show=" + orderConfig.kuaidishow + "&muti=" + orderConfig.kuaidimuti + "&order=" + orderConfig.kuaidiorder;
               string detail = Utils.HttpGet(@apiurl);
               if (detail != null) {
                  expressdetail = Utils.ToHtml(detail);
               }
            }
         }
         ViewData["model"] = model;
         ViewData["payModel"] = payModel;
         ViewData["expressdetail"] = expressdetail;
         ViewBag.This = this;
         return View(ViewName);
      }

   }
}
