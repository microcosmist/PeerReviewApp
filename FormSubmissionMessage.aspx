
<%@ Page Title="Submission Confirmation" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="FormSubmissionMessage.aspx.cs" Inherits="PTT.FormSubmissionMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="redirect_message.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="parentholder" runat="server">
    <!--Container for the submission message-->
    <div class ="submsg_container">
        <!--asp item that holds the checkmark image-->
        <asp:Image ID="checkmark" runat="server" ImageUrl="~/Assets/checkmark.png" Height="100px" Width="100px" />
        <!--our message to the user-->
        <p>Your form has been submitted!</p>
     </div>
</asp:Content>

