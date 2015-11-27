<%@ Page Language="C#" EnableEventValidation = "false" AutoEventWireup="true" CodeFile="carMain.aspx.cs" Inherits="_Default" %>

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
.buttonCss
{
height:30px;
width:60px;
}
.Label_css
{
float:left;
}
.hl_carAddCss
{
position: absolute;
left: 23pt;
top: 40pt;
}
</style>
    <title>车辆管理系统</title>

    <script type="text/javascript" src="javascript/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="javascript/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript" src="javascript/JS_checkCar.js"></script>

</head>
<body style="font-size: 9pt;">
    <form runat="server" action="">
        <h3 style="left: 23pt; position: absolute; top: 17pt;">
            车辆权限管理</h3>
        <asp:HyperLink ID="HyperLink_carAdd" runat="server" CssClass="hl_carAddCss" NavigateUrl="~/carAdd.aspx"
            Font-Underline="True"> <strong>+</strong> 添加车辆</asp:HyperLink>
        <table class="carMainTable">
            <tr>
                <td align="right" style="width: 250pt;" valign="middle" rowspan="2">
                </td>
                <td valign="middle" style="width: 200pt;" align="right">
                    <asp:CheckBox ID="CheckBox_oneTime" runat="server" Text="一次性进场车" TextAlign="Left"
                        Width="100px" Height="20px" />
                </td>
                <td valign="middle" style="width: 200pt;" align="right">
                    车牌号&nbsp;<input type="text" id="check_carNum" runat="server" style="text-transform: uppercase;
                        width: 100px;" onfocus="startCarnum()" />
                </td>
                <td valign="middle" align="right" style="width: 250pt;">
                    权限开始时间
                    <input type="text" id="check_starTime" class="Wdate" onfocus="WdatePicker({maxDate:'#F{$dp.$D(\'check_endTime\')}',dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                        runat="server" /></td>
                <td valign="middle" style="width: 193pt; background-color: #ffffff;" rowspan="2"
                    align="right">
                    &nbsp;</td>
                <td valign="bottom" style="width: 188pt; background-color: #ffffff;" rowspan="2"
                    align="right">
                    <asp:Button ID="Button_Check" CssClass="buttonCss" runat="server" Text="查询" OnClientClick="return judgeCheck()"
                        OnClick="getCheck" />
                    <asp:Button ID="Button_Clear" CssClass="buttonCss" runat="server" Text="清空" OnClientClick="return selectClear()" /></td>
            </tr>
            <tr>
                <td valign="middle" style="width: 200pt" align="right">
                    <asp:CheckBox ID="CheckBox_Delete" runat="server" Text="仅删除车辆" TextAlign="Left" Width="100px"
                        Height="20px" />
                </td>
                <td valign="middle" style="width: 200pt" align="right">
                    车类&nbsp;<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDS_carClass"
                        DataTextField="cc_name" DataValueField="cc_id" OnDataBound="DropDownList1_DataBound">
                    </asp:DropDownList>
                </td>
                <td valign="middle" rowspan="2" align="right" style="width: 250pt">
                    至
                    <input type="text" id="check_endTime" runat="server" class="Wdate" onfocus="WdatePicker({minDate:'#F{$dp.$D(\'check_starTime\')}',dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 920pt; background-color: #E8F7FC;">
            <tr>
                <td>
                    <asp:Label ID="Label_carCounts" runat="server" CssClass="Label_css" ForeColor="red"></asp:Label>
                </td>
                <td align="right">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/document.png" OnClick="ImageButton1_Click" Height="22px" Width="22px" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="Gridv_carData" runat="server" BackColor="White" BorderColor="#E0E0E0"
            BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDS_1" AllowSorting="True"
            AutoGenerateColumns="False" AllowPaging="True" OnRowDataBound="Gridv_carData_RowDataBound"
            OnRowDeleting="Gridv_carData_RowDeleting" DataKeyNames="SerialNum" ForeColor="Navy"
            Width="920pt" Font-Size="9pt" HorizontalAlign="Center" PageSize="20" EmptyDataText="No Records Found.">
            <RowStyle BackColor="White" ForeColor="#003399" HorizontalAlign="Center" />
            <FooterStyle BackColor="#006699" ForeColor="#003399" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" Width="20px" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:BoundField DataField="SerialNum" HeaderText="序列号" SortExpression="SerialNum"
                    InsertVisible="False">
                    <ItemStyle Width="5px" />
                </asp:BoundField>
                <asp:BoundField DataField="CarPlateNum" HeaderText="车牌号" >
                    <HeaderStyle Width="60pt" />
                </asp:BoundField>
                <asp:BoundField DataField="IsRemove" HeaderText="已删除" SortExpression="IsRemove">
                    <ItemStyle Width="5pt" />
                    <HeaderStyle Width="5pt" />
                </asp:BoundField>
                <asp:BoundField DataField="cc_name" HeaderText="车类" />
                <asp:BoundField DataField="IsOneTime" HeaderText="一次进场(Y/N)" SortExpression="IsOneTime">
                    <ItemStyle Width="20pt" />
                </asp:BoundField>
                <asp:BoundField DataField="DriverName" HeaderText="单位(车主)" />
                <asp:BoundField DataField="CarPlateColor" HeaderText="车牌颜色">
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="CarColor"  HtmlEncode="False"  HeaderText="车体&lt;br /&gt;颜色">
                    <ItemStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="CarStyle" HeaderText="车型" />
                <asp:BoundField DataField="note" HeaderText="备注" />
                <asp:BoundField DataField="RangeSTTM" HeaderText="权限开始时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="RangeSTTM" HtmlEncode="False"></asp:BoundField>
                <asp:BoundField DataField="RangeEDTM" HeaderText="权限结束时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="RangeEDTM" HtmlEncode="False" />
                <asp:BoundField DataField="InitTime" HeaderText="创建时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="InitTime" HtmlEncode="False" />
                <asp:BoundField DataField="RemoveTime" HeaderText="删除时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="RemoveTime" HtmlEncode="False" />
                <asp:HyperLinkField Text="修改" DataNavigateUrlFields="SerialNum" DataNavigateUrlFormatString="~/updateCar.aspx?SerialNum={0}" />
                <asp:CommandField ShowCancelButton="False" ShowDeleteButton="True">
                    <HeaderStyle Width="20px" />
                </asp:CommandField>
            </Columns>
            <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="None" BorderColor="#B2C9D3" />
            <EmptyDataRowStyle BackColor="#E0E0E0" />
        </asp:GridView>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDS_1" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" HorizontalAlign="Center" Visible="False">
            <RowStyle BackColor="White" ForeColor="#003399" HorizontalAlign="Center" />
            <FooterStyle BackColor="#006699" ForeColor="#003399" />
            <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" Width="20px" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:BoundField DataField="CarPlateNum" HeaderText="车牌号">
                    <HeaderStyle Width="60pt" />
                </asp:BoundField>
                <asp:BoundField DataField="IsRemove" HeaderText="已删除" SortExpression="IsRemove">
                    <ItemStyle Width="25pt" />
                    <HeaderStyle Width="25pt" />
                </asp:BoundField>
                <asp:BoundField DataField="cc_name" HeaderText="车类" />
                <asp:BoundField DataField="IsOneTime" HeaderText="一次进场(Y/N)" SortExpression="IsOneTime">
                    <ItemStyle Width="25pt" />
                    <HeaderStyle Width="25pt" />
                </asp:BoundField>
                <asp:BoundField DataField="DriverName" HeaderText="单位(车主)" />
                <asp:BoundField DataField="CarPlateColor" HeaderText="车牌颜色">
                    <ItemStyle Width="30px" />
                </asp:BoundField>
                <asp:BoundField DataField="CarColor" HtmlEncode="False" HeaderText="车体&lt;br /&gt;颜色">
                    <ItemStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="CarStyle" HeaderText="车型" />
                <asp:BoundField DataField="note" HeaderText="备注" />
                <asp:BoundField DataField="RangeSTTM" HeaderText="权限开始时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="RangeSTTM" HtmlEncode="False"></asp:BoundField>
                <asp:BoundField DataField="RangeEDTM" HeaderText="权限结束时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="RangeEDTM" HtmlEncode="False" />
                <asp:BoundField DataField="InitTime" HeaderText="创建时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="InitTime" HtmlEncode="False" />
                <asp:BoundField DataField="RemoveTime" HeaderText="删除时间" DataFormatString="{0:yy-MM-dd HH:mm}"
                    SortExpression="RemoveTime" HtmlEncode="False" />
            </Columns>
        </asp:GridView>
        
        <asp:SqlDataSource ID="SqlDS_1" OnSelected="getCarCount" runat="server" ConnectionString="<%$ ConnectionStrings:Pubs  %>"
            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDS_carClass" runat="server" ConnectionString="<%$ ConnectionStrings:Pubs  %>"
            ProviderName="System.Data.SqlClient"></asp:SqlDataSource>
        <asp:HiddenField ID="HidFiel_checkSQL" runat="server" />
        <%--   <asp:SqlDataSource ID="SqlDS_mySQL" runat="server" ConnectionString="<%$ ConnectionStrings:Mysqls  %>"
            ProviderName="System.Data.Odbc"></asp:SqlDataSource> --%>
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
