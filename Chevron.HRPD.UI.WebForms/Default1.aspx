<%@ Page Title="ITRC - Home Page" Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="true"
    CodeBehind="Default1.aspx.cs" Inherits="Chevron.HRPD.UI.WebForms._Default1"  %>
<%@ MasterType VirtualPath="~/MasterPages/Main.master" %>

           
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="This is a description that was created in the code bind file" />
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="welcomeMessage" runat="server">    

<div class="welcomeMessageDIV">

    <div style="position:relative; top:15px;">

    <asp:Label runat="server" id="lblWelcomeMessage" CssClass="welcomeMessageLabel"  ></asp:Label>

    </div>

</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="momentumBand" runat="server">
    <asp:Image ID="MBand" runat="server" ImageUrl="~/images/BannerTitle.JPG" Width="100%" Height="110"
        AlternateText="ITRC Template Application" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FirstColumn" runat="server">  
  <p>
        <strong>The ITRC Sample Application: Ordering System.</strong> <br />
        This sample application was created to present how the ITRC Template is implemented in a real world scenario.
        Paired with the ITRC how-to-guide, the sample applicaiton aims to help developers understand the usage of the
        ITRC template and its underlying reusable components.
  </p>
  <p>
        The ITRC Template is an extension of the IBS Templates which includes reusable component at code level. The
        template comes with reusable libraries and code components that adress application cross-cutting concerns.
 
    </p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="SecondColumn" runat="server">

This sample application was created to present how the ITRC Template is implemented in a real world scenario.
        Paired with the ITRC how-to-guide, the sample applicaiton aims to help developers understand the usage of the
        ITRC template and its underlying reusable components.
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ThirdColumn" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ColumnRight" runat="server">
<h4 runat="server" id="userTitle" style="color:#1990BE !important;" ></h4>
<h4 class="headline" >Quick Links</h4>
<ul class="separator">
<li><a href="https://collab001-hou.sp.chevron.net/sites/devnet/development/nonerp/net/bp/itrc/default.aspx">ITRC Website</a></li>
<li><a href="http://chaos.chevron.com/">CHAOS</a></li>
<li><a href="https://collab001-hou.sp.chevron.net/sites/devnet/default.aspx">Chevron DEVNET</a></li>
</ul>
</asp:Content>
