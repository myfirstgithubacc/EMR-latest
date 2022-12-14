using System;
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
using System.Text;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;

public partial class ComponentReturnList : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsBb objBb;
    clsExceptionLog objException = new clsExceptionLog();
    DataSet dsSearch;
   
   
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindControl();
            if (Request.QueryString["EncNo"] != null)
            {            
                hdnRegistrationID.Value = Convert.ToString(Request.QueryString["Regid"]);
                hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RegNo"]);
                hdnEncounterID.Value = Convert.ToString(Request.QueryString["EncId"]);
                hdnEncounterNo.Value = Convert.ToString(Request.QueryString["EncNo"]);
                hdnStatus.Value = common.myStr(Request.QueryString["Status"]);
            }

            bindData();          
        }
    }
   
    protected void gvBagEntryDetailsList_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvBagEntryDetailsList.CurrentPageIndex = e.NewPageIndex;
        bindData();
    }

    protected void gvBagEntryDetailsList_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (!common.myStr(((HiddenField)e.Item.FindControl("hdnCrossMatchNo")).Value).Equals(""))
                {
                    hdnCrossMatchNo.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnCrossMatchNo")).Value);
                    hdnRequisition.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRequisition")).Value);
                    hdnComponentID.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnComponentID")).Value);
                    hdnComponentIssueNo.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnComponentIssueNo")).Value);
                    hdnTransfusionId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnTransfusionId")).Value);
                    hdnReturnId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnReturnId")).Value);
                    hdnIssue.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnIssue")).Value);
                    hdnCrossMatchId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnCrossMatchId")).Value);
                    HHdnComponentIssueId.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnComponentIssueId")).Value);
                    
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    return;
                }
            }
           

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

  

    private void bindData()
    {
        try
        {
            objBb = new BaseC.clsBb(sConString);
            DataTable table = new DataTable();
            //if(hdnStatus.Value=="P")
                table = GetComponentIssueDetailsForReturning(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "", 1, common.myStr(hdnStatus.Value), Convert.ToString(hdnRegistrationNo.Value), Convert.ToString(hdnRegistrationID.Value), Convert.ToString(hdnEncounterNo.Value), Convert.ToString(hdnEncounterID.Value), 1).Tables[0]; //objBb.GetComponentIssueDetailsForReturning(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "", 1, common.myStr(hdnStatus.Value), Convert.ToString(hdnRegistrationNo.Value), Convert.ToString(hdnRegistrationID.Value), Convert.ToString(hdnEncounterNo.Value), Convert.ToString(hdnEncounterID.Value), 1).Tables[0];
            //if(hdnStatus.Value=="C")
                //table = GetComponentCrossMatchDetailsForReturning(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1, Convert.ToString(hdnRegistrationNo.Value), Convert.ToString(hdnRegistrationID.Value), Convert.ToString(hdnEncounterNo.Value), Convert.ToString(hdnEncounterID.Value)).Tables[0]; //objBb.GetComponentIssueDetailsForReturning(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "", 1, common.myStr(hdnStatus.Value), Convert.ToString(hdnRegistrationNo.Value), Convert.ToString(hdnRegistrationID.Value), Convert.ToString(hdnEncounterNo.Value), Convert.ToString(hdnEncounterID.Value), 1).Tables[0];

            
            if (table.Rows.Count > 0)
            {
                gvBagEntryDetailsList.DataSource = table;           
            }
            else
            {
                DataRow DR = table.NewRow();
                table.Rows.Add(DR);
                gvBagEntryDetailsList.DataSource = table;
            }
            //lblTotRecord.Text = "Total Record(s) Found - " + common.myStr(common.myInt(dsSearch.Tables[0].Rows[0]["TotalRecordsCount"]));
            gvBagEntryDetailsList.DataBind();
            hdnIssue.Value = table.Rows[0]["Issued"].ToString();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public DataSet GetComponentIssueDetailsForReturning(int HospitalLocationId, int FacilityId, string ComponentIssueNo, int Active, string CompleteRecordsOrPartial, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, int Acknowledged)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@intRegistrationID", RegistrationID);
            HshIn.Add("@intEncounterNo", EncounterNo);
            HshIn.Add("@intEncounterID", EncounterID);
            HshIn.Add("@intAcknowledged", Acknowledged);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentReturnList", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    //public DataSet GetComponentCrossMatchDetailsForReturning(int HospitalLocationId, int FacilityId, int Active, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID)
    //{
    //    DataSet ds = new DataSet();
    //    try
    //    {
    //        Hashtable HshIn = new Hashtable();
    //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
    //        HshIn.Add("@intFacilityId", FacilityId);
    //        //HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
    //        HshIn.Add("@bitActive", Active);
    //        //HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);
    //        HshIn.Add("@intRegistrationNo", RegistrationNo);
    //        HshIn.Add("@intRegistrationID", RegistrationID);
    //        HshIn.Add("@intEncounterNo", EncounterNo);
    //        HshIn.Add("@intEncounterID", EncounterID);
    //        //HshIn.Add("@intAcknowledged", Acknowledged);
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossMatchReturnList", HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    return ds;
    //}


    protected void ddlLocation_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (ddlLocation.SelectedValue != null)
        {
            Session["FacilityId"] = ddlLocation.SelectedValue.ToString();
            // bindControl();
            bindData();

        }
    }
    private void bindControl()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            DataView dv;
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active = 1 ";
            ddlLocation.DataSource = dv;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "FacilityID";
            ddlLocation.DataBind();
            ddlLocation.SelectedValue = Session["FacilityId"].ToString();
            ListItem lst = new ListItem();
            bool tf = true;
            bool tEncounter = true;
            if (common.myInt(Request.QueryString["RegEnc"]) == 1)
            {
                tf = false;
            }
            if (common.myInt(Request.QueryString["RegEnc"]) == 0)
            {
                tEncounter = false;
            }

            lst = new ListItem("DonorRegistration", "0", tf);
            //rdoRegEnc.Items.Add(lst);
            lst = new ListItem("Encounter", "1", tEncounter);
            //rdoRegEnc.Items.Add(lst);

            lst = new ListItem("Discharge", "2", true);

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

}
