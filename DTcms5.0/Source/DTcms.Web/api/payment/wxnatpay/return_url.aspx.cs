using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;

namespace DTcms.Web.api.payment.wxnatpay
{
    public partial class return_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string order_no = DTRequest.GetString("order_no").ToUpper();
            if (order_no.StartsWith("R")) //充值订单
            {
                Model.user_recharge model = new BLL.user_recharge().GetModel(order_no);
                if (model != null && model.status == 1)
                {
                    string resurl = new Web.UI.BasePage().linkurl("payment", "?action=succeed&order_no=" + order_no);
                    Response.Write("{\"status\": 1, \"url\": \"" + resurl + "\"}");
                    return;
                }
            }
            else if (order_no.StartsWith("B")) //商品订单
            {
                Model.orders model = new BLL.orders().GetModel(order_no);
                if (model != null && model.payment_status == 2)
                {
                    string resurl = new Web.UI.BasePage().linkurl("payment", "?action=succeed&order_no=" + order_no);
                    Response.Write("{\"status\": 1, \"url\": \"" + resurl + "\"}");
                    return;
                }
            }
            Response.Write("{\"status\": 0, \"msg\": \"订单未支付成功！\"}");
            return;
        }
    }
}