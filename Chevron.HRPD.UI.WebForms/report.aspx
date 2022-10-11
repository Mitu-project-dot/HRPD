<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report.aspx.cs" Inherits="Chevron.HRPD.UI.WebForms.report" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report</title>
    <style type="text/css">
        .report { display: inline; overflow: visible; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <rsweb:ReportViewer ID="rv" runat="server" ProcessingMode="Remote" Width="100%" 
            Height="100%" SizeToReportContent="True" CssClass="report"></rsweb:ReportViewer>
        <asp:Label ID="lblError" runat="server" Visible="False" Text="An error has occurred with the report, if this continues please contact technical support for this application."></asp:Label>
        
    </form>
    
</body>
</html>