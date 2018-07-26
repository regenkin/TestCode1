//====================初始化验证表单====================
//using Jquery//
$(function(){
	//提交表单
	$("#btnSubmit").bind("click", function() {
        if ($("#txtUserName").val() == "" || $("#txtPassword").val() == "" || $("#txtCorp").val() == ""){
			$("#msgtips").show();
			$("#msgtips").text("请填写企业号,用户名和登录密码！");
			return false;
		}
		$.ajax({
            type: "POST",
            url: $("#loginform").attr("url"),
			dataType: "json",
            data: {
                "txtCorp": $("#txtCorp").val(),
                "txtUserName" : $("#txtUserName").val(),
				"txtPassword" : $("#txtPassword").val(),
				"chkRemember" : $("#chkRemember").attr("checked")
            },
            timeout: 20000,
			beforeSend: function(XMLHttpRequest) {
				$("#btnSubmit").attr("disabled", true);
				$("#msgtips").show();
				$("#msgtips").text("正在登录，请稍候...");
			},
            success: function(data, textStatus) {
                if (data.status == 1){
					if(typeof(data.url)=="undefined"){
						location.href = $("#turl").val();
					}else{
						location.href = data.url;
					}
                } else {
                    $("#btnSubmit").attr("disabled", false);
                    $("#msgtips").text(data.msg);
                }
            },
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				$("#msgtips").text("状态：" + textStatus + "；出错提示：" + errorThrown);
				$("#btnSubmit").attr("disabled", false);
			} 
        });
		return false;
    });
});