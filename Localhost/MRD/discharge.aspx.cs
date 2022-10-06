﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text;
using Telerik.Web.UI;
public partial class EMR_ATD_discharge : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl;
    BaseC.ATD objatd;
    BaseC.WardManagement objawm;
    SqlDataReader dr;
    String sqlstr = "";
    BaseC.ParseData bc = new BaseC.ParseData();
    DataSet ds;
    clsExceptionLog objException = new clsExceptionLog();
    bool isUpdateMrdDischrgePatient = false;
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString["MASTER"] != "No")
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
            //  btnClose.Visible = false;
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        objatd = new BaseC.ATD(sConString);

        if (!IsPostBack)
        {
            BindGrid();
            txtipno.Enabled = false;
            dvConfirm.Visible = false;
            dtptransferdate.SelectedDate = common.myDate(DateTime.Now);
            BaseC.Security objSecurity = new BaseC.Security(sConString);  //
            ViewState["IsAuthorizedForBackHourDischarge"] = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAuthorizedForBackHourDischarge");
            string BackHourAllowDischarge = common.GetFlagValueHospitalSetup(common.myInt(Session["hospitalLocationID"]), common.myInt(Session["FacilityId"]), "LimitHoursForBackDateDischarge", sConString);
            ViewState["BackHourAllowDischarge"] = BackHourAllowDischarge;
            isUpdateMrdDischrgePatient= common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "UpdateMrdDischrgePatient"));
            if (isUpdateMrdDischrgePatient && common.myInt(Request.QueryString["AcknowledmentStatus"]) == 1)
            {
                btnsave.Visible = true;
            }
            else
            {
                btnsave.Visible = false;
            }
            if (common.myBool(ViewState["IsAuthorizedForBackHourDischarge"]) == false)
            {
                dtptransferdate.Enabled = false;
            }

            if (Request.QueryString["Mpg"] != null)
                Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();

            if (common.myStr(Request.QueryString["MASTER"]) == "No")
            {
                btnCancel.Visible = false;
                btnClose.Visible = false;
            }
            else
            {
                // btnClose.Visible = false;
                //btnsave.Visible = false;
            }

            ltrRegNo.Text = common.myStr(Session["RegistrationLabelName"]);
            if (Convert.ToString(Request.QueryString["Regno"]) != null)
            {
                ds = new DataSet();
                txtregno.Text = Request.QueryString["Regno"].ToString();
                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                ds = objatd.GetRegistrationId(common.myInt(txtregno.Text));
                if (Convert.ToString(Request.QueryString["CF"]) == null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        hdnregno.Value = ds.Tables[0].Rows[0]["id"].ToString().Trim();
                        Session["RegistrationID"] = hdnregno.Value;
                        Session["encounterid"] = ds.Tables[0].Rows[0]["EncounterId"].ToString().Trim();
                        hdnRegistrationId.Value = ds.Tables[0].Rows[0]["id"].ToString().Trim();
                        hdnEncounterId.Value = common.myStr(common.myInt(ds.Tables[0].Rows[0]["EncounterId"]));
                        ViewState["DischargeMinDate"] = common.myDate(ds.Tables[0].Rows[0]["AdmissionDate"]);
                        Session["SaveDischargeCheck"] = 0;
                    }
                    else
                    {
                        lblmsg.Text = "Patient Can Not Find.";
                    }
                }
                else
                {
                    hdnregno.Value = Request.QueryString["Regno"].ToString();
                    Session["RegistrationID"] = Request.QueryString["RegId"].ToString();
                    Session["encounterid"] = Request.QueryString["EncID"].ToString();
                    hdnRegistrationId.Value = Request.QueryString["RegId"].ToString();
                    hdnEncounterId.Value = common.myStr(common.myInt(Request.QueryString["EncID"]));
                    ViewState["DischargeMinDate"] = common.myStr(Request.QueryString["EncDate"]);
                    Session["SaveDischargeCheck"] = 0;
                }
            }
            if (Convert.ToString(Request.QueryString["encno"]) != null)
            {

                Session["Encounterno"] = Request.QueryString["encno"].ToString().Trim();

            }
            if (Convert.ToString(Request.QueryString["BedId"]) != null)
            {
                ViewState["BedId"] = Request.QueryString["BedId"].ToString().Trim();
                ds = new DataSet();
                string sqlstr = " SELECT CurrentWardID,CurrentBedCategory,CurrentBillCategory,CurrentBedId, AdmissionDate  FROM Admission with(NoLock) ";
                sqlstr += "    WHERE RegistrationId=" + common.myInt(hdnregno.Value) + " and EncounterId=" + common.myInt(Session["Encounterid"]) + " and CurrentBedId=" + common.myInt(ViewState["BedId"]) + " ";
                ds = dl.FillDataSet(CommandType.Text, sqlstr);

                hdncWardId.Value = ds.Tables[0].Rows[0]["CurrentWardID"].ToString().Trim();
                hdncBedcategoryId.Value = ds.Tables[0].Rows[0]["CurrentWardID"].ToString().Trim();
                hdncBillingCategoryId.Value = ds.Tables[0].Rows[0]["CurrentWardID"].ToString().Trim();

            }
            /////Populate Patient discharge Status Dropdown

            ds = new DataSet();
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            objawm = new BaseC.WardManagement();
            DataSet ds1 = new DataSet();
            ds1 = objawm.GetDischargeStatusfromMRD(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["encounterid"]));
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ViewState["DischargeStatus"] = common.myStr(ds1.Tables[0].Rows[0]["DischargeStatus"]);
                txtDischargeRemarks.Text = common.myStr(ds1.Tables[0].Rows[0]["DischargeRemark"]);
                ddlreason.SelectedItem.Text = common.myStr(ds1.Tables[0].Rows[0]["Reason"]);
                dtptransferdate.SelectedDate = common.myDate(ds1.Tables[0].Rows[0]["DischargeDate"]);
            }

            ds = objatd.GetDischargeStatus();

            ddldischargestatus.DataSource = ds;
            ddldischargestatus.DataTextField = "Name";
            ddldischargestatus.DataValueField = "id";
            ddldischargestatus.DataBind();
            ddldischargestatus.Items.Insert(0, new ListItem("Select", "0"));

            lnkipno.Enabled = false;
            if (common.myStr(Request.QueryString["CF"]) == "MRD")
            {
                btnew_Click(null, null);
                Session["SaveDischargeCheck"] = 0;
                // ddldischargestatus.SelectedIndex = ddldischargestatus.Items.IndexOf(ddldischargestatus.Items.FindByText("EXPIRED"));
                //ddlreason.Enabled = false;
                //ddldischargestatus.Enabled = false;
                //chkbed.Enabled = false;
                //dtptransferdate.Enabled = false;
                btnCancel.Visible = false;
                btnClose.Visible = false;
                lnkipno.Enabled = true;
            }
            else
            {
                if (common.myInt(ViewState["DischargeStatus"]) != 0)
                {

                    ddldischargestatus.SelectedIndex = ddldischargestatus.Items.IndexOf(ddldischargestatus.Items.FindByValue(common.myStr(ViewState["DischargeStatus"])));

                   
                }
            }
            BindControls();
            dtpdeathdatetime.SelectedDate = null;
            if (common.myStr(ddldischargestatus.SelectedItem.Text).ToUpper() == "EXPIRED")
            {
                showdischargepanel();
              
                if (ds1.Tables[1].Rows.Count>0)
                {
                   
                    ddlExpiredReason.SelectedIndex = ddlExpiredReason.Items.IndexOf(ddlExpiredReason.Items.FindByValue(common.myStr(ds1.Tables[1].Rows[0]["ExpiredReasonId"])));
                    dtpdeathdatetime.SelectedDate = common.myDate(ds1.Tables[1].Rows[0]["DeathDate"]);
                    ddldepositionofbody.SelectedItem.Text= common.myStr(ds1.Tables[1].Rows[0]["BodyDeposition"]);
                    txtmodeoftransfer.Text= common.myStr(ds1.Tables[1].Rows[0]["Modeoftransfer"]);
                    txtbodyreceviedby.Text = common.myStr(ds1.Tables[1].Rows[0]["BodyReceivedBy"]);
                    txtauthorised.Text = common.myStr(ds1.Tables[1].Rows[0]["Authorisedburialpermission"]);

                }
                if (ds1.Tables[2].Rows.Count > 0)
                {
                    grddeathcause.DataSource = ds1.Tables[2];
                    grddeathcause.DataBind();
                }
               
            }

           
            // if (ViewState["DischargeMinDate"] != null)
            //    dtptransferdate.MinDate = common.myDate(ViewState["DischargeMinDate"]);

            if (Convert.ToString(Session["Encounterno"]) != "")
            {
                txtregno.Text = common.myStr(Request.QueryString["Regno"]) == "" ? "" : common.myStr(Request.QueryString["Regno"]);
                txtipno.Text = common.myStr(Request.QueryString["Encno"]) == "" ? "" : common.myStr(Request.QueryString["Encno"]);
                btnfind_Click(sender, e);
            }
            else
            {
                //  Response.Redirect("/Default.aspx?RegNo=0");
            }
            txtregno.Focus();
          
            dvConfirmPrint.Visible = false;
            fillFacility();

            //chkbed.Checked = common.myBool(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsCheckStillOnBedByDefault", sConString));
        }
        if (Convert.ToString(txtregno.Text.Trim()) != "")
        {
            BindPatientHiddenDetails(common.myInt(txtregno.Text)); ;
        }
        //Om Prakash 24-01-2017
        if (common.myStr(ViewState["Source"]) == "ER")
        {
            ddldischargestatus.Enabled = false;
        }
        else
        {
            ddldischargestatus.Enabled = false;
            BaseC.Security objsec = new BaseC.Security(sConString);
            ddldischargestatus.Enabled = common.myBool(objsec.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowToChangeDischargeStatusAtIPBill"));
            objsec = null;
        }
        //Done 

    }
    void BindPatientHiddenDetails(int RegistrationNo)
    {

        if (RegistrationNo > 0)
        {
            BaseC.ParseData bParse = new BaseC.ParseData();
            BaseC.Patient bC = new BaseC.Patient(sConString);
            BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
            int HospId = common.myInt(Session["HospitalLocationID"]);
            int FacilityId = common.myInt(Session["FacilityId"]);
            int RegId = 0;

            int EncodedBy = common.myInt(Session["UserId"]);
            DataSet ds = new DataSet();
            BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
            ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, RegId, RegistrationNo, EncodedBy, 0, "");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    lblPatientName1.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblMobile.Text = common.myStr(dr["MobileNo"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                    lblAdmissionDate.Text = common.myStr(dr["AdmissionDate"]);
                    ViewState["RegistrationId"] = common.myStr(dr["RegistrationId"]);
                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    ViewState["Source"] = common.myStr(dr["Source"]);
                    lblmsg.Text = "";
                }
            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblmsg.Text = "Patient not found !";
                return;
            }
        }
    }
    protected void btnshowdeath_Click(object sender, EventArgs e)
    {
        try
        {
            if (pnldeathdetails.Visible == false)
            {
                pnldeathdetails.Visible = true;

                dtpdeathdatetime.SelectedDate = null;
            }
            else
            {
                pnldeathdetails.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void find()
    {
        try
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            if (Request.QueryString["mode"] == "edit" || Request.QueryString["CF"] == "MRD")
            {

                if (Request.QueryString["CF"] == "MRD")
                {
                    if (isUpdateMrdDischrgePatient && common.myInt(Request.QueryString["AcknowledmentStatus"]) == 1)
                    {
                        btnsave.Visible = true; btnCancel.Visible = false;
                    }
                }
                else
                {
                    btnsave.Visible = false; btnCancel.Visible = true;
                }

                Hashtable hshtableout = new Hashtable();
                Hashtable hshtablein = new Hashtable();

                hshtablein.Add("inyHosppitallocationId", Session["HospitalLocationID"]);
                hshtablein.Add("@chvEncounterNo", txtipno.Text.Trim());
                hshtablein.Add("@intRegistrationId", bc.ParseQ(hdnRegistrationId.Value.Trim()));

                dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = dl.FillDataSet(CommandType.StoredProcedure, "UspGetDischargePatient", hshtablein);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtipno.Enabled = false;
                    ViewState["DischargeId"] = ds.Tables[0].Rows[0]["DischargeID"].ToString().Trim();
                    hdnregno.Value = ds.Tables[0].Rows[0]["RegistrationID"].ToString().Trim();
                    ddldischargestatus.SelectedIndex = ddldischargestatus.Items.IndexOf(ddldischargestatus.Items.FindByValue(ds.Tables[0].Rows[0]["DischargedStatus"].ToString().Trim()));
                    dtptransferdate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DischargeDate"].ToString().Trim());
                    //ddlreason.SelectedItem.Text = ds.Tables[0].Rows[0]["Reason"].ToString().Trim();
                    ddlreason.SelectedIndex = ddlreason.Items.IndexOf(ddlreason.Items.FindByText(ds.Tables[0].Rows[0]["Reason"].ToString().Trim()));
                    // chkbed.Checked=
                    if (ddldischargestatus.SelectedItem.Text.ToUpper() == "EXPIRED")
                    {
                        showdischargepanel();
                        dtpdeathdatetime.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DeathDate"].ToString().Trim());
                        ddldepositionofbody.SelectedItem.Text = ds.Tables[0].Rows[0]["BodyDeposition"].ToString().Trim();
                        txtbodyreceviedby.Text = ds.Tables[0].Rows[0]["BodyReceivedBy"].ToString().Trim();
                        txtmodeoftransfer.Text = ds.Tables[0].Rows[0]["Modeoftransfer"].ToString().Trim();
                        txtauthorised.Text = ds.Tables[0].Rows[0]["Authorisedburialpermission"].ToString().Trim();

                        ddlExpiredReason.SelectedIndex = ddlExpiredReason.Items.IndexOf(ddlExpiredReason.Items.FindByValue(common.myStr(ds.Tables[0].Rows[0]["ExpiredReasonId"])));
                        txtOtherExpiredRemarks.Text = ds.Tables[0].Rows[0]["ExpiredReasonOther"].ToString().Trim();

                        StringBuilder i = new StringBuilder();
                        foreach (GridDataItem item in grddeathcause.MasterTableView.Items)
                        {
                            i.Append("I");
                            DropDownList ddldoctor = (DropDownList)item.FindControl("ddldoctor");
                            ddldoctor.SelectedValue = ds.Tables[0].Rows[0]["Attendingdoctor" + i].ToString().Trim();

                            TextBox txtDescription = (TextBox)item.FindControl("txtDescription");
                            txtDescription.Text = ds.Tables[0].Rows[0]["Deathcause" + i].ToString().Trim();

                        }
                    }
                }
                else
                {//////For Patient Info
                    BindPatientHiddenDetails(common.myInt(txtregno.Text));

                    /////End Patient Info

                    ViewState["IpNo"] = Convert.ToString(hshtableout["IpNo"]);
                    ViewState["RegNo"] = hdnregno.Value.Trim();

                    if (common.myInt(hshtableout["DischatreId"]) > 0)
                    {
                        ViewState["DischargeId"] = Convert.ToString(hshtableout["DischatreId"]);


                        dtptransferdate.SelectedDate = Convert.ToDateTime(hshtableout["DischargeDate"]);
                        ddldischargestatus.SelectedValue = Convert.ToString(hshtableout["DischargedstatusID"]);
                        //ddlreason.SelectedItem.Text = Convert.ToString(hshtableout["Reason"]);
                        ddlreason.SelectedIndex = ddlreason.Items.IndexOf(ddlreason.Items.FindByText(ds.Tables[0].Rows[0]["Reason"].ToString().Trim()));
                        // ddldischargestatus_SelectedIndexChanged(sender, e);
                        showdischargepanel();
                        //txtreason.Text = Convert.ToString(hshtableout["Reason"]);
                        if (Convert.ToString(hshtableout["DeathDate"]) != "")
                            dtpdeathdatetime.SelectedDate = Convert.ToDateTime(hshtableout["DeathDate"]);
                        txtbodyreceviedby.Text = Convert.ToString(hshtableout["BodyReceivedBy"]).Trim();
                        ddldepositionofbody.SelectedItem.Text = Convert.ToString(hshtableout["BodyDeposition"]).Trim();
                    }
                }
            }
            else
            {
                btnCancel.Visible = false;
                if (isUpdateMrdDischrgePatient && common.myInt(Request.QueryString["AcknowledmentStatus"]) == 1)
                {
                    btnsave.Visible = true;
                }
                else
                {
                    btnsave.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.ATD objbc;
            objbc = new BaseC.ATD(sConString);
            BaseC.EMRBilling.clsOrderNBill objcls = new BaseC.EMRBilling.clsOrderNBill(sConString);

            if (Request.QueryString["mode"] == "edit" || Request.QueryString["CF"] == "MRD")
            {
                saveRecord(common.myInt(ViewState["DischargeId"]));
                btnsave.Visible = false;
            }
            else
            {
                if (common.myStr(ViewState["Source"]) == "ER")
                {
                    saveRecord(0);
                }
                else
                {
                    saveRecord(0);
                    if (isUpdateMrdDischrgePatient && common.myInt(Request.QueryString["AcknowledmentStatus"]) == 1)

                    {
                        btnsave.Visible = true;
                    }
                    else
                    {
                        btnsave.Visible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public bool IsSave()
    {
        bool issave = true;
        string strmsg = "";

        if (common.myInt(ddldischargestatus.SelectedValue) == 0)
        {
            issave = false;
            strmsg += "Please select discharge status ! ";
        }
        BaseC.HospitalSetup hsSetup = new BaseC.HospitalSetup(sConString);
        //string IpdisRemarkNotMandatory = (string)hsSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "isAuthorizedIpdisRemarkMandatory", common.myInt(Session["FacilityId"]));
        //if (IpdisRemarkNotMandatory == "Y")
        //{
        if (common.myInt(ddlreason.SelectedValue) == 0)
        {
            issave = false;
            strmsg += "Please select Reason ! ";
        }
        //}
        if (ddldischargestatus.SelectedItem.Text.ToUpper() == "EXPIRED")
        {
            if (dtpdeathdatetime.SelectedDate == null)
            {
                issave = false;
                strmsg += "Please select Expire DateTime ! ";
            }
        }
        lblmsg.Text = "";
        if (strmsg != "")
        {
            lblmsg.Text = strmsg;
        }

        return issave;
    }
    protected void saveRecord(int DischargeId)
    {
        try
        {
            BaseC.HospitalSetup ooSetup = new BaseC.HospitalSetup(sConString);
            if (txtregno.Text.Trim() != "" && txtipno.Text.Trim() != "")
            {
                StringBuilder strapend = new StringBuilder();
                StringBuilder strCheckList = new StringBuilder();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hshtableout = new Hashtable();
                Hashtable hshtablein = new Hashtable();
                Boolean DeathYesNo;

                hshtablein.Add("HospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hshtablein.Add("intFacilityId", common.myInt(Session["FacilityId"]));
                hshtablein.Add("EncounterNo", txtipno.Text.Trim());
                hshtablein.Add("RegistrationId", common.myInt(hdnRegistrationId.Value)); //hdnregno.Value.Trim());
                hshtablein.Add("DischargeDate", dtptransferdate.SelectedDate);
                hshtablein.Add("EncodedBy", Session["UserID"]);
                hshtablein.Add("DischargedStatus", common.myInt(ddldischargestatus.SelectedValue));
                hshtablein.Add("Reason", ddlreason.SelectedItem.Text);
                hshtablein.Add("intDischargeFacility", ddlFacility.SelectedValue);
                if (ddldischargestatus.SelectedItem.Text.ToUpper() == "EXPIRED")
                {
                    DeathYesNo = true;
                }
                else
                {
                    DeathYesNo = false;
                }
                hshtablein.Add("Expired", DeathYesNo);

                 if (ddldischargestatus.SelectedItem.Text.ToUpper() == "EXPIRED")
                    {
                        if (dtpdeathdatetime.SelectedDate == null)
                        {
                            Alert.ShowAjaxMsg("Please! Fill Expired DateTime", this.Page);
                            return;
                        }
                        hshtablein.Add("DeathDate", Convert.ToString(bc.ParseQ(dtpdeathdatetime.SelectedDate.ToString())));
                        hshtablein.Add("BodyDeposition", Convert.ToString(bc.ParseQ(ddldepositionofbody.SelectedItem.Text.Trim())));
                        hshtablein.Add("BodyReceivedBy", Convert.ToString(bc.ParseQ(txtbodyreceviedby.Text.Trim())));
                        hshtablein.Add("Modeoftransfer", txtmodeoftransfer.Text.ToString().Trim());
                        hshtablein.Add("Authorisedburialpermission", txtauthorised.Text.ToString().Trim());

                        foreach (GridDataItem item in grddeathcause.MasterTableView.Items)
                        {

                            if (item is GridDataItem)
                            {
                                DropDownList ddldoctor = (DropDownList)item.FindControl("ddldoctor");
                                TextBox txtDesc = (TextBox)item.FindControl("txtDescription");

                                if (item.RowIndex == 2)
                                {
                                    if (ddldoctor.SelectedIndex == 0 || txtDesc.Text == "" || txtDesc.Text == string.Empty)
                                    {

                                        txtDesc.Focus();
                                        Alert.ShowAjaxMsg("fill first row", this.Page);
                                        return;
                                    }
                                }

                                if (ddldoctor.SelectedIndex != 0)
                                {
                                    strapend.Append("<Table1><c1>");
                                    strapend.Append(ddldoctor.SelectedValue);
                                    strapend.Append("</c1><c2>");
                                    strapend.Append(txtDesc.Text.Trim());
                                    strapend.Append("</c2></Table1>");
                                }
                            }
                        }
                        hshtablein.Add("XMLDeathCause", strapend.ToString().Trim());

                        hshtablein.Add("ExpiredReasonId", common.myStr(ddlExpiredReason.SelectedValue));
                        hshtablein.Add("ExpiredReasonOther", txtOtherExpiredRemarks.Text.ToString().Trim());
                    }
                    else
                    {
                        hshtablein.Add("DeathDate", "");
                        hshtablein.Add("BodyDeposition", "");
                        hshtablein.Add("BodyReceivedBy", "");
                        hshtablein.Add("Modeoftransfer", "");
                        hshtablein.Add("Authorisedburialpermission", "");
                        hshtablein.Add("XMLDeathCause", "");
                    }

                hshtablein.Add("DischargeRemark", txtDischargeRemarks.Text);
                hshtableout.Add("ErrorStatus", SqlDbType.VarChar);

                hshtableout = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspMRDUpdateDischarge", hshtablein, hshtableout);

                lblmsg.Text = common.myStr(hshtableout["ErrorStatus"]);


            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnPrintDischarge_OnClick(object sender, EventArgs e)
    {
        Session["DisEncounterNO"] = txtipno.Text;
        RadWindow2.NavigateUrl = "/EMRREPORTS/PrintOPDReceipt.aspx?IpNo= " + txtipno.Text + "&PrintType=DischargeNotification";
        RadWindow2.Height = 500;
        RadWindow2.Width = 750;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.VisibleOnPageLoad = true;
        RadWindow2.Modal = true;
        RadWindow2.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindow2.VisibleStatusbar = false;
        dvConfirmPrint.Visible = false;
    }

    protected void btnPrintCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmPrint.Visible = false;
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        try
        {
            find();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddldischargestatus_SelectedIndexChanged(object sender, EventArgs e)
    {

        showdischargepanel();
    }
    void showdischargepanel()
    {
        try
        {
            if (ddldischargestatus.SelectedItem.Text.ToUpper() == "EXPIRED")
            {
                uppnldeath.Update();
                pnldeathdetails.Visible = true;
                grddeathcause.Visible = true;
                dtpdeathdatetime.SelectedDate = null;
                //pnldeathcause.Visible = true;
                Requiredfield1.Enabled = false;
                sp1.Visible = false;
                lnkMultiplePayMode();
                BaseC.HospitalSetup hsSetup = new BaseC.HospitalSetup(sConString);
                string IpdisRemarkNotMandatory = (string)hsSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "isExpiredReasonRequiredOnDischarge", common.myInt(Session["FacilityId"]));
                if (IpdisRemarkNotMandatory == "Y")
                {
                    Span1.Visible = true;
                }
                else
                {
                    Span1.Visible = false;
                }

            }
            else
            {
                grddeathcause.Visible = false;
                pnldeathdetails.Visible = false; ;

                // pnldeathcause.Visible = false;



            }
            uppnldeath.Update();
            UpdatePanel2.Update();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void clear()
    {
        try
        {
            Cache["PRegNo"] = "";


            ddldischargestatus.SelectedIndex = 0;
            ddlreason.SelectedIndex = 0;
            ddldepositionofbody.SelectedIndex = 0;
            txtauthorised.Text = "";
            txtbodyreceviedby.Text = "";
            txtmodeoftransfer.Text = "";
            ddlExpiredReason.SelectedIndex = -1;
            txtOtherExpiredRemarks.Text = "";

            //            lnkMultiplePayMode();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void ibtnNew_Click(object sender, EventArgs e)
    {
        Cache["PRegNo"] = "";
        Response.Redirect(Request.Url.AbsoluteUri, false);
    }
    protected void ibtnEdit_Click(object sender, EventArgs e)
    {
        Cache["PRegNo"] = "";
        Response.Redirect("/ATD/discharge.aspx?mode=edit&MPG=" + Request.QueryString["MPG"], false);
    }
    protected void btnCheck_Click(object sender, EventArgs e)
    {

    }

    protected void GetIPNO()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dt = new DataSet();
            dt = dl.FillDataSet(CommandType.Text, "select EncounterNo  from Admission where RegistrationId=" + hdnregno.Value.Trim() + "");
            if (dt.Tables[0].Rows.Count > 0)
            {
                txtipno.Text = dt.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();

            }
            else
            {
                txtipno.Text = "";
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void grddeathcause_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                DropDownList ddldoctor = (DropDownList)e.Item.FindControl("ddldoctor");
                Label lbldoctor = (Label)e.Item.FindControl("lbldoctor");
                ddldoctor.SelectedValue = lbldoctor.Text;

                TextBox txtDescription = (TextBox)e.Item.FindControl("txtDescription");


            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void lnkAddRow_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dT = new DataTable();
            DataRow dR;
            DataTable dT1 = new DataTable();

            dT1 = (DataTable)Cache["DataTable"];
            dT = dT1.Clone();


            //IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            DataTable dTemp = new DataTable();
            Cache["DataTable"] = dTemp;


            foreach (GridDataItem item in grddeathcause.MasterTableView.Items)
            {
                if (item is GridDataItem)
                {
                    dR = dT.NewRow();
                    DropDownList ddldoctor = (DropDownList)item.FindControl("ddldoctor");
                    dR["DoctorId"] = ddldoctor.SelectedValue;


                    TextBox txtDesc = (TextBox)item.FindControl("txtDescription");
                    dR["Description"] = txtDesc.Text;

                    dT.Rows.Add(dR);

                }
            }

            Cache["DataTable"] = dT;
            dR = dT.NewRow();
            dR["DoctorId"] = 1;

            dT.Rows.Add(dR);
            grddeathcause.DataSource = (DataTable)Cache["DataTable"];
            grddeathcause.DataBind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void lnkMultiplePayMode()
    {
        try
        {

            grddeathcause.Visible = true;
            //Bind Payment mode gird

            DataTable defT;
            DataColumn defC;
            DataRow defR;
            defT = new DataTable("Doctor");
            defC = new DataColumn("DoctorId", typeof(Int32));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            defC = new DataColumn("Description", typeof(String));
            defC.AllowDBNull = true;
            defT.Columns.Add(defC);
            for (int i = 0; i < 4; i++)
            {
                defR = defT.NewRow();
                defR["DoctorId"] = 1;

                defR["Description"] = "";
                
                defT.Rows.Add(defR);
            }
            Cache["DataTable"] = defT;
            grddeathcause.DataSource = defT;
            grddeathcause.DataBind();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnpasshelp_Click(object sender, EventArgs e)
    {
        RadWindow2.NavigateUrl = "/ATD/DischargeList.aspx";
        RadWindow2.Height = 500;
        RadWindow2.Width = 1000;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        RadWindow2.OnClientClose = "OnClientClose";
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlbedcategory.SelectedValue) != common.myInt(ddlbillingcategory.SelectedValue))
            {
                dvConfirm.Visible = true;
                return;
            }
            else
            {
                ReAdmission();

            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlbedcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindBedNo(common.myInt(ddlwardno.SelectedValue), common.myInt(ddlbedcategory.SelectedValue));
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnhide_Click(object sender, EventArgs e)
    {
        try
        {
            dvLogOut.Visible = false;
            patientdetailspnl.Visible = false;
            tryesno.Visible = false;
            trwardno.Visible = false;
            trbedno.Visible = false;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lnkipno_OnClick(object sender, EventArgs e)
    {
        lblmsg.Text = "";

        RadWindow1.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?MRDSearch=IP&RegEnc=2&PageFrom=MRD";
        RadWindow1.Height = 580;
        RadWindow1.Width = 900;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "SearchPatientOnClientClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
    }
    void BindBedNo(int wardid, int BedCategory)
    {
        try
        {
            ddlbedno.Items.Clear();
            BaseC.ATD objadt = new BaseC.ATD(sConString);
            ds = new DataSet();
            ds = objadt.GetBedNo(wardid, BedCategory);

            ddlbedno.DataSource = ds;
            ddlbedno.DataTextField = "bedno";
            ddlbedno.DataValueField = "Id";
            ddlbedno.DataBind();
            ddlbedno.Items.Insert(0, "Select");
            ddlbedno.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    void BindControls()
    {
        try
        {
            ddlwardno.Items.Clear();
            ddlbedcategory.Items.Clear();
            ddlbillingcategory.Items.Clear();
            BaseC.ATD objadt = new BaseC.ATD(sConString);
            ds = new DataSet();

            ds = objadt.GetWard(common.myInt(Session["FacilityId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlwardno.DataSource = ds.Tables[0];
                ddlwardno.DataTextField = "WardName";
                ddlwardno.DataValueField = "WardId";
                ddlwardno.DataBind();

            }
            ddlwardno.Items.Insert(0, "Select");
            ddlwardno.Items[0].Value = "0";

            //-----------
            ds = new DataSet();

            ds = objadt.GetExpiredReason();

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlExpiredReason.Items.Insert(0, "Select");
                foreach (DataRowView drReferal in ds.Tables[0].DefaultView)
                {
                    ListItem item = new ListItem();
                    item.Text = (string)drReferal["Reason"];
                    item.Value = drReferal["Id"].ToString();
                    item.Attributes.Add("ShowTxtBox", common.myStr(drReferal["ShowTxtBox"]));
                    ddlExpiredReason.Items.Add(item);

                }

                ddlExpiredReason.SelectedIndex = -1;

            }



            ds = new DataSet();
            ds = objadt.GetBedCategory(common.myInt(Session["FacilityId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbedcategory.DataSource = ds.Tables[0];
                ddlbedcategory.DataTextField = "BedCategoryName";
                ddlbedcategory.DataValueField = "BedCategoryId";
                ddlbedcategory.DataBind();

                ddlbillingcategory.DataSource = ds;
                ddlbillingcategory.DataTextField = "BedCategoryName";
                ddlbillingcategory.DataValueField = "BedCategoryId";
                ddlbillingcategory.DataBind();
            }
            ddlbedcategory.Items.Insert(0, "Select");
            ddlbedcategory.Items[0].Value = "0";

            ddlbillingcategory.Items.Insert(0, "Select");
            ddlbillingcategory.Items[0].Value = "0";
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "key", "<script type='text/javascript'>CloseWindow('y');</script>", false);
    }
    protected void btnYes_OnClick(object sender, EventArgs e)
    {
        try
        {
            ViewState["Comapare"] = "Yes";
            BindPatientHiddenDetails(common.myInt(txtregno.Text)); ;
            ReAdmission();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


    }
    protected void btnCancel_OnClick(object sender, EventArgs e)
    {
        try
        {
            BindPatientHiddenDetails(common.myInt(txtregno.Text));
            dvConfirm.Visible = false;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }

    protected void ReAdmission()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hsinput = new Hashtable();
            Hashtable hsoutput = new Hashtable();
            if (ddlbedno.SelectedIndex == -1)
            {
                hsinput.Add("@intBedId", ViewState["BedId"].ToString());
            }
            else
            {
                hsinput.Add("@intBedId", ddlbedno.SelectedValue);
            }
            if (txtregno.Text != "" && Session["UserID"] != null)
            {
                ds = dl.FillDataSet(CommandType.Text, "select BedStatus,wardId  from BedMaster where Id=@intBedId And Active=1", hsinput);
                if (ds.Tables[0].Rows[0]["BedStatus"].ToString().Trim() != "V")
                {
                    lblmsg.Text = "Bed Already Allocated to Another Patient, Select to Another Bed...";
                    dvLogOut.Visible = true;
                    patientdetailspnl.Visible = true;
                    tryesno.Visible = true;
                    trwardno.Visible = true;
                    trbedno.Visible = true;

                }
                else
                {
                    ViewState["WardId"] = ds.Tables[0].Rows[0]["wardId"].ToString().Trim();

                    hsinput.Add("@intHospitalLocationId", Session["HospitalLocationID"]);
                    hsinput.Add("@intEncounterId", common.myInt(Session["Encounterid"]));

                    hsinput.Add("@intRegistrationId", hdnregno.Value.Trim());
                    if (ddlwardno.SelectedValue != "0")
                        hsinput.Add("@intWardId", ddlwardno.SelectedValue);
                    else
                        hsinput.Add("@intWardId", common.myInt(ViewState["WardId"]));

                    if (ddlbedcategory.SelectedValue != "0")
                        hsinput.Add("@intBedcategory", ddlbedcategory.SelectedValue);

                    if (ddlbillingcategory.SelectedValue != "0")
                        hsinput.Add("@intBillingCategory", ddlbillingcategory.SelectedValue);

                    hsinput.Add("@intLastchangedby", Session["UserID"].ToString());
                    hsoutput.Add("@charStrStatus", SqlDbType.VarChar);


                    hsoutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPCancelDischargedPatient", hsinput, hsoutput);
                    if (hsoutput["@charStrStatus"].ToString() != "")
                    {
                        lblmsg.Text = hsoutput["@charStrStatus"].ToString();
                    }
                    else
                    {
                        lblmsg.Text = "Patient Cancel Successfuly...  ";
                        dvConfirm.Visible = false;
                        dvLogOut.Visible = false;
                        patientdetailspnl.Visible = false;
                        tryesno.Visible = false;
                        trwardno.Visible = false;
                        trbedno.Visible = false;
                        btnhide.Visible = false;
                        btnCancel.Visible = false;
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnfilter_Click(object sender, EventArgs e)
    {
        try
        {

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hshin = new Hashtable();

            hshin.Add("@intEncounterNo", txtipno.Text.Trim());
            hshin.Add("@iFacilityId", common.myInt(Session["FacilityId"]));
            string str = "";
            if (common.myStr(Request.QueryString["CF"]) == "MRD")
                str = "select E.RegistrationId,E.RegistrationNo,D.EncounterId,E.EncounterNo From Discharge D With (nolock) Inner Join Encounter E With (nolock) On E.Id = D.EncounterId  Where D.Active = 1 And E.EncounterNo=@intEncounterNo  And D.DischargeFacility = @iFacilityId and E.Active=1";
            else
                str = "select RegistrationId,RegistrationNo,EncounterId,EncounterNo from Admission where EncounterNo=@intEncounterNo and FacilityId  = @iFacilityId  and PatadType not in('D','C') and Active=1";
            ds = dl.FillDataSet(CommandType.Text, str, hshin);
            if (ds.Tables[0].Rows.Count > 0)
            {
                hdnregno.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString().Trim();
                Session["RegistrationId"] = hdnregno.Value;
                ViewState["IpNo"] = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();
                txtipno.Text = ds.Tables[0].Rows[0]["EncounterNo"].ToString().Trim();
                txtregno.Text = ds.Tables[0].Rows[0]["RegistrationNo"].ToString().Trim();
                hdnEncounterId.Value = ds.Tables[0].Rows[0]["EncounterId"].ToString().Trim();
                hdnRegistrationId.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString().Trim();
                Session["encounterid"] = ds.Tables[0].Rows[0]["EncounterId"].ToString().Trim();
                Session["SaveDischargeCheck"] = 0;
                txtipno.Enabled = false;

            }
            else
            {
                Alert.ShowAjaxMsg("Patient Can Not Find", this.Page);
                hdnregno.Value = "";
                clear();

            }
            find();
            BindPatientHiddenDetails(common.myInt(txtregno.Text)); ;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnew_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtipno.Text = "";
            txtregno.Text = "";
            txtipno.Enabled = true;
            ddldischargestatus_SelectedIndexChanged(this, null);

            if (Convert.ToString(txtregno.Text.Trim()) != "")
            {

                BindPatientHiddenDetails(common.myInt(txtregno.Text)); ;
            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    public void fillFacility()
    {
        if (Cache["FACILITY"] == null || ((DataSet)Cache["FACILITY"]).Tables[0].Rows.Count == 0)
        {
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds1 = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
            Cache["FACILITY"] = ds1;
        }

        ddlFacility.DataSource = (DataSet)Cache["FACILITY"];
        ddlFacility.DataTextField = "FacilityName";
        ddlFacility.DataValueField = "FacilityID";
        ddlFacility.DataBind();
        //  ddlFacility.SelectedValue = Session["FacilityId"].ToString();

        if (common.myInt(Session["FacilityId"]) != 0)
        {
            ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(common.myStr(Session["FacilityId"])));
            ddlFacility.Enabled = false;
        }
        else
        {
            ddlFacility.Enabled = true;
        }

    }
    protected void ddlExpiredReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpiredReason.SelectedIndex > 0)
        {
            if (common.myStr(ddlExpiredReason.SelectedItem.Attributes["ShowTxtBox"]).Equals("T"))
                txtOtherExpiredRemarks.Visible = true;
            else
                txtOtherExpiredRemarks.Visible = false;
        }
    }
    //By  Om Prakash
    private void BindGrid()
    {
        try
        {
            BaseC.clChecklist objcheck;
            BaseC.HospitalSetup ooSetup = new BaseC.HospitalSetup(sConString);
            objcheck = new BaseC.clChecklist(sConString); ;
            ds = new DataSet();
            int compId = common.myInt(Request.QueryString["cid"]);
            if (ooSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsDischargechecklistMandatory", common.myInt(Session["FacilityId"])) != "N")
            {
                if (compId != 0)
                {
                    grvCheckList.Enabled = true;
                    ds = objcheck.GetChecklistCompanywise(common.myInt(Session["HospitalLocationId"]), compId, "D");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        //ds.Tables[0].Rows.Add();
                        //ds.Tables[0].AcceptChanges();
                    }
                    grvCheckList.DataSource = ds;
                    grvCheckList.DataBind();
                }
            }
            else
            {
                return;
            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //

}
