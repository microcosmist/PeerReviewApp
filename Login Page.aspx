
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_Page.aspx.cs" Inherits="PTT.Login_Page" %>

<!DOCTYPE html>
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" href="login.css"/>
</head>
<body>
    <form id="login_form" runat="server">
        <!--container for the login box-->
        <div class="bg">
            <!--asp item that is the login box, css is modified here as well as in the css file-->
            <asp:Login 
                ID="StudentLogin"
                runat="server" 
                BackColor="#99CCFF" 
                BorderColor="#B5C7DE" 
                BorderPadding="4" 
                BorderStyle="None" 
                BorderWidth="1px" 
                DisplayRememberMe="False" 
                Font-Names="Poppins" 
                Font-Size="0.8em" 
                ForeColor="#333333" 
                Height="400px" 
                UserNameLabelText="UTD-ID:" 
                UserNameRequiredErrorMessage="UTD-ID is required." 
                Width="600px"
                OnAuthenticate="Login_Authentication" TextLayout="TextOnTop">
                <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                <FailureTextStyle Font-Names="Poppins" HorizontalAlign="Center" VerticalAlign="Middle" />
                <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" Font-Names="Poppins" Font-Size="0.8em" ForeColor="#284E98" CssClass="loginbutton" />
                <TextBoxStyle Font-Size="0.8em" />
                <TitleTextStyle Font-Bold="True" Font-Size="0.9em" ForeColor="Black" />
            </asp:Login>
        </div>
    </form>
</body>
</html>
