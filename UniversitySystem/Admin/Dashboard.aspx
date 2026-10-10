<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="UniversitySystem.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Admin Dashboard</h2>

    <div class="stats">
        <div class="stat-card">
            <h3><asp:Label ID="lblStudents" runat="server" /></h3>
            <p>Students</p>
        </div>

        <div class="stat-card">
            <h3><asp:Label ID="lblDoctors" runat="server" /></h3>
            <p>Doctors</p>
        </div>

        <div class="stat-card">
            <h3><asp:Label ID="lblCourses" runat="server" /></h3>
            <p>Courses</p>
        </div>

        <div class="stat-card">
            <h3><asp:Label ID="lblRegistrations" runat="server" /></h3>
            <p>Registrations</p>
        </div>
    </div>

    <div class="card">
        <div class="card-title">Recent Registrations</div>

        <asp:GridView ID="gvRecent" runat="server" CssClass="grid" AutoGenerateColumns="true"></asp:GridView>
    </div>

</asp:Content>