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
        if (!this.IsPostBack)
        {
            string succeed = Request["dataNum"];
            if (succeed != null) this.Label_text.Text = succeed +"添加成功";
            FillList();
        }
    }
    /*
     * FillList(): 连接数据库，访问车辆类型表(dbo.CarClass)
     * 实现:dropdownlist控件和carClass的动态链接
     */
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

    //protected void Init_Page()
    //{
    //    this.carNum.Value = "";
    //    this.carPcolor.Items[0].Selected = true;
    //    this.carColor.Items[0].Selected = true;
    //    this.droplist_1.Items[0].Selected = true;
    //    this.carStyle.Value = "";
    //    this.RadioButtonList1.SelectedValue = "Y";
    //    this.starTime.Value = "";
    //    this.endTime.Value = "";
    //    this.Owner.Value = "";
    //    this.TexBox_other.Text = "";
    //}
    
    /*
     * Button_Add_ServerClick(object, EventArgs):添加按钮事件响应函数
     * 整理新添车辆信息，保存在data（StringBuilder类型）里
     */
    protected void saveData(object sender, EventArgs e)
    {
        insertInMysql();
        insertInsql();
        //global::System.Windows.Forms.MessageBox.Show("添加成功");
    }

    private void insertInMysql()
    {
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
        MySqlCommand my_cmd = new MySqlCommand();
        my_cmd.Connection = my_con;
        my_cmd.CommandType = CommandType.Text;
        
        try
        {
            my_cmd.CommandText = getID_SQL;
            my_con.Open();
            my_cmd.ExecuteNonQuery();
            int id = Convert.ToInt32(my_cmd.ExecuteScalar().ToString()) + 1;
            string insertSQL = "INSERT INTO carmanage(SerialNum,CarPlateNumber,CarPlateColor,CarColor,CarStyle,CompanyName) VALUES (" + id + "," + data.ToString() + ")";
            my_cmd.CommandText = insertSQL;
            my_cmd.ExecuteNonQuery();
        }
        catch (Exception err)
        {
            Response.Write(err.Message);
            my_con.Close();
            throw;
        }
        finally
        {
           my_con.Close();
        }
    }
  
    private void insertInsql()
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
        data.Append(this.TexBox_other.Text);
        data.Append("'");

        string insertSQL = "INSERT INTO dbo.CarManage (CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,CarClass,IsOneTime,RangeSTTM,RangeEDTM,note,InitTime) VALUES (" + data.ToString() + ",'"+DateTime.Now.ToString()+ "')";
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(insertSQL,con);
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
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
            Response.Redirect("carAdd.aspx?dataNum="+this.carNum.Value.ToUpper());
        }
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