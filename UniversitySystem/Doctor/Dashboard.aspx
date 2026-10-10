<%@ Page Title="Doctor Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="UniversitySystem.Doctor.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Doctor Dashboard</h2>

    <div class="stats">
        <div class="stat-card">
            <h3><asp:Label ID="lblCourses" runat="server" /></h3>
            <p>My Courses</p>
        </div>

        <div class="stat-card">
            <h3><asp:Label ID="lblStudents" runat="server" /></h3>
            <p>My Students</p>
        </div>

        <div class="stat-card">
            <h3><asp:Label ID="lblGrades" runat="server" /></h3>
            <p>Grades Entered</p>
        </div>
    </div>

    <div class="card">
        <div class="card-title">My Courses</div>
        <asp:GridView ID="gvCourses" runat="server" CssClass="grid" AutoGenerateColumns="true" OnSelectedIndexChanged="gvCourses_SelectedIndexChanged"></asp:GridView>
    </div>

</asp:Content>