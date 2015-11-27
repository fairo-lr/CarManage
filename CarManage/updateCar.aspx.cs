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

public partial class rightManage_updateCar : System.Web.UI.Page
{
    private string connectionString = WebConfigurationManager.ConnectionStrings["Pubs"].ConnectionString;
    private string connectionString_M = WebConfigurationManager.ConnectionStrings["Mysqls"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        //check_privillage();
        //int serial_num = 28;
        int serial_num = Convert.ToInt32(Request.QueryString[0].ToString());//获取更新车牌信息
        setInput();
        if (!this.IsPostBack)
        {
            FillList();
            getNewData(serial_num);
        }
    }
    private void setInput()
    {
        this.carNum.Disabled = true;
    }

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

    private void getNewData(int serial_num)
    {
        string selectSQL = "select CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,CarClass,IsRemove,IsOneTime,RangeSTTM,RangeEDTM,note from dbo.CarManage where  SerialNum = " + serial_num;

        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(selectSQL, con);
        SqlDataReader reader;

        try
        {
            con.Open();
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                //if (reader.GetValue(6).ToString() != "Y")
                //{
                    this.carNum.Value = reader.GetValue(0).ToString();
                    this.carPcolor.Items.FindByText(reader.GetValue(1).ToString()).Selected = true;
                    this.carColor.Items.FindByText(reader.GetValue(2).ToString()).Selected = true;
                    this.carStyle.Value = reader.GetValue(3).ToString();
                    this.Owner.Value = reader.GetValue(4).ToString();
                    int values = Convert.ToInt16(reader.GetValue(5));
                    this.droplist_1.Items[values - 1].Selected = true;
                    //this.RadioButtonList1.Items.FindByValue(reader.GetValue(7).ToString()).Selected = true;
                    if (reader.GetValue(7).ToString() == "N")
                    {
                        this.starTime.Value = reader.GetValue(8).ToString();
                        this.endTime.Value = reader.GetValue(9).ToString();
                    }
                    this.TexBox_other.Text = reader.GetValue(10).ToString();
                //}
                //else
                //{
                //    System.Windows.Forms.MessageBox.Show("该车已删除不能修改！");
                //产生一个可以跳转的网页
                //}
            }
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

    protected void updateData(object sender, EventArgs e)
    {
        updateInMysql();
        updateInsql();
    }

    private void updateInMysql()
    {
        string temp2 = this.carPcolor.SelectedItem.Value;
        string temp3 = this.carColor.SelectedItem.Value;
        System.Text.StringBuilder data = new System.Text.StringBuilder();
        data.Append("update carmanage set CarPlateColor ='");
        data.Append(temp2);
        data.Append("', CarColor ='");
        data.Append(temp3);
        data.Append("',CarStyle ='");
        data.Append(this.carStyle.Value);
        data.Append("',CompanyName='");
        data.Append(this.Owner.Value);
        data.Append("' where CarPlateNumber = '");
        data.Append(this.carNum.Value);
        data.Append("'");

        MySqlConnection my_con = new MySqlConnection(connectionString_M);
        MySqlCommand my_cmd = new MySqlCommand(data.ToString(), my_con);

        try
        {
            my_con.Open();
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

    private void updateInsql()
    {
        string temp = this.droplist_1.SelectedItem.Value;
        string temp2 = this.carPcolor.SelectedItem.Value;
        string temp3 = this.carColor.SelectedItem.Value;
        System.Text.StringBuilder data = new System.Text.StringBuilder();
        data.Append("update dbo.CarManage set CarClass ='");
        data.Append(temp);
        data.Append("', CarPlateColor ='");
        data.Append(temp2);
        data.Append("', CarColor ='");
        data.Append(temp3);
        data.Append("',CarStyle ='");
        data.Append(this.carStyle.Value);
        data.Append("',DriverName ='");
        data.Append(this.Owner.Value);
        if (this.RadioButtonList1.SelectedValue != "")
        {
            data.Append("',IsOneTime='");
            data.Append(this.RadioButtonList1.SelectedValue);
            data.Append("',RangeSTTM='");
            data.Append(this.starTime.Value);
            data.Append("',RangeEDTM='");
            data.Append(this.endTime.Value);
        }
        data.Append("',note='");
        data.Append(this.TexBox_other.Text);

        data.Append("' where CarPlateNum = '");
        data.Append(this.carNum.Value);
        data.Append("'");

        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(data.ToString(), con);

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
            //global::System.Windows.Forms.MessageBox.Show("已更新");
            //this.Label_infoMation.Text = "信息已更新";
            Response.Redirect("carMain.aspx");
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
#region 数据库记录项
// CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,CarClass,IsOneTime,RangeSTTM,RangeEDTM
//carmanage.SerialNum,carmanage.CarPlateNumber,
//carmanage.CarPlateColor,carmanage.CarColor,
//carmanage.CarStyle,carmanage.DriverName,
//carmanage.DriverPhone,
//carmanage.DriverIdentity,
//carmanage.CompanyPhone,
//carmanage.MemberDegrad,
//carmanage.DriverAddress,
//carmanage.CompanyName,
//carmanage.Carbrand,
//carmanage.CompanyAddress
#endregion
