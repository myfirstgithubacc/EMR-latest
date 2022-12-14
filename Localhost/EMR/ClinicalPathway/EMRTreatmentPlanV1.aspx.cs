using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net;

public partial class EMR_ClinicalPathway_EMRTreatmentPlanV1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private const int ItemsPerRequest = 10;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    
    private string cCtlType = string.Empty;
    DataSet objDs = new DataSet();
    DataTable dt = new DataTable();
   

    private enum enumNonT : byte
    {
        FieldId = 0,
        FieldName = 1,
        FieldType = 2,
        Values = 3,
        Remarks = 4,
        ParentId = 5,
        ParentValue = 6,
        Hierarchy = 7,
        SectionId = 8,
        DataObjectId = 9,
        IsMandatory = 10,
        MandatoryType = 11,
        EmployeeTypeId = 12
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            lblMessage.Text = string.Empty;

            if (!IsPostBack)
            {
                bindtemplate();
                bindEMRGetTreatmentPlanList();
                BindSpeciliazation();
                BindDepartment();
                bind_ddlBrandPrescriptionsControl();
                GetServiceData(0, 0, 0, "");
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objException = null;
        }
    }

    protected void GetServiceData(int TemplatePlanId, int DayId, int DayDetailId, string TemplateType)
    
{

        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        DataView dv = new DataView();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanDetails";

            objRoot.TemplateId = common.myStr(TemplatePlanId);
            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;
            objRoot.TemplateType = TemplateType;
            objRoot.IsCopy = false;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            objDs = ds;
            if (TemplateType == "SP")
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    gvSpecialsation.DataSource = ds.Tables[2];
                    gvSpecialsation.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[2].NewRow();
                    ds.Tables[2].Rows.InsertAt(dr, 0);
                    gvSpecialsation.DataSource = ds.Tables[2];
                    gvSpecialsation.DataBind();
                }
            }
            else if (TemplateType == "SR")
            {
                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[3].NewRow();
                    ds.Tables[3].Rows.InsertAt(dr, 0);
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
            }
            else if (TemplateType == "DC")
            {
                if (ds.Tables[4].Rows.Count > 0)
                {
                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[4].NewRow();
                    ds.Tables[4].Rows.InsertAt(dr, 0);

                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
            }
            else if (TemplateType == "DR")
            {
                if (ds.Tables[5].Rows.Count > 0)
                {
                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[5].NewRow();
                    ds.Tables[5].Rows.InsertAt(dr, 0);

                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
            }
            else if (TemplateType == "TM")
            {
                if (ds.Tables[6].Rows.Count > 0)
                {
                    gvTemplateLis.DataSource = ds.Tables[6];
                    gvTemplateLis.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[6].NewRow();
                    ds.Tables[6].Rows.InsertAt(dr, 0);

                    gvTemplateLis.DataSource = ds.Tables[6];
                    gvTemplateLis.DataBind();
                }
            }
            else if (TemplateType == "TF")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvSelectedServices.DataSource = ds.Tables[0];
                    gvSelectedServices.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    //ds.Tables[0].Rows.InsertAt(dr, 0);

                    gvSelectedServices.DataSource = BindBlankTable();

                    gvSelectedServices.DataBind();
                }
            }
            else if (TemplateType == "CH" || TemplateType == "IN" || TemplateType == "POC" || TemplateType == "HIS" || TemplateType == "EXM")
            {
                txtChiefComplaints.Text = "";
                txtFreeInstruction.Text = "";
                txtPlanOfCare.Text = "";
                txtHistory.Text = "";
                txtExamination.Text = "";
                if (ds.Tables[7].Rows.Count > 0)
                {
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='CH'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtChiefComplaints.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='IN'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtFreeInstruction.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='POC'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtPlanOfCare.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='HIS'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtHistory.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='EXM'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtExamination.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    gvSelectedServices.DataSource = ds.Tables[0];
                    gvSelectedServices.DataBind();
                }
                else
                {
                    if (TemplatePlanId != 0)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        // ds.Tables[0].Rows.InsertAt(dr, 0);

                        gvSelectedServices.DataSource = BindBlankTable();
                        gvSelectedServices.DataBind();
                    }
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    gvSpecialsation.DataSource = ds.Tables[2];
                    gvSpecialsation.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[2].NewRow();
                    ds.Tables[2].Rows.InsertAt(dr, 0);
                    gvSpecialsation.DataSource = ds.Tables[2];
                    gvSpecialsation.DataBind();
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[3].NewRow();
                    ds.Tables[3].Rows.InsertAt(dr, 0);
                    gvService.DataSource = ds.Tables[3];
                    gvService.DataBind();
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[4].NewRow();
                    ds.Tables[4].Rows.InsertAt(dr, 0);

                    gvDrugClass.DataSource = ds.Tables[4];
                    gvDrugClass.DataBind();
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[5].NewRow();
                    ds.Tables[5].Rows.InsertAt(dr, 0);

                    gvPrescription.DataSource = ds.Tables[5];
                    gvPrescription.DataBind();
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    gvTemplateLis.DataSource = ds.Tables[6];
                    gvTemplateLis.DataBind();
                }
                else
                {
                    DataRow dr = ds.Tables[6].NewRow();
                    ds.Tables[6].Rows.InsertAt(dr, 0);

                    gvTemplateLis.DataSource = ds.Tables[6];
                    gvTemplateLis.DataBind();
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    txtChiefComplaints.Text = "";
                    txtFreeInstruction.Text = "";
                    txtPlanOfCare.Text = "";
                    txtHistory.Text = "";
                    txtExamination.Text = "";
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='CH'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtChiefComplaints.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='IN'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtFreeInstruction.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='POC'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtPlanOfCare.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='HIS'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtHistory.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                    dv = new DataView(ds.Tables[7]);
                    dv.RowFilter = "TemplateCode='EXM'";
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        txtExamination.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                    }
                }
            }
            
            if (ViewState["DayDetail"] != null)
            {
                GridViewdays.DataSource = (DataTable)ViewState["DayDetail"];
                GridViewdays.DataBind();
            }
            if (common.myInt(ddlPlanTemplates.SelectedValue) > 0)
            {
                BindPlanWiseDayEnteredData(common.myInt(ddlPlanTemplates.SelectedValue));
            }
        }
        catch (Exception Ex)
        {

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            client = null;
            objRoot = null;
            ds.Dispose();
            objException = null;
            dv.Dispose();
        }
    }
    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        DataView ddv = new DataView();

        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                DataView objDv = null;
                DataView objDvValue;
                DataTable objDt = null;
                HiddenField hdnFieldType = (HiddenField)e.Row.FindControl("hdnFieldType");
                HiddenField hdnSectionId = (HiddenField)e.Row.FindControl("hdnSectionId");
                HiddenField hdnFieldId = (HiddenField)e.Row.FindControl("hdnFieldId");
                HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                if (objDs != null && objDs.Tables.Count > 1 && objDs.Tables[1].Rows.Count > 0)
                {
                    objDv = objDs.Tables[1].DefaultView;
                    objDv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                }
                ddv = new DataView(dt); // To Be Check

                if (ddv.Count > 0)
                {
                    ddv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                    objDt = ddv.ToTable();
                    if (objDt.Rows.Count > 0)
                    {
                        e.Row.Visible = true;
                    }
                }
                else
                {
                    if (objDs.Tables.Count > 1)
                    {
                        if (objDs.Tables[1].Rows.Count > 0)
                        {
                            objDvValue = objDs.Tables[1].DefaultView;
                            if (objDvValue.Table.Columns["FieldId"] != null)
                            {
                                objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            }
                            objDt = objDvValue.ToTable();
                            if (objDt.Rows.Count > 0)
                            {
                                e.Row.Visible = true;
                            }
                        }
                    }
                }


                if (!common.myStr(hdnFieldType.Value).Trim().Equals(string.Empty))
                {

                    #region Single TextBox Type
                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        cCtlType = "T";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        txtT.Enabled = true;
                        txtW.Visible = false;

                        string maxLength = common.myStr(txtT.MaxLength);

                        txtT.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtT.ToolTip = txtT.Text;
                            }

                        }


                    }
                    #endregion
                    #region start for Field type I
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("I"))
                    {
                        cCtlType = "I";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");


                        txtT.Enabled = true;
                        txtW.Visible = false;
                        txtT.Visible = true;
                    }
                    #endregion

                    #region start for Field type IS
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("IS"))
                    {
                        cCtlType = "IS";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        txtT.Enabled = false;
                        txtW.Visible = false;
                        txtT.Visible = false;
                        Hy_LinkUrl.Visible = false;
                        txtM.Visible = true;

                    }
                    #endregion
                    #region Mutiple Text Type
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        cCtlType = "M";
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        txtM.Visible = true;

                        txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtM.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtM.ToolTip = txtM.Text;
                            }
                        }
                    }
                    #endregion
                    #region WordProcessor Type
                    else if (common.myStr(hdnFieldType.Value).Trim().Equals("W")) // For WordProcessor
                    {
                        cCtlType = "W";
                        DropDownList ddl = (DropDownList)e.Row.FindControl("ddlTemplateFieldFormats");
                        ddl.Visible = true;
                        ddl.DataSource = BindFieldFormats(common.myInt(hdnFieldId.Value).ToString());
                        ddl.DataTextField = "FormatName";
                        ddl.DataValueField = "FormatId";
                        ddl.DataBind();
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = true;

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtW.Content = common.myStr(objDt.Rows[0]["FieldValue"]);
                            }
                        }
                    }
                    #endregion

                    #region CheckBox Type
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        cCtlType = "C";
                        DataList list = (DataList)e.Row.FindControl("C");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        list.Visible = true;

                        list.DataSource = objDv;
                        list.DataBind();

                        //if (hdnColumnNosToDisplay.Value != null)
                        //{
                        //    list.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                            list.RepeatDirection = RepeatDirection.Horizontal;

                        //}

                        HiddenField hdnCV = (HiddenField)list.FindControl("hdnCV");
                        foreach (DataListItem item in list.Items)
                        {
                            HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                            CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                            CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                            HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                            CheckBox chk = (CheckBox)item.FindControl("C");
                            chk.Checked = false;
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    foreach (DataRow drow in objDt.Rows)
                                    {
                                        if (common.myStr(drow["FieldValue"]).Trim().Equals(common.myStr(hdn.Value).Trim()))
                                        {
                                            chk.Checked = true;
                                        }
                                    }
                                }
                            }

                        }
                    }

                    #endregion
                    #region Boolean Type
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        cCtlType = "B";
                        RadioButtonList B = (RadioButtonList)e.Row.FindControl("B");

                        B.Attributes.Add("onclick", "radioMe(event,'" + B.ClientID + "');");


                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        B.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                objDvValue = objDt.DefaultView;
                                if (objDvValue.Table.Columns["FieldId"] != null)
                                {
                                    objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                                }

                                objDt = objDvValue.ToTable();

                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(1))//Yes
                                    {
                                        B.SelectedValue = "1";
                                    }
                                    else if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(0))//No
                                    {
                                        B.SelectedValue = "0";
                                    }
                                    else
                                    {
                                        B.SelectedValue = "-1";  //Select
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    #region DropDown Type
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        cCtlType = "D";
                        DropDownList ddl = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }

                    }
                    #endregion
                    #region DropDown Image Type
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        cCtlType = "IM";
                        RadComboBox ddl = (RadComboBox)e.Row.Cells[(byte)enumNonT.Values].FindControl("IM");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;


                        foreach (DataRow drImage in objDv.ToTable().Rows)
                        {
                            RadComboBoxItem item = new RadComboBoxItem();
                            item.Text = (string)drImage["ValueName"];
                            item.Value = drImage["ValueId"].ToString();
                            item.ImageUrl = drImage["ImagePath"].ToString();
                            ddl.Items.Add(item);
                            item.DataBind();
                        }
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (common.myStr(objDt.Rows[0]["FieldValue"]) != null)
                                {
                                    ddl.SelectedValue = common.myStr(objDt.Rows[0]["FieldValue"]);
                                }
                            }
                        }

                    }
                    #endregion

                    #region RadioButton Type
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        cCtlType = "R";
                        RadioButtonList ddl = (RadioButtonList)e.Row.Cells[(byte)enumNonT.Values].FindControl("R");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //  HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        if (hdnColumnNosToDisplay.Value != null)
                        {
                            ddl.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                            ddl.RepeatDirection = RepeatDirection.Horizontal;

                        }
                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }

                    }
                    #endregion
                    #region Date Type
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))//For Date
                    {
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        AjaxControlToolkit.CalendarExtender cal = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender3");
                        txtW.Visible = false;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();
                            }
                        }
                        tblDate.Visible = true;
                    }
                    #endregion
                    #region Header Type
                    else if (common.myStr(hdnFieldType.Value).Equals("H"))//For Heading
                    {
                        cCtlType = "H";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        //btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;
                    }
                    #endregion
                    #region Static Template Type
                    else if (common.myStr(hdnFieldType.Value).Equals("L"))
                    {
                        cCtlType = "L";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");

                        LinkButton lnkStaticTemplate = (LinkButton)e.Row.FindControl("lnkStaticTemplate");

                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");

                        string sStaticTemplateId = common.myStr(lnkStaticTemplate.CommandArgument);
                        lnkStaticTemplate.Visible = true;


                        //BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                        //DataSet dsStatic = master.GetAllTypeTemplates(common.myInt(Session["HospitalLocationId"]), "S");

                        WebClient client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetAllTypeTemplates";
                        APIRootClass.GetAllTypeTemplates objRoot = new global::APIRootClass.GetAllTypeTemplates();
                        objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                        objRoot.sType = "S";

                        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                        string sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        DataSet dsStatic = JsonConvert.DeserializeObject<DataSet>(sValue);

                        DataView dvStatic = new DataView(dsStatic.Tables[0]);
                        dvStatic.RowFilter = "DisplayInTemplate=1 AND SectionId=" + sStaticTemplateId;
                        DataTable dtStatic = dvStatic.ToTable();
                        if (dtStatic.Rows.Count > 0)
                        {
                            HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                            HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                            //if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            //{
                            //    lnkStaticTemplate.Attributes.Add("onclick", "openStaticTemplateWindow('" + common.myStr(dtStatic.Rows[0]["PageUrl"]) + "',' " + sStaticTemplateId + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim()) + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.SectionId].Text).Trim()) + "')");
                            lnkStaticTemplate.Enabled = true;
                            //}
                            //else
                            //{
                            //    lnkStaticTemplate.Enabled = false;
                            //}

                            lnkStaticTemplate.Text = common.myStr(dtStatic.Rows[0]["SectionName"]);
                        }
                        // btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;



                    }
                    #endregion
                    #region Patient Data Object
                    else if (common.myStr(hdnFieldType.Value).Equals("O"))//Patient Data Object
                    {
                        cCtlType = "O";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        // btnAdd.Visible = false;
                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;

                        int DataObjectId = common.myInt(e.Row.Cells[(byte)enumNonT.DataObjectId].Text);

                        //clsIVF objivf = new clsIVF(sConString);
                        clsIVF objivf = new clsIVF(string.Empty);

                        string strOutput = objivf.getDataObjectValue(DataObjectId);

                        if (strOutput.Length > 50)
                        {
                            txtM.Visible = true;
                            txtM.Text = strOutput;
                            txtM.ToolTip = strOutput;
                            txtM.Enabled = false;
                        }
                        else
                        {
                            txtT.Visible = true;
                            txtT.Text = strOutput;
                            txtT.ToolTip = strOutput;
                            txtT.Enabled = false;
                            txtT.Columns = strOutput.Length + 2;

                        }

                    }
                    #endregion
                }
                else
                {
                    e.Row.Cells[(byte)enumNonT.Values].Text = "No Record Found!";
                }
                if (cCtlType.Equals("T") || cCtlType.Equals("M"))
                {
                    RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                    txtW.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ddv.Dispose();
            objException = null;
        }
    }
    protected void ddlDepartment_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        BaseC.clsLabRequest objLabRequest = new BaseC.clsLabRequest(sConString);
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (ddlDepartment.SelectedValue != "-1")
            {
                ddlSubDepartment.Items.Clear();
                ddlSubDepartment.Text = "";
                ds = objLabRequest.GetDepartmentSubMaster(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlSubDepartment.DataSource = ds.Tables[0];
                        ddlSubDepartment.DataTextField = "SubName";
                        ddlSubDepartment.DataValueField = "SubDeptId";
                        ddlSubDepartment.DataBind();
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
            ds.Dispose();
            bMstr = null;
            objLabRequest = null;
            objException = null;
        }
    }
    protected void ddlTemplateFieldFormats_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.GetFormatText objRoot = new global::APIRootClass.GetFormatText();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            RadEditor txtW = (RadEditor)row.FindControl("txtW");

           
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetFormatText";
           
            objRoot.FormatId = common.myInt(ddl.SelectedValue);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtW.Content = common.myStr(ds.Tables[0].Rows[0]["FormatText"]);
            }
            else
            {
                txtW.Content = String.Empty;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            client =null;
            objRoot = null;
            objException = null;
        }
    }
    public DataTable BindFieldFormats(String strFieldId)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.GetTemplateFieldFormats objRoot = new global::APIRootClass.GetTemplateFieldFormats();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
          
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetTemplateFieldFormats";
           
            objRoot.FieldId = common.myInt(strFieldId);
            objRoot.SpecialisationId = common.myInt(ViewState["DoctorSpecialisationId"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr;
                dr = dt.NewRow();
                dr["FormatId"] = 0;
                dr["FormatName"] = "--Options--";
                dt.Rows.InsertAt(dr, 0);
            }
            ds.Dispose();
            client = null;
            objRoot = null;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dt;
    }

    protected void bindEMRGetTreatmentPlanList()
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {

           
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanList";
            
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlPlanTemplates.DataSource = ds.Tables[0];
                    ddlPlanTemplates.DataTextField = "PlanName";
                    ddlPlanTemplates.DataValueField = "PlanId";
                    ddlPlanTemplates.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
             client = null;
            objRoot = null;
            objException = null;
        }
    }

    private void bindtemplate()
    {
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            bool IsSuperUserLogin = false;

            if (common.myBool(Session["isEMRSuperUser"]).Equals(true))
            {
                IsSuperUserLogin = true;
            }
            int iDoctorSpecialisation = common.myInt(ViewState["DoctorSpecialisationId"]);
            string EmpType = common.myStr(ViewState["EmployeeType"]);
            ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), common.myStr(Session["EmployeeType"]), 0, 0, "", 0, "DR", common.myInt(Session["FacilityId"]),
                false, 0, 0, 0, IsSuperUserLogin, 0, common.myInt(Session["LoginDoctorId"]));

            ddlTemplateList.DataSource = ds.Tables[0];
            ddlTemplateList.DataTextField = "TemplateName";
            ddlTemplateList.DataValueField = "TemplateId";
            ddlTemplateList.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            objivf = null;
            ds.Dispose();
            objException = null;
        }
    }
    private void BindSpeciliazation()
    {
        DataTable dtSpeciliazation = new DataTable();
        BaseC.EMR EMR = new BaseC.EMR(sConString);
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            dtSpeciliazation = EMR.GetSpeciliazationMaster(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["facilityId"]));
            if (dtSpeciliazation.Rows.Count > 0)
            {
                ddlSpecilizationNew.Items.Clear();
                ddlSpecilizationNew.DataSource = dtSpeciliazation;
                ddlSpecilizationNew.DataTextField = "NAME";
                ddlSpecilizationNew.DataValueField = "id";
                ddlSpecilizationNew.DataBind();
                ddlSpecilizationNew.Items.Insert(0, new RadComboBoxItem("All", "0"));

            }
            else
            {

                Alert.ShowAjaxMsg("Specialization not available", Page);
                return;
            }

        }
        catch (Exception Ex)
        {
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dtSpeciliazation.Dispose();
            EMR = null;
            objException = null;
        }
    }
    private void BindPlanWiseDayEnteredData(int PlanId)
    {
        DataSet ds = new DataSet();
        string LastChangedDate = string.Empty;
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
           
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPlanWiseDayEnteredData";
           
            objRoot.PlanId = PlanId;
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            ddlEnteredDayData.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlEnteredDayData.Visible = true;
                btnCopy.Visible = true;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = (string)ds.Tables[0].Rows[i]["DurationName"];
                    item.Value = common.myStr(ds.Tables[0].Rows[i]["RowNo"]);
                    item.Attributes["DayId"] = ds.Tables[0].Rows[i]["DayId"].ToString();
                    item.Attributes["PlanId"] = ds.Tables[0].Rows[i]["PlanId"].ToString();
                    item.Attributes["DayDetailId"] = common.myStr(ds.Tables[0].Rows[i]["DayDetailId"]);
                    this.ddlEnteredDayData.Items.Add(item);
                    item.DataBind();
                }
                ViewState["EnteredData"] = ds.Tables[0];
                LastChangedDate = ds.Tables[0].Compute("MAX(LastChangedDate)", "LastChangedDate IS NOT NULL").ToString();
                lblPlanMessage.Text = "<b> Created on : " + common.myStr(ds.Tables[0].Rows[0]["CreatedDate"]) + "&nbsp;&nbsp;&nbsp;&nbsp;Last Changed Date :" + LastChangedDate + "</b>";
            }
            else
            {
                BlankGrid();
            }
            
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            LastChangedDate = string.Empty;
            client = null;
            objRoot = null;
            objException = null;
        }
    }


    protected void ddlPlanTemplates_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataTable dtNew = new DataTable();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            ViewState["EnteredData"] = null;
            ViewState["DayDetail"] = null;

            ViewState["rowIndex"] = null;
            ViewState["DayId"] = null;
            ViewState["DayDetailId"] = null;
            ViewState["SelectedRow"] = null;
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Template Name.", Page.Page);
                return;
            }

           
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetPlanTypeDurationDetails";
          
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.PlanId = common.myInt(ddlPlanTemplates.SelectedValue);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "PlanId=" + common.myInt(ddlPlanTemplates.SelectedValue);

            dtNew = dv.ToTable().Clone();

            for (int j = 0; j < dv.ToTable().Rows.Count; j++)
            {
                int iCount = 1;
                if (common.myStr(dv.ToTable().Rows[j]["Code"]) == "AD")
                {
                    for (int i = 0; i < common.myInt(dv.ToTable().Rows[j]["Days"]); i++)
                    {
                        DataRow dr = dtNew.NewRow();
                        dr["Id"] = iCount;
                        dr["DayId"] = dv.ToTable().Rows[j]["DayId"];
                        dr["PlanId"] = dv.ToTable().Rows[j]["PlanId"];
                        dr["Code"] = dv.ToTable().Rows[j]["Code"];
                        dr["Days"] = dv.ToTable().Rows[j]["Days"];
                        dr["DurationName"] = dv.ToTable().Rows[j]["DurationName"] + (i == 0 ? "" : " + " + common.myStr(i));
                        dtNew.Rows.Add(dr);
                        iCount++;
                    }
                }
                if (common.myStr(dv.ToTable().Rows[j]["Code"]) == "PO")
                {
                    for (int i = 0; i < common.myInt(dv.ToTable().Rows[j]["Days"]); i++)
                    {
                        DataRow dr = dtNew.NewRow();
                        dr["Id"] = iCount;
                        dr["DayId"] = dv.ToTable().Rows[j]["DayId"];
                        dr["PlanId"] = dv.ToTable().Rows[j]["PlanId"];
                        dr["Code"] = dv.ToTable().Rows[j]["Code"];
                        dr["Days"] = dv.ToTable().Rows[j]["Days"];
                        dr["DurationName"] = dv.ToTable().Rows[j]["DurationName"] + (i == 0 ? "" : " + " + common.myStr(i));
                        dtNew.Rows.Add(dr);
                        iCount++;
                    }
                }
                if (common.myStr(dv.ToTable().Rows[j]["Code"]) == "DC")
                {
                    DataRow dr = dtNew.NewRow();
                    dr["Id"] = iCount;
                    dr["DayId"] = dv.ToTable().Rows[j]["DayId"];
                    dr["PlanId"] = dv.ToTable().Rows[j]["PlanId"];
                    dr["Code"] = dv.ToTable().Rows[j]["Code"];
                    dr["Days"] = dv.ToTable().Rows[j]["Days"];
                    dr["DurationName"] = dv.ToTable().Rows[j]["DurationName"];
                    dtNew.Rows.Add(dr);
                    iCount++;
                }
                if (common.myStr(dv.ToTable().Rows[j]["Code"]) == "OP")
                {
                    DataRow dr = dtNew.NewRow();
                    dr["Id"] = iCount;
                    dr["DayId"] = dv.ToTable().Rows[j]["DayId"];
                    dr["PlanId"] = dv.ToTable().Rows[j]["PlanId"];
                    dr["Code"] = dv.ToTable().Rows[j]["Code"];
                    dr["Days"] = dv.ToTable().Rows[j]["Days"];
                    dr["DurationName"] = dv.ToTable().Rows[j]["DurationName"];
                    dtNew.Rows.Add(dr);
                    iCount++;
                }
            }
            dtNew.AcceptChanges();



            ddlEnteredDayData.Visible = false;
            btnCopy.Visible = false;

            BindPlanWiseDayEnteredData(common.myInt(ddlPlanTemplates.SelectedValue));

            ViewState["DayDetail"] = dtNew;
            GridViewdays.DataSource = dtNew;
            GridViewdays.DataBind();
            GetServiceData(common.myInt(ddlPlanTemplates.SelectedValue), 0, 0, "");
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
            dt.Dispose();
            dtNew.Dispose();
            client = null;
            objRoot = null;
            objException = null;
        }
    }
    private void BlankGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PlanId");
        dt.Columns.Add("DayId");
        dt.Columns.Add("DayDetailId");
        dt.Columns.Add("DurationName");
        ViewState["EnteredData"] = dt;
    }
    public void bind_ddlBrandPrescriptionsControl()
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanItemMasterList";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);



            foreach (DataRow DR in ds.Tables[0].Rows)
            {
                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                item.Text = common.myStr(DR["UnitName"]);
                item.Value = common.myStr(common.myInt(DR["Id"]));
                item.Attributes.Add("IsUnitCalculationRequired", common.myStr(DR["IsUnitCalculationRequired"]));
                ddlUnit.Items.Add(item);
            }


          
            ddlRoute.DataSource = ds.Tables[1];
            ddlRoute.DataValueField = "Id";
            ddlRoute.DataTextField = "RouteName";
            ddlRoute.DataBind();
            ddlRoute.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
           
            ddlRoute.SelectedIndex = 0;


            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(dr["Description"]);
                item.Value = common.myStr(common.myInt(dr["Id"]));
                item.Attributes.Add("Frequency", common.myStr(dr["Frequency"]));
                item.DataBind();
                ddlFrequencyId.Items.Add(item);
            }

            ddlFrequencyId.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlFrequencyId.SelectedIndex = 0;



            ddlFoodRelation.DataSource = ds.Tables[3];
            ddlFoodRelation.DataValueField = "Id";
            ddlFoodRelation.DataTextField = "FoodName";
            ddlFoodRelation.DataBind();
            ddlFoodRelation.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
            ddlFoodRelation.SelectedIndex = 0;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            client = null;
            objRoot = null;
            objException = null;
        }
    }

    protected void ddlBrandPrescriptions_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        int GenericId = 0;
        DataTable data = new DataTable();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {

            if (common.myInt(ddlGeneric.SelectedValue) > 0)
            {
                GenericId = common.myInt(ddlGeneric.SelectedValue);
            }

            data = GetBrandData_Prescription(e.Text, GenericId);
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset.Equals(data.Rows.Count);
            for (int i = itemOffset; i < endOffset; i++)
            {
                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ItemName"];
                item.Value = common.myStr(data.Rows[i]["ItemId"]);

                item.Attributes.Add("ClosingBalance", "0");
                item.Attributes.Add("CIMSItemId", "");
                item.Attributes.Add("CIMSType", "");
                item.Attributes.Add("VIDALItemId", "0");
                item.Attributes.Add("GenericId", common.myStr(data.Rows[i]["GenericId"]));
                this.ddlBrandPrescriptions.Items.Add(item);
                item.DataBind();
            }

            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            data.Dispose();
            objException = null;
        }
    }
    private DataTable GetBrandData_Prescription(string text, int GenericId)
    {
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);

        int StoreId = common.myInt(Session["StoreId"]); //common.myInt(Session["StoreId"]);
        int ItemId = 0;

        int itemBrandId = 0;
        int WithStockOnly = 0;

        int iOT = 3;

        if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString() == "OT"
            && Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString() == "CO")
        {
            iOT = 2;
        }
        else if (Request.QueryString["LOCATION"] != null && Request.QueryString["LOCATION"].ToString() == "WARD"
            && Request.QueryString["DRUGORDERCODE"] != null && Request.QueryString["DRUGORDERCODE"].ToString() == "CO")
        {
            iOT = 2;
        }
        else
        {
            iOT = 1;
        }

        if (common.myDbl(ViewState["QtyBal"]) > 0
               && common.myInt(Request.QueryString["ItemId"]) > 0)
        {
            ItemId = common.myInt(ViewState["ItemId"]);
        }

        return objPharmacy.getItemsWithStock(common.myInt(Session["HospitalLocationID"]), StoreId, ItemId == 0 && ViewState["ItemId"] != null ? Convert.ToInt32(ViewState["ItemId"]) : ItemId, itemBrandId, GenericId,
            common.myInt(Session["UserId"]), common.myInt(Session["FacilityId"]), 0,
            text.Replace("'", "''"), WithStockOnly, string.Empty, iOT).Tables[0];
    }

    protected void btnAddSevice_Onclick(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
                return;
            }
            if (common.myInt(cmbServiceName.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Service.", Page.Page);
                return;
            }
            if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }


            SaveEMRTreatmentTemplate("SR", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, common.myInt(cmbServiceName.SelectedValue), 0, 0, 0, 0, 0, "", 0, 0, 0, "", 
                0, "",0, "", "", "", "", "");
           
           
            cmbServiceName.Text = "";
            ddlDepartment.Text = "";
            ddlSubDepartment.Text = "";
            cmbServiceName.SelectedIndex = -1;
            ddlDepartment.SelectedIndex = -1;
            ddlSubDepartment.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


        finally
        {
            objException = null;
        }
    }
    private void SaveEMRTreatmentTemplate(string TemplateType, int PlanId, int DayId, int DayDetailId, int SpecialityId, int ServiceId, int DrugClassId, int GenericId,
            int ItemId, int FrequencyId, int Days, string DaysType, decimal Dose, int DoseUnitId, int FoodRelationId, string Instruction, int TemplateTaggingId,
            string xmlTemplateFieldDetails, int RouteId, string ChiefComplaints, string History, string Examination, 
            string PlanOfCare, string FreeInstruction)
    {
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        WebClient client = new WebClient();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveEMRTreatmentTemplate";

            objRoot.TemplateType = TemplateType;
            objRoot.PlanId = PlanId;
            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;

            objRoot.SpecialityId = SpecialityId;
            objRoot.ServiceId = ServiceId;

            objRoot.DrugClassId = DrugClassId;
            objRoot.GenericId = GenericId;

            objRoot.ItemId = ItemId;
            objRoot.FrequencyId = FrequencyId;

            objRoot.Days = Days;
            objRoot.DaysType = DaysType;
            objRoot.Dose = Dose;
            objRoot.DoseUnitId = DoseUnitId;
            objRoot.FoodRelationId = FoodRelationId;
            objRoot.Instruction = Instruction;
            objRoot.TemplateTaggingId = TemplateTaggingId;
            objRoot.RouteId = RouteId;


            objRoot.ChiefComplaints = txtChiefComplaints.Text;
            objRoot.History = txtHistory.Text;
            objRoot.Examination = txtExamination.Text;
            objRoot.PlanOfCare = txtPlanOfCare.Text;
            objRoot.Instruction = txtFreeInstruction.Text;

            double totalQty = 0;
            if (TemplateType == "DR")
            {
                double dose = 1;
                double frequency = common.myInt(ddlFrequencyId.SelectedValue).Equals(0) ? 0 : common.myDbl(ddlFrequencyId.SelectedItem.Attributes["Frequency"]);
                double days = common.myDbl(txtDuration.Text);


                switch (common.myStr(ddlPeriodType.SelectedValue))
                {
                    case "H":
                        days = days * 1;
                        break;
                    case "D":
                        days = days * 1;
                        break;
                    case "W":
                        days = days * 7;
                        break;
                    case "M":
                        days = days * 30;
                        break;
                    case "Y":
                        days = days * 365;
                        break;
                    default:
                        days = days * 1;
                        break;
                }
                if (common.myBool(ViewState["ISCalculationRequired"]))
                {
                    if (common.myInt(ddlUnit.SelectedValue) > 0)
                    {
                        bool IsUnitCalculationRequired = common.myBool(ddlUnit.SelectedItem.Attributes["IsUnitCalculationRequired"]);
                        if (IsUnitCalculationRequired)
                        {
                            dose = common.myDbl(txtDose.Text);
                        }
                    }
                    totalQty = frequency * days * dose;
                }
                else
                {
                    totalQty = 1;
                }
            }
            objRoot.RouteId = RouteId;
            objRoot.totalQty = totalQty;

            objRoot.xmlTemplateFieldDetails = xmlTemplateFieldDetails.ToString();

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            GetServiceData(common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]), common.myInt(ViewState["DayDetailId"]), TemplateType);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objRoot = null;
            client = null;
            objException = null;
        }
    }
    protected void btnAddDrugClass_Onclick(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Plan", Page);
                return;
            }
            if (common.myInt(ddlDrugClass.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select drug class.", Page);
                return;
            }
            if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }

            SaveEMRTreatmentTemplate("DC", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, common.myInt(ddlDrugClass.SelectedValue), 0, 0, 0, 0, "", 0, 0, 0, "", 0, "",0
                , "", "", "", "", "");
           

            btnAddPrescription.Enabled = true;
            ddlDrugClass.SelectedIndex = -1;
            ddlDrugClass.Text = "";

        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }
    protected void btnAddPrescription_Onclick(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Treatment Template Name.", Page.Page);
                return;
            }
            if (common.myInt(ddlBrandPrescriptions.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Brand Name.", Page.Page);
                return;
            }
            if (common.myInt(ddlFrequencyId.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Frequency.", Page.Page);
                return;
            }
            if (common.myStr(txtDuration.Text.Trim()) == string.Empty)
            {
                Alert.ShowAjaxMsg("Please! Enter Duration.", Page.Page);
                return;
            }
            if (common.myInt(ddlRoute.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! select route.", Page.Page);
                return;
            }
            if (common.myStr(txtDose.Text.Trim()) != string.Empty)
            {
                if (common.myStr(ddlUnit.SelectedValue) == string.Empty)
                {
                    Alert.ShowAjaxMsg("Please! Select Dose Unit.", Page.Page);
                    return;
                }
            }

            if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }

            SaveEMRTreatmentTemplate("DR", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
            common.myInt(ViewState["DayDetailId"]), 0, 0, 0, common.myInt(ddlGeneric.SelectedValue), common.myInt(ddlBrandPrescriptions.SelectedValue),
            common.myInt(ddlFrequencyId.SelectedValue), common.myInt(txtDuration.Text), common.myStr(ddlPeriodType.SelectedValue), common.myDec(txtDose.Text),
            common.myInt(ddlUnit.SelectedValue), common.myInt(ddlFoodRelation.SelectedValue), common.myStr(txtInstructions.Text), 0, "",common.myInt(ddlRoute.SelectedValue)
            , "", "", "", "", "");
           

            ddlBrandPrescriptions.ClearSelection();
            ddlBrandPrescriptions.Text = string.Empty;

            ddlFrequencyId.SelectedValue = "0";
            ddlPeriodType.SelectedValue = "D";

            txtDuration.Text = string.Empty;
            txtDose.Text = string.Empty;
            ddlUnit.SelectedValue = string.Empty;
            ddlUnit.ClearSelection();
            ddlUnit.Text = string.Empty;
            ddlFoodRelation.SelectedValue = "0";
            txtInstructions.Text = string.Empty;

            
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }
    protected void gvService_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (e.CommandName == "ItemDelete")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                HiddenField hdnPlanId = (HiddenField)row.FindControl("hdnPlanId");
                HiddenField hdnDayid = (HiddenField)row.FindControl("hdnDayid");
                HiddenField hdnDayDetailId = (HiddenField)row.FindControl("hdnDayDetailId");

                EMRDeleteTreatmentPlanDetail("SR", common.myInt(hdnId.Value), common.myInt(hdnPlanId.Value),
                    common.myInt(hdnDayid.Value), common.myInt(hdnDayDetailId.Value), common.myInt(Session["UserId"]));

            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }

    protected void gvDrugClass_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (e.CommandName == "ItemDelete")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                HiddenField hdnPlanId = (HiddenField)row.FindControl("hdnPlanId");
                HiddenField hdnDayid = (HiddenField)row.FindControl("hdnDayid");
                HiddenField hdnDayDetailId = (HiddenField)row.FindControl("hdnDayDetailId");

               EMRDeleteTreatmentPlanDetail("DC", common.myInt(hdnId.Value), common.myInt(hdnPlanId.Value),
                    common.myInt(hdnDayid.Value), common.myInt(hdnDayDetailId.Value), common.myInt(Session["UserId"]));

            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }
    protected void BindDepartment()
    {
        DataSet ds = new DataSet();
        BaseC.EMRMasters bMstr = new BaseC.EMRMasters(sConString);
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            ddlDepartment.Items.Clear();
            string strDepartmentType = "";

            strDepartmentType = "'I','IS'";

            ds = bMstr.GetHospitalDepartment(common.myInt(Session["HospitalLocationID"]), strDepartmentType);
            if (ds.Tables.Count > 0)
            {
                ddlDepartment.DataSource = ds.Tables[0];
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "DepartmentID";
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
            }
            ddlDepartment.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            bMstr = null;
            objException = null;
        }
    }



    protected void cmbServiceName_OnItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        if (e.Text.Length > 2)
        {

            DataTable data = BindSearchCombo("%" + e.Text);

            // BindCategoryTree();
            int itemOffset = e.NumberOfItems;
            if (itemOffset == 0)
            {
                this.cmbServiceName.Items.Clear();
            }
            int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
            e.EndOfItems = endOffset == data.Rows.Count;

            for (int i = itemOffset; i < endOffset; i++)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)data.Rows[i]["ServiceName"];
                item.Value = data.Rows[i]["ServiceID"].ToString();
                item.Attributes["CPTCode"] = data.Rows[i]["CPTCode"].ToString();
                item.Attributes["LongDescription"] = data.Rows[i]["LongDescription"].ToString();
                item.Attributes["ServiceType"] = data.Rows[i]["ServiceType"].ToString();
                this.cmbServiceName.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            data.Dispose();
        }
    }


    protected DataTable BindSearchCombo(String etext)
    {
        BaseC.EMROrders order = new BaseC.EMROrders(sConString);

        return dt = order.GetSearchServices(common.myInt(Session["HospitalLocationId"]), common.myInt(ddlDepartment.SelectedValue), common.myStr(ddlSubDepartment.SelectedValue)
        , common.myStr(etext, true), common.myInt(Session["FacilityId"]),0,0);

    }
    private static string GetStatusMessage(int offset, int total)
    {
        if (total <= 0)
            return "No matches";

        return String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", offset, total);
    }




    protected void btnAddPlaneName_OnClick(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "/EMR/ClinicalPathway/PopUp/AddTreatmentPlan.aspx?Source=TreatmentMasters";
        RadWindow1.Height = 1000;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "btnAddPlaneNameClose";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Close;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }



    protected void GridViewdays_RowCommand(object sender, DataListCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (e.CommandName == "DurationName")
            {
                DataListItem row = (DataListItem)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnDurationId = (HiddenField)row.FindControl("hdnDurationId");
                LinkButton lnkday = (LinkButton)row.FindControl("lnkday");
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");
                if (ViewState["rowIndex"] != null)
                {
                    DataListItem gvRow = GridViewdays.Items[common.myInt(ViewState["rowIndex"])];
                    LinkButton lnkday_p = (LinkButton)gvRow.FindControl("lnkday");
                    lnkday_p.BackColor = System.Drawing.Color.Empty;
                }
                ViewState["rowIndex"] = row.ItemIndex;
                ViewState["DayId"] = hdnDurationId.Value;
                ViewState["DayDetailId"] = hdnId.Value;
                ViewState["SelectedRow"] = row.ItemIndex;
                ViewState["DayDetailName"] = lnkday.Text;

                if (lnkday.Text != "All")
                {
                    GetServiceData(common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]), common.myInt(ViewState["DayDetailId"]), "");
                }
                GridViewdays.DataSource = (DataTable)ViewState["DayDetail"];
                GridViewdays.DataBind();



            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }



    protected void ddlGeneric_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable dt = new DataTable();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            Telerik.Web.UI.RadComboBox ddl = sender as Telerik.Web.UI.RadComboBox;
            dt = GetGenericData(common.myStr(e.Text));
            int itemOffset = e.NumberOfItems;
            int endOffset = Math.Min(itemOffset + 50, dt.Rows.Count);
            e.EndOfItems = endOffset.Equals(dt.Rows.Count);
            for (int i = itemOffset; i < endOffset; i++)
            {
                Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                item.Text = common.myStr(dt.Rows[i]["GenericName"]);
                item.Value = common.myStr(dt.Rows[i]["GenericId"]);
                item.Attributes.Add("CIMSItemId", common.myStr(dt.Rows[i]["CIMSItemId"]));
                item.Attributes.Add("CIMSType", common.myStr(dt.Rows[i]["CIMSType"]));
                item.Attributes.Add("VIDALItemId", common.myInt(dt.Rows[i]["VIDALItemId"]).ToString());
                ddl.Items.Add(item);
                item.DataBind();
            }
            e.Message = GetStatusMessage(endOffset, dt.Rows.Count);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            dt.Dispose();
            objException = null;
        }
    }
    private DataTable GetGenericData(string text)
    {
        BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
        return objPhr.GetGenericDetails(0, common.myStr(text), 1, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["UserId"])).Tables[0];
    }

    protected void ddlGeneric_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["GenericId"] = ddlGeneric.SelectedValue;
        btnAddDrugClass.Enabled = false;
    }



    protected void btnAddTemplateTagging_Click(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
                return;
            }
            if (common.myInt(ddlTemplateList.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Template.", Page.Page);
                return;
            }
            if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }

            SaveEMRTreatmentTemplate("TM", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", common.myInt(ddlTemplateList.SelectedValue), "",0
                , "", "", "", "", "");
            
            ddlTemplateList.SelectedIndex = -1;
            ddlTemplateList.Text = "";

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ScrollTo", "var needScroll = true;", true);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            objException = null;
        }
    }

    protected void gvTemplateLis_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (e.CommandName == "ItemDelete")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                HiddenField hdnPlanId = (HiddenField)row.FindControl("hdnPlanId");
                HiddenField hdnDayid = (HiddenField)row.FindControl("hdnDayid");
                HiddenField hdnDayDetailId = (HiddenField)row.FindControl("hdnDayDetailId");

                EMRDeleteTreatmentPlanDetail("TM", common.myInt(hdnId.Value), common.myInt(hdnPlanId.Value),
                    common.myInt(hdnDayid.Value), common.myInt(hdnDayDetailId.Value), common.myInt(Session["UserId"]));
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }
    private void  EMRDeleteTreatmentPlanDetail(string TemplateType, int id, int PlanId, int DayId, int DayDetailId, int EncodedBy)
    {
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRDeleteTreatmentPlanDetail";

            objRoot.TemplateType = TemplateType;
            objRoot.Id = id;
            objRoot.PlanId = PlanId;

            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;
            objRoot.EncodedBy = EncodedBy;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            GetServiceData(PlanId, DayId, DayDetailId, TemplateType);
        }
        catch( Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            client = null;
            objRoot = null;
            objException = null;
        }
    }
       

    protected void btnAddSpecialsation_Click(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
                return;
            }
            if (common.myInt(ddlSpecilizationNew.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Speciality.", Page.Page);
                return;
            }
            if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }

            SaveEMRTreatmentTemplate("SP", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), common.myInt(ddlSpecilizationNew.SelectedValue), 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", 0, "",0
                , "", "", "", "", "");
            

            ddlSpecilizationNew.SelectedIndex = -1;
            ddlSpecilizationNew.Text = "";
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            objException = null;  
        }
    }

    private DataTable BindDrugClass(string search)
    {
        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetEMRDrugClassMaster";
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
        objRoot.FacilityId = common.myInt(Session["FacilityId"]);
        objRoot.DrugName = search;
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        client = null;
        objRoot = null;
        return JsonConvert.DeserializeObject<DataSet>(sValue).Tables[0];
    }
    protected void ddlDrugClass_OnItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        DataTable data = new DataTable();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (e.Text.Length > 1)
            {

                data = BindDrugClass(common.myStr(e.Text));
                int itemOffset = e.NumberOfItems;
                int endOffset = Math.Min(itemOffset + 50, data.Rows.Count);
                e.EndOfItems = endOffset.Equals(data.Rows.Count);
                for (int i = itemOffset; i < endOffset; i++)
                {
                    Telerik.Web.UI.RadComboBoxItem item = new Telerik.Web.UI.RadComboBoxItem();
                    item.Text = (string)data.Rows[i]["DrugClassName"];
                    item.Value = common.myStr(data.Rows[i]["Id"]);

                    this.ddlDrugClass.Items.Add(item);
                    item.DataBind();
                }
                e.Message = GetStatusMessage(endOffset, data.Rows.Count);
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            data.Dispose();
            objException = null;
        }
    }




    protected void gvPrescription_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (e.CommandName == "ItemDelete")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                HiddenField hdnPlanId = (HiddenField)row.FindControl("hdnPlanId");
                HiddenField hdnDayid = (HiddenField)row.FindControl("hdnDayid");
                HiddenField hdnDayDetailId = (HiddenField)row.FindControl("hdnDayDetailId");

                EMRDeleteTreatmentPlanDetail("DR", common.myInt(hdnId.Value), common.myInt(hdnPlanId.Value),
                    common.myInt(hdnDayid.Value), common.myInt(hdnDayDetailId.Value), common.myInt(Session["UserId"]));

            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }

    protected void btnCopy_Click(object sender, EventArgs e)
    {
        if (common.myInt(ddlEnteredDayData.SelectedValue) == 0)
        {
            lblMessage.Text = "please select day for copy";
            if (ViewState["DayDetail"] != null)
            {
                GridViewdays.DataSource = (DataTable)ViewState["DayDetail"];
                GridViewdays.DataBind();
            }
            ddlEnteredDayData.SelectedIndex = -1;
            ddlEnteredDayData.SelectedValue = "0";
            ddlEnteredDayData.Text = "";
            return;
        }
        if (common.myInt(ViewState["DayDetailId"]) == 0)
        {
            lblMessage.Text = "please select day in grid";
            if (ViewState["DayDetail"] != null)
            {
                GridViewdays.DataSource = (DataTable)ViewState["DayDetail"];
                GridViewdays.DataBind();
            }
            ddlEnteredDayData.SelectedIndex = -1;
            ddlEnteredDayData.SelectedValue = "0";
            ddlEnteredDayData.Text = "";
            return;
        }

        if (common.myInt(ddlEnteredDayData.SelectedItem.Attributes["DayId"]) == common.myInt(ViewState["DayId"]) &&
            common.myInt(ddlEnteredDayData.SelectedValue) == common.myInt(ViewState["DayDetailId"]))
        {
            lblMessage.Text = "Same Day not allow to copy";
            if (ViewState["DayDetail"] != null)
            {
                GridViewdays.DataSource = (DataTable)ViewState["DayDetail"];
                GridViewdays.DataBind();
            }
            ddlEnteredDayData.SelectedIndex = -1;
            ddlEnteredDayData.SelectedValue = "0";
            ddlEnteredDayData.Text = "";
            return;
        }

        RadWindow1.NavigateUrl = "/EMR/ClinicalPathway/PopUp/CopyEMRTreatmentPlan.aspx?Source=POPUP"
            + "&PlanId=" + common.myInt(ddlPlanTemplates.SelectedValue) + "&PlanName=" + common.myStr(ddlPlanTemplates.SelectedItem.Text)
            + "&CopyDayDetailId=" + common.myInt(ddlEnteredDayData.SelectedItem.Attributes["DayDetailId"]) + "&CopyDayId=" + common.myInt(ddlEnteredDayData.SelectedItem.Attributes["DayId"]) + "&CopyDayDetailName=" + common.myStr(ddlEnteredDayData.SelectedItem.Text)
            + "&SelectedDayId=" + common.myInt(ViewState["DayId"]) + "&SelectedDayDetailId=" + common.myInt(ViewState["DayDetailId"]) + "&SelectedDayDetailName=" + common.myStr(ViewState["DayDetailName"]);
        RadWindow1.Height = 1000;
        RadWindow1.Width = 1000;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "OnClientCloseCopy";
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.Behaviors = WindowBehaviors.Close;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;

    }

    protected void ddlDrugClass_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (common.myInt(ddlDrugClass.SelectedValue) > 0)
        {
            btnAddPrescription.Enabled = false;
        }
    }

    protected void ddlBrandPrescriptions_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (common.myInt(ddlBrandPrescriptions.SelectedValue) > 0)
        {
            btnAddDrugClass.Enabled = false;
        }
    }

    protected void btnAddTemplateField_Click(object sender, EventArgs e)
    {
        StringBuilder strNonTabular = new StringBuilder();
        ArrayList coll = new ArrayList();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
                return;
            }

            if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }

            foreach (GridViewRow item2 in gvSelectedServices.Rows)
            {
                if (item2.RowType.Equals(DataControlRowType.DataRow))
                {
                    HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
                    HiddenField hdnFieldId = (HiddenField)item2.FindControl("hdnFieldId");
                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(txtT.Text);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("I"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(txtT.Text);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("IS"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(txtM.Text);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(txtM.Text);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("W")) // For Word Processor
                    {
                        RadEditor txtW = (RadEditor)item2.FindControl("txtW");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(txtW.Content);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        DropDownList ddl = (DropDownList)item2.FindControl("D");

                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(ddl.SelectedValue);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        RadComboBox ddl = (RadComboBox)item2.FindControl("IM");

                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(ddl.SelectedValue);
                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add(ddl.SelectedValue);
                        strNonTabular.Append(common.setXmlTable(ref coll));

                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
                        if (ddlB.SelectedItem != null)
                        {
                            if (!ddlB.SelectedItem.Text.Equals("Select"))
                            {
                                coll.Add(common.myInt(hdnFieldId.Value));
                                coll.Add((common.myInt(ddlB.SelectedValue).Equals(1)) ? 1 : 0);
                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                            else
                            {
                                coll.Add(common.myInt(hdnFieldId.Value));
                                coll.Add(null);
                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        DataList rptC = (DataList)item2.FindControl("C");
                        string sCheckedValues = string.Empty;
                        foreach (DataListItem rptItem in rptC.Items)
                        {
                            CheckBox chk = (CheckBox)rptItem.FindControl("C");
                            HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
                            sCheckedValues = chk.Checked == true ? hdn.Value : "0";
                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add(sCheckedValues);
                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                        sCheckedValues = string.Empty;
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))
                    {
                        RadDatePicker txtDate = (RadDatePicker)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");
                        if (txtDate.SelectedDate != null)
                        {
                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add(common.myDate(txtDate.SelectedDate).ToString("dd/MM/yyyyy"));// + " " + common.myStr(time));
                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("ST"))
                    {
                        RadDatePicker txtDate = (RadDatePicker)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");
                        if (tpTime.SelectedDate != null)
                        {
                            coll.Add(common.myInt(hdnFieldId.Value));
                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");
                            coll.Add(common.myStr("01/01/1900").Trim() + " " + common.myStr(time));
                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("SB"))
                    {
                        RadDatePicker txtDate = (RadDatePicker)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");
                        if (txtDate.SelectedDate != null)
                        {
                            coll.Add(common.myInt(hdnFieldId.Value));
                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");
                            coll.Add(common.myDate(txtDate.SelectedDate).ToString("dd/MM/yyyy") + " " + common.myStr(time));
                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                }
            }


            SaveEMRTreatmentTemplate("TF", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", 0, strNonTabular.ToString(),0
                , "", "", "", "", "");
            
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            coll = null;
            strNonTabular = null;
            objException = null;
        }
    }

    protected void gvSpecialsation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (e.CommandName == "ItemDelete")
            {
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                HiddenField hdnId = (HiddenField)row.FindControl("hdnId");

                HiddenField hdnPlanId = (HiddenField)row.FindControl("hdnPlanId");
                HiddenField hdnDayid = (HiddenField)row.FindControl("hdnDayid");
                HiddenField hdnDayDetailId = (HiddenField)row.FindControl("hdnDayDetailId");

                EMRDeleteTreatmentPlanDetail("SP", common.myInt(hdnId.Value), common.myInt(hdnPlanId.Value),
                    common.myInt(hdnDayid.Value), common.myInt(hdnDayDetailId.Value), common.myInt(Session["UserId"]));

            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }

    protected void GridViewdays_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HiddenField hdnId = (HiddenField)e.Item.FindControl("hdnId");

            HiddenField hdnDurationId = (HiddenField)e.Item.FindControl("hdnDurationId");
            if (ViewState["EnteredData"] != null)
            {
                DataView dv = new DataView((DataTable)ViewState["EnteredData"]);
                dv.RowFilter = "DayDetailId=" + common.myInt(hdnId.Value) + " AND DayId=" + common.myInt(hdnDurationId.Value);
                if (dv.ToTable().Rows.Count > 0)
                {
                    e.Item.BackColor = System.Drawing.Color.LightCyan;
                }
                if (common.myInt(ViewState["DayDetailId"]) == common.myInt(hdnId.Value)
                    && common.myInt(ViewState["DayId"]) == common.myInt(hdnDurationId.Value))
                {
                    e.Item.BackColor = System.Drawing.Color.Aqua;
                }

                dv.Dispose();
            }
        }
    }

    protected void btnRefreshCopy_Click(object sender, EventArgs e)
    {
        ddlPlanTemplates.SelectedIndex = ddlPlanTemplates.Items.IndexOf(ddlPlanTemplates.Items.FindItemByValue(common.myStr(hdnSelectedPlanId.Value)));
        ViewState["DayDetailId"] = common.myStr(hdnSelectedDayDetailId.Value);
        ViewState["DayId"] = common.myStr(hdnSelectedDayId.Value);

        GetServiceData(common.myInt(hdnSelectedPlanId.Value), common.myInt(hdnSelectedDayId.Value), common.myInt(hdnSelectedDayDetailId.Value), "");
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        bool bTrue = false;
        if (chkSelectAll.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }
        foreach (GridViewRow item2 in gvSelectedServices.Rows)
        {
            HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
            if (common.myStr(hdnFieldType.Value).Equals("C"))
            {
                DataList rptC = (DataList)item2.FindControl("C");
                foreach (DataListItem rptItem in rptC.Items)
                {
                    CheckBox chk = (CheckBox)rptItem.FindControl("C");
                    chk.Checked = bTrue;
                }
            }
        }
        GridViewdays.DataSource = (DataTable)ViewState["EnteredData"];
        GridViewdays.DataBind();
    }

    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanItemAttributes";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncodedBy = common.myInt(Session["UserId"]);
            objRoot.ItemId = common.myInt(hdnItemId.Value);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlRoute.SelectedIndex = ddlRoute.Items.IndexOf(ddlRoute.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["RouteId"]).ToString()));
                ddlUnit.SelectedIndex = ddlUnit.Items.IndexOf(ddlUnit.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["ItemUnitId"]).ToString()));
                ddlFrequencyId.SelectedIndex = ddlFrequencyId.Items.IndexOf(ddlFrequencyId.Items.FindItemByValue(common.myInt(ds.Tables[0].Rows[0]["FrequencyId"]).ToString()));

                ViewState["ISCalculationRequired"] = common.myBool(ds.Tables[0].Rows[0]["CalculationRequired"]);
            }

        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
            ds.Dispose();
            client = null;
            objRoot = null;
            
        }
    }

    protected void btnChiefComplaints_Click(object sender, EventArgs e)
    {

        if(common.myStr(txtChiefComplaints.Text)== "")
        {
            Alert.ShowAjaxMsg("Please type Chief Complaints", Page);
            return;
        }
        if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
            return;
        }
        
        if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
        {
            Alert.ShowAjaxMsg("Please select day", Page);
            return;
        }
        SaveEMRTreatmentTemplate("CH", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", 0, "", 0
                ,common.myStr(txtChiefComplaints.Text), "", "", "", "");
    }

    protected void btnHistory_Click(object sender, EventArgs e)
    {
        if (common.myStr(txtHistory.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type History", Page);
            return;
        }
        if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
            return;
        }
        
        if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
        {
            Alert.ShowAjaxMsg("Please select day", Page);
            return;
        }
        SaveEMRTreatmentTemplate("HIS", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", 0, "", 0
                , "", common.myStr(txtHistory.Text), "", "", "");
    }

    protected void btnExamination_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtExamination.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type Examination", Page);
            return;
        }
        if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
            return;
        }
        
        if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
        {
            Alert.ShowAjaxMsg("Please select day", Page);
            return;
        }
        SaveEMRTreatmentTemplate("EXM", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", 0, "", 0
                , "", "", common.myStr(txtExamination.Text), "", "");
    }

    protected void btnPlanOfCare_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtPlanOfCare.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type Plan Of Care", Page);
            return;
        }
        if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
            return;
        }
        
        if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
        {
            Alert.ShowAjaxMsg("Please select day", Page);
            return;
        }
        SaveEMRTreatmentTemplate("POC", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", 0, "", 0
                , "", "", "", common.myStr(txtPlanOfCare.Text), "");
    }

    protected void btnInstruction_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtFreeInstruction.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type Instruction", Page);
            return;
        }
        if (common.myInt(ddlPlanTemplates.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Plan", Page.Page);
            return;
        }
        
        if (common.myInt(ViewState["DayId"]) == 0 && common.myInt(ViewState["DayDetailId"]) == 0)
        {
            Alert.ShowAjaxMsg("Please select day", Page);
            return;
        }
        SaveEMRTreatmentTemplate("IN", common.myInt(ddlPlanTemplates.SelectedValue), common.myInt(ViewState["DayId"]),
                common.myInt(ViewState["DayDetailId"]), 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, "", 0, "", 0
                , "", "", "", "", common.myStr(txtFreeInstruction.Text));
    }

    //added by bhakti
    protected DataTable BindBlankTable()
    {
        DataTable dtInv = new DataTable();

        dtInv.Columns.Add("FieldName", typeof(string));
        dtInv.Columns.Add("FieldType", typeof(string));
        dtInv.Columns.Add("SectionId", typeof(string));
        dtInv.Columns.Add("FieldID", typeof(string));
        dtInv.Columns.Add("ColumnNosToDisplay", typeof(string));
        dtInv.Columns.Add("MaxLength", typeof(string));
        dtInv.Columns.Add("ValueId", typeof(string));
        dtInv.Columns.Add("ValueName", typeof(string));
        return dtInv;

    }
}