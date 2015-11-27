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
/// presentCarclass 的摘要说明
/// </summary>
public class presentCarclass
{
    public presentCarclass()
    {
        m_carPlateNum = string.Empty;
        m_carPlateColor = string.Empty;
        m_carColor = string.Empty;
        m_isOneTime = '-';
        m_carClass = string.Empty;
        m_carColor = string.Empty;
        m_carNote = string.Empty;
        m_carOwner = string.Empty;
        m_RangeSTTM = string.Empty;
        m_RangeEDTM = string.Empty;
        m_InYardTime = string.Empty;
        m_OutYardTime = string.Empty;
        m_illegalCar = ' ';
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

    //车辆基本信息
    private string m_carPlateNum;//车牌号
    private string m_carColor; //车身色
    private string m_carPlateColor; //车牌色
    private string m_carOwner;//车归属
    private string m_carStyle;//车身型
    private string m_carClass;//车辆分类
    private char m_isOneTime;//是否一次进场车
    private char m_illegalCar;//违禁车辆标识
    private string m_carNote;//车辆备注
    private string m_RangeSTTM;//进出有效期
    private string m_RangeEDTM;//
    private string m_DeleteTime;
    //进出场信息
    private string m_InYardTime;//进场时间
    private string m_OutYardTime;//出场时间
    
    public string carPlateNum
    {
        get { return m_carPlateNum; }
        set { m_carPlateNum = value; }
    }

    public string carColor
    {
        get { return m_carColor; }
        set { m_carColor = value; }
    }

    public string carPlateColor
    {
        get { return m_carPlateColor; }
        set { m_carPlateColor = value; }
    }

    public string carStyle
    {
        get { return m_carStyle; }
        set { m_carStyle = value; }
    }

    public string carClass
    {
        get { return m_carClass; }
        set { m_carClass = value; }
    }

    public char isOneTime
    {
        get { return m_isOneTime; }
        set { m_isOneTime = value; }
    }

    public char illegalCar
    {
        get { return m_illegalCar; }
        set { m_illegalCar = value; }
    }

    public string carNote
    {
        get { return m_carNote; }
        set { m_carNote = value; }
    }

    public string carOwner
    {
        get { return m_carOwner; }
        set { m_carOwner = value; }
    }

    public string RangeSTTM
    {
        get { return m_RangeSTTM; }
        set { m_RangeSTTM = value; }
    }

    public string RangeEDTM
    {
        get { return m_RangeEDTM; }
        set { m_RangeEDTM = value; }
    }

    public string DeleteTime
    {
        get { return m_DeleteTime; }
        set { m_DeleteTime = value; }
    }
    /**********************************/
    public string InYardTime
    {
        get { return m_InYardTime; }
        set { m_InYardTime = value; }
    }

    public string OutYardTime
    {
        get { return m_OutYardTime; }
        set { m_OutYardTime = value; }
    }


}
