using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Article_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Article/article_edit.cshtml";
       private string error;
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       protected int channel_id;
       protected Model.site_channel channelModel; //频道实体
       private int id = 0;
       protected string prolistview = string.Empty;
       List<DTcms.Model.article_attribute_field> otherFieldList = null;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          this.channel_id = DTRequest.GetQueryInt("channel_id");
          if (this.channel_id == 0) {
             ViewBag.ClientScript = JscriptMsg("频道参数不正确！", "back");
             filterContext.Result = result;
             return;
          }
          this.channelModel = new BLL.site_channel().GetModel(this.channel_id); //取得频道实体
          CreateOtherField(this.channel_id); //动态生成相应的扩展字段

          action = DTRequest.GetQueryString("action");
          this.prolistview = DTRequest.GetQueryString("prolistview");
          if (this.prolistview != "") {
             Utils.WriteCookie("article_list_view", this.prolistview, 14400);
          }
          else {
             this.prolistview = Utils.GetCookie("article_list_view"); //显示方式
             if (this.prolistview == "")
                this.prolistview = "Txt";
          }

          //如果是编辑或复制则检查信息是否存在
          if (action == DTEnums.ActionEnum.Edit.ToString() || action == DTEnums.ActionEnum.Copy.ToString() || action == DTEnums.ActionEnum.View.ToString()) {
             this.id = DTRequest.GetQueryInt("id");
             if (this.id == 0) {
                JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result;
                return;
             }
             if (!new DTcms.BLL.article().Exists(this.channel_id, this.id)) {
                JscriptMsg("信息不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id.ToString();
          ViewBag.ChannelId = this.channel_id.ToString();
          ViewData["ChannelModel"] = this.channelModel;
          ViewBag.ProListView = this.prolistview;
       }

        //
        // GET: /admin/Article_Edit/
       public ActionResult Index() {
          ChkAdminLevel("channel_" + this.channelModel.name + "_list", DTEnums.ActionEnum.View.ToString()); //检查权限
          ShowSysField(this.channel_id); //显示相应的默认控件
          TreeBind(this.channel_id); //绑定类别
          if (action == DTEnums.ActionEnum.Edit.ToString()) {
             ShowInfo(this.id);
          }
          else {
             DTcms.Model.article model = new DTcms.Model.article();
             model.sort_id = 99;
             model.add_time = DateTime.Now;
             ViewData["model"] = model;
          }
          return View(WEB_VIEW);
       }

       [HttpPost, ValidateInput(false)]
       public ActionResult SubmitSave(string txtContent) {
          ActionResult result = View(EDIT_RESULT_VIEW);
          if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
             ChkAdminLevel("channel_" + this.channelModel.name + "_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
             if (!DoEdit(this.id, txtContent)) {
                JscriptMsg("保存过程中发生错误啦！", error);
                return result;
             }
             JscriptMsg("修改信息成功！", "../article_list/index?channel_id=" + this.channel_id);
          }
          else //添加
            {
             ChkAdminLevel("channel_" + this.channelModel.name + "_list", DTEnums.ActionEnum.Add.ToString()); //检查权限
             if (!DoAdd(txtContent)) {
                JscriptMsg("保存过程中发生错误！", error);
                return result;
             }
             JscriptMsg("添加信息成功！", "../article_list/index?channel_id=" + this.channel_id);
          }
          return result;
       }

       #region 创建其它扩展字段=========================
       private void CreateOtherField(int _channel_id) {
          otherFieldList = new DTcms.BLL.article_attribute_field().GetModelList(this.channel_id, "is_sys=0");
          ViewData["otherFieldList"] = otherFieldList;
       }
       #endregion

       #region 显示默认扩展字段=========================
       private void ShowSysField(int _channel_id) {
          //查找该频道所选的默认字段
          List<DTcms.Model.article_attribute_field> ls = new DTcms.BLL.article_attribute_field().GetModelList(this.channel_id, "is_sys=1");
          ViewData["sysFieldList"] = ls;
       }
       #endregion

       #region 绑定类别=================================
       private void TreeBind(int _channel_id) {
          DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
          DataTable dt = bll.GetList(0, _channel_id);
          List<SelectListItem> categoryList = new List<SelectListItem>() {
             new SelectListItem(){ Text="请选择类别...", Value=""}
          };
          foreach (DataRow dr in dt.Rows) {
             string Id = dr["id"].ToString();
             int ClassLayer = int.Parse(dr["class_layer"].ToString());
             string Title = dr["title"].ToString().Trim();

             if (ClassLayer == 1) {
                categoryList.Add(new SelectListItem() { Text = Title, Value = Id });
             }
             else {
                Title = "├ " + Title;
                Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                categoryList.Add(new SelectListItem() { Text = Title, Value = Id });
             }
          }
          ViewData["categoryList"] = categoryList;
       }
       #endregion


       #region 赋值操作=================================
       private void ShowInfo(int _id) {
          DTcms.BLL.article bll = new DTcms.BLL.article();
          DTcms.Model.article model = bll.GetModel(this.channel_id, _id);
          ViewData["model"] = model;

          //扩展字段赋值
          List<DTcms.Model.article_attribute_field> ls1 = new DTcms.BLL.article_attribute_field().GetModelList(this.channel_id, "");
          ViewData["fieleList"] = ls1;
          //商品规格赋值
          List<Model.article_goods_spec> goodsSpecList = new BLL.article_goods_spec().GetList(this.channel_id, model.id, "");
          ViewBag.GoodsSpecJson = JsonHelper.ObjectToJSON(goodsSpecList);
       }
       #endregion

       #region 扩展字段赋值=============================
       private Dictionary<string, string> SetFieldValues(int _channel_id) {
          DataTable dt = new DTcms.BLL.article_attribute_field().GetList(_channel_id, "").Tables[0];
          Dictionary<string, string> dic = new Dictionary<string, string>();
          string paramName = string.Empty;
          foreach (DataRow dr in dt.Rows) {
             //查找相应的控件
             switch (dr["control_type"].ToString()) {
                case "single-text": //单行文本
                   paramName = "field_control_" + dr["name"].ToString();
                   if (Request.Form[paramName] != null) {
                      dic.Add(dr["name"].ToString(), Request.Form[paramName]);

                   }
                   break;
                case "multi-text": //多行文本
                   goto case "single-text";
                case "editor": //编辑器
                   paramName = "field_control_" + dr["name"].ToString();
                   if (Request.Form[paramName] != null) {
                      dic.Add(dr["name"].ToString(), Request.Form[paramName]);
                   }
                   break;
                case "images": //图片上传
                   goto case "single-text";
                case "video": //视频上传
                   goto case "single-text";
                case "number": //数字
                   goto case "single-text";
                case "checkbox": //复选框
                   paramName = "field_control_" + dr["name"].ToString();
                   if (Request.Form[paramName] != null) {
                      if (Request.Form[paramName].ToLower().IndexOf("true") >= 0) {
                         dic.Add(dr["name"].ToString(), "1");
                      }
                      else {
                         dic.Add(dr["name"].ToString(), "0");
                      }
                   }
                   break;
                case "multi-radio": //多项单选
                   string[] items_option = dr["item_option"].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                   for (int i = 0; i < items_option.Length; i++) {
                      string[] valItemArr = items_option[i].Split('|');
                      if (valItemArr.Length == 2) {
                         paramName = "field_control_" + dr["name"].ToString();
                         if (Request.Form[paramName] != null) {
                            if (Request.Form[paramName].ToLower().IndexOf("true") >= 0) {
                               dic.Add(dr["name"].ToString(), valItemArr[1]);
                               break;
                            }
                         }
                      }
                   }
                   break;
                case "multi-checkbox": //多项多选
                   string[] items_option2 = dr["item_option"].ToString().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                   StringBuilder tempStr = new StringBuilder();
                   for (int i = 0; i < items_option2.Length; i++) {
                      string[] valItemArr2 = items_option2[i].Split('|');
                      if (valItemArr2.Length == 2) {
                         paramName = "field_control_" + dr["name"] + "_" + i;
                         if (Request.Form[paramName] != null) {
                            if (Request.Form[paramName].ToLower().IndexOf("true") >= 0) {
                               string selval = valItemArr2[1].Replace(',', '，') + ",";
                               tempStr.Append(selval);
                            }
                         }
                      }
                   }
                   dic.Add(dr["name"].ToString(), Utils.DelLastComma(tempStr.ToString()));
                   break;
             }
          }
          return dic;
       }
       #endregion

       #region 保存远程图片到本地=======================
       private string AutoRemoteImageSave(string content) {
          if (string.IsNullOrEmpty(content)) {
             return string.Empty;
          }
          DTcms.Web.MVC.UI.UpLoad upload = new DTcms.Web.MVC.UI.UpLoad();
          Regex reg = new Regex("IMG[^>]*?src\\s*=\\s*(?:\"(?<1>[^\"]*)\"|'(?<1>[^\']*)')", RegexOptions.IgnoreCase);
          MatchCollection m = reg.Matches(content);
          foreach (Match math in m) {
             string imgUri = math.Groups[1].Value;
             if (imgUri.StartsWith("http")) {
                string newImgPath = upload.RemoteSaveAs(imgUri);
                if (newImgPath != string.Empty) {
                   content = content.Replace(imgUri, newImgPath);
                }
             }
          }
          return content;
       }
       #endregion

       #region 增加操作=================================
       private bool DoAdd(string txtContent) {
          bool result = false;
          DTcms.Model.article model = new DTcms.Model.article();
          DTcms.BLL.article bll = new DTcms.BLL.article();

          model.site_id = this.channelModel.site_id;
          model.channel_id = this.channel_id;
          model.category_id = Utils.StrToInt(Request.Form["ddlCategoryId"], 0);
          model.call_index = Request.Form["txtCallIndex"].Trim();
          model.title = Request.Form["txtTitle"].Trim();
          model.tags = Request.Form["txtTags"].Trim();
          model.link_url = Request.Form["txtLinkUrl"].Trim();
          model.img_url = Request.Form["txtImgUrl"];
          model.seo_title = Request.Form["txtSeoTitle"].Trim();
          model.seo_keywords = Request.Form["txtSeoKeywords"].Trim();
          model.seo_description = Request.Form["txtSeoDescription"].Trim();
          //内容摘要提取内容前255个字符
          if (string.IsNullOrEmpty(Request.Form["txtZhaiyao"].Trim())) {
             model.zhaiyao = Utils.DropHTML(txtContent, 255);
          }
          else {
             model.zhaiyao = Utils.DropHTML(Request.Form["txtZhaiyao"], 255);
          }
          //是否将编辑器远程图片保存到本地
          if (sysConfig.fileremote == 1) {
             model.content = AutoRemoteImageSave(txtContent);
          }
          else {
             model.content = txtContent;
          }
          model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
          model.click = int.Parse(Request.Form["txtClick"].Trim());
          model.status = Request.Form["rblStatus"].ToLower().IndexOf("true") >= 0 ? GetAdminInfo().is_audit : 2;
          model.is_msg = Request.Form["cblItem_0"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_top = Request.Form["cblItem_1"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_red = Request.Form["cblItem_2"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_hot = Request.Form["cblItem_3"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_slide = Request.Form["cblItem_4"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_sys = 1; //管理员发布
          model.user_name = GetAdminInfo().user_name; //获得当前登录用户名
          model.status = Request.Form["cbStatus"].ToLower().IndexOf("true") >= 0 ? GetAdminInfo().is_audit : 2;
          model.add_time = Utils.StrToDateTime(Request.Form["txtAddTime"].Trim());
          model.fields = SetFieldValues(this.channel_id); //扩展字段赋值

          #region 保存规格====================
          //保存商品规格
          string goodsSpecJsonStr = Request.Form["hide_goods_spec_list"];
          List<Model.article_goods_spec> specList = (List<Model.article_goods_spec>)JsonHelper.JSONToObject<List<Model.article_goods_spec>>(goodsSpecJsonStr);
          if (specList != null) {
             model.specs = specList;
          }
          //保存商品信息
          string[] specGoodsIdArr = Request.Form.GetValues("hide_goods_id");
          string[] specGoodsNoArr = Request.Form.GetValues("spec_goods_no");
          string[] specMarketPriceArr = Request.Form.GetValues("spec_market_price");
          string[] specSellPriceArr = Request.Form.GetValues("spec_sell_price");
          string[] specStockQuantityArr = Request.Form.GetValues("spec_stock_quantity");
          string[] specSpecIdsArr = Request.Form.GetValues("hide_spec_ids");
          string[] specTextArr = Request.Form.GetValues("hide_spec_text");
          string[] specGroupPriceArr = Request.Form.GetValues("hide_group_price");
          if (specGoodsIdArr != null && specGoodsNoArr != null && specMarketPriceArr != null && specSellPriceArr != null && specStockQuantityArr != null
              && specSpecIdsArr != null && specTextArr != null && specGroupPriceArr != null
              && specGoodsIdArr.Length > 0 && specGoodsNoArr.Length > 0 && specMarketPriceArr.Length > 0 && specSellPriceArr.Length > 0
              && specStockQuantityArr.Length > 0 && specSpecIdsArr.Length > 0 && specTextArr.Length > 0 && specGroupPriceArr.Length > 0) {
             List<Model.article_goods> goodsList = new List<Model.article_goods>();
             for (int i = 0; i < specGoodsNoArr.Length; i++) {
                List<Model.user_group_price> groupList = new List<Model.user_group_price>();
                if (!string.IsNullOrEmpty(specGroupPriceArr[i])) {
                   groupList = (List<Model.user_group_price>)JsonHelper.JSONToObject<List<Model.user_group_price>>(specGroupPriceArr[i]);
                }
                goodsList.Add(new Model.article_goods {
                   channel_id = this.channel_id,
                   goods_no = specGoodsNoArr[i],
                   spec_ids = specSpecIdsArr[i],
                   spec_text = specTextArr[i],
                   stock_quantity = Utils.StrToInt(specStockQuantityArr[i], 0),
                   market_price = Utils.StrToDecimal(specMarketPriceArr[i], 0),
                   sell_price = Utils.StrToDecimal(specSellPriceArr[i], 0),
                   group_prices = groupList
                });
             }
             model.goods = goodsList;
          }
          #endregion

          #region 保存相册====================
          //检查是否有自定义图片
          if (Request.Form["txtImgUrl"].Trim() == "") {
             model.img_url = Request.Form["hidFocusPhoto"];
          }
          string[] albumArr = Request.Form.GetValues("hid_photo_name");
          string[] remarkArr = Request.Form.GetValues("hid_photo_remark");
          if (albumArr != null && albumArr.Length > 0) {
             List<Model.article_albums> ls = new List<Model.article_albums>();
             for (int i = 0; i < albumArr.Length; i++) {
                string[] imgArr = albumArr[i].Split('|');
                if (imgArr.Length == 3) {
                   if (!string.IsNullOrEmpty(remarkArr[i])) {
                      ls.Add(new Model.article_albums { channel_id = this.channel_id, original_path = imgArr[1], thumb_path = imgArr[2], remark = remarkArr[i] });
                   }
                   else {
                      ls.Add(new Model.article_albums { channel_id = this.channel_id, original_path = imgArr[1], thumb_path = imgArr[2] });
                   }
                }
             }
             model.albums = ls;
          }
          #endregion

          #region 保存附件====================
          //保存附件
          string[] attachFileNameArr = Request.Form.GetValues("hid_attach_filename");
          string[] attachFilePathArr = Request.Form.GetValues("hid_attach_filepath");
          string[] attachFileSizeArr = Request.Form.GetValues("hid_attach_filesize");
          string[] attachPointArr = Request.Form.GetValues("txt_attach_point");
          if (attachFileNameArr != null && attachFilePathArr != null && attachFileSizeArr != null && attachPointArr != null
              && attachFileNameArr.Length > 0 && attachFilePathArr.Length > 0 && attachFileSizeArr.Length > 0 && attachPointArr.Length > 0) {
             List<Model.article_attach> ls = new List<Model.article_attach>();
             for (int i = 0; i < attachFileNameArr.Length; i++) {
                int fileSize = Utils.StrToInt(attachFileSizeArr[i], 0);
                string fileExt = FileHelper.GetFileExt(attachFilePathArr[i]);
                int _point = Utils.StrToInt(attachPointArr[i], 0);
                ls.Add(new Model.article_attach { channel_id = this.channel_id, file_name = attachFileNameArr[i], file_path = attachFilePathArr[i], file_size = fileSize, file_ext = fileExt, point = _point });
             }
             model.attach = ls;
          }
          #endregion

          if (bll.Add(model) > 0) {
             AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加" + this.channelModel.title + "频道内容:" + model.title); //记录日志
             result = true;
          }
          return result;
       }
       #endregion

       #region 修改操作=================================
       [ValidateInput(false)]
       private bool DoEdit(int _id, string txtContent) {
          bool result = false;
          DTcms.BLL.article bll = new DTcms.BLL.article();
          DTcms.Model.article model = bll.GetModel(this.channel_id, _id);
          model.channel_id = this.channel_id;
          model.category_id = Utils.StrToInt(Request.Form["ddlCategoryId"], 0);
          model.call_index = Request.Form["txtCallIndex"].Trim();
          model.title = Request.Form["txtTitle"].Trim();
          model.link_url = Request.Form["txtLinkUrl"].Trim();
          model.img_url = Request.Form["txtImgUrl"];
          model.seo_title = Request.Form["txtSeoTitle"].Trim();
          model.seo_keywords = Request.Form["txtSeoKeywords"].Trim();
          model.seo_description = Request.Form["txtSeoDescription"].Trim();
          //内容摘要提取内容前255个字符
          if (string.IsNullOrEmpty(Request.Form["txtZhaiyao"].Trim())) {
             model.zhaiyao = Utils.DropHTML(txtContent, 255);
          }
          else {
             model.zhaiyao = Utils.DropHTML(Request.Form["txtZhaiyao"], 255);
          }
          //是否将编辑器远程图片保存到本地
          if (sysConfig.fileremote == 1) {
             model.content = AutoRemoteImageSave(Request.Form["txtContent"]);
          }
          else {
             model.content = Request.Form["txtContent"];
          }
          model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
          model.click = int.Parse(Request.Form["txtClick"].Trim());
          //model.status = (Request.Form["rblStatus"]??"").ToLower().IndexOf("true") >= 0 ? GetAdminInfo().is_audit : 2;
          model.is_msg = Request.Form["cblItem_0"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_top = Request.Form["cblItem_1"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_red = Request.Form["cblItem_2"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_hot = Request.Form["cblItem_3"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.is_slide = Request.Form["cblItem_4"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
          model.status = Request.Form["cbStatus"].ToLower().IndexOf("true") >= 0 ? GetAdminInfo().is_audit : 2;
          model.add_time = Utils.StrToDateTime(Request.Form["txtAddTime"].Trim());
          model.update_time = DateTime.Now;
          model.fields = SetFieldValues(this.channel_id); //扩展字段赋值

          #region 保存规格====================
          if (channelModel.is_spec == 1) {
             //保存商品规格
             string goodsSpecJsonStr = Request.Form["hide_goods_spec_list"];
             List<Model.article_goods_spec> specList = (List<Model.article_goods_spec>)JsonHelper.JSONToObject<List<Model.article_goods_spec>>(goodsSpecJsonStr);
             if (specList != null) {
                model.specs = specList;
                model.goods = new List<Model.article_goods>();
             }
             //保存商品信息
             string[] specGoodsIdArr = Request.Form.GetValues("hide_goods_id");
             string[] specGoodsNoArr = Request.Form.GetValues("spec_goods_no");
             string[] specMarketPriceArr = Request.Form.GetValues("spec_market_price");
             string[] specSellPriceArr = Request.Form.GetValues("spec_sell_price");
             string[] specStockQuantityArr = Request.Form.GetValues("spec_stock_quantity");
             string[] specSpecIdsArr = Request.Form.GetValues("hide_spec_ids");
             string[] specTextArr = Request.Form.GetValues("hide_spec_text");
             string[] specGroupPriceArr = Request.Form.GetValues("hide_group_price");
             if (specGoodsIdArr != null && specGoodsNoArr != null && specMarketPriceArr != null && specSellPriceArr != null && specStockQuantityArr != null
                 && specSpecIdsArr != null && specTextArr != null && specGroupPriceArr != null
                 && specGoodsIdArr.Length > 0 && specGoodsNoArr.Length > 0 && specMarketPriceArr.Length > 0 && specSellPriceArr.Length > 0
                 && specStockQuantityArr.Length > 0 && specSpecIdsArr.Length > 0 && specTextArr.Length > 0 && specGroupPriceArr.Length > 0) {
                List<Model.article_goods> goodsList = new List<Model.article_goods>();
                for (int i = 0; i < specGoodsNoArr.Length; i++) {
                   List<Model.user_group_price> groupList = new List<Model.user_group_price>();
                   if (!string.IsNullOrEmpty(specGroupPriceArr[i])) {
                      groupList = (List<Model.user_group_price>)JsonHelper.JSONToObject<List<Model.user_group_price>>(specGroupPriceArr[i]);
                   }
                   goodsList.Add(new Model.article_goods {
                      id = Utils.StrToInt(specGoodsIdArr[i], 0),
                      channel_id = this.channel_id,
                      article_id = model.id,
                      goods_no = specGoodsNoArr[i],
                      spec_ids = specSpecIdsArr[i],
                      spec_text = specTextArr[i],
                      stock_quantity = Utils.StrToInt(specStockQuantityArr[i], 0),
                      market_price = Utils.StrToDecimal(specMarketPriceArr[i], 0),
                      sell_price = Utils.StrToDecimal(specSellPriceArr[i], 0),
                      group_prices = groupList
                   });
                }
                model.goods = goodsList;
             }
          }
          #endregion

          #region 保存相册====================
          //检查是否有自定义图片
          if (Request.Form["txtImgUrl"].Trim() == "") {
             model.img_url = Request.Form["hidFocusPhoto"];
          }
          if (model.albums != null) {
             model.albums.Clear();
          }
          string[] albumArr = Request.Form.GetValues("hid_photo_name");
          string[] remarkArr = Request.Form.GetValues("hid_photo_remark");
          if (albumArr != null) {
             List<Model.article_albums> ls = new List<Model.article_albums>();
             for (int i = 0; i < albumArr.Length; i++) {
                string[] imgArr = albumArr[i].Split('|');
                int img_id = Utils.StrToInt(imgArr[0], 0);
                if (imgArr.Length == 3) {
                   if (!string.IsNullOrEmpty(remarkArr[i])) {
                      ls.Add(new Model.article_albums { id = img_id, channel_id = this.channel_id, article_id = _id, original_path = imgArr[1], thumb_path = imgArr[2], remark = remarkArr[i] });
                   }
                   else {
                      ls.Add(new Model.article_albums { id = img_id, channel_id = this.channel_id, article_id = _id, original_path = imgArr[1], thumb_path = imgArr[2] });
                   }
                }
             }
             model.albums = ls;
          }
          #endregion

          #region 保存附件====================
          if (model.attach != null) {
             model.attach.Clear();
          }
          string[] attachIdArr = Request.Form.GetValues("hid_attach_id");
          string[] attachFileNameArr = Request.Form.GetValues("hid_attach_filename");
          string[] attachFilePathArr = Request.Form.GetValues("hid_attach_filepath");
          string[] attachFileSizeArr = Request.Form.GetValues("hid_attach_filesize");
          string[] attachPointArr = Request.Form.GetValues("txt_attach_point");
          if (attachIdArr != null && attachFileNameArr != null && attachFilePathArr != null && attachFileSizeArr != null && attachPointArr != null
              && attachIdArr.Length > 0 && attachFileNameArr.Length > 0 && attachFilePathArr.Length > 0 && attachFileSizeArr.Length > 0 && attachPointArr.Length > 0) {
             List<Model.article_attach> ls = new List<Model.article_attach>();
             for (int i = 0; i < attachFileNameArr.Length; i++) {
                int attachId = Utils.StrToInt(attachIdArr[i], 0);
                int fileSize = Utils.StrToInt(attachFileSizeArr[i], 0);
                string fileExt = FileHelper.GetFileExt(attachFilePathArr[i]);
                int _point = Utils.StrToInt(attachPointArr[i], 0);
                ls.Add(new Model.article_attach { id = attachId, channel_id = this.channel_id, article_id = _id, file_name = attachFileNameArr[i], file_path = attachFilePathArr[i], file_size = fileSize, file_ext = fileExt, point = _point, });
             }
             model.attach = ls;
          }
          #endregion

          if (bll.Update(model)) {
             AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + this.channelModel.title + "频道内容:" + model.title); //记录日志
             result = true;
          }
          return result;
       }
       #endregion

       #region 关联内容操作======================
       public ActionResult get_link_article_list() {
          return PartialView("~/Areas/admin/Views/Article/article_link_partial_list.cshtml");
       }

       /// <summary>
       /// 添加当前内容所管理内容
       /// </summary>
       /// <returns></returns>
       [HttpPost]
       public ActionResult AddLinkArticle() {
          int sucCount = 0; //成功数量
          int errorCount = 0; //失败数量
          Newtonsoft.Json.Linq.JObject jobject = Newtonsoft.Json.Linq.JObject.Parse(Request.Form["json"]);
          Newtonsoft.Json.Linq.JToken jchannel_id = jobject["channel_id"];//内容通道Id
          Newtonsoft.Json.Linq.JToken jid = jobject["id"];//内容Id
          Newtonsoft.Json.Linq.JToken list = jobject["list"];//关联内容列表(格式:[{channel_id:1,id:1}...])
          if (list == null){
             return Content(sucCount.ToString());
          }
          int _articleId = int.Parse(jid.ToString());
          int _channel_id = int.Parse(jchannel_id.ToString());
          DTcms.BLL.article bll_article = new DTcms.BLL.article();
          DTcms.Model.article _articleModel = bll_article.GetModel(_channel_id, _articleId);
          DTcms.BLL.article_link bll_article_link = new DTcms.BLL.article_link();

          foreach (Newtonsoft.Json.Linq.JToken item in list) {
             int _link_channel_id = int.Parse(item["channel_id"].ToString());
             int _link_id = int.Parse(item["id"].ToString());
             DTcms.Model.article _model = bll_article.GetModel(_link_channel_id,_link_id);
             DTcms.Model.article_link link_item = new DTcms.Model.article_link();
             link_item.site_id = _articleModel.site_id;
             link_item.channel_id = _articleModel.channel_id;
             link_item.article_id = _articleModel.id;
             link_item.link_site_id = _model.site_id;
             link_item.link_channel_id = _model.channel_id;
             link_item.link_article_id = _model.id;
             link_item.link_category_id = _model.category_id;
             link_item.add_time = DateTime.Now;
             if (bll_article_link.Add(link_item))
                sucCount++;
             else
                errorCount++;
          }
          return Content(sucCount.ToString());
       }

       /// <summary>
       /// 删除当前内容所关联内容
       /// </summary>
       /// <returns></returns>
       public ActionResult DelLinkArticle() {
          int sucCount = 0; //成功数量
          int errorCount = 0; //失败数量
          Newtonsoft.Json.Linq.JObject jobject = Newtonsoft.Json.Linq.JObject.Parse(Request.Form["json"]);
          Newtonsoft.Json.Linq.JToken jchannel_id = jobject["channel_id"];
          Newtonsoft.Json.Linq.JToken jid = jobject["id"];
          Newtonsoft.Json.Linq.JToken list = jobject["list"];//格式:[{link_id: 1}...]
          DTcms.BLL.article_link bll_article_like = new DTcms.BLL.article_link();
          int _articleId = int.Parse(jid.ToString());
          foreach (Newtonsoft.Json.Linq.JToken item in list) {
             int _id = int.Parse(item["id"].ToString());
             if (bll_article_like.Delete(_id))
                sucCount++;
             else
                errorCount++;
          }
          return Content(sucCount.ToString());
       }
       #endregion
    }
}
