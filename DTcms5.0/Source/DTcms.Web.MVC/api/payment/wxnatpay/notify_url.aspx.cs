using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.API.Payment.wxpay;
using DTcms.Common;

namespace DTcms.Web.api.payment.wxnatpay
{
    public partial class notify_url : System.Web.UI.Page
    {
        int site_payment_id = 0; //站点支付方式ID

        protected void Page_Load(object sender, EventArgs e)
        {
            WxPayData requestData = new WxPayData();
            requestData.GetRequest(); //获取微信传过来的参数
            if (!requestData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Response.Write(res.ToXml());
                return;
            }
            //获取订单信息
            string transaction_id = requestData.GetValue("transaction_id").ToString(); //微信支付订单号
            string order_no = requestData.GetValue("out_trade_no").ToString().ToLower(); //商户订单号
            string total_fee = requestData.GetValue("total_fee").ToString(); //获取总金额
            if (order_no.StartsWith("R")) //充值订单
            {
                site_payment_id = new BLL.user_recharge().GetPaymentId(order_no);
            }
            else if (order_no.StartsWith("B")) //商品订单
            {
                site_payment_id = new BLL.orders().GetPaymentId(order_no);
            }
            if (site_payment_id == 0)
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单不存在或已删除");
                Response.Write(res.ToXml());
                return;
            }

            //开始验证订单
            WxPayData notifyData = NativePay.GetNotifyData(site_payment_id);
            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Response.Write(res.ToXml());
                return;
            }

            if (order_no.StartsWith("R")) //充值订单
            {
                BLL.user_recharge bll = new BLL.user_recharge();
                Model.user_recharge model = bll.GetModel(order_no);
                if (model == null)
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "该订单号不存在");
                    Response.Write(res.ToXml());
                    return;
                }
                if (model.status == 1) //已成功
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    Response.Write(res.ToXml());
                    return;
                }
                if (model.amount != (decimal.Parse(total_fee) / 100))
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单金额和支付金额不相符");
                    Response.Write(res.ToXml());
                    return;
                }
                bool result = bll.Confirm(order_no);
                if (!result)
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "修改订单状态失败");
                    Response.Write(res.ToXml());
                    return;
                }
            }
            else if (order_no.StartsWith("B")) //商品订单
            {
                BLL.orders bll = new BLL.orders();
                Model.orders model = bll.GetModel(order_no);
                if (model == null)
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "该订单号不存在");
                    Response.Write(res.ToXml());
                    return;
                }
                if (model.payment_status == 2) //已付款
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "SUCCESS");
                    res.SetValue("return_msg", "OK");
                    Response.Write(res.ToXml());
                    return;
                }
                if (model.order_amount != (decimal.Parse(total_fee) / 100))
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "订单金额和支付金额不相符");
                    Response.Write(res.ToXml());
                    return;
                }
                bool result = bll.UpdateField(order_no, "trade_no='" + transaction_id + "',status=2,payment_status=2,payment_time='" + DateTime.Now + "'");
                if (!result)
                {
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", "修改订单状态失败");
                    Response.Write(res.ToXml());
                    return;
                }
                //扣除积分
                if (model.point < 0)
                {
                    new BLL.user_point_log().Add(model.user_id, model.user_name, model.point, "换购扣除积分，订单号：" + model.order_no, false);
                }
            }

            //返回成功通知
            WxPayData res1 = new WxPayData();
            res1.SetValue("return_code", "SUCCESS");
            res1.SetValue("return_msg", "OK");
            Response.Write(res1.ToXml());
            return;
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = NativePay.OrderQuery(site_payment_id, req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}