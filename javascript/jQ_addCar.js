// JScript 文件
var G_judge = false; //全局变量 判断车牌号exsit
$(document).ready(function () {

	$("#RadioButtonList1").click(function () {
		if ($("input[name='RadioButtonList1']:checked").val() == 'N') {
			$("#starTime").removeAttr("disabled");
			$("#endTime").removeAttr("disabled");
			$("#starTime").css("background-color", "Transparent");
			$("#endTime").css("background-color", "Transparent");
		} else {
			$("#starTime").val("");
			$("#endTime").val("");
			$("#starTime").attr("disabled", true);
			$("#endTime").attr("disabled", true);
			$("#starTime").css("background-color", "#ECECE5");
			$("#endTime").css("background-color", "#ECECE5");
		}
	});
});

function startCarnum() {
	var oText = document.getElementById("carNum");
	oText.value = "闽D";
	var rtextRange = oText.createTextRange();
	rtextRange.moveStart('character', oText.value.length);
	rtextRange.collapse(true);
	rtextRange.select();
}

function checkData() {

	//检查各信息项输入是否为空
	if ($("#carNum").val() == "闽D" || $("#carNum").val() == "") {
		alert("车牌号未填");
		return false;
	}

	/*
	bug:如果页面刷新，则点击按钮不会进行车牌检测，就会出现两个相同的车牌记录
	 *需要修改
	 */
//	if (G_judge) {
//		alert("该车牌号已存在");
//		return false;
//	}

	if ($("#Owner").val() == "") {
		alert("单位未填");
		return false;
	}
}

function checkNumOne(Num) {
	$.ajax({
		type : "POST",
		url : "./carNumcheckHandler.ashx",
		data : {
			"Num" : Num
		},
		success : function (exist) {
			if (exist == "Y") {
				G_judge = true;
				$("#txtCarNum").text("该车牌号已存在");

			} else {
				G_judge = false;
				$("#txtCarNum").text("");
			}
		}
	});
}