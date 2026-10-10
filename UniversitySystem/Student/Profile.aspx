<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="UniversitySystem.Student.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>My Profile</h2>

    <div class="card">

        <div class="card-title">Student Information</div>

        <label>Full Name</label>
        <asp:TextBox ID="txtFullName" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>

        <label>Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="input"></asp:TextBox>

        <label>Phone</label>
        <asp:TextBox ID="txtPhone" runat="server" CssClass="input"></asp:TextBox>

        <label>Department</label>
        <asp:TextBox ID="txtDepartment" runat="server" CssClass="input" ReadOnly="true"></asp:TextBox>

        <label>Password</label>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="input"></asp:TextBox>

        <asp:Button ID="btnSave" runat="server" Text="Update Profile" CssClass="btn btn-green" OnClick="btnSave_Click" />

        <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>

    </div>

</asp:Content>