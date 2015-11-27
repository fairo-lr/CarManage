// JScript 文件
function startCarnum()
{
//        var oText=document.getElementById("check_carNum");
//        oText.value = "闽D";
//        var rtextRange =oText.createTextRange(); 
//        rtextRange.moveStart('character',oText.value.length); 
//        rtextRange.collapse(true); 
//        rtextRange.select();
}
function judgeCheck()
{
    var temp_oneTime = document.getElementById("CheckBox_oneTime").checked;
    var temp_sTime = document.getElementById("check_starTime").value;
    var temp_eTime = document.getElementById("check_endTime").value;
    var adjust = true; 
    if( temp_oneTime == true && temp_sTime !="" ){
        alert("\"一次性进场\"和\"日期\"不能同时设值！请重新设定...");
        adjust = false;
        }
    else  if( temp_oneTime == true && temp_eTime !="" ){
        alert("\"一次性进场\"和\"日期\"不能同时设值！请重新设定...");
        adjust = false;
        }     
    else if( temp_sTime !=""  && temp_eTime =="" ){
    alert("设置查询\"结束日期\"！");
   adjust = false;
   }
   else if(temp_sTime ==""  && temp_eTime !=""  ){
   alert("设置查询\"开始日期\"！");
   adjust = false;
   }            
//   alert('search...');
   return adjust; 
} 

function selectClear()
{  
 //       this.DropDownList1.ClearSelection();
         document.getElementById("DropDownList1").options[0].selected = true;
         document.getElementById("check_carNum").value = "";
         document.getElementById("check_starTime").value = "";
         document.getElementById("check_endTime").value = "";
         document.getElementById("CheckBox_oneTime").checked = false;
         document.getElementById("CheckBox_Delete").checked = false;
        return false;
}
