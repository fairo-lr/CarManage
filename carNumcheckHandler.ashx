<%@ WebHandler Language="C#" Class="Hand" %>

using System;
using System.Web;
using MySql.Data.MySqlClient;
using System.Web.Configuration;
using System.Data.SqlClient;

public class Hand : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string carNum = context.Request["Num"];

        string connectionString_M = WebConfigurationManager.ConnectionStrings["Mysqls"].ConnectionString;

        string selectSQL = "SELECT * FROM carmanage WHERE CarPlateNumber='" + carNum + "'";
        
        MySqlConnection my_con = new MySqlConnection(connectionString_M);
        MySqlCommand my_cmd = new MySqlCommand(selectSQL, my_con); 
        my_con.Open();
        if (my_cmd.ExecuteScalar() == null)//如果为空则mysql数据库不存在此车牌号
        {
            context.Response.Write("N");
        }
        else
        {
            context.Response.Write("Y");
        }
        my_con.Close();
    }   

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}