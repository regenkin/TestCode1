using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.UI.Controllers
{
    public class Article_ListController : DTcms.Web.MVC.UI.Controllers.BaseController
    {
        protected string channel = string.Empty; //频道名称
        protected int page;         //当前页码
        protected int category_id;  //类别ID
        protected int totalcount;   //OUT数据总数
        protected string pagelist;  //分页页码
        protected DTcms.Model.article_category model = new DTcms.Model.article_category(); //分类的实体

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            page = DTRequest.GetQueryInt("page", 1);
            category_id = DTRequest.GetQueryInt("category_id");
            DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
            model.title = "所有类别";
            if (category_id > 0) //如果ID获取到，将使用ID
            {
                if (bll.Exists(category_id))
                    model = bll.GetModel(category_id);
            }
            ViewBag.Page = this.page.ToString();
            ViewBag.CategoryId = this.category_id.ToString();
            ViewBag.This = this;
            ViewData["model"] = model;
        }
    }
}
