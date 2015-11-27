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
    //private string connectionString = WebConfigurationManager.ConnectionStrings["Pubs"].ConnectionString;
    //private string checkSQL;

    protected void Page_Load(object sender, EventArgs e)
    {
        //check_privillage();
        if (!this.IsPostBack)
        {
            //dropdownlist 绑定cc_name数据
            this.SqlDS_carClass.SelectCommand = "SELECT cc_id,cc_name FROM dbo.CarClass";
            //调用hiddenfield控件保存SqlDS_1的查询命令
            HidFiel_checkSQL.Value = "SELECT SerialNum,CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,cc_name,IsRemove,IsOneTime,RangeSTTM,RangeEDTM,InitTime,RemoveTime,note FROM dbo.CarManage,dbo.carClass WHERE dbo.CarManage.CarClass = dbo.carClass.cc_id  ORDER BY InitTime DESC";
        }
        else
        {
            //GridviewControl.ResetGridView(this.Gridv_carData);
            //this.Gridv_carData.DataBind();
            //this.SqlDS_1.DataBind();
            //this.SqlDS_1.SelectCommand = "SELECT SerialNum,CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,cc_name,IsRemove,IsOneTime,RangeSTTM,RangeEDTM,InitTime,RemoveTime,note FROM dbo.CarManage,dbo.carClass WHERE dbo.CarManage.CarClass = dbo.carClass.cc_id";
        }
        this.SqlDS_1.SelectCommand = HidFiel_checkSQL.Value;
        //Gridv_carData.DataBind();
    }
    /*查询数据*/
    protected void getCheck(object sender, EventArgs e)
    {
        System.Text.StringBuilder data = new System.Text.StringBuilder();
        data.Append("SELECT SerialNum,CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,cc_name,IsRemove,IsOneTime,RangeSTTM,RangeEDTM,InitTime,RemoveTime,note FROM dbo.CarManage,dbo.carClass WHERE dbo.CarManage.CarClass = dbo.carClass.cc_id ");
        if (this.CheckBox_oneTime.Checked)
        {
            data.Append("AND IsOneTime='Y'");
        }
        if (this.CheckBox_Delete.Checked)
        {
            data.Append("AND IsRemove='Y'");
        }
        if (this.check_carNum.Value != "")
        {
            data.Append("AND CarPlateNum Like '%");
            data.Append(this.check_carNum.Value);
            data.Append("%'");
        }
        string var = this.DropDownList1.SelectedValue;
        if (this.DropDownList1.SelectedValue != "0")
        {
            data.Append(" AND CarClass='");
            data.Append(this.DropDownList1.SelectedItem.Value);
            data.Append("'");
        }

        if (this.check_starTime.Value != "" && this.check_endTime.Value != "")
        {
            DateTime date1 = Convert.ToDateTime(this.check_starTime.Value.ToString());
            DateTime date2 = Convert.ToDateTime(this.check_endTime.Value.ToString());

            data.Append(" AND RangeSTTM BETWEEN '");
            data.Append(date1.ToString());
            data.Append("' AND '");
            data.Append(date2.ToString());
            data.Append("'");
        }
        data.Append(" ORDER BY InitTime DESC");

        string checkSQL = data.ToString();
        this.HidFiel_checkSQL.Value = checkSQL;
        this.SqlDS_1.SelectCommand = this.HidFiel_checkSQL.Value;
    }

    //为车类多添加一空白行
    protected void DropDownList1_DataBound(object sender, EventArgs e)
    {
        ListItem onelist = new ListItem("", "0");
        this.DropDownList1.Items.Insert(0, onelist);
    }
    //“清空”按钮
    //protected void selectClear(object sender, EventArgs e)
    //{
    //    this.DropDownList1.ClearSelection();
    //    this.DropDownList1.Items[0].Selected = true;
    //    this.check_carNum.Value = "";
    //    this.CheckBox_oneTime.Checked = false;
    //    this.CheckBox_Delete.Checked = false;
    //    this.check_starTime.Value = "";
    //    this.check_endTime.Value = "";
    //}

    protected void Gridv_carData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //是数据列但不是在编辑状态 GridView中的列可能处在Header,Footer,DataRow等不同类型中
        if (DataControlRowType.DataRow == e.Row.RowType) //&& e.Row.RowState != DataControlRowState.Edit)
        {
            LinkButton lb = e.Row.Cells[15].Controls[0] as LinkButton;
            HyperLink hyl = e.Row.Cells[14].Controls[0] as HyperLink;//as HyperLinkField;
            if (e.Row.Cells[2].Text == "Y")
            {
                e.Row.Enabled = false;
                lb.Enabled = false;
                hyl.NavigateUrl = null;
            }
            if (e.Row.Cells[2].Text != "Y")
            {
                lb.Attributes.Add("onclick", "return confirm('确认删除吗?')");
            }
            if (e.Row.Cells[4].Text == "Y")
            {
                e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[10].Text = string.Empty;
                e.Row.Cells[11].Text = string.Empty;
            }
        }
        //处于编辑状态 由于GridView中的列分了普通和交替两中 要用e.Row.RowState&DataControlRowState.Edit!=0来判断
        if ((e.Row.RowState & DataControlRowState.Edit) != 0)
        {
            TextBox tb = e.Row.Cells[2].Controls[0] as TextBox;
            tb.Width = new Unit(100);
        }
    }

    //delete from sql_server
    protected void Gridv_carData_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //获取主键的值
        //string serial_num_s = this.Gridv_carData.DataKeys[e.RowIndex].Value.ToString();
        int serial_num = Convert.ToInt32(this.Gridv_carData.DataKeys[e.RowIndex].Value.ToString());
        string carNum = this.Gridv_carData.Rows[e.RowIndex].Cells[1].Text;
        this.SqlDS_1.DeleteCommand = "UPDATE  dbo.CarManage SET IsRemove = 'Y',RemoveTime ='" + DateTime.Now.ToString() + "' WHERE SerialNum =" + serial_num;
        deletedata(carNum);
    }

    //protected void Gridv_carData_RowDeleted(object sender, GridViewDeletedEventArgs e)
    //{
    //       this.SqlDS_1.DeleteCommand = "";
    //}

    protected void unbindDataSource()
    {
        this.Gridv_carData.DataSourceID = String.Empty;
        this.Gridv_carData.DataSource = SqlDS_1;
        this.Gridv_carData.DataBind();
    }
    protected void bindDataSource()
    {
        this.Gridv_carData.DataSourceID = this.SqlDS_1.ID.ToString();
    }

    //delete from mysql 从mysql数据库中删除数据
    protected void deletedata(string carNumber)
    {
        string deleteSQL = "delete from carmanage where carPlateNumber = '" + carNumber + "'";
        string getIDSQL = "select SerialNum from carmanage where carPlateNumber ='" + carNumber + "'";
        //string updateSQL = "update carmanage set SerialNum = SerialNum-1 where SerialNum>" + deleteID;
        string connectionString = WebConfigurationManager.ConnectionStrings["Mysqls"].ConnectionString;

        MySqlConnection my_con = new MySqlConnection(connectionString);
        MySqlCommand my_cmd = new MySqlCommand(getIDSQL, my_con);
        try
        {
            my_con.Open();
            my_cmd.ExecuteNonQuery();
            int deleteID = Convert.ToInt32(my_cmd.ExecuteScalar().ToString());
            my_cmd.CommandText = deleteSQL;
            my_cmd.ExecuteNonQuery();
            string updateSQL = "update carmanage set SerialNum = SerialNum-1 where SerialNum >" + deleteID + " order by SerialNum";
            my_cmd.CommandText = updateSQL;
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
     protected void getCarCount(object sender, SqlDataSourceStatusEventArgs e)
    {
        this.Label_carCounts.Text = "共有"+e.AffectedRows.ToString()+"记录";
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        ConvertExcel.toExcel(this, this.GridView1, "");
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
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
    //protected void Gridv_carData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    //this.Gridv_carData.PageIndex = e.NewPageIndex;
    //    //bindDataSource();
    //    this.Gridv_carData.DataBind();
    //}  

   
}