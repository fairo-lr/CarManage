<%@ Page Language="C#" AutoEventWireup="true" CodeFile="presentCar.aspx.cs" Inherits="presentCar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
table.carMainTable
{
   border-collapse:collapse; 
   border-width: 1px;
   border-style:solid; 
   background-color: transparent; 
   left: 100pt;
   top: 0pt; 
   width: 920pt;
 }
.table_Count
{
position:relative;
background-color: #E8F7FC;
width:920pt;
}
.buttonCss
{
height:30px;
width:60px;
}
.excelCss
{
float:right;
}

</style>
    <title>进出场车辆</title>

    <script type="text/javascript" src="javascript/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="javascript/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript" src="javascript/presentCar_Check.js"></script>

</head>
<body style="font-size: 9pt;">
    <form id="form1" runat="server">
        <h3  style="left: 23pt; position: absolute; top: 17pt;">
            车辆进出场记录</h3>
        <table class="carMainTable">
            <tr>
                <td align="right" style="width: 249pt;" valign="middle" rowspan="2">
                </td>
                <td valign="baseline" align="right" style="width: 450px;">
                    车辆进场日期&nbsp;<input type="text" id="check_starTime" class="Wdate" onfocus="WdatePicker({maxDate:'#F{$dp.$D(\'check_endTime\')}',startDate:'%y-%M-%d 08:00:00',alwaysUseStartDate:true,dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                        runat="server" />
                </td>
                <td valign="baseline" align="right">
                    车牌号&nbsp;
                    <input type="text" id="check_carNum" runat="server" style="text-transform: uppercase;
                        width: 93px;" onfocus="carNum()" /></td>
                <td rowspan="2" valign="top" align="right" style="width: 300pt;">
                    <asp:CheckBox ID="Checkbox_illegal" runat="server" Text="仅违禁车辆" TextAlign="Left"
                        Height="31px" />
                </td>
                <td valign="middle" style="width: 200pt; background-color: #ffffff;" rowspan="2"
                    align="right">
                    &nbsp;</td>
                <td valign="bottom" style="width: 188pt; background-color: #ffffff;" rowspan="2"
                    align="right">
                    <asp:Button ID="Button_Check" CssClass="buttonCss" runat="server" Text="查询" OnClick="getPresentCheck"
                        OnClientClick="return TimeCheck()" />
                    <asp:Button ID="Button_Clear" CssClass="buttonCss" runat="server" Text="清空" OnClientClick="return Clear()" />
                </td>
            </tr>
            <tr>
                <td valign="middle" style="width: 450px; height: 26px;" align="right">
                    至&nbsp;<input type="text" id="check_endTime" runat="server" class="Wdate" onfocus="WdatePicker({minDate:'#F{$dp.$D(\'check_starTime\')}',maxDate:'%y-%M-%d',startDate:'%y-%M-%d %H:00:00',alwaysUseStartDate:true,dateFmt:'yyyy-MM-dd HH:mm:ss'})" /></td>
                <td valign="middle" style="width: 300pt; height: 26px;" align="right">
                </td>
            </tr>
        </table>
        <br />
        <table class="table_Count">
            <tr>
                <td>
                    <asp:Label ID="Lab_count" runat="server" CssClass="countCss" ForeColor="red"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" CssClass="excelCss" ImageUrl="~/document.png"
                        OnClick="ImageButton1_Click" Height="22px" Width="22px" />
                </td>
            </tr>
        </table>
        <div>
            <asp:GridView ID="GridView_nowCar" runat="server" AutoGenerateColumns="False" BackColor="White"
                BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" Font-Size="9pt"
                AllowPaging="True" OnPageIndexChanging="GridView_nowCar_PageIndexChanging" PageSize="20"
                EmptyDataText="No Records Found." Width="920pt" HorizontalAlign="Center" OnRowDataBound="GridView_nowCar_RowDataBound"
                AllowSorting="True" OnSorting="GridView_nowCar_Sorting" OnSelectedIndexChanged="GridView_nowCar_SelectedIndexChanged">
                <RowStyle BackColor="White" ForeColor="#003399" HorizontalAlign="Center" />
                <Columns>
                    <asp:BoundField DataField="carPlateNum" HeaderText="车牌号" />
                    <asp:BoundField DataField="InYardTime" HeaderText="进场时间" DataFormatString="{0:yy-MM-dd HH:mm}" HtmlEncode="False" SortExpression="InYardTime"/>
                    <asp:BoundField DataField="OutYardTime" HeaderText="出场时间" DataFormatString="{0:yy-MM-dd HH:mm}"  HtmlEncode="False" SortExpression="OutYardTime"/>
                    <asp:BoundField HeaderText="违禁&lt;br /&gt;(Y/N)" HtmlEncode="False" DataField="illegalCar" SortExpression="illegalCar" />
                    <asp:BoundField DataField="carClass" HeaderText="车类" SortExpression="carClass"/>
                    <asp:BoundField DataField="isOneTime" HeaderText="一次进场(Y/N)" SortExpression="isOneTime">
                        <ItemStyle Width="20pt" />
                        <HeaderStyle Width="20pt" />
                    </asp:BoundField>
                    <asp:BoundField DataField="carOwner" HeaderText="单位（车主）" />
                    <asp:BoundField DataField="carPlateColor" HeaderText="车牌色" />
                    <asp:BoundField DataField="carColor" HeaderText="车身色"  />
                    <asp:BoundField DataField="carStyle" HeaderText="车型" />
                    <asp:BoundField DataField="RangeSTTM" HeaderText="权限开始时间" DataFormatString="{0:yy-MM-dd HH:mm}" HtmlEncode="False" SortExpression="RangeSTTM" />
                    <asp:BoundField DataField="RangeEDTM" HeaderText="权限结束时间" DataFormatString="{0:yy-MM-dd HH:mm}" HtmlEncode="False" SortExpression="RangeEDTM"/>
                    <asp:BoundField DataField="carNote" HeaderText="备注" SortExpression="carNote" />
                </Columns>
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" Width="20px" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="#E0E0E0" />
            </asp:GridView>
        </div>
        <asp:HiddenField ID="HidFiel_selectSQL" runat="server" />
        <asp:HiddenField ID="HidFiel_selectSQL_sql" runat="server" />
        <asp:HiddenField ID="HidFiel_sort" runat="server" />
        <asp:HiddenField ID="HidFiel_sortExpression" runat="server" />
        <asp:HiddenField ID="HidFiel_sortDirection" runat="server" />
        <asp:SqlDataSource ID="SqlDS_carClass" runat="server" ConnectionString="Data Source=172.30.6.200;Initial Catalog=Car_manage;User ID=sa;password=xsct.it"
            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDS_nowCar" runat="server" 
            ProviderName="System.Data.Odbc" ConnectionString="DSN=carManage;DESC=MySQL ODBC 3.51 Driver DSN;DATABASE=testcar;SERVER=172.30.23.1;UID=root;PORT=3306;OPTION=3;STMT=;"></asp:SqlDataSource>
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
    </form>
</body>
</html>
<%-- 车类&nbsp;<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDS_carClass"
                        DataTextField="cc_name" DataValueField="cc_id" OnDataBound="DropDownList1_DataBound">
                    </asp:DropDownList>--%>