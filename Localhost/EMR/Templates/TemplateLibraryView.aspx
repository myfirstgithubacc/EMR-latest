<%@ Page Title="" Language="C#" MasterPageFile="~/Include/Master/EMRMaster.master" AutoEventWireup="true" CodeFile="TemplateLibraryView.aspx.cs" Inherits="EMR_Templates_TemplateLibraryView" %>


<%@ Register TagPrefix="asplNew" TagName="UserDetailsHeader" Src="~/Include/Components/TopPanelNew.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AJAX" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../../Include/css/open-sans.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/mainStyle.css" rel="stylesheet" runat="server" />
    <link href="../../Include/css/emr.css" rel="stylesheet" type="text/css" />
    <link href="../../Include/css/emr_new.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .blink {
            text-decoration: blink;
        }

        .blinkNone {
            text-decoration: none;
        }

        .RadGrid td .ajax__calendar td {
            padding: 0;
        }

        .FormatRadioButtonList input {
            margin: 7px 2px !important;
            font-weight: bold !important;
        }

        .FormatRadioButtonList label {
            margin: 4px 5px 4px 1px !important;
            font-weight: 600 !important;
            font-size: 12px;
        }

        #ctl00_ContentPlaceHolder1_gvSelectedServices_ctl03_C .checkbox {
            margin: 2px 0px !important;
            padding: 0px !important;
            height: auto !important;
        }
    </style>
    <div>
     
        <script type="text/javascript">


            function returnToParent() {
                var oArg = new Object();

                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            if (window.captureEvents) {
                window.captureEvents(Event.KeyUp);
                window.onkeyup = executeCode;
            }
            else if (window.attachEvent) {
                document.attachEvent('onkeyup', executeCode);
            }

            function executeCode(evt) {
                if (evt == null) {
                    evt = window.event;
                }
                var theKey = parseInt(evt.keyCode, 10);
                switch (theKey) {
                    case 114:  // F3
                        $get('<%=btnSave.ClientID%>').click();
                    break;
            }
            evt.returnValue = false;
            return false;
        }
        </script>

        <script type="text/javascript">

            function SelectAllTemplateRequiredServices(id) {
                //get reference of GridView control
                var grid = document.getElementById("<%=gvTemplateRequiredServices.ClientID%>");
                //variable to contain the cell of the grid
                var cell;

                if (grid.rows.length > 0) {
                    //loop starts from 1. rows[0] points to the header.
                    for (ridx = 1; ridx < grid.rows.length; ridx++) {
                        //get the reference of first column
                        cell = grid.rows[ridx].cells[0];

                        //loop according to the number of childNodes in the cell
                        for (cIdx = 0; cIdx < cell.childNodes.length; cIdx++) {
                            //if childNode type is CheckBox
                            if (cell.childNodes[cIdx].type == "checkbox") {
                                //assign the status of the Select All checkbox to the cell checkbox within the grid
                                cell.childNodes[cIdx].checked = document.getElementById(id).checked;
                            }
                        }
                    }
                }
            }

            function isValidateDate(sender, args) {
                var dateString = document.getElementById(sender.controltovalidate).value;

                var regex = /((0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[012])\/((19|20)\d\d))$/;

                if (regex.test(dateString)) {
                    var parts = dateString.split("/");
                    var dt = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
                    args.IsValid = (parseInt(dt.getDate()) == parseInt(parts[0]) && parseInt((dt.getMonth() + 1)) == parseInt(parts[1]) && parseInt(dt.getFullYear()) == parseInt(parts[2]));

                } else {
                    args.IsValid = false;
                }

                if (!args.IsValid) {
                    if (dateString == "__/__/____" || dateString == "") {
                        args.IsValid = true;
                    }
                    else {
                        alert("Invalid date format.");
                        document.getElementById(sender.controltovalidate).value = '__/__/____';
                    }
                }
            }

            //alert(parseInt(parts[0]) + "/" + parseInt(parts[1]) + "/" + parseInt(parts[2]));
            //alert(parseInt(dt.getDate()) + "/" + parseInt((dt.getMonth() + 1)) + "/" + parseInt(dt.getFullYear()));

            function isValidateDateTabular(sender, args) {
                var dateString = document.getElementById(sender.controltovalidate).value;

                var regex = /((0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[012])\/((19|20)\d\d))$/;

                if (regex.test(dateString)) {
                    var parts = dateString.split("/");
                    var dt = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
                    args.IsValid = (parseInt(dt.getDate()) == parseInt(parts[0]) && parseInt((dt.getMonth() + 1)) == parseInt(parts[1]) && parseInt(dt.getFullYear()) == parseInt(parts[2]));

                } else {
                    args.IsValid = false;
                }

                if (!args.IsValid) {
                    if (dateString == "__/__/____" || dateString == "") {
                        args.IsValid = true;
                    }
                    else {
                        document.getElementById(sender.controltovalidate).value = '__/__/____';
                    }
                }
            }

        </script>

        <script type="text/javascript" src="/Include/JS/Functions.js" language="javascript"></script>

        <script type="text/javascript">
            function OnClientSelectionChange(editor, args) {
                var tool = editor.getToolByName("RealFontSize");
                if (tool && !$telerik.isIE) {
                    setTimeout(function () {
                        var value = tool.get_value();

                        switch (value) {
                            case "11px":
                                value = value.replace("11px", "9pt");
                                break;
                            case "12px":
                                value = value.replace("12px", "9pt");
                                break;
                            case "14px":
                                value = value.replace("14px", "11pt");
                                break;
                            case "16px":
                                value = value.replace("16px", "12pt");
                                break;
                            case "15px":
                                value = value.replace("15px", "11pt");
                                break;
                            case "18px":
                                value = value.replace("18px", "14pt");
                                break;
                            case "24px":
                                value = value.replace("24px", "18pt");
                                break;
                            case "26px":
                                value = value.replace("26px", "20pt");
                                break;
                            case "32px":
                                value = value.replace("32px", "24pt");
                                break;
                            case "34px":
                                value = value.replace("34px", "26pt");
                                break;
                            case "48px":
                                value = value.replace("48px", "36pt");
                                break;
                        }
                        tool.set_value(value);
                    }, 0);
                }
            }
        </script>

        <script type='text/javascript'>
            function isNumber(evt) {
                evt = (evt) ? evt : window.event;
                var charCode = (evt.which) ? evt.which : evt.keyCode;

                if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 8 && charCode != 45 && charCode != 46) {
                    return false;
                }
                return true;
            }

            function OnClientEditorLoad(editor, args) {
                var style = editor.get_contentArea().style;
                style.fontFamily = 'Tahoma';
                style.fontSize = 11 + 'pt';
            }
            function OnClientCalenderClick(oWnd, args) {
                $get('<%=btnCalender.ClientID%>').click();
            }
            function OnClientCloseReportSetup(oWnd, args) {
                $get('<%=btnReportSetup.ClientID%>').click();
            }

            function OnClientLoad(editor) {
                editor.AttachEventHandler("RADEVENT_SEL_CHANGED", function (e) {
                    if (!document.all) {
                        var oTool = editor.GetToolByName("RealFontSize");
                        oTool.HeaderElement.firstChild.nodeValue = editor.GetSelection().GetParentElement().style.fontSize;
                    }
                }
 );
            }
        </script>

        <script type="text/javascript">
            var ilimit = 40;
            function AutoChange(txtRemarks) {
                var txt = document.getElementById(txtRemarks);
                if (txt.value.length >= 10) {
                    if (txt.value.length >= 40 * txt.rows) {
                        txt.rows = txt.rows + 1;
                        ilimit = 0;
                    }
                    else if (txt.value.length < 40 * (txt.rows - 1)) {
                        txt.rows = Math.round(txt.value.length / 40) + 1;
                    }
                    else if (txt.value.length >= 500) {
                        txt.value.length = txt.value.substring(0, 500)
                        return false;
                    }
                    else {
                        if (txt.value.length <= ilimit * txt.rows && txt.rows >= ilimit) {
                            txt.cols = (txt.cols * 1) + 1;
                        }
                    }
                }
                return true;
            }

            function MaxLenTxt(TXT, intMax) {
                if (TXT.value.length > intMax) {
                    TXT.value = TXT.value.substr(0, intMax);
                    alert("Maximum length is " + intMax + " characters only.");
                }
            }

            //            function maxLength(TXT, intMax) {
            //                if (TXT.value.length > intMax) {
            //                    TXT.value = TXT.value.substr(0, intMax);
            //                    alert("Maximum length is " + intMax + " characters only.");
            //                }
            //            }

            function openRadWindow(ID, ControlType) {

                var oWnd = radopen("SentanceGallery.aspx?ID=" + ID + "&ControlType=" + ControlType, "Radwindow1");
                oWnd.Center();

            }
            function openSentenceWindow(ID, ControlType, TemplateFieldId, SectionId, RowIndex) {
                $get('<%=hdnSelCell.ClientID%>').value = RowIndex;
                var oWnd = radopen("SentanceGallery.aspx?ID=" + ID + "&ControlType=" + ControlType + "&TemplateFieldId=" + TemplateFieldId + "&SectionId=" + SectionId, "Radwindow1");
                oWnd.Center();
            }
            function openStaticTemplateWindow(url, StaticTemplateId, TemplateFieldId, SectionId) {
                var oWnd = radopen(url + "?StaticTemplateId=" + StaticTemplateId + "&TemplateFieldId=" + TemplateFieldId + "&POPUP=StaticTemplate&SectionId=" + SectionId, "Radwindow1");
                oWnd.minimize();
                oWnd.maximize();
                oWnd.Center();
            }
            function openResultStaticTemplateWindow(url, SOURCE, DIAG_SAMPLEID, SERVICEID, AgeInDays, StatusCode, ServiceName) {
                var oWnd = radopen(url + "?SOURCE=" + SOURCE + "&DIAG_SAMPLEID=" + DIAG_SAMPLEID + "&SERVICEID=" + SERVICEID + "&AgeInDays=" + AgeInDays + "&StatusCode=" + StatusCode + "&ServiceName=" + ServiceName, "RadWindowForNew");
                // window.open(url + "?SOURCE=" + SOURCE + "&DIAG_SAMPLEID=" + DIAG_SAMPLEID + "&SERVICEID=" + SERVICEID + "&AgeInDays=" + AgeInDays+ "&StatusCode=" + StatusCode+ "&ServiceName=" + ServiceName, "popup",'width=400,height=400'); 
                //               oWnd.minimize();
                //               oWnd.maximize();
                oWnd.Center();
            }

            function openNewWindowExternlink(url) {
                window.open(url, "popup", 'width=400,height=400');

            }


            function openRadWindowForFieldValue(ID, ControlType, SectionId, FieldId, RowIndex) {
                $get('<%=hdnSelCell.ClientID%>').value = RowIndex;
                var oWnd = radopen("AddFieldValue.aspx?ID=" + ID + "&ControlType=" + ControlType + "&SectionId=" + SectionId + "&FieldId=" + FieldId, "RadWindow12");
                oWnd.Center();



            }

            function addRowInTabular(ID) {
                var hdnSelectedRow = document.getElementById("<%=hdnSelectedRow.ClientID%>");
                hdnSelectedRow.value = ID;
                $get('<%=btnAddRowInTabular.ClientID%>').click();
            }

            function openRadWindowW(ID, ControlType, RowIndex) {
                //var lbl = document.getElementById(hdnSelCell);
                $get('<%=hdnSelCell.ClientID%>').value = RowIndex;
                // lbl.value = RowIndex;
                //alert(RowIndex);
                //  var oWnd = radopen("SentanceGallery.aspx?ID=" + ID + "&ControlType=" + ControlType, "Radwindow1");
                oWnd.Center();

            }
            function OnCloseSentenceGalleryRadWindow(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {

                    var Sentence = arg.Sentence;
                    var ctrl = arg.ControlId;
                    var ctrltype = arg.ControlType;
                    ctrl = document.getElementById(ctrl);

                    if (ctrltype == "W") {
                        $get('<%=btnCheck.ClientID%>').click();
                    }
                    else {
                        ctrl.value = Sentence;

                    }
                }
            }
            function OnCloseFieldValueRadWindow(oWnd, args) {
                var arg = args.get_argument();
                if (arg) {
                    var ValueId = arg.Sentence;
                    var ctrl = arg.ControlId;
                    var ctrltype = arg.ControlType;
                    ctrl = document.getElementById(ctrl);
                    if (ctrltype == "W") {
                        $get('<%=btnCheck.ClientID%>').click();
                    }
                    else if (ctrltype == "T" || ctrltype == "M") {
                        ctrl.value = ValueId;
                    }
                    else if (ctrltype == "D" || ctrltype == "C") {
                        $get('<%=btnBindFieldData.ClientID%>').click();
                    }
            $get('<%=hdnValueId.ClientID%>').value = ValueId;
                    $get('<%=hdnControlType.ClientID%>').value = ctrltype;
                }
            }

        </script>

        <script type="text/javascript">
            var editorList = new Object();
            var editorLengthArray = [3000, 3000, 3000, 3000, 3000, 3000];
            var counter = 0;

            function isAlphaNumericKey(keyCode) {
                if ((keyCode > 47 && keyCode < 58) || (keyCode > 64 && keyCode < 91)) {
                    return true;
                }
                return false;
            }

            function LimitCharacters(editor) {

                editorList[editor.get_id()] = editorLengthArray[counter];
                counter++;
                var mode = editor.get_mode();
                editor.attachEventHandler("onkeydown", function (e) {
                    e = (e == null) ? window.event : e;
                    if (isAlphaNumericKey(e.keyCode)) {
                        var maxTextLength = editorList[editor.get_id()];
                        if (mode == 2) {
                            textLength = editor.get_textArea();
                        }
                        else {
                            textLength = editor.get_text().length;
                        }
                        if (textLength >= maxTextLength) {
                            alert('We are not able to accept more than ' + maxTextLength + ' symbols!');
                            e.returnValue = false;
                        }
                    }
                });
            }
            function CalculateLength(editor, value) {
                var textLength = editor.get_text().length;
                var clipboardLength = value.length;
                textLength += clipboardLength;
                return textLength;
            }
            //            window.onbeforeunload = function(evt) {
            //                var IsUnsave = $get('<%=hdnIsUnSavedData.ClientID%>').value;
            //                if (IsUnsave == 1) {
            //                    return false;
            //                }
            //            }
            function OnClientPasteHtml(editor, args) {
                var maxTextLength = editorList[editor.get_id()];
                var commandName = args.get_commandName();
                var value = args.get_value();

                if (commandName == "PasteFromWord"
                    || commandName == "PasteFromWordNoFontsNoSizes"
                    || commandName == "PastePlainText"
                    || commandName == "PasteAsHtml"
                    || commandName == "Paste") {
                    var textLength = CalculateLength(editor, value);
                    if (textLength >= maxTextLength) {
                        alert('We are not able to accept more than ' + maxTextLength + ' symbols!');
                        args.set_cancel(true);

                    }
                }
            }
            function OnClientToggleStateChanging(sender, eventArgs) {
                var toggleindex = sender.get_selectedToggleStateIndex();
                $get('<%=txtToggleIndex.ClientID%>').value = toggleindex;
            }

        </script>

        <script type="text/javascript">
            function PrintContent() {
                var DocumentContainer = $get('<%=Printtab.ClientID%>')

                var WindowObject = window.open('', 'PrintWindow', 'width=750,height=650,top=50,left=50,toolbars=no,scrollbars=yes,status=no,resizable=yes');
                WindowObject.document.writeln(DocumentContainer.innerHTML);
                WindowObject.document.close();
                WindowObject.focus();
                WindowObject.print();
                WindowObject.close();
            }

            function PrintSetupContent() {

                var ReportContent = $get('<%=hdnReportContent.ClientID%>')

                var WindowObject = window.open('', 'PrintWindow2', 'width=750,height=650,top=50,left=50,toolbars=yes,scrollbars=yes,status=no,resizable=yes');
                WindowObject.document.writeln(ReportContent.value);
                WindowObject.document.close();
                WindowObject.focus();
                WindowObject.print();
                //WindowObject.close();
            }
            function radioMe(e, CurrentGridRowCheckBoxListID) {
                if (!e) e = window.event;
                var sender = e.target || e.srcElement;

                if (sender.nodeName != 'INPUT') return;
                var checker = sender;
                var chkBox = document.getElementById(CurrentGridRowCheckBoxListID);
                var chks = chkBox.getElementsByTagName('INPUT');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i] != checker)
                        chks[i].checked = false;
                }
            }


            function keyPress(sender, args) {

                if (args.get_keyCode() == 9) {

                    document.getElementById('<%= gvTabularFormat.ClientID %>').focus();
                }

            }

        </script>

        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Width="650" Height="580"
            EnableViewState="false" VisibleStatusbar="false" Behaviors="Close,Move" OnClientClose="OnCloseFieldValueRadWindow"
            VisibleOnPageLoad="false" ReloadOnShow="false">
        </telerik:RadWindowManager>
        <asp:UpdatePanel ID="valMain" runat="server">
            <ContentTemplate>
                <div class="patient-TopNew">
                    <div class="patientTopNew-Fixed">
                        <div class="VisitHistoryDiv">
                            <div class="container-fluid">
                                <div class="row">
                                    <asp:UpdatePanel ID="upd1" runat="server">
                                        <ContentTemplate>
                                            <div class="col-md-3 col-sm-3">
                                                <div class="WordProcessorDivText">
                                                    <h2></h2>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6">
                                                <asp:Label ID="lblMessage" ForeColor="Green" Font-Bold="true" runat="server" />
                                            </div>
                                            <div class="col-md-2 col-sm-2">
                                                <asp:Button ID="btnOPVisit" runat="server" Text="OP Visit" CssClass="PatientBtn01"
                                                    ToolTip="OP Visit" Visible="false" OnClick="btnOPVisit_Click" />
                                                <%--<asp:Button ID="btnBackMenu"  CssClass="PatientBtn01a" runat="server" Text="Back"
                                                    ValidationGroup="Save" ToolTip="Back"  />--%>
                                                <%--<asp:Button ID="btnCaseSheet" runat="server" Text="Case Sheet" SkinID="Button" ToolTip="Back to Case Sheet" OnClick="btnCaseSheet_Click" />--%>
                                                <asp:Button ID="btnSave" CssClass="PatientBtn01a" runat="server" Text="Save (Ctrl+F3)"
                                                    ValidationGroup="Save" ToolTip="Save" OnClick="btnsave_Click" />
                                                <asp:Button ID="btnNew" runat="server" ToolTip="New&nbsp;Record" CssClass="PatientBtn01a"
                                                    Text="New" OnClick="btnNew_OnClick" />
                                                <asp:Button ID="btnClose" Visible="false" runat="server" Text="Close" CssClass="PatientBtn01a"
                                                    OnClientClick="window.close();" /><%-- onclick="javascript:window.close();" />--%>
                                                <asp:HiddenField ID="hdnIsUnSavedData" runat="server" />
                                                <asp:Button ID="btnPrintReport2" runat="server" Text="Print" ToolTip="Print Report"
                                                    OnClick="btnPrintReport2_Click" CssClass="PatientBtn01a" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <table border="0" width="100%" class="clsheader" cellpadding="0" cellspacing="0"
                            style="display: none;">
                            <tr>
                                <td style="padding-left: 5px; width: 200px;">
                                    <asp:Label ID="lblHeading" runat="server" SkinID="label" />
                                </td>
                                <td style="width: 60px;">
                                    <asp:Button ID="btnLock" SkinID="button" runat="server" Text="Lock" OnClick="btnLock_OnClick"
                                        Visible="false" />
                                </td>
                                <td style="width: 450px;">
                                    <table id="tblReport" runat="server" border="0" cellpadding="0" cellspacing="2">
                                        <tr>
                                            <td>
                                                <asp:Label ID="label100" runat="server" SkinID="label" Text="Report List" />
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="ddlReport" runat="server" Width="200px" EmptyMessage="[ Select ]" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibtnReportSetup" runat="server" ImageUrl="~/Images/PopUp.jpg"
                                                    ToolTip="Report Setup" Height="18px" Width="17px" OnClick="ibtnReportSetup_Click" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btnPrintReport" SkinID="button" runat="server" Text="Print" ToolTip="Print Report"
                                                    OnClick="btnPrintReport_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <table id="tblManualLabNo" runat="server" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" SkinID="label" Font-Bold="true" BackColor="Aqua"
                                                    Text="Manual&nbsp;Lab&nbsp;No" Font-Size="Larger" />&nbsp;:&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblManualLabNo" runat="server" SkinID="label" BackColor="Aqua" Font-Bold="true"
                                                    Font-Size="Larger" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div class="VisitHistoryBorderNew">
                            <div class="container-fluid">
                                <div class="row">
                                    <asplNew:UserDetailsHeader ID="asplHeaderUD" runat="server" />
                                </div>
                            </div>
                        </div>
                        <%--  
                    <table border="0" style="background: #F5DEB3; margin-left: 0px; padding-top: 0px; border-style: solid none solid none; border-width: 1px; border-color: #808080;" cellpadding="2" cellspacing="2" width="100%">
                        <tr><td><asp:Label ID="lblPatientDetail" runat="server" Text="" Font-Bold="true"></asp:Label></td></tr>
                    </table>--%>
                        <div class="emrRoll-New">
                            <div class="emrRoll-Center">
                                <div class="VitalHistory-Div02">
                                    <div class="container-fluid">
                                        <div class="row" id="tblMainTable" runat="server">
                                            <div class="col-md-3 col-sm-3">
                                                <div class="PatientHistoryText01b">
                                                    <h2>
                                                        <asp:Label ID="lblTemplate2" runat="server" Text="Template" /></h2>
                                                    <h3>
                                                        <telerik:RadComboBox ID="ddlTemplateMain" runat="server" EmptyMessage="[ Select ]"
                                                            Width="176px" Height="400px" DropDownWidth="350px" Filter="Contains" Visible="false"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateMain_SelectedIndexChanged" />
                                                    </h3>
                                                </div>
                                            </div>
                                            <div class="col-md-3 col-sm-3">
                                                <div class="PatientHistoryText01">
                                                    <h2>
                                                        <asp:Label ID="Label1" runat="server" Text="Session&nbsp;List(s)" /></h2>
                                                    <h3>
                                                        <telerik:RadComboBox ID="ddlRecord" SkinID="DropDown" runat="server" Width="103px"
                                                            DropDownWidth="185px" EmptyMessage="[ Select ]" AutoPostBack="true" OnSelectedIndexChanged="ddlRecord_OnSelectedIndexChanged" />
                                                        <h4>
                                                            <asp:Button ID="btnNewRecord" runat="server" CssClass="PatientBtn03a" OnClick="btnNewRecord_OnClick"
                                                                Text="Add" ToolTip="New Record" />
                                                        </h4>
                                                        <%--<h4><asp:Button ID="btnNewRecord" CssClass="PatientBtn03" runat="server" Text="Add New Session" ToolTip="New Record" OnClick="btnNewRecord_OnClick" /></h4>--%><%-- <asp:LinkButton ID="lnkClinicalDetail" runat="server" Text="Clinical Details" OnClick="lnkClinicalDetail_OnClick"></asp:LinkButton>--%>
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                    </h3>
                                                </div>
                                            </div>
                                            <div class="col-md-2 col-sm-3">
                                                <div class="PatientHistoryText01">
                                                    <h3>
                                                        <h4>
                                                            <asp:Button ID="btnsetPreviousData" runat="server" CssClass="PatientBtn02" OnClick="btnsetPreviousData_Click"
                                                                Text="Copy Previous Episode" ToolTip="Show Previous Data" Visible="false" />
                                                        </h4>
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                    </h3>
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-3">
                                                <asp:Label ID="lblTemplateName" runat="server"></asp:Label>
                                                <div class="PatientHistoryText01a">
                                                    <h2>
                                                        <asp:Label ID="Label2" runat="server" Text="Patient&nbsp;Template" />
                                                        <span id="spnTemplatePatient" runat="server" visible="false" style="color: Red; font-weight: bold; font-size: 14px">
                                                            <blink>*</blink>
                                                        </span>
                                                    </h2>
                                                    <h3>
                                                        <telerik:RadComboBox ID="ddlTemplatePatient" runat="server" EmptyMessage="[ Select ]"
                                                            Height="400px" Width="126px" DropDownWidth="350px" Filter="Contains" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlTemplatePatient_SelectedIndexChanged" />
                                                        <h3>
                                                            <asp:Button ID="btnResultSetOpen" runat="server" CssClass="PatientBtn03a" OnClick="btnResultSetOpen_OnClick" Text="Result Set" />
                                                        </h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                        <h3></h3>
                                                    </h3>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:UpdatePanel ID="updPanel1" runat="server">
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="tvCategory" />
                                                        <asp:PostBackTrigger ControlID="btnSave" />
                                                        <%--<asp:AsyncPostBackTrigger  ControlID="btnSave" />--%>
                                                        <asp:AsyncPostBackTrigger ControlID="btnLock" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <table border="0" cellspacing="0" cellpadding="0" width="100%" style="vertical-align: middle;">
                            <tr>
                                <td class="clsheader" style="padding-left: 5px;" align="left" valign="middle">
                                     Template Details
                                </td>
                                <td class="clsheader" align="center">
                                    <asp:UpdatePanel ID="upNewSave" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lbl_Msg" runat="server" ForeColor="Green"></asp:Label>
                                            <asp:Label ID="lblTemplateLibraryName" runat="server" ForeColor="Green"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="right" class="clsheader" style="padding-right: 5px;">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnHeaderClose" runat="server" OnClientClick="window.close();" Text="Close" SkinID="Button" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>

                                        </div>
                                        <div class="row">
                                            <div class="PatientHistoryLeft">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TreeView ID="tvCategory" runat="server" ImageSet="Msdn" Font-Size="8pt" NodeIndent="10"
                                                            OnSelectedNodeChanged="tvCategory_SelectedNodeChanged" NodeWrap="true">
                                                            <ParentNodeStyle Font-Bold="False" />
                                                            <HoverNodeStyle Font-Underline="True" BackColor="#CCCCCC" BorderColor="#888888" BorderStyle="Solid"
                                                                BorderWidth="0px" />
                                                            <SelectedNodeStyle BackColor="gray" ForeColor="White" Font-Underline="False" HorizontalPadding="0px"
                                                                VerticalPadding="0px" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" />
                                                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" NodeSpacing="1px"
                                                                HorizontalPadding="0px" VerticalPadding="0px" />
                                                        </asp:TreeView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <%--<div class="PatientHistoryImg">
                                    <table id="Table3" border="0" runat="server" width="100%" cellpadding="0" cellspacing="0">
                                        <tr><td align="center"><img src="../../Images/Fading-Line1.jpg" height="100%" alt="line" /></td></tr>
                                        <tr><td align="center"><img src="../../Images/Fading-Line1.jpg" height="100%" alt="line" /></td></tr>
                                        <tr><td align="center"><img src="../../Images/Fading-Line1.jpg" height="100%" alt="line" /></td></tr>
                                        <tr><td align="center"><img src="../../Images/Fading-Line1.jpg" height="100%" alt="line" /></td></tr>
                                        <tr><td align="center"><img src="../../Images/Fading-Line1.jpg" height="100%" alt="line" /></td></tr>
                                        <tr><td align="center"><img src="../../Images/Fading-Line1.jpg" height="100%" alt="line" /></td></tr>
                                    </table>
                                </div>--%>
                                            <%--<div class="PatientHistoryRight">--%>
                                            <div class="PatientHistoryRight">
                                                <div style="height: auto !important; width: 100% !important;" valign="top">
                                                    <table id="Printtab" runat="server" cellpadding="0" cellspacing="0">
                                                        <tr style="display: none;">
                                                            <td align="center">
                                                                <asp:Label ID="lblGrid" runat="server" SkinID="label" Visible="false" Font-Bold="true"
                                                                    Font-Size="Larger" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblConfidentail" runat="server" SkinID="label" Visible="false" Text="(Confidential)"
                                                                    Font-Bold="true" Font-Size="Larger" />&nbsp;&nbsp;&nbsp;
                                                                <asp:LinkButton ID="lnkViewConfidential" runat="server" Visible="false" Text="Viewable By"
                                                                    OnClick="lnkViewConfidential_OnClick" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>
                                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Button ID="btnScoreCalc" Text="Score Calculation" CssClass="PatientBtn03b" OnClick="btnScoreCalc_Onclick"
                                                                                        runat="server" Visible="false" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <%--<asp:Panel ID="pnlNonTabularFormat" ScrollBars="Auto" Width="750px" runat="server">--%>
                                                                                    <asp:Panel ID="pnlNonTabularFormat" Width="1040px" runat="server">
                                                                                        <asp:GridView ID="gvSelectedServices" SkinID="gridview3" runat="server" HeaderStyle-Wrap="false"
                                                                                            AutoGenerateColumns="False" ShowHeader="false" CellSpacing="0" CellPadding="0"
                                                                                            Width="100%" CssClass="YellowBorder" AllowPaging="false" PagerSettings-Visible="true"
                                                                                            OnRowDataBound="gvSelectedServices_RowDataBound">
                                                                                            <EmptyDataTemplate>
                                                                                                No Data Found.
                                                                                            </EmptyDataTemplate>
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="FieldID" HeaderText="ID" />
                                                                                                <asp:BoundField DataField="FieldName" HeaderText="Property Name" HeaderStyle-Width="15%"
                                                                                                    ItemStyle-Width="15%" ItemStyle-VerticalAlign="Top" HeaderStyle-Font-Size="10pt" />
                                                                                                <asp:BoundField DataField="FieldType" HeaderText="PropertyType" />
                                                                                                <asp:TemplateField HeaderText="Values" ItemStyle-Width="35%" HeaderStyle-Width="35%"
                                                                                                    HeaderStyle-Font-Size="10pt" ItemStyle-VerticalAlign="Top">
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl1" cellpadding="0" cellspacing="1" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT" SkinID="textbox" CssClass="TextboxTemplate" Columns='<%#common.myInt(Eval("MaxLength"))%>'
                                                                                                                        Visible="false" MaxLength='<%#common.myInt(Eval("MaxLength"))%>' runat="server"
                                                                                                                        Height="16" />
                                                                                                                </td>
                                                                                                                <td></td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" Style="min-height: 50px; max-height: 40px; min-width: 800px; max-width: 800px; background-color: #fff !important;"
                                                                                                                        MaxLength="5000" onkeyup="return MaxLenTxt(this,5000);"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="ddlTemplateFieldFormats" Font-Size="10pt" runat="server" OnSelectedIndexChanged="ddlTemplateFieldFormats_OnSelectedIndexChanged"
                                                                                                                        SkinID="DropDown" Width="200px" AutoPostBack="true" Visible="false">
                                                                                                                    </asp:DropDownList>
                                                                                                                    <telerik:RadEditor ID="txtW" ToolbarMode="ShowOnFocus" OnClientSelectionChange="OnClientSelectionChange"
                                                                                                                        EnableResize="true" runat="server" Skin="Outlook" Height="99%" Width="800px"
                                                                                                                        ToolsFile="~/Include/XML/PrescriptionRTF.xml" EditModes="Design" OnClientLoad="OnClientEditorLoad" />
                                                                                                                    <asp:LinkButton ID="lnkStaticTemplate" Text="" Visible="false" ToolTip="Open Static Template"
                                                                                                                        runat="server" CommandArgument='<%#Eval("StaticTemplateId")%>' />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp" Text="H" Visible="false" ToolTip="Sentence Gallery" runat="server"
                                                                                                                        SkinID="button" Enabled="false"  />
                                                                                                                    <asp:HiddenField ID="hdnisLinkRequire" runat="server" Value='<%#Eval("IsLinkRequire")%>' />
                                                                                                                    <asp:HiddenField ID="hdnLinkUrl" runat="server" Value='<%#Eval("LinkUrl") %>' />
                                                                                                                    <asp:HyperLink ID="Hy_LinkUrl" runat="server" Visible="false" Style="cursor: pointer" />
                                                                                                                    <asp:HiddenField ID="hdnEmployeeTypeID" runat="server" Value='<%#Eval("EmployeeTypeID") %>' />
                                                                                                                    <asp:HiddenField ID="hdnIsEmployeeTypeTagged" runat="server" Value='<%#Eval("IsEmployeeTypeTagged") %>' />
                                                                                                                    <asp:HiddenField ID="hdnColumnNosToDisplay" runat="server" Value='<%#Eval("ColumnNosToDisplay")%>' />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <table id="Table1" cellpadding="0" cellspacing="1" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:DropDownList ID="D" SkinID="DropDown" AutoPostBack="true" OnSelectedIndexChanged="D_OnClick"
                                                                                                                        Visible="false" runat="server" Width="227px" Font-Size="10pt" AppendDataBoundItems="true">
                                                                                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                    <telerik:RadComboBox ID="IM" Visible="false" runat="server" Width="227px" Font-Size="10pt"
                                                                                                                        AppendDataBoundItems="true" Skin="Default" EnableAutomaticLoadOnDemand="True"
                                                                                                                        EnableVirtualScrolling="true">
                                                                                                                        <Items>
                                                                                                                            <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                                        </Items>
                                                                                                                    </telerik:RadComboBox>
                                                                                                                    <asp:DataList ID="C" runat="server" Visible="false" CellPadding="1" CellSpacing="1">
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:HiddenField ID="hdnCV" runat="server" Value='<%#Eval("ValueId")%>' />
                                                                                                                            <asp:CheckBox ID="C" SkinID="checkbox" Font-Size="10pt" runat="server" Text='<%#Eval("ValueName")%>' />
                                                                                                                            <textarea id="CT" class="Textbox" visible="false" runat="server" onkeypress="AutoChange()"
                                                                                                                                rows="1" cols="40"></textarea>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:DataList>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/PopUp.jpg" ToolTip="Add New Value"
                                                                                                                        Width="15px" Visible="false" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:RadioButtonList ID="B" Width="100px" OnSelectedIndexChanged="B_OnClick" AutoPostBack="true"
                                                                                                            runat="server" Visible="false" RepeatDirection="Horizontal">
                                                                                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                                                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                                                                        </asp:RadioButtonList>
                                                                                                        <asp:RadioButtonList ID="R" CssClass="FormatRadioButtonList" runat="server" Visible="false"
                                                                                                            AutoPostBack="true" OnSelectedIndexChanged="B_OnClick">
                                                                                                        </asp:RadioButtonList>
                                                                                                        <table id="tblDate" runat="server" visible="false" cellpadding="0" cellspacing="0">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="85px" Height="25px" runat="server" MaxLength="10" />
                                                                                                                </td>
                                                                                                                <td style="margin:1px 7px 0 -4px; float:left;">
                                                                                                                    <asp:Image  ImageUrl="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate" runat="server" />
                                                                                                                </td>
                                                                                                                
                                                                                                                <td>
                                                                                                                    <style>.ajax__calendar_container{z-index:9}</style>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDate"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                    <asp:CustomValidator ID="CustomValidator"  CssClass="hide" runat="server" ClientValidationFunction="isValidateDate"
                                                                                                                        ControlToValidate="txtDate" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <telerik:RadTimePicker ID="tpTime" runat="server" AutoPostBack="True" DateInput-ReadOnly="true"
                                                                                                                 OnSelectedDateChanged="tpTime_SelectedIndexChanged"
                                                                                                                        PopupDirection="BottomLeft"
                                                                                                                        TimeView-Columns="3" Width="95px"   />
                                                                                                                    <telerik:RadComboBox ID="ddlTime" runat="server" AutoPostBack="True" 
                                                                                                                   OnSelectedIndexChanged="ddlTime_SelectedIndexChanged"
                                                                                                                        Height="300px" Skin="Outlook" Width="50px" />
                                                                                                               <%--     &nbsp;<asp:Literal ID="ltDateTime" runat="server" Text="HH   MM" Visible="false" />&nbsp;--%>

                                                                                                                </td>
                                                                                                                <%--  <td>
                                                                                                                 <telerik:RadDateTimePicker ID="RadDateTimePicker1" runat="server" AutoPostBackControl="Both"
                                                                        TabIndex="37" Width="500px" CssClass="inlin-bl1" DateInput-ReadOnly="true" DateInput-DateFormat="dd/MM/yyyy HH:mm tt" />
                                                                                                                    </td>--%>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Remarks" Visible="false" ItemStyle-VerticalAlign="Top">
                                                                                                    <ItemTemplate>
                                                                                                        <textarea id="txtRemarks" class="Textbox" runat="server" onkeypress="AutoChange()"
                                                                                                            rows="1" cols="40"></textarea>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="ParentId" HeaderText="ParentId" />
                                                                                                <asp:BoundField DataField="ParentValue" HeaderText="ParentValue" />
                                                                                                <asp:BoundField DataField="Hierarchy" HeaderText="Hierarchy" />
                                                                                                <asp:BoundField DataField="SectionID" HeaderText="Section ID" />
                                                                                                <asp:BoundField DataField="DataObjectId" HeaderText="DataObjectId" />
                                                                                                <asp:BoundField DataField="IsMandatory" HeaderText="IsMandatory" />
                                                                                                <asp:BoundField DataField="MandatoryType" HeaderText="MandatoryType" />
                                                                                                <asp:BoundField DataField="EmployeeTypeId" HeaderText="EmployeeTypeID" />
                                                                                                  <%--<asp:BoundField DataField="DateType" HeaderText="DateType" />--%>
                                                                                                <%--<asp:BoundField DataField="FieldValue" HeaderText="Field Value" />--%>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <%--Another GridView for Tabular Data--%>
                                                                        <table border="0" cellpadding="0" cellspacing="1">
                                                                            <tr>
                                                                                <td>
                                                                                    <table cellpadding="2" cellspacing="1">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Button ID="btnAddRow" runat="server" Text="Add New Record" OnClick="btnAddRow_Click"
                                                                                                    SkinID="Button" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Button ID="btnFormulaCalculate" runat="server" Text="Formula Calculate" OnClick="btnFormulaCalculate_OnClick"
                                                                                                    SkinID="Button" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadGrid ID="gvTabularFormat" runat="server" AutoGenerateColumns="False"
                                                                                        Skin="Windows7" Height="400px" Width="100%" CssClass="borderNone" CellPadding="0"
                                                                                        CellSpacing="0" AllowPaging="false" ShowHeader="true" ShowFooter="true" AllowSorting="false"
                                                                                        OnItemDataBound="gvTabularFormat_OnItemDataBound">
                                                                                        <ClientSettings>
                                                                                            <Scrolling FrozenColumnsCount="0" AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                                            <%--<ClientEvents OnKeyPress="keyPress" />--%>
                                                                                        </ClientSettings>
                                                                                        <MasterTableView TableLayout="Auto">
                                                                                            <NoRecordsTemplate>
                                                                                                <div style="font-weight: bold; color: Red;">
                                                                                                    No Record Found.
                                                                                                </div>
                                                                                            </NoRecordsTemplate>
                                                                                            <Columns>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl0" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT0" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM0" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" Visible="false" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp0" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink0" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D0" SkinID="DropDown" runat="server" Width="105px" Font-Size="10pt"
                                                                                                            Visible="false" AppendDataBoundItems="true" />
                                                                                                        <telerik:RadComboBox ID="IM0" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B0" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate0" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td valign="top">
                                                                                                                    <asp:TextBox ID="txtDate0" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate0" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender0" runat="server" TargetControlID="txtDate0"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate0" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender0" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate0" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender0" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate0" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator0" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate0" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId0" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT0" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl1" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT1" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM1" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp1" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink1" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D1" SkinID="DropDown" runat="server" Width="105px" Font-Size="10pt"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM1" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B1" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate1" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate1" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate1" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate1"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate1" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate1" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender1" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate1" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate1" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId1" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT1" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl2" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT2" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM2" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp2" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink2" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D2" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM2" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B2" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate2" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate2" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate2" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDate2"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate2" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate2" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender2" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate2" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate2" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId2" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT2" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl3" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT3" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM3" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp3" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink3" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D3" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM3" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B3" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate3" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate3" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate3" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDate3"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate3" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate3" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender3" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate3" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate3" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId3" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT3" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl4" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT4" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM4" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp4" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink4" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D4" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM4" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B4" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate4" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate4" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate4" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtDate4"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate4" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate4" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender4" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate4" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate4" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId4" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT4" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl5" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT5" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM5" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp5" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink5" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D5" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM5" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B5" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate5" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate5" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate5" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtDate5"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate5" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate5" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender5" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate5" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator5" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate5" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId5" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT5" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl6" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT6" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM6" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp6" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink6" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D6" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM6" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B6" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate6" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate6" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td align="left" valign="top">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate6" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtDate6"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate6" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate6" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender6" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate6" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator6" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate6" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId6" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT6" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl7" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT7" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM7" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp7" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink7" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D7" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM7" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B7" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate7" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate7" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate7" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtDate7"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate7" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate7" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender7" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate7" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator7" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate7" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId7" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT7" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl8" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT8" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM8" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp8" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink8" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D8" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM8" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B8" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate8" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate8" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate8" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtDate8"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate8" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate8" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender8" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate8" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator8" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate8" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId8" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT8" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl9" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT9" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM9" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp9" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink9" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D9" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM9" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B9" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate9" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate9" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate9" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txtDate9"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate9" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate9" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender9" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate9" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator9" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate9" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId9" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT9" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl10" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT10" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM10" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp10" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink10" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D10" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM10" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B10" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate10" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate10" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate10" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txtDate10"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate10" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate10" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender10" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate10" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator10" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate10" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId10" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT10" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl11" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT11" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM11" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp11" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink11" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D11" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM11" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B11" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate11" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate11" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate11" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender11" runat="server" TargetControlID="txtDate11"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate11" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate11" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender11" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate11" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator11" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate11" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId11" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT11" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl12" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT12" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM12" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp12" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink12" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D12" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM12" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B12" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate12" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate12" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate12" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender12" runat="server" TargetControlID="txtDate12"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate12" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate12" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender12" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate12" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator12" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate12" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId12" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT12" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl13" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT13" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM13" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp13" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink13" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D13" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM13" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B13" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate13" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate13" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate13" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender13" runat="server" TargetControlID="txtDate13"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate13" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate13" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender13" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate13" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator13" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate13" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId13" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT13" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl14" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT14" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM14" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp14" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink14" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D14" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM14" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B14" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate14" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate14" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td align="left" valign="top">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate14" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender14" runat="server" TargetControlID="txtDate14"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate14" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate14" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender14" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate14" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator14" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate14" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId14" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT14" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl15" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT15" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM15" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp15" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink15" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D15" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM15" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B15" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate15" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate15" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td align="left" valign="top">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate15" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender15" runat="server" TargetControlID="txtDate15"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate15" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate15" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender15" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate15" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator15" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate15" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId15" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT15" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl16" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT16" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM16" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp16" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink16" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D16" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM16" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B16" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate16" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate16" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate16" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender16" runat="server" TargetControlID="txtDate16"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate16" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate16" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender16" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate16" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator16" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate16" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId16" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT16" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl17" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT17" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM17" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp17" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink17" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D17" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM17" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B17" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate17" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate17" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate17" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender17" runat="server" TargetControlID="txtDate17"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate17" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate17" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender17" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate17" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator17" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate17" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId17" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT17" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl18" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT18" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM18" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp18" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink18" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D18" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM18" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B18" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate18" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate18" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate18" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender18" runat="server" TargetControlID="txtDate18"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate18" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate18" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender18" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate18" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator18" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate18" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId18" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT18" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl19" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT19" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM19" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp19" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink19" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D19" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM19" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B19" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate19" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate19" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate19" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender19" runat="server" TargetControlID="txtDate19"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate19" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate19" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender19" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate19" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator19" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate19" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId19" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT19" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl20" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT20" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM20" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp20" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink20" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D20" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM20" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B20" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate20" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate20" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate20" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender20" runat="server" TargetControlID="txtDate20"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate20" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate20" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender20" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate20" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator20" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate20" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId20" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT20" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl21" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT21" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM21" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp21" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink21" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D21" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM21" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B21" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate21" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate21" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate21" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender21" runat="server" TargetControlID="txtDate21"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate21" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate21" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender21" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate21" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator21" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate21" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId21" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT21" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl22" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT22" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM22" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp22" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink22" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D22" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM22" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B22" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate22" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate22" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate22" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender22" runat="server" TargetControlID="txtDate22"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate22" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate22" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender22" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate22" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator22" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate22" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId22" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT22" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl23" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT23" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM23" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp23" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink23" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D23" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM23" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B23" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate23" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate23" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate23" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender23" runat="server" TargetControlID="txtDate23"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate23" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate23" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender23" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate23" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator23" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate23" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId23" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT23" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl24" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT24" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM24" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp24" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink24" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D24" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM24" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B24" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="Select" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate24" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate24" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate24" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender24" runat="server" TargetControlID="txtDate24"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate24" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate24" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender24" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate24" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator24" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate24" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId24" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT24" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl25" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT25" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM25" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp25" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink25" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D25" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM25" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B25" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="Select" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate25" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate25" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate25" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender25" runat="server" TargetControlID="txtDate25"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate24" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender25" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate25" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender25" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate25" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator25" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate25" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId25" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT25" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl26" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT26" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM26" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp26" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink26" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D26" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM26" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B26" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="Select" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate26" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate26" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate26" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender26" runat="server" TargetControlID="txtDate26"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate26" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender26" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate26" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender26" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate26" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator26" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate26" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId26" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT26" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl27" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT27" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM27" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp27" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink27" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D27" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM27" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B27" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="Select" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate27" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate27" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate27" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender27" runat="server" TargetControlID="txtDate27"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate27" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender27" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate27" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender27" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate27" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator27" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate27" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId27" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT27" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl28" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT28" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM28" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp28" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink28" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D28" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM28" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B28" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="Select" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate28" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate28" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate28" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender28" runat="server" TargetControlID="txtDate28"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate28" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender28" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate28" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender28" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate28" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator28" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate28" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId28" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT28" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl29" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT29" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM29" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp29" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink29" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D29" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM29" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B29" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="Select" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate29" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate29" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate29" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender29" runat="server" TargetControlID="txtDate29"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate29" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender29" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate29" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender29" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate29" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator29" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate29" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId29" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT29" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridTemplateColumn>
                                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                                    <ItemTemplate>
                                                                                                        <table id="tbl30" cellpadding="0" cellspacing="0" border="0" runat="server">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtT30" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        Visible="false" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtM30" SkinID="textbox" CssClass="TextboxTemplate" runat="server"
                                                                                                                        TextMode="MultiLine" onkeyup="return MaxLenTxt(this,5000);" Style="min-height: 70px; max-height: 70px; min-width: 220px; max-width: 220px;"
                                                                                                                        MaxLength="5000" Visible="false" />
                                                                                                                </td>
                                                                                                                <td valign="top">
                                                                                                                    <asp:Button ID="btnHelp30" Text="H" ToolTip="Sentence Gallery" runat="server" SkinID="button"
                                                                                                                        Visible="false" />
                                                                                                                    <%--<NewCode Added 22-3-2014- >--%>
                                                                                                                    <asp:HyperLink ID="hypLink30" runat="server" Visible="false" Style="cursor: pointer"></asp:HyperLink>
                                                                                                                    <%-- <NewCode End -22-03-2014>--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:DropDownList ID="D30" SkinID="DropDown" runat="server" Font-Size="10pt" Width="105px"
                                                                                                            Visible="false" />
                                                                                                        <telerik:RadComboBox ID="IM30" Visible="false" runat="server" Width="120px" Font-Size="10pt"
                                                                                                            DropDownWidth="300px">
                                                                                                            <Items>
                                                                                                                <telerik:RadComboBoxItem Value="0" Text="Select" />
                                                                                                            </Items>
                                                                                                        </telerik:RadComboBox>
                                                                                                        <asp:DropDownList ID="B30" SkinID="DropDown" runat="server" Font-Size="10pt" Width="100px"
                                                                                                            Visible="false">
                                                                                                            <asp:ListItem Value="" Text="Select" />
                                                                                                            <asp:ListItem Value="0" Text="No" />
                                                                                                            <asp:ListItem Value="1" Text="Yes" />
                                                                                                        </asp:DropDownList>
                                                                                                        <table id="tblDate30" cellpadding="0" cellspacing="0" border="0" runat="server" visible="false">
                                                                                                            <tr valign="top">
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtDate30" SkinID="textbox" CssClass="TextboxTemplateDate" Font-Size="13px"
                                                                                                                        Text="" Width="67px" Height="25px" runat="server" />
                                                                                                                </td>
                                                                                                                <td valign="top" align="left">
                                                                                                                    <img src="~/Images/calendar.gif" alt="Click here to get date" width="19" height="20"
                                                                                                                        vspace="0" border="0" id="imgFromDate30" runat="server" />
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <AJAX:CalendarExtender ID="CalendarExtender30" runat="server" TargetControlID="txtDate30"
                                                                                                                        Format="dd/MM/yyyy" PopupButtonID="imgFromDate30" EnabledOnClient="true">
                                                                                                                    </AJAX:CalendarExtender>
                                                                                                                    <AJAX:FilteredTextBoxExtender ID="FilteredTextBoxExtender30" runat="server" Enabled="True"
                                                                                                                        TargetControlID="txtDate30" FilterType="Custom, Numbers" ValidChars="_/">
                                                                                                                    </AJAX:FilteredTextBoxExtender>
                                                                                                                    <AJAX:MaskedEditExtender ID="MaskedEditExtender30" runat="server" CultureAMPMPlaceholder=""
                                                                                                                        CultureCurrencySymbolPlaceholder="" ClearMaskOnLostFocus="false" CultureDatePlaceholder=""
                                                                                                                        CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                                                        Enabled="True" TargetControlID="txtDate30" MessageValidatorTip="false" AcceptAMPM="true"
                                                                                                                        AcceptNegative="None" AutoComplete="true" Mask="99/99/9999" MaskType="Number"
                                                                                                                        ErrorTooltipEnabled="false" InputDirection="LeftToRight">
                                                                                                                    </AJAX:MaskedEditExtender>
                                                                                                                </td>
                                                                                                                <td>
                                                                                                                    <asp:CustomValidator ID="CustomValidator30" runat="server" ClientValidationFunction="isValidateDateTabular"
                                                                                                                        ControlToValidate="txtDate30" ErrorMessage="Invalid date format." />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <asp:Label ID="lblFieldId30" runat="server" Visible="false" />
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Label ID="lblT30" runat="server" Style="width: 99%; text-align: right;" Text="&nbsp;"
                                                                                                            BorderColor="Silver" BorderWidth="1px" SkinID="label" />
                                                                                                    </FooterTemplate>
                                                                                                </telerik:GridTemplateColumn>
                                                                                                <telerik:GridBoundColumn DataField="SectionId" HeaderText="Section Id" />
                                                                                                <telerik:GridBoundColumn DataField="RowNum" HeaderText="Row Num" />
                                                                                                <telerik:GridBoundColumn DataField="IsData" HeaderText="IsData" />
                                                                                                <telerik:GridBoundColumn DataField="Id" HeaderText="Id" />
                                                                                                <telerik:GridBoundColumn DataField="RowCaptionId" HeaderText="RowCaptionId" />
                                                                                                <telerik:GridBoundColumn DataField="RowCaptionName" HeaderText="RowCaptionName" />
                                                                                            </Columns>
                                                                                        </MasterTableView>
                                                                                    </telerik:RadGrid>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="tvCategory" EventName="SelectedNodeChanged" />
                                                                        <%--<asp:AsyncPostBackTrigger ControlID="btnSetDefault" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnUndoChanges" />--%>
                                                                        <asp:PostBackTrigger ControlID="btnSave" />
                                                                        <%--<asp:AsyncPostBackTrigger ControlID="btnSave" />--%>
                                                                        <asp:AsyncPostBackTrigger ControlID="btnLock" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                                <asp:TextBox ID="hdnSelCell" runat="server" Text="0" Style="visibility: hidden;" />
                                                                <asp:Label ID="hdnSentence" runat="server" Text="" Style="visibility: hidden;" />
                                                                <asp:Button ID="btnCheck" Text="" SkinID="Button" OnClick="btnCheck_Onclick" runat="server"
                                                                    BackColor="Transparent" BorderColor="Transparent" BorderWidth="0" Height="0"
                                                                    Width="0" />
                                                                <asp:Button ID="btnAddRowInTabular" Text="" SkinID="Button" OnClick="btnAddRowInTabular_Onclick"
                                                                    runat="server" Width="0" Style="visibility: hidden" />
                                                                <asp:HiddenField ID="hdnSelectedRow" runat="server" Value="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Comment Part -->
                        <table border="0" width="99%" cellpadding="0" cellspacing="2" style="display: none;">
                            <tr>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="4">
                                        <tr runat="server" id="TemplateRequiredServices">
                                            <td>
                                                <asp:Label ID="lblServices" runat="server" SkinID="label" Text="Services" Visible="false" />
                                            </td>
                                            <td colspan="7">
                                                <telerik:RadComboBox ID="ddlTemplateRequiredServices" runat="server" EmptyMessage="[ Select ]"
                                                    Width="250px" Filter="Contains" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateRequiredServices_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <!-- Comment Part -->
                                        <tr valign="middle">
                                            <td>
                                                <table id="tblEpisode" runat="server" cellpadding="0" cellspacing="2" visible="false">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" SkinID="label" Text="Episode&nbsp;List(s)" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadComboBox ID="ddlEpisode" runat="server" EmptyMessage="[ Select ]" Width="200px"
                                                                Filter="Contains" AutoPostBack="true" OnSelectedIndexChanged="ddlEpisode_SelectedIndexChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnEpisodeStart" SkinID="button" runat="server" Text="Create" OnClick="btnEpisodeStart_Click" />
                                                            <asp:Button ID="btnEpisodeClose" SkinID="button" runat="server" Text="Close" Visible="false"
                                                                OnClick="btnEpisodeClose_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnEpisodeCancel" SkinID="button" runat="server" Text="Cancel" Visible="false"
                                                                OnClick="btnEpisodeCancel_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcase" Visible="false" runat="server" SkinID="DropDown">
                                                    <asp:ListItem Selected="True" Text="Original Data" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Default Value" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <div id="dvlegend" runat="server" style="display: none;">
                                                    <asp:Label ID="lblColorCodeForMandatoryTemplate" runat="server" BorderWidth="1px"
                                                        Text="&nbsp;" Width="20px" />
                                                    <asp:Label ID="Label14" runat="server" SkinID="label" Text="Investigation Specification(s) Optional"></asp:Label>
                                                    <asp:Label ID="lblColorCodeForTemplateRequired" runat="server" BorderWidth="1px"
                                                        Text="&nbsp;" Width="20px" />
                                                    <asp:Label ID="Label12" runat="server" SkinID="label" Text="Investigation Specification(s) Mandatory"></asp:Label>
                                                </div>
                                            </td>
                                            <td>
                                                <%--<asp:LinkButton ID="lnkPatientImmunization" runat="server" Text="Patient Immunization" Visible="false" OnClick="lnkPatientImmunization_OnClick" />--%>
                                                <asp:CheckBox ID="chkPullForward" runat="server" Text="Pull Forward From Prior Exam"
                                                    TextAlign="Right" Visible="false" />
                                                <asp:TextBox ID="txtToggleIndex" runat="server" Style="visibility: hidden;" Width="10" />
                                                <asp:Button ID="btnPrint" runat="server" Text="Section Print" SkinID="Button" Visible="false"
                                                    OnClientClick="javaScript:PrintContent();" />
                                            </td>
                                        </tr>
                                        <!-- Comment Part -->
                                        <tr runat="server" id="trGridTemplateRequiredServices">
                                            <td valign="top">
                                                <asp:Label ID="lblServicesLBL" runat="server" SkinID="label" Text="Services" Visible="false" />
                                            </td>
                                            <td valign="top" colspan="8">
                                                <asp:GridView ID="gvTemplateRequiredServices" SkinID="gridview2" runat="server" AutoGenerateColumns="False"
                                                    Width="740px" Visible="false" OnRowDataBound="gvTemplateRequiredServices_RowDataBound"
                                                    OnRowCommand="gvTemplateRequiredServices_OnRowCommand">
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-Width="20px" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAll" runat="server" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkRow" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderText="Service(s)" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblServiceName" SkinID="label" runat="server" Text='<%#Eval("ServiceName")%>' />
                                                                <asp:HiddenField ID="hdnServiceId" runat="server" Value='<%#Eval("ServiceId")%>' />
                                                                <asp:HiddenField ID="hdnServiceOrderDetailId" runat="server" Value='<%#Eval("ServiceOrderDetailId")%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" Text='Edit' CommandName="SERVICE" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnDelete" runat="server" CommandName="DEL" ImageUrl="~/Images/DeleteRow.png"
                                                                    ToolTip="Delete data of service template" Width="16px" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <!-- Comment Part -->
                            <tr>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="4">
                                        <tr valign="middle">
                                            <td>
                                                <%--  
                                                <asp:Button ID="btnViewHistroy" SkinID="Button" runat="server" Text="View Histroy" Visible="false" OnClick="btnViewHistroy_Click" />
                                                <asp:Button ID="btnViewDymanicTemplate" SkinID="Button" runat="server" Text="View Dymanic Template" Visible="false" OnClick="btnViewDymanicTemplate_Click" />
                                                <asp:Button ID="btnSetDefault" SkinID="button" runat="server" Text="Set Default" OnClick="btnSetDefault_Click" />
                                                
                                                <telerik:RadButton ID="btnSetPositive" ToolTip="Set Positive/Negative to Boolean Values" Visible="false" runat="server" ButtonType="StandardButton" ToggleType="CustomToggle" Text="Set Positive/Negative" OnClientToggleStateChanging="OnClientToggleStateChanging" OnClick="btnSetPositive_OnClick">
                                                    <ToggleStates>
                                                        <telerik:RadButtonToggleState Text="Set All Positive" />
                                                        <telerik:RadButtonToggleState Text="Set All Negative" />
                                                        <telerik:RadButtonToggleState Text="Set All None" />
                                                    </ToggleStates>
                                                </telerik:RadButton>
                                                <asp:Button ID="btnUndoChanges" SkinID="button" runat="server" Text="Undo Changes" OnClick="btnUndoChanges_Click" Visible="false" />
                                                --%>
                                            </td>
                                        </tr>
                                        <%--<asp:Button ID="btnDiagnosis" runat="server" Text="Diagnosis" SkinID="Button" OnClick="btnDiagnosis_OnClick" />--%>
                                        <%--<asp:Button ID="btnLabResult" SkinID="button" runat="server" Text="Lab Result" OnClick="btnLabResult_OnClick" />--%>
                                        <%-- <tr id="trEpisode" runat="server" visible="false"></tr>--%>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table id="tblProviderDetails" runat="server" visible="false">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblProvider" runat="server" SkinID="label" Text="Provider" />
                                                <asp:Label ID="lblProviderStart" runat="server" SkinID="label" Text="*" ForeColor="Red" />
                                            </td>
                                            <td>
                                                <telerik:RadComboBox ID="ddlProvider" runat="server" EmptyMessage="[ Select ]" Height="250px"
                                                    Width="250px" DropDownWidth="250px" Filter="Contains" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblChangeDate" runat="server" SkinID="label" Text="Date" />
                                                <asp:Label ID="lblChangeDateStar" runat="server" SkinID="label" Text="*" ForeColor="Red" />
                                            </td>
                                            <td>
                                                <telerik:RadDatePicker ID="dtpChangeDate" runat="server" MinDate="01/01/1870" DateInput-ReadOnly="true"
                                                    Width="168px">
                                                </telerik:RadDatePicker>
                                                &nbsp;
                                                <asp:Literal ID="Literal1" runat="server" Text="Time"></asp:Literal>
                                                <asp:Label ID="Label4" runat="server" SkinID="label" Text="*" ForeColor="Red" />
                                                <telerik:RadTimePicker ID="RadTimeFrom" runat="server" DateInput-ReadOnly="true"
                                                    PopupDirection="BottomLeft" TimeView-Columns="10" Width="95px" />
                                                <telerik:RadComboBox ID="ddlMinute" runat="server" AutoPostBack="True" Height="300px"
                                                    Skin="Outlook" Width="50px" OnSelectedIndexChanged="ddlMinute_SelectedIndexChanged">
                                                </telerik:RadComboBox>
                                                &nbsp;
                                                <asp:Literal ID="ltDateTime" runat="server" Text="HH   MM"></asp:Literal>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp;
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lblRange" runat="server" SkinID="label" Text="*" Font-Bold="true"
                                                    ForeColor="Red" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="2" style="display: none;">
                            <tr>
                                <td>
                                    <div id="dvMandatory" runat="server" visible="false" style="width: 400px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 75px; left: 300px; top: 150px">


                                        <table width="100%" cellspacing="2" cellpadding="0">
                                            <tr>
                                                <td colspan="3" align="center">
                                                    <asp:Label ID="lblMandatoryMessage" Font-Size="12px" runat="server" Font-Bold="true"
                                                        Text="Mandatory Message" />
                                                </td>
                                            </tr>


                                            <tr>
                                                <td colspan="3">&nbsp;
                                                </td>
                                            </tr>


                                            <tr>
                                                <td align="center"></td>


                                                <td align="center">
                                                    <asp:Button ID="btnMandatorySave" SkinID="Button" runat="server" Text="Save" OnClick="btnMandatorySave_OnClick" />&nbsp;
                                                    <asp:Button ID="btnMandatoryOk" SkinID="Button" runat="server" Text="Ok" OnClick="btnMandatoryOk_OnClick"
                                                        Visible="false" />&nbsp;
                                                    <asp:Button ID="btnMandatoryCancel" SkinID="Button" runat="server" Text="Cancel"
                                                        OnClick="btnMandatoryCancel_OnClick" />
                                                </td>


                                                <td align="center"></td>

                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <div id="dvResultSet" runat="server" visible="false" style="width: 790px; z-index: 200; border-bottom: 4px solid #CCCCCC; border-left: 4px solid #CCCCCC; border-right: 4px solid #CCCCCC; border-top: 4px solid #CCCCCC; background-color: #FFFFCC; position: absolute; bottom: 0; height: 45px; left: 270px; top: 70px; padding-top: 8px;">

                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:Button ID="btnSaveAs" runat="server" CssClass="btn btn-primary" Text="Save Result Set"
                                            OnClick="btnSaveAs_Click" Width="117px" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtResultSet" runat="server" MaxLength="50" Width="100%" />
                                    </div>

                                    <div class="col-md-1" style="width: 90px">
                                        <asp:Label ID="lblResultSet" runat="server" Text="Result&nbsp;Set(s)" />
                                    </div>

                                    <div class="col-md-2">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <telerik:RadComboBox ID="ddlResultSet" runat="server" EmptyMessage="[ Select ]" Filter="Contains"
                                                    Width="130px" DropDownWidth="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlResultSet_SelectedIndexChanged" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-md-3" style="margin-left: 30px;">
                                        <asp:Button ID="btnDeleteResultSet" runat="server" CssClass=" btn btn-primary" Text="Delete Result Set"
                                            OnClick="btnDeleteResultSet_Click" />
                                        <asp:Button ID="btnResultSetClose" runat="server" CssClass=" btn btn-primary" Text="Close"
                                            OnClick="btnResultSetClose_OnClick" Width="60px" />
                                    </div>

                                </div>

                            </div>


                        </div>
                        <table width="98%" style="display: none;">
                            <tr>
                                <td>
                                    <telerik:RadWindowManager ID="RadWindowManager2" EnableViewState="false" runat="server">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowPrint" runat="server" />
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <telerik:RadWindowManager ID="RadWindowManager" runat="server" EnableViewState="false">
                                        <Windows>
                                            <telerik:RadWindow ID="RadWindowForNew" runat="server" Behaviors="Maximize,Close,Move">
                                            </telerik:RadWindow>
                                        </Windows>
                                    </telerik:RadWindowManager>
                                    <asp:Button ID="btnReportSetup" runat="server" Text="" Style="visibility: hidden;"
                                        OnClick="btnReportSetup_Click" />
                                    <asp:Button ID="btnCalender" runat="server" Text="" Style="visibility: hidden;" OnClick="btnCalender_Click" />
                                    <asp:Button ID="btnBindFieldData" runat="server" Style="visibility: hidden;" OnClick="btnBindFieldData_Click" />
                                    <asp:HiddenField ID="hdnDoctorImage" runat="server" />
                                    <asp:HiddenField ID="hdnReportContent" runat="server" />
                                    <asp:HiddenField ID="hdnValueId" runat="server" />
                                    <asp:HiddenField ID="hdnControlType" runat="server" />
                                    <asp:HiddenField ID="hdnMandatoryStar" runat="server" Value="<span runat='server' style='color: Red; font-weight: bold;'>*</span>" />
                                    <%--<asp:HiddenField ID="hdnMandatoryStar" runat="server" Value="*"  />--%>
                                </td>
                            </tr>
                        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

