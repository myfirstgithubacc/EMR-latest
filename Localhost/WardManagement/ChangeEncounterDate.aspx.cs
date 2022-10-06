﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Telerik.Web.UI;
using BaseC;
using System.IO;
using System.Xml;

public partial class WardManagement_ChangeEncounterDate : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    DataSet ds;
    BaseC.ATD objatd;
    BaseC.clsEMRBilling baseEBill;
    BaseC.clsLISMaster objLISMaster;
    BaseC.EMRBilling.clsOrderNBill bOrdernBill;
    BaseC.WardManagement objwd;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            //if (common.myStr(Request.QueryString["EncId"]) != "")
            //{
            //    Session["StatusCngCheck"] = 0;
            //    BindPatientHiddenDetails(common.myStr(Request.QueryString["RegNo"]));
            //}
            dtpEod.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
            dtpEod.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
            dtpEod.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            dtpEod.MinDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));
            hdnIsPasswordRequired.Value = common.myStr(Request.QueryString["IsPasswordRequired"]);
        }
    }




    void BindPatientHiddenDetails(String RegistrationNo)
    {
        try
        {
            ViewState["StatusId"] = common.myStr(Request.QueryString["StatusId"]);
            if (Session["PatientDetailString"] != null)
            {
                lblPatientDetail.Text = Session["PatientDetailString"].ToString();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if(common.myBool(hdnIsPasswordRequired.Value))
        {
            IsValidPassword();
            return;
        }
        SaveData();
    }
    private void SaveData()
    {
        try
        {
            lblMessage.Text = string.Empty;
            DateTime? EOD = null;
            if (common.myStr(Request.QueryString["EncId"]) != "")
            {
                EOD = common.myDate(dtpEod.SelectedDate);
            }
            objwd = new BaseC.WardManagement();
            string str = "";
            objwd.UpdateEncouterProbableDischargeDate(common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), EOD);
            lblMessage.Text = "Date Updated!";
            if (str.Contains("Updated"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    //public string UpdateEncouterProbableDischargeDate(int EncounterId, int FacilityId, int UserId, DateTime? EDod)
    //{
    //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    Hashtable hstInput = new Hashtable();
    //    Hashtable houtPut = new Hashtable();
    //    DataSet ds = new DataSet();
    //    hstInput.Add("@intEncounterId", EncounterId);
    //    hstInput.Add("@intFacilityId", FacilityId);
    //    hstInput.Add("@ExpectedDateOfDischarge", EDod);
    //    //houtPut.Add("@chvErrorStatus", SqlDbType.VarChar);
    //   ds= objDl.FillDataSet(CommandType.StoredProcedure, "uspWardUpdateProbableDOD", hstInput);
    //    return "Updated";
    //}

    #region Transaction password validation
    private void IsValidPassword()
    {
        hdnIsValidPassword.Value = "0";
        RadWindow1.NavigateUrl = "/Pharmacy/Components/PasswordCheckerAllUser.aspx?UseFor=OPIP";
        RadWindow1.Height = 120;
        RadWindow1.Width = 340;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientIsValidPasswordClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void btnIsValidPasswordClose_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            if (common.myInt(hdnIsValidPassword.Value).Equals(0))
            {
                lblMessage.Text = "Invalid Username/Password!";
                return;
            }

            SaveData();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = Ex.Message;
            objException.HandleException(Ex);
        }
    }
    #endregion

}
