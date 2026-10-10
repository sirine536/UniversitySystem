<%@ Page Title="My Courses" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyCourses.aspx.cs" Inherits="UniversitySystem.Student.MyCourses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>My Courses</h2>

    <div class="card">
        <div class="card-title">Registered Courses</div>

        <asp:GridView ID="gvCourses" runat="server" CssClass="grid" AutoGenerateColumns="true"></asp:GridView>
    </div>

</asp:Content>