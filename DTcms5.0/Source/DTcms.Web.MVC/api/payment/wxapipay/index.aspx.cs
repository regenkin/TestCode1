using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.API.Payment.wxpay;
using DTcms.Common;

namespace DTcms.Web.api.payment.wxapipay
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //获得订单信息
            int site_payment_id = 0; //订单支付方式
            string order_no = DTRequest.GetFormString("pay_order_no").ToUpper();
            decimal order_amount = DTRequest.GetFormDecimal("pay_order_amount", 0);

            //检查参数是否正确
            if (string.IsNullOrEmpty(order_no) || order_amount == 0)
            {
               Response.Redirect(new Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您提交的参数有误！")));
                return;
            }
            //===============================判断订单==================================
            if (order_no.StartsWith("R")) //R开头为在线充值订单
            {
                Model.user_recharge model = new BLL.user_recharge().GetModel(order_no);
                if (model == null)
                {
                   Response.Redirect(new Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您充值的订单号不存在或已删除！")));
                    return;
                }
                if (model.amount != order_amount)
                {
                   Response.Redirect(new Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您充值的订单金额与实际金额不一致！")));
                    return;
                }
                site_payment_id = model.payment_id; //站点支付方式ID
            }
            else //B开头为商品订单
            {
                Model.orders model = new BLL.orders().GetModel(order_no);
                if (model == null)
                {
                   Response.Redirect(new Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您支付的订单号不存在或已删除！")));
                    return;
                }
                if (model.order_amount != order_amount)
                {
                   Response.Redirect(new Web.MVC.UI.Controllers.BaseController().linkurl("error", "?msg=" + Utils.UrlEncode("对不起，您支付的订单金额与实际金额不一致！")));
                    return;
                }
                site_payment_id = model.payment_id; //站点支付方式ID
            }

            //调用【网页授权获取用户信息】接口获取用户的openid和access_token
            JsApiConfig jsApiConfig = new JsApiConfig(site_payment_id);
            WxPayData data = new WxPayData();
            data.SetValue("appid", jsApiConfig.AppId);
            data.SetValue("redirect_uri", HttpUtility.UrlEncode(jsApiConfig.Redirect_url));
            data.SetValue("response_type", "code");
            data.SetValue("scope", "snsapi_base");
            data.SetValue("state", order_no + "#wechat_redirect"); //传入订单号
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
            try
            {
                //触发微信返回code码         
                Response.Redirect(url);//Redirect函数会抛出ThreadAbortException异常，不用处理这个异常
            }
            catch (System.Threading.ThreadAbortException ex)
            {

            }
        }
    }
}