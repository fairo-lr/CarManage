<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="carMain.aspx.cs"
    Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <style type="text/css">
.Search
{   
border-collapse:collapse; 
border: 1px solid #4D90FE; 
text-align:right;
width: 100%;
}
.Search td
{
padding:5px;
}
.buttonCss
{ 
margin-right:5px;
}
.Label_css
{ 
}
 .hl_carAddCss
{
 position: absolute;
 left: 30px;
 top: 20px;
} 
</style>
    <title>车辆管理系统</title>

    <script type="text/javascript" src="javascript/jquery-1.7.min.js"></script>

    <script type="text/javascript" src="javascript/My97DatePicker/WdatePicker.js"></script>

    <script type="text/javascript" src="javascript/JS_checkCar.js"></script>

    <script type="text/javascript">
    
    $(document).ready(function() {

    	$("#Gridv_carData").find(".del").click(function() {
    	if($(this).attr("disabled") != "disabled")
    	{
    		if (!confirm("是否确认删除?")) return false;
         }
    	});
    	
    }); 
    </script>

</head>
<body style="font-size: 9pt;">
    <form id="Form1" runat="server" >
        <h3 style="position: absolute; left: 12px; top: 10px;">
            车辆权限管理</h3>
        <span style="position: absolute; left: 136px; top: 12px; width: 100px;">
            <asp:HyperLink ID="HyperLink_carAdd" runat="server" CssClass="hl_carAddCss" NavigateUrl="~/carAdd.aspx"
                Font-Underline="True" Style="left: 3px; top: 0px"> 添加车辆</asp:HyperLink></span>
        <div style="width: 980px; position: absolute; top: 40px;">
            <!-- 查询-->
          <div> 
            <table class="Search">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddl_IsDelete" runat="server">
                            <asp:ListItem Value="N" Text="未删除"></asp:ListItem>
                                <asp:ListItem Value="Y" Text="已删除"></asp:ListItem>
                                <asp:ListItem Value="All" Text="所有"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBox ID="CheckBox_oneTime" runat="server" Text="一次进场" TextAlign="Left" />
                        </td>
                        <td align="right">
                            车号:</td>
                        <td align="left">
                            <input type="text" id="check_carNum" runat="server" style="text-transform: uppercase;
                                width: 60px;" onfocus="startCarnum()" />
                        </td>
                        <td align="right">
                            权限时间:
                        </td>
                        <td align="left">
                            <input type="text" id="check_starTime" class="Wdate" onfocus="WdatePicker({maxDate:'#F{$dp.$D(\'check_endTime\')}',dateFmt:'yyyy-MM-dd HH:mm'})"
                                runat="server" style="width: 110px" />
                            起
                            <input type="text" id="check_endTime" runat="server" class="Wdate" style="width: 110px"
                                onfocus="WdatePicker({minDate:'#F{$dp.$D(\'check_starTime\')}',dateFmt:'yyyy-MM-dd HH:mm'})" />
                            止
                        </td>
                        <td align="right">
                            车类:</td>
                        <td align="left">
                            <asp:DropDownList ID="ddl_carClass" runat="server" DataSourceID="DS_carClass" DataTextField="cc_name"
                                DataValueField="cc_id" OnDataBound="ddl_carClass_DataBound">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Button ID="Button_Check" CssClass="buttonCss" runat="server" Text="查询" OnClientClick="return judgeCheck()"
                                OnClick="getCheck" />
                            <asp:Button ID="Button_Clear" CssClass="buttonCss" runat="server" Text="清空" OnClick="Button_Clear_Click" />
                            <asp:Button ID="Button_ToExcel" CssClass="buttonCss" runat="server" Text="导出" OnClick="Button_ToExcel_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:Label ID="Label_Message" runat="server" CssClass="Label_css" ForeColor="red"></asp:Label></td>
                    <td align="right">
                        <asp:Label ID="Label_carCounts" runat="server" CssClass="Label_css" ForeColor="red"></asp:Label>
                    </td>
                </tr>
            </table>
            <!--  权限车辆列表 -->
            <div>
                <asp:GridView ID="Gridv_carData" runat="server" BackColor="White" BorderColor="#E0E0E0"
                    BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="SqlDS_1" AllowSorting="True"
                    AutoGenerateColumns="False" AllowPaging="True" OnRowDataBound="Gridv_carData_RowDataBound"
                    DataKeyNames="SerialNum" ForeColor="Navy" Width="100%" Font-Size="9pt" HorizontalAlign="Center"
                    PageSize="20" EmptyDataText="No Records Found." OnRowCommand="Gridv_carData_RowCommand"
                    OnRowCreated="Gridv_carData_RowCreated">
                    <RowStyle BackColor="White" ForeColor="#003399" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#006699" ForeColor="#003399" />
                    <PagerStyle BackColor="#006699" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" Width="20px" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:BoundField DataField="SerialNum" HeaderText="序列号" SortExpression="SerialNum"
                            Visible="False"></asp:BoundField>
                        <asp:BoundField DataField="CarPlateNum" HeaderText="车牌号">
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
                        <asp:HyperLinkField Text="修改" DataNavigateUrlFields="SerialNum" DataNavigateUrlFormatString="~/updateCar.aspx?SerialNum={0}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtDelete" runat="server" CssClass="del" CausesValidation="false"
                                    CommandName="del" Text="删除"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle BackColor="#E0E0E0" BorderStyle="None" BorderColor="#B2C9D3" />
                    <EmptyDataRowStyle BackColor="#E0E0E0" />
                </asp:GridView>
            </div>
            <!-- ToExcel-->
            <div>
                <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDS_1" AutoGenerateColumns="False"
                    OnRowDataBound="GridView1_RowDataBound" HorizontalAlign="Center" Visible="False">
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
            </div>
            <div>
                <asp:HiddenField ID="HidFiel_checkSQL" runat="server" />
                <asp:SqlDataSource ID="SqlDS_1" OnSelected="getCarCount" runat="server" ConnectionString="<%$ ConnectionStrings:Pubs  %>"
                    ProviderName="System.Data.SqlClient" DeleteCommand="" SelectCommand="SELECT SerialNum,CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,cc_name,IsRemove,IsOneTime,RangeSTTM,RangeEDTM,InitTime,RemoveTime,note FROM dbo.CarManage,dbo.carClass WHERE dbo.CarManage.CarClass = dbo.carClass.cc_id  ORDER BY InitTime DESC">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DS_carClass" runat="server" ConnectionString="<%$ ConnectionStrings:Pubs  %>"
                    ProviderName="System.Data.SqlClient" SelectCommand="SELECT cc_id,cc_name FROM dbo.CarClass">
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
        </div>
    </form>
</body>
</html>
