using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Security.Principal;
using System.Net;
using System.Configuration;
using System.Collections;

public partial class ICM_HealthCheckUpCheckList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    string path = string.Empty;
    public string RTF1Content = string.Empty;
    string url = HttpContext.Current.Request.Url.AbsoluteUri;
    string FromDate = string.Empty;
    string ToDate = string.Empty;
    ArrayList coll;
    bool flag = false;
    string sFontSize = string.Empty;

    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["Master"]).ToUpper() == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        path = url.Replace("http://", "");
        path = "http://" + path.Substring(0, path.IndexOf("/") + 1);
        if (!IsPostBack)
        {

            hdnFontName.Value = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "DischargeSummaryFont", sConString);
            if (common.myStr(hdnFontName.Value).Equals(string.Empty))
            {
                hdnFontName.Value = "Candara";
            }

            hdnReportId.Value = common.myStr(Request.QueryString["ReportId"]);
            hdnEncounterId.Value = common.myStr(Session["EncounterId"]);
            hdnRegistrationId.Value = common.myStr(Session["RegistrationId"]);
            BindHealthCheckLists();
            if (common.myInt(Request.QueryString["PrintAllowed"]) == 0)
            {
                btnPrint.Visible = false;
            }
        }
        FromDate = common.myStr(Session["EncounterDate"]).Trim();
        ToDate = common.myDate(DateTime.Now).ToString("yyyy/MM/dd");
    }
    private void BindHealthCheckLists()
    {
        DataSet ds = new DataSet();
        DataView dvFilter = new DataView();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {
            ds = emr.getHealthCheckUpCheckLIsts(common.myInt(hdnEncounterId.Value), common.myInt(hdnRegistrationId.Value), common.myInt(hdnReportId.Value), common.myInt(Session["DoctorId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //dvFilter = new DataView(ds.Tables[0]);
                    //dvFilter.RowFilter = "IsData=0";
                    //if (dvFilter.ToTable().Rows.Count > 0)
                    //{
                    //    hdnAllowPrint.Value = "0";
                    //}

                    gvCheckListsTemplates.DataSource = ds.Tables[0];
                    gvCheckListsTemplates.DataBind();
                }
            }
            if (ds.Tables.Count > 4)
            {
                if (ds.Tables[4].Rows.Count > 0)
                {
                    //dvFilter = new DataView(ds.Tables[1]);
                    //dvFilter.RowFilter = "IsData=0";
                    //if (dvFilter.ToTable().Rows.Count > 0)
                    //{
                    //    hdnAllowPrint.Value = "0";
                    //}
                    gvCheckListsStaticTemplates.DataSource = ds.Tables[4];
                    gvCheckListsStaticTemplates.DataBind();
                    ViewState["PrintAdviceTemplate"] = ds.Tables[4];
                }
            }
            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    //dvFilter = new DataView(ds.Tables[2]);
                    //dvFilter.RowFilter = "IsData=0";
                    //if (dvFilter.ToTable().Rows.Count > 0)
                    //{
                    //    hdnAllowPrint.Value = "0";
                    //}
                    gvCheckListsSections.DataSource = ds.Tables[2];
                    gvCheckListsSections.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            emr = null;
        }
    }
    protected void gvCheckLists_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTemplate = (CheckBox)e.Row.FindControl("chkTemplate");
                HiddenField hdnIsData = (HiddenField)e.Row.FindControl("hdnIsData");
                if (common.myBool(hdnIsData.Value) == true)
                {
                    chkTemplate.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void gvCheckListsStaticTemplates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTemplate = (CheckBox)e.Row.FindControl("chkTemplate");
                HiddenField hdnIsData = (HiddenField)e.Row.FindControl("hdnIsData");
                if (common.myBool(hdnIsData.Value) == true)
                {
                    chkTemplate.Checked = true;
                }
                if (chkTemplate.Checked)
                {
                    chkTemplate.Enabled = true;
                }
                else
                {
                    chkTemplate.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void gvCheckListsSections_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkTemplate = (CheckBox)e.Row.FindControl("chkTemplate");
                HiddenField hdnIsData = (HiddenField)e.Row.FindControl("hdnIsData");
                if (common.myBool(hdnIsData.Value) == true)
                {
                    chkTemplate.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
    }
    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(hdnAllowPrint.Value) == 1)
        {
            RadWindow1.NavigateUrl = "/ICM/PrintHealthCheckUp.aspx?page=Ward&EncId=" + hdnEncounterId.Value
                    + "&DoctorId=" + hdnDoctorId.Value + "&RegId=" + hdnRegistrationId.Value + "&ReportId=" + hdnReportId.Value + "&HC=HC";
            RadWindow1.Height = 600;
            RadWindow1.Width = 800;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.Modal = true;
            RadWindow1.VisibleOnPageLoad = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.Behaviors = Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Minimize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Pin;
        }
        else
        {
            Alert.ShowAjaxMsg("Please enter all unchecked templates", Page);
            return;
        }
    }

    protected void btnPrintAdvice_OnClick(object sender, EventArgs e)
    {

        clsIVF objivf = new clsIVF(sConString);
        hdnReportContent.Value = string.Empty;
        try
        {
            //if (IsAllowPopup())
            //{
            //    return;
            //}

            //if (common.myStr(ViewState["EMRFollowUpAppointmentOPSummaryValidation"]).ToUpper().Equals("Y"))
            //{
            //    objivf.SaveIsNoFollowUpRequired(common.myInt(Session["EncounterId"]), chkIsNoFollowUpRequired.Checked);

            //    if (!chkIsNoFollowUpRequired.Checked)
            //    {
            //        if (!IsallowFlowUpRequired())
            //        {

            //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //            lblMessage.Text = "Kindly define the follow-up appointment !";

            //            Alert.ShowAjaxMsg(lblMessage.Text, this.Page);

            //            return;
            //        }
            //    }

            //}

            DataSet ds = new DataSet();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            //ds = objEMR.getReportFormatDetails(common.myInt(Session["DoctorId"]));
            //if (ds != null)
            //{
            //    if (ds.Tables.Count > 0)
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            ViewState["reportid"] = common.myStr(ds.Tables[0].Rows[0]["reportid"]);
            //            ViewState["reportname"] = common.myStr(ds.Tables[0].Rows[0]["reportname"]);
            //            ViewState["headerid"] = common.myStr(ds.Tables[0].Rows[0]["headerid"]);
            //        }
            //        else
            //        {
            //            Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
            //        return;
            //    }
            //}
            //else
            //{
            //    Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
            //    return;
            //}
             //getDoctorImage();
            generateReport();


            if (common.myLen(hdnReportContent.ClientID) > 0)
            {
                Session["PrintReportWordProcessorWiseData"] = common.myStr(hdnReportContent.Value);


                string var = path + "Editor/PrintMHCAdvisoryTemplate.aspx?ReportId=" + common.myInt(ViewState["reportid"]) +
                                     "&HeaderId=" + common.myInt(ViewState["headerid"]) +
                                     "&RegistrationId=" + common.myInt(Session["RegistrationId"]);

                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "ShowPAgePrint('" + var + "');", true);
                RadWindow1.NavigateUrl = var;
                RadWindow1.Height = 600;
                RadWindow1.Width = 800;
                RadWindow1.Top = 10;
                RadWindow1.Left = 10;
                RadWindow1.Modal = true;
                RadWindow1.VisibleOnPageLoad = true;
                RadWindow1.VisibleStatusbar = false;
                RadWindow1.Behaviors = Telerik.Web.UI.WindowBehaviors.Maximize | Telerik.Web.UI.WindowBehaviors.Minimize | Telerik.Web.UI.WindowBehaviors.Close | Telerik.Web.UI.WindowBehaviors.Move | Telerik.Web.UI.WindowBehaviors.Pin;



            }
            else
            {
                Alert.ShowAjaxMsg("Data not found!", this.Page);
                return;
            }
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
    }

    private string PrintReport(bool sign, int iTemplateId)
    {

        string strDisplayEnteredByInCaseSheet = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
        Session["DisplayEnteredByInCaseSheet"] = string.Empty;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder TemplateString;
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        DataTable dtEncounter = new DataTable();

        string Templinespace = "";
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;
        bool bPatientBookingDisplay = false;

        //sb.Append(getReportHeader(common.myInt(ddlReport.SelectedValue)));
        //   sb.Append(getReportHeader(common.myInt(ViewState["reportid"])));

        string getReportHeaderText = common.myStr(getReportHeader(common.myInt(ViewState["reportid"])));

        if(getReportHeaderText.Equals(string.Empty))
        { 
         getReportHeaderText = "<br/>";
        }

        clsIVF objIVF = new clsIVF(sConString);

        //string strPatientHeader = objIVF.getCustomizedPatientReportHeader(common.myInt(ViewState["headerid"]));
        string strPatientHeader = getCustomizedPatientReportHeader();
        if (common.myLen(strPatientHeader).Equals(0))
        {
            // sb.Append(getIVFPatient().ToString());
            //Session["strPatientHeader"] = common.myStr(getReportHeaderText) + common.myStr(ViewState["HeadingName"]) + getIVFPatient().ToString();
            Session["strPatientHeader"] = common.myStr(getReportHeaderText) + getIVFPatient().ToString();
        }
        else
        {
            Session["strPatientHeader"] = common.myStr(getReportHeaderText) + strPatientHeader;
            //Session["strPatientHeader"] = common.myStr(getReportHeaderText) + common.myStr(ViewState["HeadingName"]) + strPatientHeader;
            //sb.Append(strPatientHeader);
        }

        string sTemplateName = common.myStr("ALL") == "ALL" ? "" : common.myStr("ALL");

        DataSet dsTemplateData = new DataSet();
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);

        try
        {
            string EMRServicePrintSeperatedWithCommas = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                          common.myInt(Session["FacilityId"]), "EMRServicePrintSeperatedWithCommas", sConString);

            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            //dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
            //                        common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
            //                        common.myDate(FromDate).ToString("yyyy/MM/dd"),
            //                        common.myDate(ToDate).ToString("yyyy/MM/dd"),
            //                        string.Empty, 6004, string.Empty, false, 0);

            //for (int iTemplateId=0; iTemplateId< ; iTemplateId++)

            dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                      common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myDate(FromDate).ToString("yyyy/MM/dd"),
                            common.myDate(ToDate).ToString("yyyy/MM/dd"), "", iTemplateId,
                      "D", false, 0, 0, false);


            dvDataFilter = new DataView(dsTemplateData.Tables[21]);
            dtEncounter = dsTemplateData.Tables[22];
            for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
            {
                if (dvDataFilter.ToTable().Rows.Count > 0)
                {
                    #region Template Wise
                    {
                        dtTemplate = dvDataFilter.ToTable();
                        TemplateString = new StringBuilder();
                        for (int i = 0; i < dtTemplate.Rows.Count; i++)
                        {

                            #region All the Templates except Hitory and Plan of case
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //    || (common.myInt(0) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call all Dynamic Template Except Hostory and Plan of Care template

                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(0) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(0);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());


                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                    {
                                        TemplateString.Append(sbTemp + "<br/>");
                                    }
                                }
                                sbTemp = null;
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion

                        }
                        if (TemplateString.Length > 30)
                        {
                            //if (iEn == 0)
                            //{
                            //sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
                            //sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
                            //sb.Append("</span>");
                            //}
                            sb.Append("<span style='" + String.Empty + "'>");
                            sb.Append(TemplateString);
                            sb.Append("</span><br/>");
                            //  sb.Append("</span>");
                            TemplateString = null;
                        }
                    }
                    #endregion
                }
            }
            Session["NoAllergyDisplay"] = null;
            if (sign == true)
            {
                //sb.Append("</span>");
                sb.Append(hdnDoctorImage.Value);
            }
            else if (sign == false)
            {
                if (RTF1Content != null)
                {
                    if (RTF1Content.Contains("dvDoctorImage") == true)
                    {
                        string signData = RTF1Content.Replace('"', '$');
                        string st = "<div id=$dvDoctorImage$>";
                        int start = signData.IndexOf(@st);
                        if (start > 0)
                        {
                            int End = signData.IndexOf("</div>", start);
                            StringBuilder sbte = new StringBuilder();
                            sbte.Append(signData.Substring(start, (End + 6) - start));
                            StringBuilder ne = new StringBuilder();
                            ne.Append(signData.Replace(sbte.ToString(), ""));
                            sb.Append(ne.Replace('$', '"').ToString());
                            sbte = null;
                            ne = null;
                            signData = "";
                            st = "";
                            start = 0;
                            End = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            objException.HandleException(ex);
        }
        finally
        {
            Session["DisplayEnteredByInCaseSheet"] = strDisplayEnteredByInCaseSheet;

            sbTemplateStyle = null;
            TemplateString = null;
            ds.Dispose();
            dsTemplateStyle.Dispose();
            dvDataFilter.Dispose();
            drTemplateStyle = null;
            dtTemplate.Dispose();
            Templinespace = "";
            bnotes = null;
            fun = null;
            emr = null;
            sbTemp = null;
            BindCaseSheet = null;
            dsTemplateData.Dispose();
        }
        //StringBuilder sbfinal = new StringBuilder();
        //sbfinal.Append( sb.ToString().Replace("&lt", "&lt;").Replace("&gt", "&gt;"));
        sb.ToString().Replace("&lt", "&lt;").Replace("&gt", "&gt;");
        //if (sb.ToString().Contains("<br/><br/>") || sb.ToString().Contains("<br /><br />"))
        //{
        //    sb.ToString().Replace("<br/><br/>", "<br/>").Replace("<br /><br />", "<br/>");
        //}
        //else
        //{
        //    //sb.Append(sbTemp + "<br/><br/>");
        //}
        return sb.ToString();

    }

    public string getCustomizedPatientReportHeader()
    {
        BaseC.Hospital objHospital = new BaseC.Hospital(sConString);
        DataSet dsHeader = new DataSet();
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();
        BindSummary bnotes = new BindSummary(sConString);
        ds = bnotes.GetPrintDischargeSummary(common.myInt(Session["HealthCheckUpCheckListPrintAdviceEncounterId"]), common.myInt(HttpContext.Current.Session["HospitalLocationId"]),
                                        common.myInt(Session["HealthCheckUpCheckListPrintAdviceRegistrationId"]), common.myInt(ViewState["reportid"]),
                                        common.myInt(HttpContext.Current.Session["FacilityId"]));
        if (ds.Tables[0].Rows.Count > 0)
      // Session["HeadingPackageName"] =    "<table cellpadding='0' cellspacing='0'><tr ><td valign='top' align='center' style='border-style:none;font-size:12pt;font-weight:bold;font-family: " + common.myStr(hdnFontName.Value) + ";text-decoration: underline;'>" + common.myStr(ds.Tables[0].Rows[0]["HeadingName"]) + "</td></tr><tr ><td valign='top' align='center' style='border-style:none;font-size:12pt;font-weight:bold;font-family: " + common.myStr(hdnFontName.Value) + ";text-decoration: underline;'>" + "Impression" + "</td></tr></table>";
        Session["HeadingPackageName"]="<table cellpadding='0' cellspacing='0'><tr ><td valign='top' align='left' style='border-style:none;font-size:12pt;font-weight:bold;font-family: " + common.myStr(hdnFontName.Value) + ";'>" + "Impression" + "</td></tr></table>";
        StringBuilder sbtopheader = new StringBuilder();
        int RowNo = 0;
        string colspan = "";
        string headerRow = "<hr width='100%' size='1px' />";
        if (common.myInt(Request.QueryString["ReportId"]) > 0)
        {

            dsHeader = objHospital.GetReportHeader(string.Empty, common.myInt(Request.QueryString["ReportId"]));

        }
        else
        {
            dsHeader = objHospital.GetReportHeader("HC");
        }
        DataTable dtHeaderMerge = dsHeader.Tables[0];


        sb.Append(" <table border='0' cellpadding='0' cellspacing='0'>");

        int ColumnCount = common.myInt(dsHeader.Tables[0].Compute("MAX(ColNo)", string.Empty));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!common.myStr(ds.Tables[0].Rows[0]["HeadingName"]).Equals(string.Empty))
                {
                    ViewState["HeadingName"] = "<table border='0' cellpadding='0' cellspacing='0'><tr><td align='center'  colspan='" + (ColumnCount * 2) + "' > <b>" + common.myStr(ds.Tables[0].Rows[0]["TopHeadingName"]) + "</td></tr></table>";
                }
            }
        }
        sb.Append("<tr><td colspan='" + (ColumnCount * 2) + "' >" + headerRow + "</td></tr> ");

        for (int i = 0; i < dsHeader.Tables[0].Rows.Count; i++)
        {
            if (RowNo == 0)
            {
                RowNo = common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]);
                sb.Append("<tr align='left'>");

                if (ColumnCount == 1)
                {
                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                    //sb.Append("<td align='left'></td></tr>");
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td width=30% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td width=20% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                }

                if (ColumnCount == 2)
                {
                    //i = +1;
                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                    //sb.Append("<td align='left'>dsadas</td>");
                    //sb.Append("<td align='left'>" + common.myStr(dsHeader.Tables[0].Rows[i]["FieldCaption"]) + "</td>");
                    //sb.Append("<td align='left'>Male</td></tr>");

                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        i += 1;
                        colspan = "";
                    }
                    else
                    {
                        colspan = " colspan='3'";
                    }

                    dsHeader.Tables[0].DefaultView.RowFilter = "";

                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td                 width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td " + colspan + " width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                }

                if (ColumnCount == 3)
                {
                    i += 2;
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                        {
                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                            sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                        }
                        else
                        {
                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                            sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                        }
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                    }
                    //else
                    //{
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                    //}
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                }
            }
            else if (RowNo == common.myInt(dsHeader.Tables[0].Rows[i]["Rowno"]))
            {
                sb.Append("<tr align='left'>");
                if (ColumnCount == 1)
                {
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }


                }
                if (ColumnCount == 2)
                {
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        i += 1;
                        colspan = "";
                    }
                    else
                    {
                        colspan = " colspan='3'";
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "";

                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td                 width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td " + colspan + " width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    //else
                    //{
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                    //}

                }
                if (ColumnCount == 3)
                {
                    i += 2;

                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");

                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                        {
                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                            sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                        }
                        else
                        {
                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                            sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                            // sb.Append("<td  colspan='3' align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                        }
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");

                    }
                    //else
                    //{
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                    //}
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td align='left' valign='top' style='font-family:" + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                }
            }
            else
            {
                sb.Append("<tr align='left'>");
                if (ColumnCount == 1)
                {
                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                    //sb.Append("<td align='left'></td></tr>");
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                }
                if (ColumnCount == 2)
                {
                    //i = +1;
                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                    //sb.Append("<td align='left'>dsadas</td>");
                    //sb.Append("<td align='left'>" + common.myStr(dr["FieldCaption"]) + "</td>");
                    //sb.Append("<td align='left'>Male</td></tr>");

                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        i += 1;
                        colspan = "";
                    }
                    else
                    {
                        colspan = " colspan='3'";
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "";

                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td                 width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td " + colspan + " width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    //else
                    //{
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                    //    sb.Append("<td align='left' valign='top' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'>&nbsp;</td>");
                    //}
                }
                if (ColumnCount == 3)
                {
                    i += 2;
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        dtHeaderMerge.DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;
                        if (dtHeaderMerge.DefaultView.ToTable().Rows.Count > 0)// this for merge td
                        {
                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                            sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                        }
                        else
                        {
                            dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=1 AND Rowno=" + RowNo;
                            sb.Append("<td colspan='3' valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                        }
                    }
                    else
                    {
                        sb.Append("<td width=20% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td width=30% valign='top' align='left' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                    }
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=2 AND Rowno=" + RowNo;

                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td>");
                    }
                    //else
                    //{
                    //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                    //    sb.Append("<td align='left' style='font-family: "+ common.myStr(hdnFontName.Value) +";font-size:9pt;'></td>");
                    //}
                    dsHeader.Tables[0].DefaultView.RowFilter = "ColNo=3 AND Rowno=" + RowNo;
                    if (dsHeader.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>" + common.myStr(dsHeader.Tables[0].DefaultView[0]["FieldCaption"]) + "</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>: " + Addvalue(common.myStr(dsHeader.Tables[0].DefaultView[0]["ObjectValue"]), ds) + "</td></tr>");
                    }
                    else
                    {
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;'>&nbsp;</td>");
                        sb.Append("<td align='left' valign='top' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'>&nbsp;</td>");
                    }
                }

            }
            //if (i + 1 != common.myInt(dsHeader.Tables[0].Rows.Count))
            if ((i + 1) < common.myInt(dsHeader.Tables[0].Rows.Count))
            {
                RowNo = common.myInt(dsHeader.Tables[0].Rows[i + 1]["Rowno"]);

                sb.Append("<tr><td colspan='6'>" + headerRow + "</td></tr>");

            }

            if ((RowNo - 1) < 3)
            {
                StringBuilder sbNextHeader = new StringBuilder();
                sbNextHeader.Append(sb.ToString());

                sbNextHeader.Append("</table>");
            }
        }
        sb.Append("</table>");

        sb.Append("<table cellpadding='0' cellspacing='0'><tr><td>" + headerRow + "</td></tr></table>");
        return sb.ToString();
    }

    public string Addvalue(string Caption, DataSet ds)
    {
        string value = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(Caption) == "PN")//Patient Name-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
            }
            if (common.myStr(Caption) == "RN")//Registration Number-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
            }
            if (common.myStr(Caption) == "SD")//Sample Collected Date
            {
                value = common.myStr(ds.Tables[0].Rows[0]["SampleCollectedDate"]);
            }
            if (common.myStr(Caption) == "PGA")// Patient Age
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Age"]);
            }
            if (common.myStr(Caption) == "EN")//Encounter Number-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
            }
            if (common.myStr(Caption) == "RD")//Result Date
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ResultDate"]);
            }
            if (common.myStr(Caption) == "BNW") //Ward
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Ward"]);
            }
            if (common.myStr(Caption) == "LN")//Lab No
            {
                value = common.myStr(ds.Tables[0].Rows[0]["LabNo"]);
            }
            if (common.myStr(Caption) == "RS")//Report Status
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ReportStatus"]);
            }
            if (common.myStr(Caption) == "RB")//Referred By
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Referred By"]);
            }
            if (common.myStr(Caption) == "PM")//Nationality Name
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Nationality_Name"]);
            }
            if (common.myStr(Caption) == "CN")//Company Name
            {
                value = common.myStr(ds.Tables[0].Rows[0]["CompanyName"]);
            }
            //  -- 
            if (common.myStr(Caption) == "DOA")//Date Of Admission-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["DOA"]);
            }
            if (common.myStr(Caption) == "DOD")//Date Of Discharge-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["DOD"]);
            }
            if (common.myStr(Caption) == "CDN")//Consulting Doctor Name-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctor"]);
            }
            if (common.myStr(Caption) == "CDD")//Consulting Doctor Designation
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDesignation"]);
            }
            if (common.myStr(Caption) == "CND")//Consulting Doctor Department
            {
                value = common.myStr(ds.Tables[0].Rows[0]["ConsultingDoctorDepartment"]);
            }
            if (common.myStr(Caption) == "PAG")//Patient Of Gender-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["Age/Gender"]);
            }
            if (common.myStr(Caption) == "BNO")//Bed Number-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["BedNo"]);
            }
            if (common.myStr(Caption) == "BCM")//Bed Category-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["BedCategoryName"]);
            }
            if (common.myStr(Caption) == "PADD")//Patient Address-
            {
                value = common.myStr(ds.Tables[0].Rows[0]["PatientAddress"]);
            }
            if (common.myStr(Caption) == "EDT")//EncounterDate
            {
                value = common.myStr(ds.Tables[0].Rows[0]["EncounterDate"]);
            }
        }
        return value;
    }

    protected void generateReport()
    {
        bool IsPrintDoctorSignature = true;
        DataSet ds = new DataSet();
        DataTable dtTemplate = new DataTable();
        clsIVF objivf = new clsIVF(sConString);
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (common.myStr(Session["OPIP"]) == "I" && common.myBool(Session["EnablePrintCaseSheet"]) == false
            && common.myStr(Request.QueryString["callby"]) != "mrd")
            {
                if (common.myStr(Request.QueryString["OPIP"]) == "I")
                {
                    Alert.ShowAjaxMsg("You are not Authorise to print IP Clinical Data", Page);
                    return;
                }
            }
            if (common.myStr(Request.QueryString["OPIP"]) == "I" && common.myBool(Session["EnablePrintCaseSheet"]) == false
                && common.myStr(Request.QueryString["callby"]) == "mrd")
            {
                Alert.ShowAjaxMsg("You are not Authorise to print IP Clinical Data", Page);
                return;
            }
            //  hdnReportContent.Value = "";

            //if (common.myInt(ddlReport.SelectedValue) > 0)
            //{
            //if (common.myInt(ViewState["reportid"]) > 0)
            //{
            //ds = objivf.EditReportName(common.myInt(ddlReport.SelectedValue));
            
            //ds = objivf.EditReportName(common.myInt(ViewState["reportid"]));

            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        IsPrintDoctorSignature = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
            //    }
            //}
            //dtTemplate = (DataTable)ViewState["PrintAdviceTemplate"];


            foreach (GridViewRow gv in gvCheckListsStaticTemplates.Rows)
            {
                CheckBox chkTemplate = (CheckBox)gv.FindControl("chkTemplate");
                HiddenField hdnTemplateId = (HiddenField)gv.FindControl("hdnTemplateId");
                HiddenField hdnDoctorIdMHC = (HiddenField)gv.FindControl("hdnDoctorIdMHC");
                
                if (chkTemplate.Checked)
                {


                    #region ReportFormatDetails
                    ds = objEMR.getReportFormatDetails(common.myInt(hdnDoctorIdMHC.Value));
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                ViewState["reportid"] = common.myStr(ds.Tables[0].Rows[0]["reportid"]);
                                ViewState["reportname"] = common.myStr(ds.Tables[0].Rows[0]["reportname"]);
                                ViewState["headerid"] = common.myStr(ds.Tables[0].Rows[0]["headerid"]);
                                ViewState["headingname"] = common.myStr(ds.Tables[0].Rows[0]["headingname"]);
                            }
                            else
                            {
                                Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                                return;
                            }
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                            return;
                        }
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                        return;
                    }


                    ds = objivf.EditReportName(common.myInt(ViewState["reportid"]));

                    //if (ds.Tables.Count > 0)
                    //{
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        IsPrintDoctorSignature = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
                    //    }
                    //}
                    #endregion

                    getDoctorImage(common.myInt(hdnDoctorIdMHC.Value));
                    hdnReportContent.Value = hdnReportContent.Value + PrintReport(true, common.myInt(hdnTemplateId.Value));

                    //comment as follow-up appointment is check inside the printreport function --Saten
                    StringBuilder sbD = new StringBuilder();
                    sbD.Append("<table border='0' width='99%' cellpadding='0' cellspacing='0'>");
                    //sbD.Append("<tr><td>Follow Up : </td></tr>");

                    //string SignatureLabel = common.myStr(ddlReport.SelectedItem.Attributes["SignatureLabel"]).Trim();
                    string SignatureLabel = string.Empty;
                    IsPrintDoctorSignature = true;
                    if (IsPrintDoctorSignature)
                    {
                        sbD.Append("<tr><td align='right'>" + PrintReportSignature(IsPrintDoctorSignature, common.myInt(hdnDoctorIdMHC.Value)) + "</td></tr>");
                    }
                    else
                    {
                        if (SignatureLabel == "")
                        {
                            sbD.Append("<tr><td align='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>Consultant&nbsp;Signature</b></td></tr>");
                        }
                        else
                        {
                            sbD.Append("<tr><td align='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>" + SignatureLabel + "</b></td></tr>");
                        }
                    }
                    //  sbD.Append("<tr><td align='right'> </td></tr>");
                    sbD.Append("</table>");
                    hdnReportContent.Value = "<div style='margin-left:3em; '>" + hdnReportContent.Value + sbD.ToString() + "</div>";
                    //}
                    //else
                    //{
                    //    //btnPrintReport.Visible = false;
                    //    return;
                    //}
                    //btnPrintReport.Visible = true;
                }
            }

            #region Printing MHC Recommendation
            BaseC.ICM ObjIcm = new BaseC.ICM(sConString);
            DataSet dsMHCRecommendation = new DataSet();
            dsMHCRecommendation = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(Session["HealthCheckUpCheckListPrintAdviceRegistrationId"])
                       , common.myStr(Session["HealthCheckUpCheckListPrintAdviceEncounterId"]), 0, common.myInt(Session["FacilityId"]), "HC");
            #region ReportFormatDetails
            ds = objEMR.getReportFormatDetails(common.myInt(dsMHCRecommendation.Tables[0].Rows[0]["SignDoctorID"]));
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["reportidMHCRecommendation"] = common.myStr(ds.Tables[0].Rows[0]["reportid"]);
                        ViewState["reportnameMHCRecommendation"] = common.myStr(ds.Tables[0].Rows[0]["reportname"]);
                        ViewState["headeridMHCRecommendation"] = common.myStr(ds.Tables[0].Rows[0]["headerid"]);
                    }
                    else
                    {
                        Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                        return;
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                    return;
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Report Format not tagged", this.Page);
                return;
            }


            ds = objivf.EditReportName(common.myInt(ViewState["reportidMHCRecommendation"]));

            //if (ds.Tables.Count > 0)
            //{
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        IsPrintDoctorSignature = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
            //    }
            //}
            #endregion

            getDoctorImage(common.myInt(dsMHCRecommendation.Tables[0].Rows[0]["SignDoctorID"]));
            hdnReportContent.Value = hdnReportContent.Value + PrintReport(true, common.myInt(22078)); //22078 -- MHC Recommendation Template id

            //comment as follow-up appointment is check inside the printreport function --Saten
            StringBuilder sbDMHCRecommendation = new StringBuilder();
            sbDMHCRecommendation.Append("<table border='0' width='99%' cellpadding='0' cellspacing='0'>");
            //sbD.Append("<tr><td>Follow Up : </td></tr>");

            //string SignatureLabel = common.myStr(ddlReport.SelectedItem.Attributes["SignatureLabel"]).Trim();
            string SignatureLabelMHCRecommendation = string.Empty;
            IsPrintDoctorSignature = true;
            if (IsPrintDoctorSignature)
            {
                sbDMHCRecommendation.Append("<tr><td align='right'>" + PrintReportSignature(IsPrintDoctorSignature, common.myInt(dsMHCRecommendation.Tables[0].Rows[0]["SignDoctorID"])) + "</td></tr>");
            }
            else
            {
                if (SignatureLabelMHCRecommendation == "")
                {
                    sbDMHCRecommendation.Append("<tr><td align='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>Consultant&nbsp;Signature</b></td></tr>");
                }
                else
                {
                    sbDMHCRecommendation.Append("<tr><td align='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>" + SignatureLabelMHCRecommendation + "</b></td></tr>");
                }
            }

            sbDMHCRecommendation.Append("</table>");
            hdnReportContent.Value = "<div style='margin-left:3em; '>" + hdnReportContent.Value + sbDMHCRecommendation.ToString() + "</div>";
            #endregion
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
    }

    private void getDoctorImage(int DoctorId)
    {
        BaseC.clsLISPhlebotomy lis = new BaseC.clsLISPhlebotomy(sConString);
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        int intCheckImage = 0; // Check image from signed note signature
        Stream strm;
        Object img;
        DateTime SignatureDate;
        String UserName = "", ShowSignatureDate = "", UserDoctorId = "";
        String SignImage = "", SignNote = "";
        String DivStartTag = "<div id='dvDoctorImage' align='right'>";
        String SignedDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        String Education = string.Empty;
        String FileName = string.Empty;
        string strimgData = string.Empty;
        try
        {
            if (common.myInt(DoctorId) > 0)
            {
                //ds = lis.getDoctorImageDetails(common.myInt(ViewState["DoctorId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //                                common.myInt(ViewState["EncounterId"]));
                ds = lis.getDoctorImageDetails(common.myInt(DoctorId), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                              common.myInt(Session["EncounterId"]));
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0] as DataRow;
                    if (common.myStr(dr["SignatureImage"]) != "")
                    {
                        SignedDate = common.myStr(dr["SignedDate"]);
                        FileName = common.myStr(dr["SignatureImageName"]);
                        ShowSignatureDate = " on " + SignedDate;
                        Education = common.myStr(dr["SignedProviderEducation"]);
                        img = dr["SignatureImage"];
                        UserName = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]);
                        Session["EmpName"] = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]).Trim();
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);

                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");
                        strm = new MemoryStream((byte[])img);
                        byte[] buffer = new byte[strm.Length];
                        int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                        FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                        fs.Write(buffer, 0, byteSeq);
                        fs.Dispose();
                        //    RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName.Trim() + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                        SignImage = "<img align='right' width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
                        strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                        intCheckImage = 1;
                        strimgData = common.myStr(dr["ImageId"]);
                        SignNote = "Electronically signed by " + UserName.Trim() + " " + Education.Trim() + " " + ShowSignatureDate.Trim() + "</div>";
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (intCheckImage == 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0] as DataRow;
                        img = dr["SignatureImage"];
                        FileName = common.myStr(dr["ImageType"]);
                        UserName = common.myStr(dr["EmployeeName"]);
                        Session["EmpName"] = common.myStr(dr["EmployeeName"]).Trim();
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(dr["DoctorId"]);
                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");

                        if (common.myStr(dr["Education"]).Trim() != ""
                            && common.myStr(dr["Education"]).Trim() != "&nbsp;")
                        {
                            Education = common.myStr(dr["Education"]);
                        }
                        SignNote = "Electronically signed by " + UserName + " " + Education + " " + ShowSignatureDate + "</div>";

                        if (FileName != "")
                        {
                            //RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img align='right' width='100px' height='80px' src='../PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                            strimgData = common.myStr(dr["ImageId"]);
                        }
                        else if (common.myStr(dr["SignatureImage"]) != "")
                        {
                            strm = new MemoryStream((byte[])img);
                            byte[] buffer = new byte[strm.Length];
                            int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                            FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                            fs.Write(buffer, 0, byteSeq);
                            fs.Dispose();
                            //  RTF.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img align='right' width='100px' height='80px' src='../PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                            strimgData = common.myStr(dr["ImageId"]);
                        }
                    }
                }
                if (File.Exists(strSingImagePath))
                {
                    hdnDoctorImage.Value = DivStartTag + "<table align='right' border='0' cellpadding='0' cellspacing='0' style='font-size:10pt; font-family:Tahoma;'><tbody  align='right'><tr  align='right'><td align='right'>" + SignImage + "</td></tr></tbody></table><br />";
                }
                else
                {
                    hdnDoctorImage.Value = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {

            objException.HandleException(ex);
        }
        finally
        {
            lis = null;
            ds.Dispose();
            strm = null;
            img = null;
            UserName = null;
            ShowSignatureDate = null;
            UserDoctorId = null;
            SignImage = null;
            SignNote = null;
            DivStartTag = null;
            SignedDate = null;
            strSQL = null;
            strSingImagePath = null;
        }
    }

    private string PrintReportSignature(bool Isdoctorsignature, int DoctorId)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(getReportsSignature(Isdoctorsignature, DoctorId));
        return sb.ToString();
    }
    private StringBuilder getReportsSignature(bool IsPrintDoctorSignature, int DoctorId)
    {
        StringBuilder sb = new StringBuilder();
        try
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            clsIVF objivf = new clsIVF(sConString);
            ds = new DataSet();



            dt = objivf.getDoctorSignatureDetails(DoctorId, common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"])).Tables[0];
            if (IsPrintDoctorSignature)
            {
                sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' >");
                if (dt.Rows.Count > 0)
                {
                    if (common.myStr(dt.Rows[0]["DoctorName"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>" + common.myStr(dt.Rows[0]["DoctorName"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    //if (common.myStr(dt.Rows[0]["Education"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["Education"]).Trim() + "</b></td>");
                    //    sb.Append("</tr>");
                    //}
                    //if (common.myStr(dt.Rows[0]["Designation"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");
                    //    sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["Designation"]).Trim() + "</b></td>");
                    //    sb.Append("</tr>");
                    //}
                    //if (common.myStr(dt.Rows[0]["UPIN"]).Trim().Length > 0)
                    //{
                    //    sb.Append("<tr>");

                    //    if (common.isNumeric(common.myStr(dt.Rows[0]["UPIN"]).Trim()))
                    //    {
                    //        sb.Append("<td align ='right'><b>Regn. No. : " + common.myStr(dt.Rows[0]["UPIN"]).Trim() + "</b></td>");
                    //    }
                    //    else
                    //    {
                    //        sb.Append("<td align ='right'><b>" + common.myStr(dt.Rows[0]["UPIN"]).Trim() + "</b></td>");
                    //    }

                    //    sb.Append("</tr>");
                    //}
                    if (common.myStr(dt.Rows[0]["SignatureLine1"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>" + common.myStr(dt.Rows[0]["SignatureLine1"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine2"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>" + common.myStr(dt.Rows[0]["SignatureLine2"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine3"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>" + common.myStr(dt.Rows[0]["SignatureLine3"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                    if (common.myStr(dt.Rows[0]["SignatureLine4"]).Trim().Length > 0)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td align ='right' style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;'><b>" + common.myStr(dt.Rows[0]["SignatureLine4"]).Trim() + "</b></td>");
                        sb.Append("</tr>");
                    }
                }
                sb.Append("</table>");
            }
            else
            {
                sb.Append("<br />");
                sb.Append("<br />");
                sb.Append("<br />");
            }
            return sb;
        }

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }

    //private StringBuilder getReportHeader(int ReportId)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    try
    //    {

    //        DataSet ds = new DataSet();

    //        bool IsPrintHospitalHeader = false;
    //        clsIVF objivf = new clsIVF(sConString);
    //        ds = objivf.EditReportName(ReportId);

    //        if (ds.Tables.Count > 0)
    //        {
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
    //            }
    //        }

    //        ds = new DataSet();
    //        ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

    //        sb.Append("<div>");

    //        if (IsPrintHospitalHeader)
    //        {
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
    //                for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
    //                {
    //                    DataRow DR = ds.Tables[0].Rows[idx];

    //                    sb.Append("<tr>");

    //                    sb.Append("<td align ='center'>");
    //                    sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
    //                    sb.Append("<tr>");
    //                    sb.Append("<td></td><td></td><td></td><td></td><td></td>");
    //                    sb.Append("</tr>");
    //                    sb.Append("<tr>");
    //                    //sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='../Icons/SmallLogo.jpg' border='0' width='30px' height='25px'  alt='Image'/></td>");
    //                    sb.Append("<td colspan='2' align ='right' valign='middle' style='font-size:9px'><img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='30px' height='25px'  alt='Image'/></td>");
    //                    sb.Append("<td colspan='3' align ='left' valign='middle' style='font-size:9px'><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
    //                    sb.Append("</tr>");
    //                    sb.Append("</table>");
    //                    sb.Append("</td>");

    //                    sb.Append("</tr>");

    //                    sb.Append("<tr>");
    //                    sb.Append("<td align ='center'  style='font-size:9px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
    //                    sb.Append("</tr>");

    //                    //sb.Append("<tr>");
    //                    //sb.Append("<td align ='center'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");
    //                    //sb.Append("</tr>");

    //                    sb.Append("<tr>");
    //                    sb.Append("<td align ='center'  style='font-size:9px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
    //                    sb.Append("</tr>");
    //                }
    //                sb.Append("</table>");
    //            }
    //        }
    //        else
    //        {
    //            //sb.Append("<br />");
    //            //sb.Append("<br />");
    //            //sb.Append("<br />");
    //        }

    //        // sb.Append("<br />");
    //        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
    //        //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
    //        //sb.Append("<td align=center><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
    //        sb.Append("<td align=center style='font-family: " + common.myStr(hdnFontName.Value) + ";font-size:10pt;font-weight:bold;' ><U>" + common.myStr(ViewState["headingname"]) + "</U></td>");
    //        sb.Append("</tr></table></div>");

    //        return sb;
    //    }

    //    catch (Exception Ex)
    //    {
    //        objException.HandleException(Ex);
    //        sb = new StringBuilder();
    //        return sb;
    //    }
    //}
    private StringBuilder getReportHeader(int ReportId)
    {
        string ShowDischargeSummaryNABHLogoImage = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                                  common.myInt(Session["FacilityId"]), "ShowDischargeSummaryNABHLogoImage", sConString);

        if (ShowDischargeSummaryNABHLogoImage.Equals("Y"))
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                DataSet ds = new DataSet();

                bool IsPrintHospitalHeader = false;
                clsIVF objivf = new clsIVF(sConString);
                ds = objivf.EditReportName(ReportId);

                //ViewState["IsPrintDoctorSignature"] = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
                ViewState["IsPrintDoctorSignature"] = false;

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
                    }
                }

                ds = new DataSet();
                ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

                sb.Append("<div>");

                if (IsPrintHospitalHeader)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        //SignImage = "<img width='145px' height='66px' src='" + Server.MapPath("~") + FileName + "' />";
                        //strSingImagePath = Server.MapPath("~") + FileName;
                        string FileNameLogoImagePath = common.myStr(ds.Tables[0].Rows[0]["LogoImagePath"]);
                        string FileNameNABHLogoImagePath = common.myStr(ds.Tables[0].Rows[0]["NABHLogoImagePath"]);
                        sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                        sb.Append("<tr>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr>");
                        sb.Append("<td colspan='2'>");
                        //sb.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='100px' height='50px'  alt='Image'/>");
                        sb.Append("<img src='" + Server.MapPath("~") + FileNameLogoImagePath + "' border='0' width='100px' height='50px'  alt='Image'/>");
                        sb.Append("</td>");

                        sb.Append("<td colspan='6'>");

                        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[idx];

                            sb.Append("<table border='0' cellpadding='0' cellspacing='0' style='font-size:small'>");
                            sb.Append("<tr>");
                            sb.Append("<td  align ='center' style='font-size:26px' ><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td align ='center' style='font-size:10px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td align ='center' style='font-size:10px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td align ='center' style='font-size:10px'>E-mail : " + common.myStr(DR["EmailId"]) + " Website : " + common.myStr(DR["WebSite"]) + "</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                        }

                        sb.Append("</td>");
                        sb.Append("<td colspan='2'>");
                        //sb.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='100px' height='50px'  alt='Image'/>");
                        sb.Append("<img src='" + Server.MapPath("~") + FileNameNABHLogoImagePath + "' border='0' width='100px' height='50px'  alt='Image'/>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                    }
                }
                else
                {
                    //sb.Append("<br />");
                    //sb.Append("<br />");
                    //sb.Append("<br />");
                }

                // sb.Append("<br />");
                sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
                sb.Append("<td align=center><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
                sb.Append("</tr></table></div>");

                return sb;
            }

            catch (Exception Ex)
            {
                clsExceptionLog objException = new clsExceptionLog();
                sb = new StringBuilder();
                return sb;
            }
        }
        else
        {

            StringBuilder sb = new StringBuilder();
            try
            {

                DataSet ds = new DataSet();

                bool IsPrintHospitalHeader = false;
                clsIVF objivf = new clsIVF(sConString);
                ds = objivf.EditReportName(ReportId);

                // ViewState["IsPrintDoctorSignature"] = common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]);
                ViewState["IsPrintDoctorSignature"] = false;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
                    }
                }

                ds = new DataSet();
                ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

               

                if (IsPrintHospitalHeader)
                {
                    sb.Append("<div>");
                    string IsShowCustomMHCLogo = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "IsShowCustomMHCLogo", sConString);
                    if (IsShowCustomMHCLogo.Equals("Y")) // Arvind MHC
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                            sb.Append("<tr>");
                            sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td colspan='10' align='left' >");
                            sb.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='200px' height='50px'  alt='Image'/>");
                            sb.Append("</td>");

                            //sb.Append("<td colspan='8'>");

                            //for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                            //{
                            //    DataRow DR = ds.Tables[0].Rows[idx];

                            //    sb.Append("<table border='0' cellpadding='0' cellspacing='0' style='font-size:small'>");
                            //    sb.Append("<tr>");
                            //    sb.Append("<td  align ='left' style='font-size:9px' ><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                            //    sb.Append("</tr>");

                            //    sb.Append("<tr>");
                            //    sb.Append("<td align ='left' style='font-size:9px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                            //    sb.Append("</tr>");

                            //    sb.Append("<tr>");
                            //    sb.Append("<td align ='left' style='font-size:9px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                            //    sb.Append("</tr>");
                            //    sb.Append("</table>");
                            //}

                            //sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("</table><br/>");
                        }
                        //sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                        ////sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
                        //sb.Append("<td align=center><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
                        //sb.Append("</tr></table></div>");
                    }
                    else
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small'>");
                            sb.Append("<tr>");
                            sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            sb.Append("<td colspan='2'>");
                            sb.Append("<img src='" + Server.MapPath("/Icons/SmallLogo.jpg") + "' border='0' width='100px' height='50px'  alt='Image'/>");
                            sb.Append("</td>");

                            sb.Append("<td colspan='8'>");

                            for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                            {
                                DataRow DR = ds.Tables[0].Rows[idx];

                                sb.Append("<table border='0' cellpadding='0' cellspacing='0' style='font-size:small'>");
                                sb.Append("<tr>");
                                sb.Append("<td  align ='center' style='font-size:26px' ><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                                sb.Append("</tr>");

                                sb.Append("<tr>");
                                sb.Append("<td align ='center' style='font-size:10px'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                                sb.Append("</tr>");

                                sb.Append("<tr>");
                                sb.Append("<td align ='center' style='font-size:10px'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                                sb.Append("</tr>");

                                sb.Append("<tr>");
                                sb.Append("<td align ='center' style='font-size:10px'>E-mail : " + common.myStr(DR["EmailId"]) + " Website : " + common.myStr(DR["WebSite"]) + "</td>");
                                sb.Append("</tr>");
                                sb.Append("</table>");
                            }

                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                        }

                        else
                        {
                            //sb.Append("<br />");
                            //sb.Append("<br />");
                            //sb.Append("<br />");
                        }

                        // sb.Append("<br />");
                        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                        //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
                        sb.Append("<td align=center><U>" + common.myStr(ViewState["reportname"]) + "</U></td>");
                        sb.Append("</tr></table></div>");
                    }

                }
                return sb;
            }

            catch (Exception Ex)
            {
                clsExceptionLog objException = new clsExceptionLog();
                sb = new StringBuilder();
                return sb;
            }
        }
    }

    private StringBuilder getIVFPatient()
    {
        StringBuilder sb = new StringBuilder();
        try
        {

            DataSet ds = new DataSet();

            clsIVF objivf = new clsIVF(sConString);

            ds = objivf.getIVFPatient(common.myInt(ViewState["RegistrationId"]), 0);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView DV = ds.Tables[0].Copy().DefaultView;
                DV.RowFilter = "RegistrationId=" + common.myInt(ViewState["RegistrationId"]);

                DataTable tbl = DV.ToTable();

                if (tbl.Rows.Count > 0)
                {
                    DataRow DR = tbl.Rows[0];

                    DataView DVSpouse = ds.Tables[0].Copy().DefaultView;
                    DVSpouse.RowFilter = "RegistrationId<>" + common.myInt(ViewState["RegistrationId"]);
                    DataTable tblSpouse = DVSpouse.ToTable();

                    sb.Append("<div><table border='0' width='100%' style='font-size:smaller; border-collapse:collapse;' cellpadding='2' cellspacing='3' ><tr valign='top'>");
                    //sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "ivfno")) + "</td><td>: " + common.myStr(Session["IVFNo"]) + "</td>");
                    sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td>: " + common.myStr(DR["RegistrationNo"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "PatientName")) + "</td><td>: " + common.myStr(DR["PatientName"]) + "</td>");
                    sb.Append("<td style='width: 109px;'>Age/Gender</td><td>: " + common.myStr(DR["Age/Gender"]) + "</td>");
                    sb.Append("</tr>");

                    if (tblSpouse.Rows.Count > 0)
                    {
                        sb.Append("<tr valign='top'>");
                        sb.Append("<td>Spouse</td><td>: " + common.myStr(tblSpouse.Rows[0]["PatientName"]) + "</td>");
                        sb.Append("<td>Spouse Age/Gender</td><td>: " + common.myStr(tblSpouse.Rows[0]["Age/Gender"]) + "</td>");
                        sb.Append("</tr>");
                    }

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>Reg. Date</td><td>: " + common.myStr(DR["RegistrationDate"]) + "</td>");
                    sb.Append("<td>Occupation</td><td>: " + common.myStr(DR["Occupation"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "email")) + "</td><td>: " + common.myStr(DR["Email"]) + "</td>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "phone")) + "</td><td>: " + common.myStr(DR["PhoneHome"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "Address")) + "</td><td>: " + common.myStr(DR["PatientAddress"]) + "</td>");
                    sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "mobile")) + "</td><td>: " + common.myStr(DR["MobileNo"]) + "</td>");
                    sb.Append("</tr>");

                    sb.Append("</table></div>");
                }

                sb.Append("<hr />");

            }
            return sb;
        }

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }

        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }
    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        ViewState["CurrentTemplateFontName"] = string.Empty;
        ViewState["CurrentTemplateFontName"] = FontName;
        if (FontName != "")
        {
            sBegin += " font-family: " + FontName + ";";

            //sBegin += " font-family: " + FontName + ", sans-serif;";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myInt(Session["HospitalLocationId"]).ToString());
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                {
                    sBegin += " font-family: " + FontName + ";";
                }
            }
        }

        return sBegin;
    }

    protected void bindData(DataSet dsDynamicTemplateData, string TemplateId, StringBuilder sb, string GroupingDate)
    {
        DataSet ds = new DataSet();
        DataSet dsAllNonTabularSectionDetails = new DataSet();
        DataSet dsAllTabularSectionDetails = new DataSet();
        DataSet dsAllFieldsDetails = new DataSet();

        DataTable dtFieldValue = new DataTable();
        DataTable dtEntry = new DataTable();
        DataTable dtFieldName = new DataTable();

        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dv2 = new DataView();

        DataRow dr3;

        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        StringBuilder str = new StringBuilder();
        string sEntryType = "V";
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string BeginList2 = string.Empty;
        string BeginList3 = string.Empty;
        string EndList3 = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;

        int t = 0;
        int t2 = 0;
        int t3 = 0;
        int iRecordId = 0;
        DataView dvDyTable1 = new DataView();
        try
        {
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;

            t = 0;
            t2 = 0;
            t3 = 0;

            dvDyTable1 = new DataView(dsDynamicTemplateData.Tables[0]);
            DataView dvDyTable2 = new DataView(dsDynamicTemplateData.Tables[1]);
            DataView dvDyTable3 = new DataView(dsDynamicTemplateData.Tables[2]);

            dvDyTable1.ToTable().TableName = "TemplateSectionName";
            dvDyTable2.ToTable().TableName = "FieldName";
            dvDyTable3.ToTable().TableName = "PatientValue";
            dsAllNonTabularSectionDetails = new DataSet();
            if (dvDyTable3.ToTable().Rows.Count > 0)
            {
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable2.ToTable());
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable3.ToTable());
            }
            dvDyTable2.Dispose();
            dvDyTable3.Dispose();

            dsDynamicTemplateData.Dispose();

            #region Non Tabular
            if (dsAllNonTabularSectionDetails.Tables.Count > 0 && dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvNonTabular = new DataView(dvDyTable1.ToTable());
                dvNonTabular.RowFilter = "Tabular=0";
                if (dvNonTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvNonTabular.ToTable());//Section Name Table

                    dv = new DataView(dsAllNonTabularSectionDetails.Tables[1]);

                    dv.Sort = "RecordId DESC";
                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvNonTabular.Dispose();

                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllNonTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllNonTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]) + " AND SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllNonTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;
                                            dr3 = dsAllNonTabularSectionDetails.Tables[0].Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();

                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));



                                            //if(!common.myStr(item["TemplateName"].ToString()).Contains(common.myStr (ViewState["CheckTemplateName"])))
                                            //{
                                            //    str.Append("<br/> ");
                                            //}

                                            //ViewState["CheckTemplateName"] = common.myStr(item["TemplateName"]);
                                            //str.Append("<br/> ");

                                            //if (!common.myStr(ViewState["CheckCode"]).Equals(string.Empty))
                                            //{
                                            //    if (!common.myStr(item["Code"].ToString()).Equals(common.myStr(ViewState["CheckCode"])))
                                            //    {
                                            //          str.Append("<br/> ");
                                            //    }
                                            //}

                                            //ViewState["CheckCode"] = common.myStr(item["Code"]);

                                            //  str.Append("<br/> ");



                                            dr3 = null;
                                            dsAllNonTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();
                                            string sBreak = common.myBool(item["IsConfidential"]) == true ? "<br/>" : "";
                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        ////// objStrTmp.Append("<br />"); //code commented  for Examination (SectonName and fieldname getting extra space)
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (sBegin.Contains("<br/>"))
                                                        {
                                                            sBegin = sBegin.Remove(0, 5);

                                                            if (!common.myInt(item["TemplateId"]).Equals(6048) && !flag)
                                                            {
                                                                //  str.Append("<br/>");
                                                                // flag = true;
                                                                objStrTmp.Append("<br/>");
                                                                objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd);
                                                                flag = true;
                                                            }
                                                            else
                                                            {
                                                                if (common.myInt(item["TemplateId"]).Equals(2))
                                                                {
                                                                    objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");

                                                                }
                                                                else
                                                                {
                                                                    objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd);
                                                                }
                                                            }


                                                        }
                                                        else
                                                        {
                                                            objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd);
                                                        }
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(sBreak + common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {

                                                            if (sBegin.StartsWith("<br/>"))
                                                            {
                                                                if (sBegin.Length > 5)
                                                                {

                                                                    //sBegin = sBegin.Remove(0, 5);
                                                                    //objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                                                    objStrTmp.Append(BeginList2 + "<br/>" + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);

                                                            }

                                                            //if (sBegin.Contains("<br/>"))
                                                            //{
                                                            //    sBegin = sBegin.Remove(0, 5);
                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}
                                                            //else
                                                            //{

                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}

                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            //if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            //{
                                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                            ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            // }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Tabular
            DataView dvDyTable4 = new DataView(dsDynamicTemplateData.Tables[3]);
            DataView dvDyTable5 = new DataView(dsDynamicTemplateData.Tables[4]);
            DataView dvDyTable6 = new DataView(dsDynamicTemplateData.Tables[5]);

            dvDyTable4.ToTable().TableName = "TabularData";
            dvDyTable5.ToTable().TableName = "TabularColumnCount";
            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";

            dsAllTabularSectionDetails = new DataSet();
            if (dvDyTable4.ToTable().Rows.Count > 0)
            {
                dsAllTabularSectionDetails.Tables.Add(dvDyTable4.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable5.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable6.ToTable());
            }

            dvDyTable4.Dispose();
            dvDyTable5.Dispose();



            if (dsAllTabularSectionDetails.Tables.Count > 0 && dsAllTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvTabular = new DataView(dvDyTable1.ToTable());
                dvTabular.RowFilter = "Tabular=1";
                if (dvTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvTabular.ToTable());//Section Name Table
                    dv = new DataView(dsAllTabularSectionDetails.Tables[0]);
                    dv.Sort = "RecordId DESC";
                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvTabular.Dispose();
                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                DataView dvFieldStyle = new DataView(dsAllTabularSectionDetails.Tables[2]);
                                dvFieldStyle.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = " SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dsAllFieldsDetails.Tables.Add(dvDyTable6.ToTable());
                                dvDyTable6.Dispose();
                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;
                                            dr3 = dvFieldStyle.ToTable().Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();
                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                            str.Append("<br/> ");

                                            dr3 = null;
                                            dsAllTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();

                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        objStrTmp.Append("<br />");
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            if (sBegin.Contains("<br/>"))
                                                            {
                                                                sBegin = sBegin.Remove(0, 5);
                                                                objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
                                                            }
                                                            else
                                                            {
                                                                objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
                                                            }
                                                        }
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            {
                                                iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                                ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            if (t2.Equals(1) && t3.Equals(1))
            {
                objStrTmp.Append(EndList3);
            }
            else
            {
                objStrTmp.Append(EndList);
            }
            if (GetPageProperty("1") != null)
            {
                objStrSettings.Append(objStrTmp.ToString());
                sb.Append(objStrSettings.ToString());
            }
            else
            {
                sb.Append(objStrTmp.ToString());
            }
        }
        catch (Exception ex)
        {

            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            dsAllNonTabularSectionDetails.Dispose();
            dsAllTabularSectionDetails.Dispose();
            dsAllFieldsDetails.Dispose();

            dtFieldValue.Dispose();
            dtEntry.Dispose();
            dtFieldName.Dispose();
            dvDyTable1.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dv2.Dispose();

            dr3 = null;

            objStrTmp = null;
            objStrSettings = null;

            sEntryType = string.Empty;
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;
            sBegin = string.Empty;
            sEnd = string.Empty;
        }
    }
    protected DataSet GetPageProperty(string iFormId)
    {
        Hashtable hstInput = new Hashtable();
        if (common.myInt(Session["HospitalLocationID"]) > 0 && iFormId != "")
        {
            if (Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"] == null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", iFormId);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }

    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myInt(Session["HospitalLocationId"]).ToString());
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
            {
                sFontSize = " font-size: " + sFontSize + ";";
            }
        }
        return sFontSize;
    }

    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        sFontSize = string.Empty;

        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sFontSize += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sFontSize += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sFontSize += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sFontSize += GetFontFamily(typ, item);
            };

            if (common.myStr(item[typ + "Bold"]) == "True")
            {
                sFontSize += " font-weight: bold;";
            }
            if (common.myStr(item[typ + "Italic"]) == "True")
            {
                sFontSize += " font-style: italic;";
            }
            if (common.myStr(item[typ + "Underline"]) == "True")
            {
                sFontSize += " text-decoration: underline;";
            }
        }

        return sFontSize;
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType,
       string sectionId, string EntryType, int RecordId, string GroupingDate, bool IsConfidential)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objStr = new StringBuilder();
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        DataSet dsTabulerTemplate = new DataSet();
        try
        {
            if (objDs != null)
            {
                if (IsConfidential == false)
                {
                    #region Tabular
                    if (bool.Parse(TabularType) == true)
                    {
                        DataView dvFilter = new DataView(objDs.Tables[0]);
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;
                            dvFilter.Sort = "RowNum ASC";
                            if (GroupingDate != "")
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND RecordId= " + RecordId;
                            }
                            DataTable dtNewTable = dvFilter.ToTable();
                            if (dtNewTable.Rows.Count > 0)
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                StringBuilder sbCation = new StringBuilder();
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();
                                        for (int k = 0; k < column; k++)
                                        {
                                            sbCation.Append("<td>");
                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                            sbCation.Append("</td>");
                                            count++;
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId;
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 1; i < ColumnCount + 1; i++)
                                            {
                                                if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        dt.Dispose();
                                        dvData.Dispose();
                                    }
                                    sbCation.Append("</table>");
                                }
                                objStr.Append(sbCation);
                                dsTabulerTemplate.Dispose();
                                sbCation = null;

                            }
                            else
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                if (GroupingDate != "")
                                {
                                    dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                                }
                                else
                                {
                                    dvRowCaption.RowFilter = "RecordId= " + RecordId;
                                }
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    StringBuilder sbCation = new StringBuilder();
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    // dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'   cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();

                                        for (int k = 0; k < column + 1; k++)
                                        {
                                            if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                                && ColumnCount == 0)
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(" + ");
                                                sbCation.Append("</td>");
                                            }
                                            else
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                            }
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0";
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 0; i < ColumnCount; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["RowCaptionName"]) + "</td>");
                                                }
                                                else
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td align='center' ><img id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        sbCation.Append("</table>");
                                        dvData.Dispose();
                                    }
                                    objStr.Append(sbCation);
                                    dt.Dispose();
                                    sbCation = null;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Non Tabular
                    else // For Non Tabular Templates
                    {
                        string BeginList = "", EndList = "";
                        string sBegin = "", sEnd = "";
                        int t = 0;
                        string FieldId = "";
                        string sStaticTemplate = "";
                        string sEnterBy = "";
                        string sVisitDate = "";
                        foreach (DataRow item in objDs.Tables[0].Rows)
                        {
                            objDv = new DataView(objDs.Tables[1]);
                            objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                            objDt = objDv.ToTable();
                            if (t == 0)
                            {
                                t = 1;
                                if (common.myStr(item["FieldsListStyle"]) == "1")
                                {
                                    BeginList = "<ul>"; EndList = "</ul>";
                                }
                                else if (item["FieldsListStyle"].ToString() == "2")
                                {
                                    BeginList = "<ol>"; EndList = "</ol>";
                                }
                            }
                            if (common.myStr(item["FieldsBold"]) != ""
                                || common.myStr(item["FieldsItalic"]) != ""
                                || common.myStr(item["FieldsUnderline"]) != ""
                                || common.myStr(item["FieldsFontSize"]) != ""
                                || common.myStr(item["FieldsForecolor"]) != ""
                                || common.myStr(item["FieldsListStyle"]) != "")
                            {
                                //rafat1
                                if (objDt.Rows.Count > 0)
                                {
                                    sBegin = "";
                                    sEnd = "";

                                    MakeFont("Fields", ref sBegin, ref sEnd, item);
                                    if (common.myBool(item["DisplayTitle"]))
                                    {
                                        // if (EntryType != "M")
                                        // {


                                        ////if (sBegin.StartsWith("<br/>"))
                                        ////{
                                        ////    if (sBegin.Length > 5)
                                        ////    {
                                        ////        sBegin = sBegin.Substring(5, sBegin.Length - 5);

                                        ////    }
                                        ////}

                                        objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                        //}
                                        //else
                                        //{
                                        //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                        //}
                                        // 28/08/2011
                                        //if (objDt.Rows.Count > 0)
                                        //{
                                        if (objStr.ToString() != "")
                                        {
                                            //  objStr.Append(sEnd + "</li>");
                                        }
                                        ViewState["sBegin"] = sBegin;
                                    }

                                    BeginList = "";
                                    sBegin = "";
                                    sEnd = "";

                                }

                            }
                            else
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (sStaticTemplate != "<br/><br/>")
                                    {
                                        objStr.Append(common.myStr(item["FieldName"]));
                                    }
                                }
                            }
                            if (objDs.Tables.Count > 1)
                            {

                                objDv = new DataView(objDs.Tables[1]);
                                objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                objDt = objDv.ToTable();
                                DataView dvFieldType = new DataView(objDs.Tables[0]);
                                dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                                sBegin = "";
                                sEnd = "";

                                string sbeginTemp = string.Empty;
                                MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);
                                // MakeFont("Fields", ref sBegin, ref sEnd, item);
                                for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                                {
                                    if (objDt.Rows.Count > 0)
                                    {

                                        sbeginTemp = common.myStr(ViewState["sBegin"]);
                                        if (sbeginTemp.StartsWith("<br/>"))
                                        {
                                            if (sbeginTemp.Length > 5)
                                            {
                                                sbeginTemp = sbeginTemp.Substring(0, 5);

                                                //objStrTmp.Append(sBegin + common.myStr(item["SectionName"]) + sEnd);
                                            }
                                        }



                                        string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                        if (FType == "C")
                                        {
                                            FType = "C";
                                        }
                                        if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
                                        {
                                            if (FType == "B")
                                            {
                                                //  objStr.Append(" : " + objDt.Rows[i]["TextValue"]);
                                                objStr.Append(" " + objDt.Rows[i]["TextValue"]);
                                            }
                                            else
                                            {
                                                //////BindDataValue(objDs, objDt, objStr, i, FType) //comeented by niraj , create and added below overloading methd
                                                BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);
                                            }
                                        }
                                        else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                        {
                                            if (common.myStr(ViewState["iTemplateId"]) != "163")
                                            {
                                                if (i == 0)
                                                {
                                                    if (FType == "W")
                                                    {
                                                        objStr.Append(sBegin + " <br /> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    }
                                                    else if (FType == "M")
                                                    {
                                                        //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                        objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]).Replace("\n\r", "<br/>").Replace("\n", "<br/>") + sEnd);
                                                    }
                                                    else
                                                    {
                                                        //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                        objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]).Replace("<", "&lt;").Replace(">", "&gt;") + sEnd);
                                                    }

                                                }
                                                else
                                                {
                                                    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //if (FType == "M" || FType == "W")
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //}
                                                    //else
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                    //}

                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    //objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else if (FType == "L")
                                        {
                                            objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                        }
                                        else if (FType == "IM")
                                        {
                                            objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
                                        }
                                        if (common.myStr(item["FieldsListStyle"]) == "")
                                        {
                                            if (ViewState["iTemplateId"].ToString() != "163")
                                            {
                                                if (FType != "C")
                                                {

                                                    if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        objStr.Append("<br />");

                                                    }

                                                }

                                            }
                                            else
                                            {
                                                if (FType != "C" && FType != "T")
                                                {
                                                    objStr.Append("<br />");
                                                }
                                            }
                                        }





                                    }
                                    sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                                    sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                                    //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
                                    //{
                                    //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=' font-size:8pt;'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                                    //}
                                }
                                sBegin = "";
                                sEnd = "";
                                dvFieldType.Dispose();
                                dtFieldType.Dispose();

                                // Cmt 25/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                //    if (objStr.ToString() != "")
                                //        objStr.Append(sEnd + "</li>");
                                //}
                            }

                            //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                        }

                        if (objStr.ToString() != "")
                        {
                            objStr.Append(EndList);
                        }
                    }
                    #endregion
                }
                string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
                if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
                {
                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            //objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");

                            if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                            {
                                // objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; '>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");

                            }
                            else
                            {
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");
                            }
                        }
                        dvValues.Dispose();
                    }
                    else
                    {
                        if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == true)
                        {
                            DataView dvValues = new DataView(objDs.Tables[0]);
                            dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
                            if (dvValues.ToTable().Rows.Count > 0)
                            {
                                // objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span><br/>");
                                if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                                {
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span></b>");
                                }
                                else
                                {
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span></b>");
                                }
                            }
                            dvValues.Dispose();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

            objException.HandleException(Ex);
        }
        finally
        {
            objDv.Dispose();
            objDt.Dispose();
            dsMain.Dispose();
            objDs.Dispose();
            dsTabulerTemplate.Dispose();
        }
        return objStr.ToString();
    }

    //protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType,
    //                       string sectionId, string EntryType, int RecordId, string GroupingDate, bool IsConfidential)
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    StringBuilder objStr = new StringBuilder();
    //    DataView objDv = new DataView();
    //    DataTable objDt = new DataTable();
    //    DataSet dsMain = new DataSet();
    //    StringBuilder objStrTmp = new StringBuilder();
    //    DataSet dsTabulerTemplate = new DataSet();
    //    try
    //    {
    //        if (objDs != null)
    //        {
    //            if (IsConfidential == false)
    //            {
    //                #region Tabular
    //                if (bool.Parse(TabularType) == true)
    //                {
    //                    DataView dvFilter = new DataView(objDs.Tables[0]);
    //                    if (objDs.Tables[0].Rows.Count > 0)
    //                    {
    //                        string sBegin = string.Empty;
    //                        string sEnd = string.Empty;
    //                        dvFilter.Sort = "RowNum ASC";
    //                        if (GroupingDate != "")
    //                        {
    //                            dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
    //                        }
    //                        else
    //                        {
    //                            dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND RecordId= " + RecordId;
    //                        }
    //                        DataTable dtNewTable = dvFilter.ToTable();
    //                        if (dtNewTable.Rows.Count > 0)
    //                        {
    //                            DataView dvRowCaption = new DataView(objDs.Tables[0]);
    //                            StringBuilder sbCation = new StringBuilder();
    //                            if (dvRowCaption.ToTable().Rows.Count > 0)
    //                            {
    //                                dvRowCaption.RowFilter = "RowNum>0";
    //                                DataTable dt = dvRowCaption.ToTable();
    //                                dvRowCaption.Dispose();
    //                                if (dt.Rows.Count > 0)
    //                                {
    //                                    sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellspacing='3' ><tr align='center'>");
    //                                    DataView dvColumnCount = new DataView(objDs.Tables[1]);
    //                                    dvColumnCount.RowFilter = "SectionId=" + sectionId;

    //                                    int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
    //                                    int ColumnCount = 0;
    //                                    int count = 1;
    //                                    dvColumnCount.Dispose();
    //                                    for (int k = 0; k < column; k++)
    //                                    {
    //                                        sbCation.Append("<td>");
    //                                        sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
    //                                        sbCation.Append("</td>");
    //                                        count++;
    //                                        ColumnCount++;
    //                                    }
    //                                    sbCation.Append("</tr>");

    //                                    DataView dvData = new DataView(dt);
    //                                    if (GroupingDate != "")
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId + " AND GroupDate='" + GroupingDate + "'";
    //                                    }
    //                                    else
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId;
    //                                    }

    //                                    for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
    //                                    {
    //                                        sbCation.Append("<tr>");
    //                                        for (int i = 1; i < ColumnCount + 1; i++)
    //                                        {
    //                                            if (dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                            {
    //                                                if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                {
    //                                                    sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
    //                                                }
    //                                                else
    //                                                {
    //                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                {
    //                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
    //                                                }
    //                                                else
    //                                                {
    //                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                }
    //                                            }
    //                                        }
    //                                        sbCation.Append("</tr>");
    //                                    }
    //                                    dt.Dispose();
    //                                    dvData.Dispose();
    //                                }
    //                                sbCation.Append("</table>");
    //                            }
    //                            objStr.Append(sbCation);
    //                            dsTabulerTemplate.Dispose();
    //                            sbCation = null;

    //                        }
    //                        else
    //                        {
    //                            DataView dvRowCaption = new DataView(objDs.Tables[0]);
    //                            if (GroupingDate != "")
    //                            {
    //                                dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
    //                            }
    //                            else
    //                            {
    //                                dvRowCaption.RowFilter = "RecordId= " + RecordId;
    //                            }
    //                            if (dvRowCaption.ToTable().Rows.Count > 0)
    //                            {
    //                                StringBuilder sbCation = new StringBuilder();
    //                                dvRowCaption.RowFilter = "RowNum>0";
    //                                DataTable dt = dvRowCaption.ToTable();
    //                                // dvRowCaption.Dispose();
    //                                if (dt.Rows.Count > 0)
    //                                {
    //                                    sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'   cellspacing='3' ><tr align='center'>");
    //                                    DataView dvColumnCount = new DataView(objDs.Tables[1]);
    //                                    dvColumnCount.RowFilter = "SectionId=" + sectionId;

    //                                    int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
    //                                    int ColumnCount = 0;
    //                                    int count = 1;
    //                                    dvColumnCount.Dispose();

    //                                    for (int k = 0; k < column + 1; k++)
    //                                    {
    //                                        if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
    //                                            && ColumnCount == 0)
    //                                        {
    //                                            sbCation.Append("<td>");
    //                                            sbCation.Append(" + ");
    //                                            sbCation.Append("</td>");
    //                                        }
    //                                        else
    //                                        {
    //                                            sbCation.Append("<td>");
    //                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
    //                                            sbCation.Append("</td>");
    //                                            count++;
    //                                        }
    //                                        ColumnCount++;
    //                                    }
    //                                    sbCation.Append("</tr>");

    //                                    DataView dvData = new DataView(dt);
    //                                    if (GroupingDate != "")
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";
    //                                    }
    //                                    else
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0";
    //                                    }

    //                                    for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
    //                                    {
    //                                        sbCation.Append("<tr>");
    //                                        for (int i = 0; i < ColumnCount; i++)
    //                                        {
    //                                            if (i == 0)
    //                                            {
    //                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["RowCaptionName"]) + "</td>");
    //                                            }
    //                                            else
    //                                            {
    //                                                if (dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                                {
    //                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                    {
    //                                                        sbCation.Append("<td align='center' ><img id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                        sbCation.Append("</tr>");
    //                                    }
    //                                    sbCation.Append("</table>");
    //                                    dvData.Dispose();
    //                                }
    //                                objStr.Append(sbCation);
    //                                dt.Dispose();
    //                                sbCation = null;
    //                            }
    //                        }
    //                    }
    //                }
    //                #endregion
    //                #region Non Tabular
    //                else // For Non Tabular Templates
    //                {
    //                    string BeginList = "", EndList = "";
    //                    string sBegin = "", sEnd = "";
    //                    int t = 0;
    //                    string FieldId = "";
    //                    string sStaticTemplate = "";
    //                    string sEnterBy = "";
    //                    string sVisitDate = "";
    //                    objStr.Append("<table border='0' style='border: 0px;'>"); //string tagTable = "<table border='0' style='border: 0px;'>";
    //                                                                              // objStr.Append("<tr><td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> <td></td> </tr>");
    //                    foreach (DataRow item in objDs.Tables[0].Rows)
    //                    {
    //                        objDv = new DataView(objDs.Tables[1]);
    //                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
    //                        objDt = objDv.ToTable();
    //                        if (t == 0)
    //                        {
    //                            t = 1;
    //                            if (common.myStr(item["FieldsListStyle"]) == "1")
    //                            {
    //                                BeginList = "<ul>"; EndList = "</ul>";
    //                            }
    //                            else if (item["FieldsListStyle"].ToString() == "2")
    //                            {
    //                                BeginList = "<ol>"; EndList = "</ol>";
    //                            }
    //                        }
    //                        if (common.myStr(item["FieldsBold"]) != ""
    //                            || common.myStr(item["FieldsItalic"]) != ""
    //                            || common.myStr(item["FieldsUnderline"]) != ""
    //                            || common.myStr(item["FieldsFontSize"]) != ""
    //                            || common.myStr(item["FieldsForecolor"]) != ""
    //                            || common.myStr(item["FieldsListStyle"]) != "")
    //                        {
    //                            //rafat1
    //                            if (objDt.Rows.Count > 0)
    //                            {
    //                                sBegin = "";
    //                                sEnd = "";

    //                                MakeFont("Fields", ref sBegin, ref sEnd, item);
    //                                if (sBegin.Contains("<br/>"))
    //                                {
    //                                    sBegin = sBegin.Remove(0, 5);
    //                                }
    //                                string sBeginFontWeightNormal = sBegin.Replace("bold", "normal");
    //                                if (Convert.ToBoolean(item["DisplayTitle"]))
    //                                {

    //                                    // if (EntryType != "M")
    //                                    // {


    //                                    ////if (sBegin.StartsWith("<br/>"))
    //                                    ////{
    //                                    ////    if (sBegin.Length > 5)
    //                                    ////    {
    //                                    ////        sBegin = sBegin.Substring(5, sBegin.Length - 5);

    //                                    ////    }
    //                                    ////}


    //                                    //}
    //                                    //else
    //                                    //{
    //                                    //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
    //                                    //}
    //                                    // 28/08/2011
    //                                    //if (objDt.Rows.Count > 0)
    //                                    //{

    //                                    objStr.Append("<tr><td colspan='2' border='0' style='border: 0px; width:100px''>" + BeginList + sBegin + common.myStr(item["FieldName"]) + "</td>");
    //                                    // objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));

    //                                    if (objStr.ToString() != "")
    //                                    {
    //                                        objStr.Append(sEnd + "</li>");
    //                                    }
    //                                    ViewState["sBegin"] = sBegin;
    //                                }

    //                                BeginList = "";
    //                                sBegin = "";
    //                                sEnd = "";

    //                            }

    //                        }
    //                        else
    //                        {
    //                            if (objDt.Rows.Count > 0)
    //                            {
    //                                if (sStaticTemplate != "<br/><br/>")
    //                                {
    //                                    // objStr.Append(common.myStr(item["FieldName"]));
    //                                    objStr.Append("<tr><td colspan='2' border='0' style='border: 0px;width:100px'>" + common.myStr(item["FieldName"]) + "</td>");
    //                                }
    //                            }
    //                        }
    //                        if (objDs.Tables.Count > 1)
    //                        {

    //                            objDv = new DataView(objDs.Tables[1]);
    //                            objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
    //                            objDt = objDv.ToTable();
    //                            DataView dvFieldType = new DataView(objDs.Tables[0]);
    //                            dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
    //                            DataTable dtFieldType = dvFieldType.ToTable("FieldType");
    //                            sBegin = "";
    //                            sEnd = "";

    //                            string sbeginTemp = string.Empty;
    //                            MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);
    //                            string sBeginFontWeightNormalWithoutBR = sBegin.Replace("bold", "normal");
    //                            // MakeFont("Fields", ref sBegin, ref sEnd, item);
    //                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
    //                            {
    //                                if (objDt.Rows.Count > 0)
    //                                {

    //                                    sbeginTemp = common.myStr(ViewState["sBegin"]);
    //                                    if (sbeginTemp.StartsWith("<br/>"))
    //                                    {
    //                                        if (sbeginTemp.Length > 5)
    //                                        {
    //                                            sbeginTemp = sbeginTemp.Substring(0, 5);

    //                                            //objStrTmp.Append(sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                        }
    //                                    }



    //                                    string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
    //                                    FieldId = common.myStr(dtFieldType.Rows[0]["FieldId"]);
    //                                    if (FType == "C")
    //                                    {
    //                                        FType = "C";
    //                                    }
    //                                    if (FType == "C" || FType == "D" || FType == "B" || FType == "R" || (FType == "T" && FieldId == "141708") || (FType == "T" && FieldId == "141709") || (FType == "T" && FieldId == "141698") || (FType == "T" && FieldId == "143774"))
    //                                    {
    //                                        if (FType == "B")
    //                                        {
    //                                            if (objStr.ToString().EndsWith("<tr>"))
    //                                            {
    //                                                objStr.Append("<td colspan='3'></td>");
    //                                            }

    //                                            objStr.Append("<td colspan='7' style='border: 0px; '>" + " : " + objDt.Rows[i]["TextValue"] + "</td>");

    //                                            //  objStr.Append("<td colspan='4'> </td>");
    //                                            //objStr.Append("  " + objDt.Rows[i]["TextValue"]);
    //                                        }
    //                                        else if (FType == "T")
    //                                        {
    //                                            objStr.Append("<td colspan='2' style='border: 0px;width:100px'>" + common.myStr(item["FieldName"]) + "</td>");
    //                                            objStr.Append("<td colspan='5' style='border: 0px; '>" + " : " + objDt.Rows[i]["TextValue"] + "</td>");
    //                                        }

    //                                        else
    //                                        {
    //                                            //////BindDataValue(objDs, objDt, objStr, i, FType) //comeented by niraj , create and added below overloading methd
    //                                            //   BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);
    //                                            BindDataValueNew(objDs, objDt, objStr, i, FType, sBegin, sEnd);
    //                                        }
    //                                    }
    //                                    else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
    //                                    {
    //                                        if (common.myStr(ViewState["iTemplateId"]) != "163")
    //                                        {
    //                                            if (i == 0)
    //                                            {
    //                                                if (FType == "M" || FType == "W")
    //                                                {

    //                                                    objStr.Append(sBeginFontWeightNormalWithoutBR + " <br /> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                }
    //                                                else
    //                                                {
    //                                                    //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

    //                                                    //objStr.Append(sBeginFontWeightNormalWithoutBR + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

    //                                                    //if (objStr.ToString().EndsWith("<tr>"))
    //                                                    //{
    //                                                    //    objStr.Append("<td colspan='3'></td>");
    //                                                    //}
    //                                                    //if(common.myStr(ViewState["iTemplateId"]).Equals("29084") && FType == "T")
    //                                                    //{
    //                                                    //    objStr.Append("<td colspan='2'></td>");
    //                                                    //}

    //                                                    if (objStr.ToString().Trim().EndsWith("</td></tr>"))
    //                                                    {
    //                                                        int objStrLen = objStr.ToString().Length - 10;
    //                                                        objStr = objStr.Remove(objStrLen, 10);
    //                                                        //  objStr.ToString().Trim().Replace("</td></tr>", "</td>");
    //                                                    }
    //                                                    objStr.Append("<td colspan='5' border='0' style='border: 0px;'>" + sBeginFontWeightNormalWithoutBR + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + "</td></tr>" + sEnd);
    //                                                    // objStr.Append(sBeginFontWeightNormalWithoutBR + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                    // objStr.Append("<td colspan='4'> </td>");

    //                                                }

    //                                            }
    //                                            else
    //                                            {
    //                                                objStr.Append(sBeginFontWeightNormalWithoutBR + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                //if (FType == "M" || FType == "W")
    //                                                //{
    //                                                //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                //}
    //                                                //else
    //                                                //{
    //                                                //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

    //                                                //}

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (i == 0)
    //                                            {
    //                                                objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                            }
    //                                            else
    //                                            {
    //                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                            }
    //                                        }
    //                                    }
    //                                    else if (FType == "L")
    //                                    {
    //                                        objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
    //                                    }
    //                                    else if (FType == "IM")
    //                                    {
    //                                        objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
    //                                    }
    //                                    if (common.myStr(item["FieldsListStyle"]) == "")
    //                                    {
    //                                        if (ViewState["iTemplateId"].ToString() != "163")
    //                                        {
    //                                            if (FType != "C")
    //                                            {

    //                                                if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
    //                                                {

    //                                                }
    //                                                else
    //                                                {
    //                                                    objStr.Append("<br />");

    //                                                }

    //                                            }

    //                                        }
    //                                        else
    //                                        {
    //                                            if (FType != "C" && FType != "T")
    //                                            {
    //                                                objStr.Append("<br />");
    //                                            }
    //                                        }
    //                                    }





    //                                }
    //                                sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
    //                                sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
    //                                //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
    //                                //{
    //                                //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=' font-size:8pt;'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
    //                                //}
    //                            }
    //                            sBegin = "";
    //                            sEnd = "";
    //                            dvFieldType.Dispose();
    //                            dtFieldType.Dispose();

    //                            // Cmt 25/08/2011
    //                            //if (objDt.Rows.Count > 0)
    //                            //{
    //                            //    if (objStr.ToString() != "")
    //                            //        objStr.Append(sEnd + "</li>");
    //                            //}
    //                        }

    //                        //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
    //                    }
    //                    objStr.Append("</table>");
    //                    if (objStr.ToString() != "")
    //                    {
    //                        objStr.Append(EndList);
    //                    }
    //                }
    //                #endregion
    //            }
    //            string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
    //            if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
    //            {
    //                if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
    //                {
    //                    DataView dvValues = new DataView(objDs.Tables[1]);
    //                    dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
    //                    if (dvValues.ToTable().Rows.Count > 0)
    //                    {
    //                        objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
    //                    }
    //                    dvValues.Dispose();
    //                }
    //                else
    //                {
    //                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == true)
    //                    {
    //                        DataView dvValues = new DataView(objDs.Tables[0]);
    //                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
    //                        if (dvValues.ToTable().Rows.Count > 0)
    //                        {
    //                            objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(hdnFontName.Value) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span><br/>");
    //                        }
    //                        dvValues.Dispose();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        //lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        objDv.Dispose();
    //        objDt.Dispose();
    //        dsMain.Dispose();
    //        objDs.Dispose();
    //        dsTabulerTemplate.Dispose();
    //    }
    //    return objStr.ToString();
    //}
    private string BindNonTabularImageTypeFieldValueTemplates(DataTable dtIMTypeTemplate)
    {
        StringBuilder sb = new StringBuilder();
        if (dtIMTypeTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) != "")
            {
                sb.Append("<table id='dvImageType' runat='server'><tr><td>" + common.myStr(dtIMTypeTemplate.Rows[0]["TextValue"]) + "</td></tr><tr align='left'><td align='center'><img src='" + common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) + "' width='80px' height='80px' border='0' align='left' alt='Image' /></td></tr></table>");
            }
        }
        return sb.ToString();
    }
    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        int RegId = 0;
        int EncounterId = 0;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EREncounterId"]).ToString());
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();

        sb.Append("<span style='" + string.Empty + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                drTemplateStyle = null;// = dv[0].Row;
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAllergies(common.myInt(ViewState["RegistrationId"]), sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(),
                            common.myInt(Session["UserID"]).ToString(), common.myStr(dtTemplate.Rows[0]["PageID"]),
                            common.myDate(FromDate).ToString(),
                            common.myDate(ToDate).ToString(), TemplateFieldId, "");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindVitals(common.myInt(Session["HospitalLocationID"]).ToString(), common.myInt(ViewState["EncounterId"]), sbStatic, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                    common.myDate(FromDate).ToString(),
                                    common.myDate(ToDate).ToString(), TemplateFieldId, common.myInt(ViewState["EREncounterId"]).ToString(), "");

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAssessments(common.myInt(ViewState["RegistrationId"]), common.myInt(Session["HospitalLocationID"]), common.myInt(ViewState["EncounterId"]), Convert.ToInt16(common.myInt(Session["UserID"])),
                            DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                            common.myDate(FromDate).ToString(),
                            common.myDate(ToDate).ToString(), TemplateFieldId, common.myInt(ViewState["EREncounterId"]).ToString(), "");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    protected void MakeFontWithoutBR(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            //if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            //{
            //    sBegin += "<br/>";
            //}
            //else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            //{
            //    sBegin += "; ";
            //}
            //else
            //{
            //    sBegin += "<br/>";
            //}
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }

    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        if (i == 0)
        {
            //objStr.Append(" : " + common.myStr(objDt.Rows[i]["TextValue"]));
            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(" and " + common.myStr(objDt.Rows[i]["TextValue"]) + ".");
                }
                else
                {
                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                }
            }
        }
        //}
    }

    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    {
        if (i == 0)
        {
            //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(sBegin + ", " + sBegin + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(sBegin + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
                }
                else
                {
                    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
            }
        }
        //}
    }
    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
                //// //sBegin += "<br/>";
            }
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }

    protected void BindDataValueNew(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    {
        string sBeginFontWeightNormal = sBegin.Replace("bold", "normal");
        bool FlagTagCloseTable = true;
        //for (int iFlagTagCloseTable = 0; iFlagTagCloseTable < objDs.Tables[0].Rows.Count; iFlagTagCloseTable++)
        //{
        //if (common.myStr(objDs.Tables[0].Rows[iFlagTagCloseTable]["FieldType"]).Equals("T")
        //    || common.myStr(objDs.Tables[0].Rows[iFlagTagCloseTable]["FieldType"]).Equals("M")
        //    || common.myStr(objDs.Tables[0].Rows[iFlagTagCloseTable]["FieldType"]).Equals("S")
        //   || common.myStr(objDs.Tables[0].Rows[iFlagTagCloseTable]["FieldType"]).Equals("W")
        //    )
        //{
        FlagTagCloseTable = false;
        //}
        //}
        //for
        //if ("C" || FType == "D" || FType == "B" || FType == "R")
        if (i == 0)
        {
            // objStr.Append(sBeginFontWeightNormal + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            if (FlagTagCloseTable)
            {
                objStr.Append("<td   border='0' style='border: 0px;'>" + sBeginFontWeightNormal + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + "</td></tr>" + sEnd);
                // objStr.Append("<td colspan='4'> </td>");
            }
            else
            {
                objStr.Append("<td colspan='5'  border='0' style='border: 0px;'> " + sBeginFontWeightNormal + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                // objStr.Append("<td colspan='4'> </td>");
            }
            if (i + 1 == objDt.Rows.Count)
            {
                objStr.Append("</td></tr>");
            }
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append("<td   border='0' style='border: 0px;'>" + sBeginFontWeightNormal + ", " + sBeginFontWeightNormal + common.myStr(objDt.Rows[i]["TextValue"]) + "</td>" + sEnd);
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(sBeginFontWeightNormal + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(sBeginFontWeightNormal + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
                }
                else
                {
                    objStr.Append(sBeginFontWeightNormal + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                if (i + 1 == objDt.Rows.Count)
                {
                    objStr.Append("</td></tr>");
                }
            }
        }
    }

}
