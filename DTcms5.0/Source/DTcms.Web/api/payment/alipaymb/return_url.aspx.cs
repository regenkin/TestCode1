using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using DTcms.API.Payment.alipaymb;

namespace DTcms.Web.api.payment.alipaymb
{
    public partial class return_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //读取站点配置信息
            Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig();
            int site_payment_id = 0; //站点支付方式ID
            Dictionary<string, string> sPara = GetRequestGet();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                string order_no = DTRequest.GetString("out_trade_no").ToUpper();//获取订单号
                string trade_no = DTRequest.GetString("trade_no"); //支付宝交易号
                string result = DTRequest.GetString("result"); //交易状态
                if (order_no.StartsWith("R")) //充值订单
                {
                    site_payment_id = new BLL.user_recharge().GetPaymentId(order_no);
                }
                else if (order_no.StartsWith("B")) //商品订单
                {
                    site_payment_id = new BLL.orders().GetPaymentId(order_no);
                }

                //找到站点支付方式ID开始验证
                if (site_payment_id > 0)
                {
                    Notify aliNotify = new Notify(site_payment_id);
                    bool verifyResult = aliNotify.VerifyReturn(sPara, Request.QueryString["sign"]);

                    if (verifyResult)//验证成功
                    {
                        if (result == "success")
                        {
                            //成功状态
                            Response.Redirect(new Web.UI.BasePage().linkurl("payment", "?action=succeed&order_no=" + order_no));
                            return;
                        }
                    }
                }
            }
            //失败状态
            Response.Redirect(new Web.UI.BasePage().linkurl("payment", "?action=error"));
            return;
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            coll = Request.QueryString;
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

    }
}