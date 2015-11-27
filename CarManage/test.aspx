<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head >
<style type="text/css">
#menu {
padding:0;
margin:0;
height:2em;
list-style-type:none;
border-left:1px solid #444;
background:#dfd7ca;
}
#menu li {
float:left; 
width:5em; 
height:2em;
line-height:2em;
border-right:1px solid #444;
position:relative;
text-align:center;
}
#menu li a, #menu li a:visited {
display:block;
text-decoration:none; 
color:#000;
}
#menu li a span, #menu li a:visited span {
display:none; /* needed to trigger IE */
}
#menu li a:hover {
border:0; 
}
#menu li a:hover span {
display:block;
width:5em;
height:2em;
text-align:center;
position:absolute; 
left:1px; 
top:1px; 
color:#fff; 
cursor:pointer;
}
#menu2 li:first-child:before { content: "";color:red; }
#menu2 li:before { content: "|"; padding-right: 10px; }


</style>
    <title>无标题页</title>
<script type="text/javascript">


</script>
</head>
<body>
<form runat="server" action="">


<ul id="menu">
<li><a href="#nogo">增加<span>增加</span></a></li>
<li><a href="#nogo">在场<span>在场</span></a></li>
<li><a href="#nogo">ITEM 3<span>ITEM 3</span></a></li>
<li><a href="#nogo">ITEM 4<span>ITEM 4</span></a></li>
<li><a href="#nogo">ITEM 5<span>ITEM 5</span></a></li>
</ul>
<div>
<ul id="menu2" style="text-align:center; display:inline;">
<li >添加</li>
<li >管理</li></ul></div>
<asp:Label ID="txtCN" runat="server"></asp:Label>
   <asp:Button id="Button_Add" Text="ADD" style="width: 102px; height: 63px; font-size:larger;" runat="server"  OnClientClick="return checkData()"  />
      </form>
</body>
</html>
<%--    <asp:SqlDataSource ID="Sql_mysql" runat="server" ConnectionString="Dsn=carManage" SelectCommand="SELECT * FROM carmanage" ProviderName="System.Data.Odbc"></asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" DataSourceID="Sql_mysql">
    </asp:GridView>
   --%>