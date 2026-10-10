<%@ Page Title="My Students" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyStudents.aspx.cs" Inherits="UniversitySystem.Doctor.MyStudents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>My Students</h2>

    <div class="card">
        <div class="card-title">Students Registered In My Courses</div>

        <asp:GridView ID="gvStudents" runat="server" CssClass="grid" AutoGenerateColumns="true"></asp:GridView>
    </div>

</asp:Content>