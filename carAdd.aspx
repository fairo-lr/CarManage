<%@ Page Language="C#" AutoEventWireup="true" CodeFile="carAdd.aspx.cs" Inherits="rightManage_carAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
   table.addTable
   {
    position: relative;
    left: 5%;
   border-collapse:collapse; 
   border: 1px solid gray;
   width: 600px;
   height: 150px;
   }
  td.lefttd
  {
    border-right:1px solid gray;
   text-align:center; 
   }
   td.righttd
   {
  padding:5px;
   text-align:left;
   }
    td.toptd
    {
      border-top :1px solid gray;
     } 
   td.bottomtd
   {
      border-bottom :1px solid gray;
   } 
   b
   {
   color: Red; 
   font-size:small;
   }
   #txtCarNum
   {
   color: Red; 
   font-size:small;
   }
   .dbl
   {
position: relative;
top: 10px;
left: 29%;
Height: 30px;
width: 200px;
   }  
   .label_css
   {
   Font-Size:Larger;
   position: relative;
    left: 25%;
   } 
   .ttr1
   {
   background:#E0E0E0;
   }
   .ttr2
   {
   background:#fffffff;
   }
   </style> 
    <script type="text/javascript" src="javascript/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="javascript/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript" src="javascript/jQ_addCar.js"></script>

    <title>添加进场车辆</title>
</head>
<body style="font-size: 9pt;">
 
    <form runat="server" action="">
        <h3>
            添加车辆</h3>
        <table class="addTable" >
            <tr class="ttr1"  >
                <td class="lefttd" style="width: 150px">
                    车类<b>*</b></td>
                <td class="righttd">
                    <asp:DropDownList ID="droplist_1" runat="server" DataSourceID="DS_ddlcarClass" DataTextField="cc_name"
                        DataValueField="cc_id" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr class="ttr2">
                <td class="lefttd">
                    车牌号码<b>*</b></td>
                <td class="righttd">
                    <input type="text" id="carNum" maxlength="20" runat="server" style="text-transform: uppercase"
                        onfocus="startCarnum()"/>
                    <asp:Label ID="txtCarNum" runat="server"></asp:Label></td>
            </tr>
            <tr class="ttr1">
                <td class="lefttd">
                    车牌颜色</td>
                <td class="righttd">
                    <asp:DropDownList ID="carPcolor" runat="server" Width ="150px">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>黄色</asp:ListItem>
                        <asp:ListItem>蓝色</asp:ListItem>
                        <asp:ListItem>其他</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr class="ttr2">
                <td class="lefttd">
                    车身颜色</td>
                <td class="righttd">
                    <asp:DropDownList ID="carColor" runat="server"  Width ="150px" >
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>黑色</asp:ListItem>
                        <asp:ListItem>白色</asp:ListItem>
                        <asp:ListItem>香槟金</asp:ListItem>
                        <asp:ListItem>蓝色</asp:ListItem>
                        <asp:ListItem>红色</asp:ListItem>
                        <asp:ListItem>其他</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr class="ttr1">
                <td class="lefttd">
                    车型</td>
                <td class="righttd">
                    <input type="text" id="carStyle" runat="server" />
                </td>
            </tr>
            <tr class="ttr2">
                <td class="lefttd">
                    单位( 或车主)<b>*</b></td>
                <td class="righttd">
                    <input type="text" id="Owner" style="width: 200px" runat="server" />
                </td>
            </tr>
            <tr class="ttr1">
                <td class="lefttd bottomtd">
                    一次性进出场<b>*</b></td>
                <td class="righttd bottomtd">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                        RepeatLayout="Flow" >
                        <asp:ListItem Value="Y" Selected="True">是</asp:ListItem>
                        <asp:ListItem Value="N">否</asp:ListItem>
                    </asp:RadioButtonList>
                    <b>如果车辆只有一次进场权限请选“是” </b>
                </td>
            </tr>
            <tr class="TMclass ">
                <td colspan="2" class="lefttd" style="text-align:left;padding:10px;">
                    权限有效期<b>*</b></td>
            </tr>
            <tr class="TMclass">
                <td align="center" class="lefttd toptd">
                    开始时间</td>
                <td class="righttd toptd">
                    <input type="text" id="starTime" disabled="disabled" class="Wdate" onfocus="WdatePicker({maxDate:'#F{$dp.$D(\'endTime\')}',dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                        runat="server" style="background-color:#ECECE5;"/>
                </td>
            </tr>
            <tr class="TMclass">
                <td align="center" class="lefttd bottomtd">
                    结束时间
                </td>
                <td class="righttd bottomtd">
                    <input type="text" id="endTime" disabled="disabled" class="Wdate" onfocus="WdatePicker({minDate:'#F{$dp.$D(\'starTime\')}',dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                        runat="server" style="background-color:#ECECE5;" />
                </td>
            </tr>
            <tr class="ttr1">
                <td valign="top" class="lefttd bottomtd" style="vertical-align:middle;">
                    备注</td>
                <td class="righttd bottomtd">
                    <asp:TextBox ID="TexBox_other" runat="server" TextMode="MultiLine" Height="40px"
                        Width="200px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" class="lefttd" style="text-align:left;">
                    <b>*</b><strong style="font-size: small">为必填项</strong></td>
            </tr>
        </table>
        <div class="dbl">
            <asp:Button ID="Button_Add" Text="添加" Style="font-size: larger;" runat="server" OnClick="saveData"
                OnClientClick="return checkData()" />
            <asp:Label ID="Label_text" runat="server" CssClass="label_css" ForeColor="red"></asp:Label>
        </div>
        <div>
            <asp:SqlDataSource ID="DS_ddlcarClass" runat="server" ConnectionString="<%$ ConnectionStrings:Pubs  %>"
                ProviderName="System.Data.SqlClient" SelectCommand="SELECT cc_name,cc_id FROM dbo.CarClass">
            </asp:SqlDataSource>
            <asp:AccessDataSource ID="AccessDataSource1" runat="server" ConflictDetection="CompareAllValues"
                DataFile="user_mng.mdb" DeleteCommand="DELETE FROM [user_mng] WHERE [user_id] = ? AND [user_name] = ? AND [user_psw] = ?"
                InsertCommand="INSERT INTO [user_mng] ([user_id], [user_name], [user_psw]) VALUES (?, ?, ?)"
                OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [user_id], [user_name], [user_psw] FROM [user_mng] ORDER BY [user_id]"
                UpdateCommand="UPDATE [user_mng] SET [user_name] = ?, [user_psw] = ? WHERE [user_id] = ? AND [user_name] = ? AND [user_psw] = ?">
                <InsertParameters>
                    <asp:Parameter Name="user_id" Type="Int32" />
                    <asp:Parameter Name="user_name" Type="String" />
                    <asp:Parameter Name="user_psw" Type="String" />
                </InsertParameters>
                <DeleteParameters>
                    <asp:Parameter Name="original_user_id" Type="Int32" />
                    <asp:Parameter Name="original_user_name" Type="String" />
                    <asp:Parameter Name="original_user_psw" Type="String" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="user_name" Type="String" />
                    <asp:Parameter Name="user_psw" Type="String" />
                    <asp:Parameter Name="original_user_id" Type="Int32" />
                    <asp:Parameter Name="original_user_name" Type="String" />
                    <asp:Parameter Name="original_user_psw" Type="String" />
                </UpdateParameters>
            </asp:AccessDataSource>
        </div>
    </form>
</body>
</html>
