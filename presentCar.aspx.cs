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

public partial class presentCar : System.Web.UI.Page
{
    private List<presentCarclass> carDataList;//,carCkDataList;= new List<presentCarclass>();
    private List<presentCarclass> carDSortList;
    private string yes;

    protected void Page_Load(object sender, EventArgs e)
    {
        //check_privillage();
        if (!IsPostBack)
        {
            this.SqlDS_carClass.SelectCommand = "SELECT cc_id,cc_name FROM dbo.CarClass";
            //this.HidFiel_selectSQL.Value = "SELECT CarBoard,PassTime,comecartime FROM peccancy20130607  WHERE RunDirection = 1 ORDER BY PassTime DESC ";
            this.HidFiel_selectSQL.Value = "SELECT CarBoard,PassTime,comecartime FROM peccancy" + DateTime.Now.ToString("yyyyMMdd") + "  WHERE RunDirection = 1 ORDER BY PassTime DESC ";//+DateTime.Now.ToString("yyyyMMdd") 
            this.check_starTime.Value = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            this.check_endTime.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:00");
            this.HidFiel_selectSQL_sql.Value = "SELECT CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,CarClass,cc_name,IsOneTime,RangeSTTM,RangeEDTM,note,InitTime,RemoveTime FROM dbo.CarManage,dbo.CarClass WHERE cc_id = CarClass ";
        }
        else
        { }
        getInCarData();
    }

    //button of check
    protected void getPresentCheck(object sender, EventArgs e)
    {
        //查询
        //date check 
        DateTime timeS = Convert.ToDateTime(this.check_starTime.Value);
        DateTime timeE = Convert.ToDateTime(this.check_endTime.Value);
        System.Text.StringBuilder data = new System.Text.StringBuilder(null);

        //carNumber check
        if (this.check_carNum.Value != "")
        {
            if (this.check_carNum.Value == "未识别")
            {
                data.Append("AND CarBoard = '' ");
            }
            else
            {
                data.Append("AND CarBoard Like '%");
                data.Append(this.check_carNum.Value);
                data.Append("%'");
            }
        }
        dateCheck(timeS, timeE, data.ToString());
        getInCarData();
    }

    protected void dateCheck(DateTime Stadt, DateTime Enddt, string oCarSelect)
    {
        //from the span of date get all tables' name;
        List<string> TableNameList = new List<string>();
        //TimeSpan ts = Enddt.Subtract(Stadt);
        DateTime tmp = Stadt;
        string tableName = string.Empty; //要查询表
        while(true)
        { 
            tableName = tmp.ToString ("yyyyMMdd");
            if( 0 == tableName.CompareTo(Enddt.ToString("yyyyMMdd")))
                break;
            TableNameList.Add("peccancy" + tableName);
            tmp = tmp.AddDays(1);
        }
        TableNameList.Add("peccancy" + tableName);
     
        //产生进出场表的SQL查询语句
        string selectSQL_temp = "( SELECT CarBoard,PassTime,comecartime FROM " + TableNameList[0] + " WHERE RunDirection = 1 " + oCarSelect + " )";
        for (int i = 1; i < TableNameList.Count; i++)
        {
            //System.Windows.Forms.MessageBox.Show(TableNameList[i]);
            selectSQL_temp += " UNION ( SELECT CarBoard,PassTime,comecartime FROM " + TableNameList[i] + " WHERE RunDirection = 1 " + oCarSelect + " )";
        }
        this.HidFiel_selectSQL.Value = selectSQL_temp + "  ORDER BY PassTime DESC";
    }

    protected void getInCarData() 
    {
        //进出场车详细信息获取
        string connectionString = WebConfigurationManager.ConnectionStrings["Mysqls"].ConnectionString;
        string connectionString_sql = WebConfigurationManager.ConnectionStrings["Pubs"].ConnectionString;

        //设定查询语句：进出场表（selectSQL）、权限表（selectSQL_sql）
        string selectSQL = this.HidFiel_selectSQL.Value;

        string selectSQL_sql = this.HidFiel_selectSQL_sql.Value;
        // "SELECT CarPlateNum,CarPlateColor,CarColor,CarStyle,DriverName,CarClass,cc_name,IsOneTime,RangeSTTM,RangeEDTM,note,InitTime,RemoveTime FROM dbo.CarManage,dbo.CarClass WHERE cc_id = CarClass " + temp;

        //连接数据库(mysql)
        MySqlConnection my_con = new MySqlConnection(connectionString);
        MySqlCommand my_cmd = new MySqlCommand(selectSQL, my_con);
        MySqlDataReader reader;

        //sql_server
        //当进出表有车牌记录，就查询权限表确认该车牌是否为可通行车辆。
        //若是，显示车辆信息。若否，标记为违禁车辆
        SqlConnection con = new SqlConnection(connectionString_sql);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        SqlDataReader s_reader;
        carDataList = new List<presentCarclass>();

        try
        {
            my_con.Open();
            reader = my_cmd.ExecuteReader();
            //process reading
            while (reader.Read())
            {
                presentCarclass carData = new presentCarclass();

                DateTime tmp_timeS = Convert.ToDateTime(this.check_starTime.Value);
                DateTime tmp_timeE = Convert.ToDateTime(this.check_endTime.Value);
                DateTime tmp_InYardT = reader.IsDBNull(1) ? DateTime.MinValue : Convert.ToDateTime(reader["PassTime"]);

                if (-1 == tmp_InYardT.CompareTo(tmp_timeS)) // || 1 == tmp_InYardT.CompareTo(tmp_timeE))
                    //进场时间小于查询开始时间、大于结束时间的排除
                    continue;
                else if (1 == tmp_InYardT.CompareTo(tmp_timeE))
                    continue;

                /*时间段内进出场车辆记录*/
                carData.carPlateNum = reader["CarBoard"].ToString();     
                carData.InYardTime = reader.IsDBNull(1) ? null : Convert.ToDateTime(reader["PassTime"]).ToString("yy-MM-dd HH:mm");
                carData.OutYardTime = reader.IsDBNull(2) ? null : Convert.ToDateTime(reader["comecartime"]).ToString("yy-MM-dd HH:mm"); //here try to define null value /pass?

                //若车牌号不为空，同权限表比较判断该车牌是否违禁
                if (carData.carPlateNum != "")
                {
                    //查询sql_server的语句。依靠车牌号、该记录创建时间是否小于其入场时间
                    string selectSQL_temp = selectSQL_sql + " AND CarPlateNum = '" + carData.carPlateNum + "' AND InitTime < '" + carData.InYardTime + "' ORDER BY InitTime DESC ";
                    cmd.CommandText = selectSQL_temp;
                    //查询权限表，获取车辆基本信息
                    try
                    {
                        con.Open();
                        s_reader = cmd.ExecuteReader();
                        s_reader.Read();
                        if (s_reader.HasRows)//车牌存在
                        {
                            carData.carPlateColor = s_reader["CarPlateColor"].ToString();
                            carData.carColor = s_reader["CarColor"].ToString();
                            carData.isOneTime = Convert.ToChar(s_reader["IsOneTime"]);
                            carData.carClass = s_reader["cc_name"].ToString();//车类
                            carData.carStyle = s_reader["CarStyle"].ToString();
                            carData.carNote = s_reader["note"].ToString();
                            carData.carOwner = s_reader["DriverName"].ToString();
                            carData.RangeSTTM = Convert.ToDateTime(s_reader["RangeSTTM"]).ToString("yy-MM-dd HH:mm:ss");
                            carData.RangeEDTM = Convert.ToDateTime(s_reader["RangeEDTM"]).ToString("yy-MM-dd HH:mm:ss"); //s_reader["RangeEDTM"].ToString();
                            carData.DeleteTime = s_reader.IsDBNull(12) ? "0" : Convert.ToDateTime(s_reader["RemoveTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                            //进行比较
                            if (carData.DeleteTime.ToString() != "0" && 0 > carData.DeleteTime.CompareTo(carData.InYardTime))
                            {
                                //已删除记录存在删除时间，且删除时间小于其进场时间的
                                carData.illegalCar = 'Y';
                                carData.carNote = "权限超时";
                            }
                        }
                        else
                        {
                            carData.illegalCar = 'Y';
                            carData.carNote = "未登记";
                        }
                        s_reader.Close();
                    }
                    catch (Exception err)
                    {
                        con.Close();
                        throw;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                
                if (this.Checkbox_illegal.Checked == true && carData.illegalCar != 'Y')  //查询判断式（gridview仅显示违禁车辆）
                    continue;
                else
                    carDataList.Add(carData);

            } 
            getcarCount(carDataList.Count);//获取车辆数量
            reader.Close();
        }
        catch (Exception err)
        {
            my_con.Close();
            throw;
        }
        finally
        {
            my_con.Close();
            bindDataSource();
        }
    }

    protected void GridView_nowCar_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //gridview's special fields show
        if (DataControlRowType.DataRow == e.Row.RowType) //&& e.Row.RowState != DataControlRowState.Edit)
        {
            if (e.Row.Cells[0].Text == "&nbsp;")
            {
                e.Row.Cells[0].Text = "“未识别”";
            }

            if (e.Row.Cells[2].Text == "01-01-01 00:00")
            {
                e.Row.Cells[2].Text = String.Empty;
            }
            if (e.Row.Cells[5].Text == "Y")
            {
                e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;
                e.Row.Cells[11].Text = String.Empty;
                e.Row.Cells[12].Text = String.Empty;
            }
            if (e.Row.Cells[5].Text == "N")
            {
                e.Row.Cells[5].Text = String.Empty;
            }
            if (e.Row.Cells[12].Text == "导入的数据")
            {
                e.Row.Cells[12].Text = String.Empty;
            }
            if (e.Row.Cells[3].Text == "Y")
            {
                e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void bindDataSource()
    {
        this.GridView_nowCar.DataSourceID = String.Empty;
        this.GridView_nowCar.DataSource = carDataList;
        this.GridView_nowCar.DataBind();
    }
    //display car records count
    protected void getcarCount(int count)
    {
        this.Lab_count.Text = "共有" + count + "条记录";
    }
    //changing gridview's pages 
    protected void GridView_nowCar_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.GridView_nowCar.PageIndex = e.NewPageIndex;
        if (this.HidFiel_sort.Value == "sorted")
        {
            sortList(this.HidFiel_sortExpression.Value, this.HidFiel_sortDirection.Value);// sortExpression, sortDirection);
        }

        this.GridView_nowCar.DataSource = carDataList;

        this.GridView_nowCar.DataBind();
    }

    //GridView 列项 排序
    protected void GridView_nowCar_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression.ToString();
        string sortDirection = "ASC";

        if (sortExpression == this.GridView_nowCar.Attributes["SortExpression"])
        {
            sortDirection = (this.GridView_nowCar.Attributes["SortDirection"].ToString() == sortDirection ? "DESC" : "ASC");
        }

        this.GridView_nowCar.Attributes["SortExpression"] = sortExpression;
        this.GridView_nowCar.Attributes["SortDirection"] = sortDirection;

        sortList(sortExpression, sortDirection);

        this.GridView_nowCar.DataSource = carDataList;
        this.GridView_nowCar.DataBind();
        this.HidFiel_sort.Value = "sorted";
        this.HidFiel_sortDirection.Value = sortDirection;
        this.HidFiel_sortExpression.Value = sortExpression;

    }

    private void sortList(string sortExpression, string sortDirection)
    {
        if (sortExpression == "InYardTime")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
            {
                return sortDirection == "DESC" ?
                    Comparer<string>.Default.Compare(car2.InYardTime, car1.InYardTime) : Comparer<string>.Default.Compare(car1.InYardTime, car2.InYardTime);
            });
            return;
        }

        if (sortExpression == "OutYardTime")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
            {
                return sortDirection == "DESC" ?
                    Comparer<string>.Default.Compare(car1.OutYardTime, car2.OutYardTime) : Comparer<string>.Default.Compare(car2.OutYardTime, car1.OutYardTime);
            });
            return;
        }

        if (sortExpression == "illegalCar")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
            {
                return sortDirection == "DESC" ?
                    Comparer<char>.Default.Compare(car1.illegalCar, car2.illegalCar) : Comparer<char>.Default.Compare(car2.illegalCar, car1.illegalCar);
            });
        }

        if (sortExpression == "isOneTime")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
            {
                return sortDirection == "DESC" ?
                    Comparer<char>.Default.Compare(car1.isOneTime, car2.isOneTime) : Comparer<char>.Default.Compare(car2.isOneTime, car1.isOneTime);
            });
        }

        if (sortExpression == "RangeSTTM")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
                {
                    return sortDirection == "DESC" ?
                        Comparer<string>.Default.Compare(car1.RangeSTTM, car2.RangeSTTM) : Comparer<string>.Default.Compare(car2.RangeSTTM, car1.RangeSTTM);
                });

        }

        if (sortExpression == "carClass")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
                {
                    return sortDirection == "DESC" ?
                        Comparer<string>.Default.Compare(car1.carClass, car2.carClass) : Comparer<string>.Default.Compare(car2.carClass, car1.carClass);
                });

        }

        if (sortExpression == "RangeEDTM")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
            {
                return sortDirection == "DESC" ?
                    Comparer<string>.Default.Compare(car1.RangeEDTM, car2.RangeEDTM) : Comparer<string>.Default.Compare(car2.RangeEDTM, car1.RangeEDTM);
            });

        }

        if (sortExpression == "carNote")
        {
            carDataList.Sort(delegate(presentCarclass car1, presentCarclass car2)
            {
                return sortDirection == "DESC" ?
                    Comparer<string>.Default.Compare(car1.carNote, car2.carNote) : Comparer<string>.Default.Compare(car2.carNote, car1.carNote);
            });
        }

    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
           ConvertExcel.toExcel(this, this.GridView_nowCar);
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
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

    protected void GridView_nowCar_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
