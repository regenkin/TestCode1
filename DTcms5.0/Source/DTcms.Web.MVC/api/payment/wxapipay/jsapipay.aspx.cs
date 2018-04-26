using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.API.Payment.wxpay;
using DTcms.Common;

namespace DTcms.Web.api.payment.wxapipay {
   public partial class jsapipay : System.Web.UI.Page {
      public static string wxJsApiParam { get; set; } //H5调起JS API参数
      public static string returnUrl { get; set; } //支付成功跳转的地址
      protected void Page_Load(object sender, EventArgs e) {
         int site_payment_id = 0; //订单支付方式
         string openid = DTRequest.GetQueryString("openid");
         string order_no = DTRequest.GetQueryString("order_no").ToUpper();
         decimal order_amount = 0; //订单金额
         string subject1 = string.Empty; //订单备注1
         string subject2 = string.Empty; //订单备注2

         //检查参数是否正确
         if (string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(order_no)) {
            Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，获取OPENID参数有误！")));
            return;
         }
         if (order_no.StartsWith("R")) //R开头为在线充值订单
            {
            Model.user_recharge model = new BLL.user_recharge().GetModel(order_no);
            if (model == null) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您充值的订单号不存在或已删除！")));
               return;
            }
            order_amount = model.amount; //订单金额
            subject1 = "充值订单";
            subject2 = "用户名：" + model.user_name;
            site_payment_id = model.payment_id; //站点支付方式ID
         }
         else //B开头为商品订单
            {
            Model.orders model = new BLL.orders().GetModel(order_no);
            if (model == null) {
               Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您支付的订单号不存在或已删除！")));
               return;
            }
            order_amount = model.order_amount; //订单金额
            subject1 = "商品订单";
            if (model.user_id > 0) {
               subject2 = "用户名：" + model.user_name;
            }
            else {
               subject2 = "匿名用户";
            }
            site_payment_id = model.payment_id; //站点支付方式ID
         }

         //JSAPI支付预处理
         try {
            //统一下单
            string sendUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            JsApiConfig jsApiConfig = new JsApiConfig(site_payment_id);
            WxPayData data = new WxPayData();
            data.SetValue("body", subject1); //商品描述
            data.SetValue("detail", subject2); //商品详情
            data.SetValue("out_trade_no", order_no); //商户订单号
            data.SetValue("total_fee", (Convert.ToDouble(order_amount) * 100).ToString()); //订单总金额,以分为单位
            data.SetValue("trade_type", "JSAPI"); //交易类型
            data.SetValue("openid", openid); //公众账号ID
            data.SetValue("appid", jsApiConfig.AppId); //公众账号ID
            data.SetValue("mch_id", jsApiConfig.Partner); //商户号
            data.SetValue("nonce_str", JsApiPay.GenerateNonceStr()); //随机字符串
            data.SetValue("notify_url", jsApiConfig.Notify_url); //异步通知url
            data.SetValue("spbill_create_ip", DTRequest.GetIP()); //终端IP
            data.SetValue("sign", data.MakeSign(jsApiConfig.Key)); //签名
            string xml = data.ToXml(); //转换成XML
            var startTime = DateTime.Now; //开始时间
            string response = HttpService.Post(xml, sendUrl, false, 6); //发送请求
            var endTime = DateTime.Now; //结束时间
            int timeCost = (int)((endTime - startTime).TotalMilliseconds); //计算所用时间
            WxPayData result = new WxPayData();
            result.FromXml(response, jsApiConfig.Key);
            JsApiPay.ReportCostTime(site_payment_id, sendUrl, timeCost, result); //测速上报

            //获取H5调起JS API参数
            WxPayData jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", result.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", JsApiPay.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", JsApiPay.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + result.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign(jsApiConfig.Key));
            wxJsApiParam = jsApiParam.ToJson();

            //支付成功后跳转的URL
            returnUrl = new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("payment", "?action=succeed&order_no=" + order_no);
         }
         catch (Exception ex) {
            Response.Redirect(new DTcms.Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("调用JSAPI下单失败，请检查微信授权目录是否已注册！")));
            return;
         }

      }
   }
}