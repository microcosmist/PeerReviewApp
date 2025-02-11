

<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="PeriodStillActive.aspx.cs" Inherits="PTT.PeriodStillActive " %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="redirect_message.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="parentholder" runat="server">
     <div class ="submsg_container">
          <!--asp item that holds the warining image for the message-->
        <asp:Image ID="warning" runat="server" ImageUrl="~/Assets/warning-icon.png" Height="100px" Width="100px" />

          <!--the message we display to the user-->
        <p>Performance Reports will be available after the current review period is over.</p>
     <div>
</asp:Content>
