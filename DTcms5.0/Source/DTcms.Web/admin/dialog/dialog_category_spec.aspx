<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog_category_spec.aspx.cs" Inherits="DTcms.Web.admin.dialog.dialog_category_spec" %>

<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>类别规格</title>
<link rel="stylesheet" type="text/css" href="../skin/icon/iconfont.css" />
<link rel="stylesheet" type="text/css" href="../skin/default/style.css" />
<script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
<script type="text/javascript">
    var api = top.dialog.get(window); ; //获取父窗体对象
    $(function () {
        //设置窗口按钮及事件
        api.button([{
            value: '确定',
            callback: function () {
                appendSpecHtml();
            },
            autofocus: true
        }, {
            value: '取消',
            callback: function () { }
        }
        ]);

        //设置按钮事件
        $(".spec-item li a").click(function () {
            if ($(this).parent().hasClass("selected")) {
                $(this).parent().removeClass("selected");
            } else {
                $(this).parent().addClass("selected");
            }
        });

        //初始化已选择的规格
        $(api.data).parent().find("input[name='hide_spec_id']").each(function () {
            var hideId = $(this).val();
            $(".spec-item li").each(function () {
                if (!$(this).hasClass("selected") && $(this).attr("id") == hideId) {
                    $(this).addClass("selected");
                }
            });
        });
    });

    //插入规格节点
    function appendSpecHtml() {
        $(api.data).siblings("li").remove(); //先删除所有同辈节点
        $(".spec-item li").each(function () {
            if ($(this).hasClass("selected")) {
                execSpecHtml($(this).attr("title"), $(this).attr("id"));
            }
        });
    }

    //创建品牌的HTML
    function execSpecHtml(title, id) {
        var liHtml = '<li>'
        + '<input name="hide_spec_id" type="hidden" value="' + id + '" />'
        + '<a href="javascript:;" class="del" title="删除" onclick="delNode(this);"><i class="iconfont icon-remove"></i></a>'
        + '<span>' + title + '</span>'
        + '</li>';
        $(api.data).before(liHtml);
    }
</script>
</head>

<body>
<form id="form1" runat="server">
<div style="width:500px; height:280px;overflow:auto;">
  <div class="div-content">
    <dl>
      <dt>规格列表</dt>
      <dd>
        <ul class="spec-item">
          <asp:Repeater ID="rptList" runat="server">
          <ItemTemplate>
          <li title="<%#Eval("title")%>" id="<%#Eval("id")%>">
            <a href="javascript:;"><%#Eval("title")%></a>
          </li>
          </ItemTemplate>
          </asp:Repeater>
        </ul>
      </dd>
    </dl>
  </div>
</div>
</form>
</body>
</html>