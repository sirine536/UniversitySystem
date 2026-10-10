<%@ Page Title="Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Grades.aspx.cs" Inherits="UniversitySystem.Admin.Grades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>All Grades</h2>

    <div class="card">
        <div class="card-title">Grades List</div>

        <asp:GridView ID="gvGrades" runat="server" CssClass="grid" AutoGenerateColumns="true"></asp:GridView>
    </div>

</asp:Content>