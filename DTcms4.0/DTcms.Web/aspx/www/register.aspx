<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.register" ValidateRequest="false" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Text" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="DTcms.Common" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by DTcms Template Engine at 2016/4/2 23:27:30.
		本页面代码由DTcms模板引擎生成于 2016/4/2 23:27:30. 
	*/

	base.OnInit(e);
	StringBuilder templateBuilder = new StringBuilder(220000);

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<title>会员注册 - ");
	templateBuilder.Append(Utils.ObjectToStr(site.name));
	templateBuilder.Append("</title>\r\n<meta name=\"keywords\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_keyword));
	templateBuilder.Append("\" />\r\n<meta name=\"description\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_description));
	templateBuilder.Append("\" />\r\n<link href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/artdialog/ui-dialog.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<link href=\"");
	templateBuilder.Append("/templates/www");
	templateBuilder.Append("/css/style.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/jquery/jquery-1.11.2.min.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/artdialog/dialog-plus-min.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append("/templates/www");
	templateBuilder.Append("/js/common.js\"></");
	templateBuilder.Append("script>\r\n</head>\r\n\r\n<body>\r\n<!--Header-->\r\n");


	templateBuilder.Append("<div style=\"width:100%;height:28px;line-height:28px;vertical-align:middle;background:#e7e7e7;color:green;position:fixed;left:0px;top:0px;z-index:9999;font-size:13px;\"  >\r\n     <div style=\"margin-left:20px;height:100%\">\r\n           <a style=\"width:150px;margin-left:20px;height:100%\" href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\">君飞首页</a>\r\n           <a style=\"width:50px;margin-left:20px;height:100%\" href=\"");
	templateBuilder.Append(linkurl("www_blog_list",0));

	templateBuilder.Append("\">博文阅读</a>\r\n     </div>\r\n</div>");


	templateBuilder.Append("\r\n<div style=\"width:100%;height:28px;\"></div>\r\n<div class=\"header\" style=\"\">\r\n<div class=\"content\">\r\n <span></span>\r\n <div class=\"menu\">\r\n");
	string _url="/"+System.IO.Path.GetFileName(Request.PhysicalPath);
	

	templateBuilder.Append("\r\n  <a href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("index"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">首页</a>\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_product"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("www_product"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">产品服务</a>\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_case"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("www_case"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">案例</a>\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_about"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("www_about"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">关于我们</a>\r\n </div>\r\n </div>\r\n</div>");


	templateBuilder.Append("\r\n<!--/Header-->\r\n\r\n<div class=\"main-box\">\r\n  <div class=\"section clearfix\">\r\n  ");
	if (action=="")
	{

	templateBuilder.Append("\r\n    <!--用户注册-->\r\n    <link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("css/validate.css\" />\r\n    <script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/jquery/jquery.form.min.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/jquery/Validform_v5.3.2_min.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append("/templates/www");
	templateBuilder.Append("/js/register-validate.js\"></");
	templateBuilder.Append("script>\r\n\r\n    <div class=\"main-tit\">\r\n      <h2>会员注册</h2>\r\n    </div>\r\n    <div class=\"inner-box\">\r\n      <form id=\"regform\" name=\"regform\" url=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=user_register&site=");
	templateBuilder.Append(Utils.ObjectToStr(site.build_path));
	templateBuilder.Append("\">\r\n      <div class=\"dl-list\">\r\n        <dl>\r\n          <dt>用 户 名：</dt>\r\n          <dd>\r\n            <input id=\"txtUserName\" name=\"txtUserName\" type=\"text\" class=\"input txt\" placeholder=\"输入登录用户名\" datatype=\"s3-50\" nullmsg=\"请输入登录的用户名\" sucmsg=\" \" ajaxurl=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=validate_username\" />\r\n            <span class=\"Validform_checktip\">请输入登录的用户名</span>\r\n          </dd>\r\n        </dl>\r\n        <dl>\r\n          <dt>密&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;码：</dt>\r\n          <dd>\r\n            <input id=\"txtPassword\" name=\"txtPassword\" type=\"password\" class=\"input txt\" placeholder=\"输入登录密码\" datatype=\"*6-20\" nullmsg=\"请输入登录密码\" errormsg=\"密码范围在6-20位之间\" sucmsg=\" \" />\r\n            <span class=\"Validform_checktip\">请输入6-20位的登录密码</span>\r\n          </dd>\r\n        </dl>\r\n        <dl>\r\n          <dt>确认密码：</dt>\r\n          <dd>\r\n            <input id=\"txtPassword1\" name=\"txtPassword1\" type=\"password\" class=\"input txt\" placeholder=\"请再次输入密码\" datatype=\"*\" recheck=\"txtPassword\" nullmsg=\"请再输入一次密码\" errormsg=\"两次输入的密码不一致\" sucmsg=\" \" />\r\n            <span class=\"Validform_checktip\">请再次输入登录密码</span>\r\n          </dd>\r\n        </dl>\r\n        \r\n        ");
	if (uconfig.regstatus==1||uconfig.regstatus==2)
	{

	templateBuilder.Append("\r\n        <!--开放注册及手机注册-->\r\n        <dl>\r\n          <dt>手&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;机：</dt>\r\n          <dd>\r\n            <input id=\"txtMobile\" name=\"txtMobile\" type=\"text\" class=\"input txt\" placeholder=\"输入手机号码\" datatype=\"m\" nullmsg=\"请输入手机号码\" sucmsg=\" \" />\r\n            <span class=\"Validform_checktip\">请输入手机号码</span>\r\n          </dd>\r\n        </dl>\r\n        <!--/开放注册及手机注册-->\r\n        ");
	}	//end for if

	if (uconfig.regstatus==2)
	{

	templateBuilder.Append("\r\n        <!--开启手机注册-->\r\n        <dl>\r\n          <dt>确 认 码：</dt>\r\n          <dd>\r\n            <input id=\"txtCode\" name=\"txtCode\" type=\"text\" class=\"input code\" placeholder=\"输入手机收到的确认码\"  datatype=\"s4-20\" nullmsg=\"请输入手机收到的确认码\" sucmsg=\" \" />\r\n            <a class=\"send\" href=\"javascript:;\" onclick=\"sendSMS(this,'#txtMobile','");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=user_verify_smscode&site=");
	templateBuilder.Append(Utils.ObjectToStr(site.build_path));
	templateBuilder.Append("');\">发送确认码</a>\r\n            <span class=\"Validform_checktip\">请手机短信收到的验证码</span>\r\n          </dd>\r\n        </dl>\r\n        <!--开启手机注册-->\r\n        ");
	}	//end for if

	if (uconfig.regstatus==1||uconfig.regstatus==3||uconfig.regstatus==4)
	{

	templateBuilder.Append("\r\n        <!--开放注册及邮箱邀请注册-->\r\n        <dl>\r\n          <dt>邮&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;箱：</dt>\r\n          <dd>\r\n            <input id=\"txtEmail\" name=\"txtEmail\" type=\"text\" class=\"input txt\" placeholder=\"输入邮箱账号\" datatype=\"e\" nullmsg=\"请输入电子邮箱账号\" sucmsg=\" \" />\r\n            <span class=\"Validform_checktip\">请输入电子邮箱账号</span>\r\n          </dd>\r\n        </dl>\r\n        <!--/开放注册及邮箱邀请注册-->\r\n        ");
	}	//end for if

	if (uconfig.regstatus==4)
	{

	templateBuilder.Append("\r\n        <!--开启邀请注册-->\r\n        <dl>\r\n          <dt>邀 请 码：</dt>\r\n          <dd>\r\n            <input id=\"txtCode\" name=\"txtCode\" type=\"text\" class=\"input txt\" placeholder=\"输入好友提供的邀请码\" datatype=\"s2-20\" nullmsg=\"请输入注册邀请码\" sucmsg=\" \" />\r\n            <span class=\"Validform_checktip\">输入好友提供的邀请码</span>\r\n          </dd>\r\n        </dl>\r\n        <!--/开启邀请注册-->\r\n        ");
	}	//end for if

	if (uconfig.regstatus==1)
	{

	templateBuilder.Append("\r\n        <!--开放注册-->\r\n        <dl>\r\n          <dt>验 证 码：</dt>\r\n          <dd>\r\n            <input id=\"txtCode\" name=\"txtCode\" type=\"text\" class=\"input code\" placeholder=\"输入验证码\" datatype=\"s4-20\" nullmsg=\"请输入右边显示的验证码\" sucmsg=\" \" />\r\n            <a class=\"send\" title=\"点击切换验证码\" href=\"javascript:;\" onclick=\"ToggleCode(this, '");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/verify_code.ashx');return false;\">\r\n              <img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/verify_code.ashx\" width=\"80\" height=\"22\" />\r\n            </a>\r\n          </dd>\r\n        </dl>\r\n        <!--/开放注册-->\r\n        ");
	}	//end for if

	if (uconfig.regrules==1)
	{

	templateBuilder.Append("\r\n        <!--开启注册协议-->\r\n        <dl>\r\n          <dt></dt>\r\n          <dd>\r\n              <input id=\"chkAgree\" name=\"chkAgree\" type=\"checkbox\" value=\"1\">\r\n              <label for=\"chkAgree\">我已仔细阅读并接受 <a href=\"javascript:;\" onclick=\"showWindow('#regrules');\">注册许可协议</a></label>\r\n              <div id=\"regrules\" title=\"注册许可协议\" style=\"display:none;\">");
	templateBuilder.Append(Utils.ObjectToStr(uconfig.regrulestxt));
	templateBuilder.Append("</div>\r\n            \r\n          </dd>\r\n        </dl>\r\n        <!--开启注册协议-->\r\n        ");
	}	//end for if

	templateBuilder.Append("\r\n        \r\n        <dl>\r\n          <dt></dt>\r\n          <dd>\r\n            ");
	if (uconfig.regrules==1)
	{

	templateBuilder.Append("\r\n              <input id=\"btnSubmit\" name=\"btnSubmit\" class=\"btn btn-success\" type=\"submit\" value=\"立即注册\" disabled=\"disabled\" />\r\n            ");
	}
	else
	{

	templateBuilder.Append("\r\n              <input id=\"btnSubmit\" name=\"btnSubmit\" class=\"submit\" type=\"submit\" value=\"立即注册\" />\r\n            ");
	}	//end for if

	templateBuilder.Append("\r\n          </dd>\r\n        </dl>\r\n      </div>\r\n      </form>\r\n    </div>\r\n    <!--用户注册-->\r\n  ");
	}	//end for if

	if (action=="close")
	{

	templateBuilder.Append("\r\n    <!--关闭会员注册-->\r\n    <div class=\"main-tit\">\r\n      <h2>温馨提示</h2>\r\n    </div>\r\n    <div class=\"inner-box\">\r\n      <div class=\"msg-tips\">\r\n        <div class=\"ico warning\"></div>\r\n        <div class=\"msg\">\r\n          <strong>非常抱歉，系统暂停注册会员服务！</strong>\r\n          <p>由于某些原因，系统暂停注册会员，如对您造成不便之处，我们深感遗憾！</p>\r\n          <p>如需了解开放时间，请联系本站客服或管理员。</p>\r\n          <p>您可以点击这里<a href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\">返回网站首页</a></p>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <!--关闭会员注册-->\r\n  ");
	}	//end for if

	if (action=="sendmail")
	{

	templateBuilder.Append("\r\n    <!--发送邮箱验证-->\r\n    <div class=\"main-tit\">\r\n      <h2>温馨提示</h2>\r\n    </div>\r\n    <div class=\"inner-box\">\r\n      <div class=\"msg-tips\">\r\n        <div class=\"ico warning\"></div>\r\n        <div class=\"msg\">\r\n          <strong>注册成功，您的账户目前处于未验证状态！</strong>\r\n          <p>欢迎您成为本站会员，您的账户目前处于未验证状态，请尽快登录您的注册邮箱激活该会员账户。</p>\r\n          <p>系统已经自动为您发送了一封验证邮件，如果您长时间未收到邮件，请点击这里<a href=\"javascript:;\" onclick=\"sendEmail('");
	templateBuilder.Append(Utils.ObjectToStr(username));
	templateBuilder.Append("', '");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=user_verify_email&site=");
	templateBuilder.Append(Utils.ObjectToStr(site.build_path));
	templateBuilder.Append("');\">重新发送邮件</a>！</p>\r\n          <i>温馨提示：邮件验证有效期为\r\n          ");
	if (uconfig.regemailexpired>0)
	{

	templateBuilder.Append("\r\n          ");
	templateBuilder.Append(Utils.ObjectToStr(uconfig.regemailexpired));
	templateBuilder.Append("天\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          无限制\r\n          ");
	}	//end for if

	templateBuilder.Append("\r\n          </i>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <!--发送邮箱验证-->\r\n  ");
	}	//end for if

	if (action=="checkmail")
	{

	templateBuilder.Append("\r\n    <!--邮箱验证成功-->\r\n    <div class=\"main-tit\">\r\n      <h2>温馨提示</h2>\r\n    </div>\r\n    <div class=\"inner-box\">\r\n      <div class=\"msg-tips\">\r\n        <div class=\"ico\"></div>\r\n        <div class=\"msg\">\r\n          <strong>恭喜您");
	templateBuilder.Append(Utils.ObjectToStr(username));
	templateBuilder.Append("，已通过邮件激活会员账户</strong>\r\n          <p>您的会员账户已经激活啦，从现在起，你可以享受更多的会员服务，还等什么呢？</p>\r\n          <p>赶快点击这里返回<a href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\">首页</a>，点击这里<a href=\"");
	templateBuilder.Append(linkurl("usercenter","index"));

	templateBuilder.Append("\">进入会员中心</a>吧！</p>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <!--邮箱验证成功-->\r\n  ");
	}	//end for if

	if (action=="checkerror")
	{

	templateBuilder.Append("\r\n    <!--注册验证失败-->\r\n    <div class=\"main-tit\">\r\n      <h2>温馨提示</h2>\r\n    </div>\r\n    <div class=\"inner-box\">\r\n      <div class=\"msg-tips\">\r\n        <div class=\"ico error\"></div>\r\n        <div class=\"msg\">\r\n          <strong>出错啦，该用户不存在或验证已过期！</strong>\r\n          <p>无法验证你的账户，不知神马原因，可能是你的用户名不存在或者验证码已经过期啦！</p>\r\n          <p>不过别担心，如果您还记得你的会员名称的话，点击这里<a href=\"");
	templateBuilder.Append(linkurl("login"));

	templateBuilder.Append("\">进入会员中心</a>吧。</p>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <!--注册验证失败-->\r\n  ");
	}	//end for if

	if (action=="verify")
	{

	templateBuilder.Append("\r\n    <!--人工审核-->\r\n    <div class=\"main-tit\">\r\n      <h2>温馨提示</h2>\r\n    </div>\r\n    <div class=\"inner-box\">\r\n      <div class=\"msg-tips\">\r\n        <div class=\"ico warning\"></div>\r\n        <div class=\"msg\">\r\n          <strong>账户处于未审核状态，请等待人工审核通过！</strong>\r\n          <p>很抱歉亲爱的，您的会员账户还没有审核通过呢，再等等吧，实在等不及的话请联系本站客服人员！</p>\r\n          <p>由于种种原因，本站不得以暂时开启人工审核，如对您造成不便敬请原谅哦。</p>\r\n          <p>您可以点击这里<a href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\">返回网站首页</a></p>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <!--人工审核-->\r\n  ");
	}	//end for if

	templateBuilder.Append("\r\n\r\n  </div>\r\n</div>\r\n\r\n<!--Footer-->\r\n");

	templateBuilder.Append("<div class=\"foot m-bg text-center col-md-12\">Copyright &copy; 2007-2016 kinfar.net studio,All Rights Reserved.</p></div>\r\n");


	templateBuilder.Append("\r\n<!--/Footer-->\r\n</body>\r\n</html>");
	Response.Write(templateBuilder.ToString());
}
</script>
