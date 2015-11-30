using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

public partial class _Default : System.Web.UI.Page
{
    #region SQL
    // 查询Carmanage表内的所有车辆
    string SelectSQL = @"
            SELECT SerialNum,CarPlateNum
            ,CarPlateColor,CarColor,CarStyle,DriverName,cc_name,IsRemove
            ,IsOneTime,RangeSTTM,RangeEDTM,InitTime,RemoveTime,note 
            FROM dbo.CarManage,dbo.carClass 
            WHERE dbo.CarManage.CarClass = dbo.carClass.cc_id 
            "; 
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        check_privillage();
        if (!IsPostBack)
        {
            this.HidFiel_checkSQL.Value = SelectSQL+ @" 
            and IsRemove is  null  
            ORDER BY InitTime DESC 
            ";
        }
        this.SqlDS_1.SelectCommand = this.HidFiel_checkSQL.Value;

    }

    protected void ddl_carClass_DataBound(object sender, EventArgs e)
    {
        ListItem onelist = new ListItem(" 所有", "0");
        this.ddl_carClass.Items.Insert(0, onelist);
    }
    //Search 
    protected void getCheck(object sender, EventArgs e)
    {
        System.Text.StringBuilder tempSQL = new System.Text.StringBuilder();
        tempSQL.Append(SelectSQL);
        
        //是否一次进场
        if (this.CheckBox_oneTime.Checked)
        {
            tempSQL.Append("AND IsOneTime='Y'");
        } 
   
        //车牌号
        if (this.check_carNum.Value != "")
        {
            tempSQL.Append("AND CarPlateNum Like '%");
            tempSQL.Append(this.check_carNum.Value);
            tempSQL.Append("%'");
        }
        else
        {
            //是否删除车辆
            if (this.ddl_IsDelete.SelectedValue == "Y")
            {
                tempSQL.Append("AND IsRemove='Y'");
            }
            else if (this.ddl_IsDelete.SelectedValue == "N")
            {
                tempSQL.Append("AND IsRemove is null ");//测试库跟实际库不同
            }
            else
            {
                //查看未删和删除;
            }
        }
        //车辆类型
        if (this.ddl_carClass.SelectedValue != "0")
        {
            tempSQL.Append(" AND CarClass='");
            tempSQL.Append(this.ddl_carClass.SelectedItem.Value);
            tempSQL.Append("' ");
        }
        //权限起始时间
        if (this.check_starTime.Value != "" && this.check_endTime.Value != "")
        {
            DateTime date1 = Convert.ToDateTime(this.check_starTime.Value.ToString());
            DateTime date2 = Convert.ToDateTime(this.check_endTime.Value.ToString());

            tempSQL.Append(" AND RangeSTTM BETWEEN '");
            tempSQL.Append(date1.ToString());
            tempSQL.Append("' AND '");
            tempSQL.Append(date2.ToString());
            tempSQL.Append("'");
        }

        tempSQL.Append(" ORDER BY InitTime DESC");

        this.HidFiel_checkSQL.Value = tempSQL.ToString();
        this.SqlDS_1.SelectCommand = this.HidFiel_checkSQL.Value;
    }
    protected void Button_Clear_Click(object sender, EventArgs e)
    {
        Response.Redirect("carMain.aspx");
    } 

    protected void Gridv_carData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbt = (LinkButton)e.Row.FindControl("lbtDelete");
            lbt.CommandArgument = e.Row.RowIndex.ToString();
        }
    }
    protected void Gridv_carData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (DataControlRowType.DataRow == e.Row.RowType)
        { 
            LinkButton lbt = (LinkButton)e.Row.FindControl("lbtDelete"); 
            HyperLink hyl = e.Row.Cells[14].Controls[0] as HyperLink;

            if (e.Row.Cells[2].Text == "Y")
            {  
                e.Row.Enabled = false;
                lbt.Enabled = false;
                hyl.NavigateUrl = null;
            } 
            if (e.Row.Cells[4].Text == "Y")
            {
                e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[10].Text = string.Empty;
                e.Row.Cells[11].Text = string.Empty;
            }
        }
    }
    protected void Gridv_carData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            int indexs = Convert.ToInt32(e.CommandArgument);//获取点击行的序号

            int st_seriNum = Convert.ToInt32(this.Gridv_carData.DataKeys[indexs].Value); //序列号
            string st_carPlateNum = this.Gridv_carData.Rows[indexs].Cells[1].Text;

            int deleteID = DeleteFromMysql(st_carPlateNum);  //Delete From Mysql 
            DeleteFromSqlServer(st_seriNum, deleteID);    //Delete From SqlServer

            this.Label_Message.Text = st_carPlateNum + " 删除成功！";
            this.Gridv_carData.DataBind();
            //Response.Redirect("carMain.aspx");
        } 
    }
 
    protected int DeleteFromMysql(string carNumber)
    {
        int deleteID=0; // 被删除对象在Mysql里面的序列号
        string getIdSQL = string.Format(@"
        select SerialNum  from carmanage where carPlateNumber = '{0}' 
        ", carNumber);
     　
        MySqlConnection my_con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["Mysqls"].ConnectionString.ToString());
        MySqlCommand my_cmd = new MySqlCommand(getIdSQL, my_con);

        try
        {
            my_con.Open();
            object obj = my_cmd.ExecuteScalar();
            if (obj != null)
            {
                //删除
                deleteID = Convert.ToInt32(obj.ToString());
                my_cmd.CommandText = string.Format("delete from carmanage where carPlateNumber = '{0}' ", carNumber);
                my_cmd.ExecuteNonQuery();

                //更新序列号
                string updateSQL = "update carmanage set SerialNum = SerialNum-1 where SerialNum >" + deleteID + " order by SerialNum";
                my_cmd.CommandText = updateSQL;
                my_cmd.ExecuteNonQuery(); 
            }
            my_con.Close();
        }
        catch (Exception err)
        {
            my_con.Close();
            Response.Write(err.Message);
            throw;
        }
        return deleteID;
        //deleteID 作为是否在mysql中删除成功的参照物插入到sqlserver，note字段；
        //如果插入到sqlserver里面的deleteID为0证明mysql删除过程有些异常
        //反之则是被删除车号在mysql的id

    }

     protected void DeleteFromSqlServer(int serialNum, int myID)
    {
        string SeleteSQL = string.Format(@"
        SELECT note FROM dbo.CarManage WHERE SerialNum = {0} 
        ", serialNum); 
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Pubs"].ConnectionString.ToString());
        SqlCommand cmd = new SqlCommand(SeleteSQL, con);
        SqlDataReader read;

        try
        {
            con.Open();
            read = cmd.ExecuteReader();
            read.Read();
            string notes = " " + read["note"].ToString() == "导入的数据" ? "" : read["note"].ToString();//获取SqlServer note列里面的内容
            //删除的SQL
            string DeleteSQL = string.Format(@"
            UPDATE  dbo.CarManage 
            SET IsRemove = 'Y', RemoveTime ='{0}' , note = '{1}'
            WHERE SerialNum = {2}  
            ", DateTime.Now.ToString(), notes, serialNum);
            read.Close();

            cmd.CommandText = DeleteSQL;
            cmd.ExecuteNonQuery(); 
            con.Close();
        }
        catch (Exception err)
        {
            con.Close();
            throw;
        }
    }
    
    protected void getCarCount(object sender, SqlDataSourceStatusEventArgs e)
    {
        this.Label_carCounts.Text = "共有" + e.AffectedRows.ToString() + "记录";
    }

    protected void Button_ToExcel_Click(object sender, EventArgs e)
    {
        ConvertExcel.toExcel(this, this.GridView1, "");
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (DataControlRowType.DataRow == e.Row.RowType) //&& e.Row.RowState != DataControlRowState.Edit)
        {
            if (e.Row.Cells[3].Text == "Y")
            {
                e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[10].Text = string.Empty;
                e.Row.Cells[9].Text = string.Empty;
            }
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { }

    #region 权限测试
    public void check_privillage()
    {
        string Select_SQL, Select_SQL2, Select_SQL3, Update_SQL1;
        OleDbConnection SqlConnection1 = new OleDbConnection();
        SqlConnection1.ConnectionString = AccessDataSource1.ConnectionString;
        Select_SQL = "select module_name,page_name,user_name from module_user_mng,user_mng,module_mng where module_mng.module_id = module_user_mng.module_id and user_mng.user_id = module_user_mng.user_id and page_name='" + System.IO.Path.GetFileName(Request.PhysicalPath) + "' and user_name='" + Session["xsctintranet"] + "'";

        SqlConnection1.Open();
        OleDbCommand SqlCommand = SqlConnection1.CreateCommand();
        SqlCommand.CommandText = Select_SQL;
        OleDbDataReader SqlDataReader = SqlCommand.ExecuteReader();
        if (SqlDataReader.HasRows)
        {
            while (SqlDataReader.Read())
            {
                OleDbCommand SqlCommand2 = SqlConnection1.CreateCommand();
                Update_SQL1 = "insert into visit_mng (visit_module_name,visit_user_name,visit_date,visit_ip,visit_module_page) VALUES ('" + SqlDataReader.GetString(0) + "','" + SqlDataReader.GetString(2) + "','" + DateTime.Now.ToString() + "','" + Page.Request.UserHostAddress + "','" + SqlDataReader.GetString(1) + "')";
                SqlCommand2.CommandText = Update_SQL1;
                SqlCommand2.ExecuteNonQuery();

            }
        }
        else
        {
            OleDbCommand SqlCommand2 = SqlConnection1.CreateCommand();
            Select_SQL2 = "select * from module_mng where page_name='" + System.IO.Path.GetFileName(Request.PhysicalPath) + "'";
            SqlCommand2.CommandText = Select_SQL2;
            OleDbDataReader SqlDataReader2 = SqlCommand2.ExecuteReader();
            if (SqlDataReader2.HasRows)
            {
                Response.Write("您没有访问该页面的权限！");
                SqlDataReader2.Close();
                SqlDataReader.Close();
                SqlConnection1.Close();
                Response.End();
            }
            else
            {
                OleDbCommand SqlCommand3 = SqlConnection1.CreateCommand();
                Select_SQL3 = "select * from user_mng where user_name='" + Session["xsctintranet"] + "'";
                SqlCommand3.CommandText = Select_SQL3;
                OleDbDataReader SqlDataReader3 = SqlCommand3.ExecuteReader();
                if (SqlDataReader3.HasRows)
                { }
                else
                {
                    Response.Write("您没有访问该页面的权限！");
                    SqlDataReader3.Close();
                    SqlDataReader2.Close();
                    SqlDataReader.Close();
                    SqlConnection1.Close();
                    Response.End();
                }
                SqlDataReader3.Close();
            }
            SqlDataReader2.Close();
        }
        SqlDataReader.Close();
        SqlConnection1.Close();
    }
    #endregion 
}

#region Old Code 2014-3-11
//private bool IsDataRemoveInSqlServer(int serialNum, string carNum)
//{
//    bool TOrF = true;
//    string tempNum, tempIsRemove;
//    string selectSQL = string.Format("SELECT * FROM dbo.CarManage WHERE SerialNum = '{0}' ", serialNum);
//    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Pubs"].ConnectionString.ToString());
//    SqlCommand cmd = new SqlCommand(selectSQL, con);
//    SqlDataReader reader;

//    con.Open();
//    reader = cmd.ExecuteReader();
//    if (reader.Read())
//    {
//        tempNum = reader["CarPlateNum"].ToString();
//        tempIsRemove = reader["IsRemove"].ToString();
//        if (tempNum == carNum && tempIsRemove != "Y")
//        { TOrF = false; }
//    }
//    reader.Close();
//    con.Close();

//    return TOrF;
//}

#endregion 

 
