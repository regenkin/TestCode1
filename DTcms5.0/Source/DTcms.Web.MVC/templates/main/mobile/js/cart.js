﻿/* 
*作者：一些事情
*时间：2015-4-17
*购物车方法*需要结合jquery一起使用
----------------------------------------------------------*/
//商品数量加减一
function addCartNum(num){
	var numObj = $("#commoditySelectNum");
	var selectNum = 0;
	if(numObj.val().length > 0){
		selectNum = parseInt(numObj.val());
	}
	selectNum += num;
	//最小值
	if(selectNum < 1){
		selectNum = 1;
	}
	//最大值
	if(selectNum > parseInt(numObj.attr("maxValue"))){
		selectNum = parseInt(numObj.attr("maxValue"));
	}
	numObj.val(selectNum);
}

//删除元素
function hintRemove(obj){
	$(obj).remove();
}

//添加进购物车
function cartAdd(obj, webpath, linktype, linkurl){
	var articleId = parseInt($("#commodityArticleId").val());
	var selectNum = parseInt($("#commoditySelectNum").val());
	if($(obj).prop("disabled") == true){
		return false;
	}
	//检查文章ID
	if(isNaN(articleId)){
		alert("商品参数不正确！");
		return false;
	}
	//检查购买数量
	if(isNaN(selectNum) || selectNum == 0){
		alert("购买数量不能为零！");
		return false;
	}
	//检查库存数量
	if(parseInt(selectNum) > parseInt($("#commodityStockNum").text())){
		alert("购买数量大于库存数量，库存不足！");
		return false;
	}
	//记住按钮文字
	var buttonText = $(obj).text();
	//如果是立即购买
	if(linktype == 1){
		var jsondata = '[{"article_id":'+articleId+', "quantity":'+selectNum+'}]'; //结合商品参数
		$.ajax({
			type: "post",
			url: webpath + "tools/submit_ajax.ashx?action=cart_goods_buy",
			data: { "jsondata": jsondata },
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				//发送前动作
				$(obj).prop("disabled",true).text("请稍候...");
			},
			success: function(data, textStatus) {
				if (data.status == 1) {
					location.href = linkurl;
				}else{
					alert("尝试加入购物清单失败，请重试！");
				}
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				alert("状态：" + textStatus + "；出错提示：" + errorThrown);
			},
			complete: function (XMLHttpRequest, textStatus) {
				$(obj).prop("disabled",false).text(buttonText);
			},
			timeout: 20000
		});
		return false;
	}else{
		$.ajax({
			type: "post",
			url: webpath + "tools/submit_ajax.ashx?action=cart_goods_add",
			data: {
				"article_id" : articleId,
				"quantity" : selectNum
			},
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				//发送前动作
				$(obj).prop("disabled",true).text("请稍候...");
			},
			success: function(data, textStatus) {
				if (data.status == 1) {
					$("#cartInfoHint").remove();
					var HintHtml = '<div id="cartInfoHint" class="msg-tips cart-info">'
						+ '<div class="ico"></div>'
						+ '<div class="msg">'
						+ '<strong>商品已成功添加到购物车！</strong>'
						+ '<p>购物车共有<b>' + data.quantity + '</b>件商品，合计：<b class="red">' + data.amount + '</b>元</p>'
						+ '<a class="btn btn-success" title="去购物车结算" href="' + linkurl + '">去结算</a>&nbsp;&nbsp;'
						+ '<a title="再逛逛" href="javascript:;" onclick="hintRemove(\'#cartInfoHint\');">再逛逛</a>'
						+ '<i class="close" title="关闭" onclick="hintRemove(\'#cartInfoHint\');"><span>关闭</span></i>'
						+ '</div>'
						+ '</div>'
					//$(obj).after(HintHtml); //添加节点
					layer.open({
                  content: HintHtml
					});
					$("#shoppingCartCount").text(data.quantity); //赋值给显示购物车数量的元素
				} else {
					$("#cartInfoHint").remove();
					var HintHtml = '<div id="cartInfoHint" class="msg-tips cart-info">'
						+ '<div class="ico error"></div>'
						+ '<div class="msg">'
						+ '<strong>商品添加到购物车失败！</strong>'
						+ '<p>' + data.msg + '</p>'
						+ '<i class="close" title="关闭" onclick="hintRemove(\'#cartInfoHint\');"><span>关闭</span></i>'
						+ '</div>'
						+ '</div>'
					$(obj).after(HintHtml); //添加节点
				}
			},
			error: function (XMLHttpRequest, textStatus, errorThrown) {
				alert("状态：" + textStatus + "；出错提示：" + errorThrown);
			},
			complete: function (XMLHttpRequest, textStatus) {
				$(obj).prop("disabled",false).text(buttonText);
			},
			timeout: 20000
		});
		return false;
	}
}

//修改购物车数量
function updateCart(obj, webpath, num){
	var objInput;
	var goodsQuantity; //购买数量
	var $content = $(obj).parents(".cart_content");
	var stockQuantity = parseInt($content.find("input[name='hideStockQuantity']").val()); //库存数量
	var articleId = $content.find("input[name='hideArticleId']").val(); //文章ID
	var goodsPrice = $content.find("input[name='hideGoodsPrice']").val(); //商品单价
	var goodsPoint = $content.find("input[name='hidePoint']").val(); //商品积分
	if(arguments.length == 2){
		objInput = $(obj);
		goodsQuantity = parseInt(objInput.val());
	}else{
		objInput = $(obj).siblings("input[name='goodsQuantity']");
		goodsQuantity = parseInt(objInput.val()) + parseInt(num);
	}
	if(isNaN(goodsQuantity)){
		alert('商品数量只能输入数字!');
		return false;
	}
	if(isNaN(stockQuantity)){
		alert('检测不到商品库存数量!');
		return false;
	}
	if(goodsQuantity < 1){
		goodsQuantity = 1;
	}
	if(goodsQuantity > stockQuantity){
		alert('购买数量不能大于库存数量!');
		goodsQuantity = stockQuantity;
	}
	$.ajax({
		type: "post",
		url: webpath + "tools/submit_ajax.ashx?action=cart_goods_update",
		data: {
			"article_id" : articleId,
			"quantity" : goodsQuantity
		},
		dataType: "json",
		beforeSend: function(XMLHttpRequest) {
			//发送前动作
		},
		success: function(data, textStatus) {
			if (data.status == 1) {
				objInput.val(goodsQuantity);
				var totalPrice = parseFloat(goodsPrice)*goodsQuantity; //金额
				var totalPoint = parseFloat(goodsPoint)*goodsQuantity; //积分
				$(obj).parents(".cart_content").find("label[name='amountCount']").text(totalPrice.toFixed(2));
				if(totalPoint > 0){
				   $(obj).parents(".cart_content").find("label[name='pointCount']").text("+" + totalPoint);
				}else{
				   $(obj).parents(".cart_content").find("label[name='pointCount']").text(totalPoint);
				}
				cartAmountTotal(); //重新统计
			} else {
				alert(data.msg);
			}
		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			alert("状态：" + textStatus + "；出错提示：" + errorThrown);
		},
		timeout: 20000
	});
	return false;
}

//删除购物车商品
function deleteCart(webpath, obj){
	if(!confirm("您确认要从购物车中移除吗？")){
		return false;
	}
	//组合参数
	var datastr;
	var arglength = arguments.length; //参数个数
	if(arglength == 1){
		datastr = {"clear": 1}
	}else{
	   var articleId = $(obj).parents(".bl_select").find("input[name='hideArticleId']").val();
		datastr = {"article_id": articleId}
	}
	$.ajax({
		type: "post",
		url: webpath + "tools/submit_ajax.ashx?action=cart_goods_delete",
		data: datastr,
		dataType: "json",
		beforeSend: function(XMLHttpRequest) {
			//发送前动作
		},
		success: function(data, textStatus) {
			if (data.status == 1) {
				if(arglength == 1){
					location.reload();
				}else{
					$(obj).parents("tr").remove();
					cartAmountTotal(); //重新统计
				}
			} else {
				alert(data.msg);
			}
		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			alert("状态：" + textStatus + "；出错提示：" + errorThrown);
		},
		timeout: 20000
	});
	return false;
}

//选择商品
function selectCart(obj){
	var arglength = arguments.length; //参数个数
	if(arglength == 1){
		if($(obj).text()=="全选"){
			$(obj).text("取消");
			$(".checkall").prop("checked", true);
		}else{
			$(obj).text("全选");
			$(".checkall").prop("checked", false);
		}
	}
	cartAmountTotal(); //统计商品
}

//计算购物车金额
function cartAmountTotal(){
	var jsondata = ""; //商品组合参数
	var totalAmount = 0; //商品总计
	$(".checkall:checked").each(function(i) {
	   var articleId = $(this).parents(".cart_content").find("input[name='hideArticleId']").val(); //文章ID
	   var goodsPrice = $(this).parents(".cart_content").find("input[name='hideGoodsPrice']").val(); //商品单价
	   var goodsQuantity = $(this).parents(".cart_content").find("input[name='goodsQuantity']").val(); //购买数量
		totalAmount += parseFloat(goodsPrice) * parseFloat(goodsQuantity);
		jsondata += '{"article_id":"'+articleId+'", "quantity":"'+goodsQuantity+'"}';
		if(i < $(".checkall:checked").length-1){
			jsondata += ',';
		}
    });
	$("#totalQuantity").text($(".checkall:checked").length);
	$("#totalAmount").text(totalAmount.toFixed(2));
	if(jsondata.length > 0){
		jsondata = '[' + jsondata + ']';
	}
	$("#jsondata").val(jsondata);
}

//进入结算中心
function formSubmit(obj, webpath, linkurl){
	var jsondata = $("#jsondata").val();
	if(jsondata == ""){
		alert("未选中要购买的商品，请选中后提交！");
		return false;
	}
	//记住按钮文字
	var buttonText = $(obj).text();
	//加入购物清单
	$.ajax({
		type: "post",
		url: webpath + "tools/submit_ajax.ashx?action=cart_goods_buy",
		data: { "jsondata": jsondata },
		dataType: "json",
		beforeSend: function(XMLHttpRequest) {
			//发送前动作
			$(obj).prop("disabled",true).text("请稍候...");
		},
		success: function(data, textStatus) {
			if (data.status == 1) {
				location.href = linkurl;
			}else{
				$(obj).prop("disabled",false).text(buttonText);
				alert("尝试进入结算中心失败，请重试！");
			}
		},
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			$(obj).prop("disabled",false).text(buttonText);
			alert("状态：" + textStatus + "；出错提示：" + errorThrown);
		},
		timeout: 20000
	});
	return false;
}

/*初始化收货地址*/
function initUserAddress(obj){
	//初始化省市区
	var mypcas = new PCAS("province,所属省份","city,所属城市","area,所属地区");
}

//计算支付手续费总金额
function paymentAmountTotal(obj){
	var paymentPrice = $(obj).siblings("input[name='payment_price']").val();
	$("#paymentFee").text(paymentPrice); //运费
	orderAmountTotal();
}
//计算配送费用总金额
function freightAmountTotal(obj){
	var expressPrice = $(obj).siblings("input[name='express_price']").val();
	$("#expressFee").text(expressPrice); //运费
	orderAmountTotal();
}

//计算税金
function taxAmoutTotal(obj){
	var taxesFee = 0 //税费
	if($(obj).prop("checked") == true){
		taxesFee = parseFloat($("#taxAmout").val());
		$("#invoiceBox").show();
	}else{
		$("#invoiceBox").hide();
	}
	$("#taxesFee").text(taxesFee.toFixed(2));
	orderAmountTotal();
}

//计算订单总金额
function orderAmountTotal(){
	var goodsAmount = $("#goodsAmount").text(); //商品总金额
	var paymentFee = $("#paymentFee").text(); //手续费
	var expressFee = $("#expressFee").text(); //运费
	var taxesFee = 0 //税费
	if($("#is_invoice").prop("checked") == true){
		taxesFee = parseFloat($("#taxAmout").val());
	}
	//订单总金额 = 商品金额 + 手续费 + 运费 + 税费
	var totalAmount = parseFloat(goodsAmount) + parseFloat(paymentFee) + parseFloat(expressFee) + parseFloat(taxesFee);
	$("#totalAmount").text(totalAmount.toFixed(2));
}