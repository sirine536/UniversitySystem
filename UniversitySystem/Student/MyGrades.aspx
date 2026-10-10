<%@ Page Title="My Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyGrades.aspx.cs" Inherits="UniversitySystem.Student.MyGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>My Grades</h2>

    <asp:Label ID="lblProgram" runat="server" Text="Select Program: "></asp:Label>
    <asp:DropDownList ID="ddlPrograms" runat="server" AutoPostBack="true" 
    OnSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" DataTextField="ProgramName" DataValueField="ProgramID">
</asp:DropDownList>
    <div class="card">
        <div class="card-title">Grades List</div>

        <asp:GridView ID="gvGrades" runat="server" CssClass="grid" AutoGenerateColumns="true"></asp:GridView>
    </div>

    <asp:Label ID="lblGPA" runat="server" Font-Bold="true" ForeColor="Blue"></asp:Label>

</asp:Content>