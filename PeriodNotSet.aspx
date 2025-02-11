

<%@ Page Title="Review Period Not Available" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="PeriodNotSet.aspx.cs" Inherits="PTT.ReviewWarning_WebForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="redirect_message.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="parentholder" runat="server">
     <div class ="submsg_container">
           <!--asp item that holds the warining image for the message-->
        <asp:Image ID="warning" runat="server" ImageUrl="~/Assets/warning-icon.png" Height="100px" Width="100px" />

         <!--the message we display to the user-->
        <p>The review period has not been activated yet.</p>
     <div>
</asp:Content>
