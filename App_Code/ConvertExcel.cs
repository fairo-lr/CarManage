using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// ConvertExcel 的摘要说明
/// </summary>
public class ConvertExcel
{
	public ConvertExcel()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public static void toExcel(Page page1, GridView GridView1)
    {
        page1.Response.AddHeader("content-disposition", "attachment;filename=newExcel.xls");
        page1.Response.ContentType = "application/vnd.xls";
        page1.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new HtmlTextWriter(sw);
   
        //GridView1.Visible = true;
        GridView1.AllowSorting = false;
        GridView1.AllowPaging = false;

        GridView1.DataBind();
        GridView1.RenderControl(htw);
        page1.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
        page1.Response.Write("<style type='text/css'>td{text-align:center;border:solid 1px Black;}</style>");
        if (GridView1.Rows.Count == 0)
        {
            page1.Response.Write("清单中无数据");
        }
        else
        {
            page1.Response.Write(sw.ToString());
        }
        page1.Response.Write("</body></html>");

        page1.Response.End();
    }

    public static void toExcel(Page page1, GridView GridView1, string strName)
    {
        //page1.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(strName, System.Text.Encoding.UTF8).ToString());
        page1.Response.AddHeader("content-disposition", "attachment;filename=newExcel.xls");
        page1.Response.ContentType = "application/vnd.xls";
        page1.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new HtmlTextWriter(sw);

        GridView1.Visible = true;
        GridView1.AllowSorting = false;
        GridView1.AllowPaging = false;
       
        GridView1.DataBind();
        GridView1.RenderControl(htw);
        page1.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">");
        page1.Response.Write("<style type='text/css'>td{text-align:center;border:solid 1px Black;}</style>");
        if (GridView1.Rows.Count == 0)
        {
            page1.Response.Write("清单中无数据");
        }
        else
        {
            page1.Response.Write(sw.ToString());
        }
        page1.Response.Write("</body></html>");

        page1.Response.End();
    }
    protected void toExcel(String string1)
    {
    }

}
