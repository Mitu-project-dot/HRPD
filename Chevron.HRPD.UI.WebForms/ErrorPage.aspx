<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" 
    CodeBehind="ErrorPage.aspx.cs" Inherits="Chevron.HRPD.UI.WebForms.ErrorPage" %>

<%@ MasterType VirtualPath="~/MasterPages/Main.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="css_custom/redmond/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="welcomeMessage" runat="server">
    Error page
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="momentumBand" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FirstColumn" runat="server">
    <div class="panel shadow" style="width: 98%">
        <h2>
            The system encountered an unexpected error - please contact support.</h2>
        <h3>
            <asp:Label ID="ErrorMessage" runat="server" ForeColor="#C00000" Font-Bold="true" Width="98%"
                Font-Size="12px" /></h3>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SecondColumn" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ThirdColumn" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ColumnRight" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="overlayContent" runat="server">
</asp:Content>