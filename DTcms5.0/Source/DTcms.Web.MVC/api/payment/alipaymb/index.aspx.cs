using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using DTcms.API.Payment.alipaymb;

namespace DTcms.Web.api.payment.alipaymb {
   public partial class index : System.Web.UI.Page {
      protected void Page_Load(object sender, EventArgs e) {
         //读取站点配置信息
         Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig();
         int site_payment_id = 0; //订单支付方式

         //=============================获得订单信息================================
         string order_no = DTRequest.GetFormString("pay_order_no").ToUpper();
         decimal order_amount = DTRequest.GetFormDecimal("pay_order_amount", 0);
         string user_name = DTRequest.GetFormString("pay_user_name");
         string subject = DTRequest.GetFormString("pay_subject");
         //检查参数是否正确
         if (string.IsNullOrEmpty(order_no) || order_amount == 0) {
            Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您提交的参数有误！")));
            return;
         }
         //===============================判断订单==================================
         if (order_no.StartsWith("R")) //R开头为在线充值订单
            {
            Model.user_recharge model = new BLL.user_recharge().GetModel(order_no);
            if (model == null) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您充值的订单号不存在或已删除！")));
               return;
            }
            if (model.amount != order_amount) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您充值的订单金额与实际金额不一致！")));
               return;
            }
            site_payment_id = model.payment_id; //站点支付方式ID
         }
         else //B开头为商品订单
            {
            Model.orders model = new BLL.orders().GetModel(order_no);
            if (model == null) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您支付的订单号不存在或已删除！")));
               return;
            }
            if (model.order_amount != order_amount) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您支付的订单金额与实际金额不一致！")));
               return;
            }
            site_payment_id = model.payment_id; //站点支付方式ID
         }
         if (user_name != "") {
            user_name = "支付会员：" + user_name;
         }
         else {
            user_name = "匿名用户";
         }
         //===============================建立请求==================================
         string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?"; //支付宝新网关地址
         Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
         Config config = new Config(site_payment_id);
         sParaTemp.Add("partner", config.Partner);
         sParaTemp.Add("seller_id", config.Partner);
         sParaTemp.Add("_input_charset", config.Input_charset.ToLower());
         sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
         sParaTemp.Add("payment_type", "1");
         sParaTemp.Add("notify_url", config.Notify_url);
         sParaTemp.Add("return_url", config.Return_url);
         sParaTemp.Add("out_trade_no", order_no);
         sParaTemp.Add("subject", sysConfig.webname + "-" + subject);
         sParaTemp.Add("total_fee", order_amount.ToString());
         sParaTemp.Add("show_url", sysConfig.weburl);
         sParaTemp.Add("app_pay", "Y");//启用此参数可唤起钱包APP支付。
         sParaTemp.Add("body", user_name);

         //建立请求
         Submit submit = new Submit(site_payment_id);
         string sHtmlText = submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");
         Response.Write(sHtmlText);

      }
   }
}