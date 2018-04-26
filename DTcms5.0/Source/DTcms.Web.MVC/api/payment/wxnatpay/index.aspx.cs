using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.API.Payment.wxpay;
using DTcms.Common;

namespace DTcms.Web.api.payment.wxnatpay {
   public partial class index : System.Web.UI.Page {
      protected string order_no = string.Empty;
      protected void Page_Load(object sender, EventArgs e) {
         //读取站点配置信息
         Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig();
         int site_payment_id = 0; //订单支付方式

         //=============================获得订单信息================================
         order_no = DTRequest.GetFormString("pay_order_no").ToUpper();
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
            if (model.status == 1) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("payment", "?action=succeed&order_no=" + order_no));
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
            if (model.payment_status == 2) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("payment", "?action=succeed&order_no=" + order_no));
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

         //===========================调用统一下单接口==============================
         string sendUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
         NativeConfig nativeConfig = new NativeConfig(site_payment_id);
         WxPayData data = new WxPayData();
         data.SetValue("body", user_name); //商品描述
         data.SetValue("detail", sysConfig.webname + "-" + subject); //商品详情
         data.SetValue("out_trade_no", order_no); //商户订单号
         data.SetValue("total_fee", (Convert.ToDouble(order_amount) * 100).ToString()); //订单总金额,以分为单位
         data.SetValue("trade_type", "NATIVE");//交易类型
         data.SetValue("product_id", order_no);//商品ID
         data.SetValue("notify_url", nativeConfig.Notify_url); //异步通知url
         data.SetValue("spbill_create_ip", DTRequest.GetIP()); //终端IP
         data.SetValue("appid", nativeConfig.AppId); //公众账号ID
         data.SetValue("mch_id", nativeConfig.Partner); //商户号
         data.SetValue("nonce_str", NativePay.GenerateNonceStr()); //随机字符串
         data.SetValue("sign", data.MakeSign(nativeConfig.Key)); //签名
         string xml = data.ToXml(); //转换成XML
         var startTime = DateTime.Now; //开始时间
         string response = HttpService.Post(xml, sendUrl, false, 6); //发送请求
         var endTime = DateTime.Now; //结束时间
         int timeCost = (int)((endTime - startTime).TotalMilliseconds); //计算所用时间
         WxPayData result = new WxPayData();
         result.FromXml(response, nativeConfig.Key);
         NativePay.ReportCostTime(site_payment_id, sendUrl, timeCost, result); //测速上报

         string codeUrl = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
         imgQRCode.ImageUrl = "qrcode.aspx?data=" + HttpUtility.UrlEncode(codeUrl);
         litText.Text = order_amount.ToString("#0.00");
      }
   }
}