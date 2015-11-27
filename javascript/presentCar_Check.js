// JScript 文件
function carNum()
{
//        var oText=document.getElementById("check_carNum");
//        oText.value = "闽D";
//        var rtextRange =oText.createTextRange(); 
//        rtextRange.moveStart('character',oText.value.length); 
//        rtextRange.collapse(true); 
//        rtextRange.select();
}
function TimeCheck()
{
    var timeS = document.getElementById("check_starTime");
    var timeE = document.getElementById("check_endTime");
  
    if(timeS.value == "" && timeE.value != "")
   {
        alert("日期不完整");
        return false;  
   }
   
   if(timeS.value =="" && timeE.value == "")
   {
        var today = new Date();     
         
        var month = today.getMonth() + 1;    
         if (month <= 9)
             month = "0" + month;
             
        var day = today.getDate(); 
        if (day <= 9)
             day = "0" + day;  
             
        var date = today.getYear() + "-" + month + "-" + day;      
        timeS.value = date + " 00:00:00";
        timeE.value = date + " " + today.getHours() + ":" +today.getMinutes() + ":" +today.getSeconds();     
    }
   
   if(timeS.value != "" && timeE.value =="")
   {  
       timeE.value = timeS.value;
   }
    //  return false; 
}

function Clear()
{
     //    document.getElementById("DropDownList1").options[0].selected = true;
         document.getElementById("check_carNum").value = "";
         document.getElementById("check_starTime").value = "";
         document.getElementById("check_endTime").value = "";
        document.getElementById("Checkbox_illegal").checked = false;
        return false;
}
