using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.API.Payment.wxpay;
using DTcms.Common;

namespace DTcms.Web.api.payment.wxapipay
{
    public partial class redirect_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int site_payment_id = 0; //订单支付方式
            //获取code码，以获取openid和access_token
            string code = DTRequest.GetQueryString("code");
            //获取传过来的订单号
            string order_no = DTRequest.GetQueryString("state").ToUpper();

            //检查参数是否正确
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(order_no))
            {
                Response.Redirect(new Web.UI.BasePage().linkurl("error", "?msg=" + Utils.UrlEncode("出错了，获取CODE回调参数有误！")));
                return;
            }
            if (order_no.StartsWith("R")) //充值订单
            {
                site_payment_id = new BLL.user_recharge().GetPaymentId(order_no);
            }
            else if (order_no.StartsWith("B")) //商品订单
            {
                site_payment_id = new BLL.orders().GetPaymentId(order_no);
            }

            //获取openid及access_token的url
            try
            {
                JsApiConfig jsApiConfig = new JsApiConfig(site_payment_id);
                WxPayData data = new WxPayData();
                data.SetValue("appid", jsApiConfig.AppId);
                data.SetValue("secret", jsApiConfig.AppSecret);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();

                //请求url以获取数据
                string result = HttpService.Get(url);
                //获取用户openid
                Dictionary<string, object> dic = JsonHelper.DataRowFromJSON(result);
                string openid = (string)dic["openid"];
                //跳转转值到处理页面
                Response.Redirect("jsapipay.aspx?openid=" + openid + "&order_no=" + order_no);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}