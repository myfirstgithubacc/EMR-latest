﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintHealthCheckUpArvind.aspx.cs"
    Inherits="EMRReports_PrintHealthCheckUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Health Check Up Medical Summary</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hdnDoctorImage" runat="server" />
        <asp:HiddenField ID="hdnFacilityImage" runat="server" />
         <asp:HiddenField ID="hdnFontName" runat="server" />
    </div>
    </form>
</body>
</html>
