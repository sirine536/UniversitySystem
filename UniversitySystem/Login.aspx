<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UniversitySystem.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login - University System</title>
    <link href="CSS/style.css" rel="stylesheet" />
</head>
<body class="login-body">
    <form id="form1" runat="server">
        <div class="login-box">
            <h2>University System</h2>
            <p>Please login to your account</p>

            <asp:TextBox ID="txtUsername" runat="server" CssClass="input" Placeholder="Username" OnTextChanged="txtUsername_TextChanged"></asp:TextBox>

            <asp:TextBox ID="txtPassword" runat="server" CssClass="input" TextMode="Password" Placeholder="Password"></asp:TextBox>

            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />

            <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
        </div>
    </form>
</body>
</html>