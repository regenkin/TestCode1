using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Senparc.Weixin.MP.Entities.Menu;
using DTcms.API.Weixin.Common;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin
{
    public class Menu_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
        private const string WEB_VIEW = "~/Areas/admin/Views/weixin/menu_edit.cshtml";

        CRMComm cpp = new CRMComm(); //获取AccessToKen类
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0; //公众账户ID

        private Senparc.Weixin.MP.Entities.GetMenuResult menuResult;
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            ChkAdminLevel("weixin_custom_menu", DTEnums.ActionEnum.View.ToString()); //检查权限
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ActionResult result = View(EDIT_RESULT_VIEW);
            string _action = DTRequest.GetQueryString("action");

            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    filterContext.Result = result;
                    return;
                }
                if (!new BLL.weixin_account().Exists(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", "back");
                    filterContext.Result = result;
                    return;
                }
            }
            ViewBag.Id = this.id.ToString();
            ViewBag.This = this;
        }

        //
        // GET: /admin/Menu_Edit/
        public ActionResult Index()
        {
            TreeBind(); //绑定公众账户
            if (action == DTEnums.ActionEnum.Edit.ToString())
            {
                ShowInfo(this.id);
            }
            return View(WEB_VIEW);
        }

        public ActionResult SubmitSave()
        {
            ActionResult result = View(EDIT_RESULT_VIEW);
            try
            {
                string error = string.Empty;
                string accessToken = cpp.GetAccessToken(this.id, out error);

                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.ClientScript = JscriptMsg(error, "back");
                    return result;
                }

                //重新整理按钮信息
                ButtonGroup bg = new ButtonGroup();
                string txtName = string.Empty;
                string txtKey = string.Empty;
                string txtUrl = string.Empty;
                IList<BaseButton> topList = new List<BaseButton>();
                IList<SingleButton> subList = new List<SingleButton>();
                //菜单设置
                for (int i = 0; i < 3; i++)
                {
                    txtName = Request.Form["txtTop" + (i + 1) + "Name"];
                    txtKey = Request.Form["txtTop" + (i + 1) + "Key"];
                    txtUrl = Request.Form["txtTop" + (i + 1) + "Url"];
                    if (txtName.Trim() == "")
                    {
                        // 如果名称为空，则忽略该菜单，以及下面的子菜单
                        continue;
                    }

                    subList = new List<SingleButton>();
                    string txtSubName = string.Empty;
                    string txtSubKey = string.Empty;
                    string txtSubUrl = string.Empty;
                    for (int j = 0; j < 5; j++)
                    {
                        //子菜单的设置
                        txtSubName = Request.Form["txtMenu" + (i + 1) + (j + 1) + "Name"];
                        txtSubKey = Request.Form["txtMenu" + (i + 1) + (j + 1) + "Key"];
                        txtSubUrl = Request.Form["txtMenu" + (i + 1) + (j + 1) + "Url"];
                        if (txtSubName.Trim() == "")
                        {
                            continue;
                        }

                        if (txtSubUrl.Trim() != "")
                        {
                            SingleViewButton sub = new SingleViewButton();
                            sub.name = txtSubName.Trim();
                            sub.url = txtSubUrl.Trim();
                            subList.Add(sub);
                        }
                        else if (txtSubKey.Trim() != "")
                        {
                            SingleClickButton sub = new SingleClickButton();
                            sub.name = txtSubName.Trim();
                            sub.key = txtSubKey.Trim();
                            subList.Add(sub);
                        }
                        else
                        {
                            //报错 :子菜单必须有key和name
                            ViewBag.ClientScript = JscriptMsg("二级菜单的名称和key或者url必填！", "back");
                            return result;
                        }
                    }

                    if (subList != null && subList.Count > 0)
                    {
                        //有子菜单, 该一级菜单是SubButton
                        if (subList.Count < 1)
                        {
                            ViewBag.ClientScript = JscriptMsg("子菜单个数必须为2至5个！", "back");
                            return result;
                        }
                        SubButton topButton = new SubButton(Utils.CutString(txtName.Trim(), 16));
                        topButton.sub_button.AddRange(subList);
                        topList.Add(topButton);
                    }
                    else
                    {
                        // 无子菜单
                        if (txtKey.Trim() == "" && txtUrl.Trim() == "")
                        {
                            ViewBag.ClientScript = JscriptMsg("如无子菜单，必须填写Key或者URL值！", "back");
                            return result;
                        }

                        if (txtUrl.Trim() != "")
                        {  //view 页面跳转
                            SingleViewButton topSingleButton = new SingleViewButton();
                            topSingleButton.name = txtName.Trim();
                            topSingleButton.url = txtUrl.Trim();
                            topList.Add(topSingleButton);
                        }
                        else if (txtKey.Trim() != "")
                        {
                            SingleClickButton topSingleButton = new SingleClickButton();
                            topSingleButton.name = txtName.Trim();
                            topSingleButton.key = txtKey.Trim();
                            topList.Add(topSingleButton);
                        }
                    }
                }

                bg.button.AddRange(topList);
                Senparc.Weixin.Entities.WxJsonResult jsonResult = Senparc.Weixin.MP.CommonAPIs.CommonApi.CreateMenu(accessToken, bg);
                JscriptMsg("自定义菜单保存成功！", "../menu_list/index");
            }
            catch (Exception ex)
            {
                JscriptMsg("出错了：" + ex.Message, "back");
            }
            return result;
        }

        #region 绑定公众账户=============================
        private void TreeBind()
        {
            BLL.weixin_account bll = new BLL.weixin_account();
            DataTable dt = bll.GetList(0, "", "sort_id asc,id desc").Tables[0];
            List<SelectListItem> weixinListItems = new List<SelectListItem>() {
            new SelectListItem(){ Text="请选择公众账户", Value=""}
         };
            foreach (DataRow dr in dt.Rows)
            {
                weixinListItems.Add(new SelectListItem() { Text = dr["name"].ToString(), Value = dr["id"].ToString() });
            }
            ViewData["weixinListItems"] = weixinListItems;
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int account_id)
        {
            GetMenu(account_id);
        }
        #endregion

        #region 获取最新菜单=============================
        private void GetMenu(int account_id)
        {
            try
            {
                string error = string.Empty;
                string accessToken = cpp.GetAccessToken(account_id, out error);

                if (!string.IsNullOrEmpty(error))
                {
                    JscriptMsg(error, string.Empty);
                    return;
                }
                Senparc.Weixin.MP.Entities.GetMenuResult result = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetMenu(accessToken);
                if (result == null)
                {
                    return;
                }
                menuResult = result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 获取指定菜单项
        /// </summary>
        /// <param name="rootCount">主菜单序号(1-3)</param>
        /// <param name="subCount">子菜单序号(1-5),0 = 选择主菜单项</param>
        /// <returns></returns>
        public DTcms.Model.WeixinMunu GetMenu(int rootCount, int subCount)
        {
            DTcms.Model.WeixinMunu result = new DTcms.Model.WeixinMunu();
            //菜单序号减一, 转换为0起始
            rootCount--;
            subCount--;

            if (menuResult != null && menuResult.menu != null && menuResult.menu.button != null)
            {
                List<BaseButton> topButtonList = menuResult.menu.button;
                if (subCount < 0)
                {
                    //获取主菜单
                    if (topButtonList.Count > rootCount)
                    {
                        result.name = topButtonList[rootCount].name;
                        if (topButtonList[rootCount].GetType() == typeof(SingleViewButton))
                        {
                            //下面无子菜单
                            result.url = ((SingleViewButton)topButtonList[rootCount]).url;
                        }
                    }
                }
                else
                {
                    //获取子菜单,判断主菜单是否存在并且下面有子菜单
                    if (topButtonList.Count > rootCount && topButtonList[rootCount].GetType() != typeof(SingleViewButton))
                    {
                        IList<SingleButton> subButtonList = ((SubButton)topButtonList[rootCount]).sub_button;
                        if (subButtonList.Count > subCount)
                        {
                            if (subButtonList[subCount].GetType() == typeof(SingleViewButton))
                            {
                                SingleViewButton sub = (SingleViewButton)subButtonList[subCount];
                                result.name = sub.name;
                                result.url = sub.url;
                            }
                            else
                            {
                                SingleClickButton sub = (SingleClickButton)subButtonList[subCount];
                                result.name = sub.name;
                                result.key = sub.key;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
