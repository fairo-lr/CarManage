using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

/*
 *添加新的通行车辆权限
 */
public partial class rightManage_carAdd : System.Web.UI.Page
{
    //connectionString_M mysql数据库连接串
    private string connectionString_M = WebConfigurationManager.ConnectionStrings["Mysqls"].ConnectionString;
    //connectionString sql_server数据库连接串
    private string connectionString = WebConfigurationManager.ConnectionStrings["Pubs"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        check_privillage();
        Response.Cache.SetNoStore();
        if (Session["IsSubmit"] != null)
        {
            if ((bool)Session["IsSubmit"])
            {
                Session["IsSubmit"] = false;
                string succeed = Request["dataNum"];
                if (succeed != null) this.Label_text.Text = succeed + "添加成功！";
            }
            else
                this.Label_text.Text = "";
        }
    }

    protected void saveData(object sender, EventArgs e)
    {
        if (IsNotExistDataInSqlServer() && IsNotExistDataInMySql())
        {
            InsertInMysql();
        }
        else
        {
            this.Label_text.Text = "添加失败[车号已存在]！";
        }
    }

    private void InsertInMysql()
    {
        string s_id = "-1";//插入到MySql里面的序号
        string temp2 = this.carPcolor.SelectedItem.Value;
        string temp3 = this.carColor.SelectedItem.Value;
        System.Text.StringBuilder data = new System.Text.StringBuilder();
        data.Append("'");
        data.Append(this.carNum.Value.ToUpper());
        data.Append("','");
        data.Append(temp2);
        data.Append("','");
        data.Append(temp3);
        data.Append("','");
        data.Append(this.carStyle.Value);
        data.Append("','");
        data.Append(this.Owner.Value);
        data.Append("'");

        string getID_SQL = "select Max(SerialNum)  from carmanage";
        MySqlConnection my_con = new MySqlConnection(connectionString_M);
        MySqlCommand my_cmd = new MySqlCommand(getID_SQL, my_con);
      
        try
        {
            my_con.Open();
            my_cmd.ExecuteNonQuery();
            int id = Convert.ToInt32(my_cmd.ExecuteScalar().ToString()) + 1;
            s_id=id.ToString();
            string insertSQL = "INSERT INTO carmanage(SerialNum,CarPlateNumber,CarPlateColor,CarColor,CarStyle,CompanyName) VALUES (" + id + "," + data.ToString() + ")";
            my_cmd.CommandText = insertSQL;
            if (my_cmd.ExecuteNonQuery() == 1)
            {
                InsertInSqlServer(s_id);
                Session["IsSubmit"] = true;
                Response.Redirect("carAdd.aspx?dataNum=" + this.carNum.Value.ToUpper());
            }
            else
            { 
                this.Label_text.Text = "添加异常[InMySql]！";
            }
            my_con.Close();
        }
        catch (Exception err)
        {
            my_con.Close();
            Response.Write(err.Message);
            throw;
        }
        
    }
    private void InsertInSqlServer(string s_id)
    {
        string temp = this.droplist_1.SelectedItem.Value;
        string temp2 = this.carPcolor.SelectedItem.Value;
        string temp3 = this.carColor.SelectedItem.Value;
        System.Text.StringBuilder data = new System.Text.StringBuilder();
        data.Append("'");
        data.Append(this.carNum.Value.ToUpper());
        data.Append("','");
        data.Append(temp2);
        data.Append("','");
        data.Append(temp3);
        data.Append("','");
        data.Append(this.carStyle.Value);
        data.Append("','");
        data.Append(this.Owner.Value);
        data.Append("',");
        data.Append(temp);
        data.Append(",'");
        data.Append(this.RadioButtonList1.SelectedValue);
        data.Append("','");
        data.Append(this.starTime.Value);
        data.Append("','");
        data.Append(this.endTime.Value);
        data.Append("','");
        data.Append(this.TexBox_other.Text + "+" + s_id);
        data.Append("'");

        string insertSQL = "INSERT INTO dbo.CarManage (CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,CarClass,IsOneTime,RangeSTTM,RangeEDTM,note,InitTime) VALUES (" + data.ToString() + ",'" + DateTime.Now.ToString() + "')";
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(insertSQL, con);

        try
        {
            con.Open();
            if (cmd.ExecuteNonQuery() != 1)
            {
                this.Label_text.Text = "添加异常[InSqlServer]！";
            }
            con.Close();
        }
        catch (Exception err)
        {
            con.Close();
            Response.Write(err.Message);
            throw;
        } 
    }

    private bool IsNotExistDataInMySql()
    {
        bool isNotExist = false;
        string carNum = this.carNum.Value.ToUpper();

        string selectSQL = string.Format("SELECT CarPlateNumber FROM carmanage WHERE CarPlateNumber='{0}' ", carNum);
        MySqlConnection my_con = new MySqlConnection(WebConfigurationManager.ConnectionStrings["Mysqls"].ConnectionString.ToString());
        MySqlCommand my_cmd = new MySqlCommand(selectSQL, my_con);

        my_con.Open();
        if (my_cmd.ExecuteScalar() == null)// 为空则mysql不存在此车牌号
        {
            isNotExist = true;
        }
        my_con.Close();

        return isNotExist;
    }
    private bool IsNotExistDataInSqlServer()
    {
        bool isNotExist = true;
        string tempNum, tempIsRemove;
        string carNum = this.carNum.Value.ToUpper();

        string selectSQL = string.Format("SELECT CarPlateNum,IsRemove FROM dbo.CarManage WHERE CarPlateNum = '{0}' ORDER BY InitTime DESC", carNum);
        SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Pubs"].ConnectionString.ToString());
        SqlCommand cmd = new SqlCommand(selectSQL, con);
        SqlDataReader reader;
        try
        {
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tempNum = reader["CarPlateNum"].ToString();
                tempIsRemove = reader["IsRemove"].ToString();
                if (tempNum == carNum && tempIsRemove == "Y")
                {
                    isNotExist = true;
                }
                else
                {
                    isNotExist = false;
                }
            }
            reader.Close();
            con.Close(); 
        }
        catch (Exception err)
        {
              con.Close();
            throw;
        }

        return isNotExist;
    }

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
/*
     * FillList(): 连接数据库,访问车辆类型表(dbo.CarClass)
     * 实现:dropdownlist控件和carClass的动态链接
 
    private void FillList()
    {
        droplist_1.Items.Clear();
        //gain carclass
        string selectSQL = "SELECT cc_name,cc_id FROM dbo.CarClass";
        //define ado.net object
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(selectSQL, con);
        SqlDataReader reader;
        //open the db
        try
        {
            con.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListItem newItem = new ListItem();
                newItem.Text = reader["cc_name"].ToString();
                newItem.Value = reader["cc_id"].ToString();
                droplist_1.Items.Add(newItem);
            }
            //droplist_1.Items[0].Selected = true;
            reader.Close();
        }
        catch (Exception err)
        {
            Response.Write(err.Message);
            con.Close();
            throw;
        }         
        finally
        {
            con.Close();
        }
    }
     */
