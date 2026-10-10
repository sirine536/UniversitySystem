<%@ Page Title="Assign Doctors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="UniversitySystem.Admin.Courses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Doctor Assignment to Courses</h2>

    <div class="card">
        <div class="card-title">Assign Doctor to a Specific Course</div>

        <label>1. Select Program</label>
        <asp:DropDownList ID="ddlProgram" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>

        <label>2. Select Course</label>
        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="input"  AutoPostBack="true" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged"></asp:DropDownList>

        <label>3. Select Doctor</label>
        <asp:DropDownList ID="ddlDoctor" runat="server"  AutoPostBack="true" CssClass="input"></asp:DropDownList>

        <div style="margin-top:15px;">
            <asp:Button ID="btnAssign" runat="server" Text="Confirm Assignment" CssClass="btn btn-green" OnClick="btnAssign_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear Selection" CssClass="btn" OnClick="btnClear_Click" />
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="error" />
    </div>

    <div class="card">
        <div class="card-title">Courses List (Current Assignments)</div>

        <asp:GridView ID="gvCourses" runat="server" CssClass="grid" AutoGenerateColumns="False" DataKeyNames="CourseID">
            <Columns>
                <asp:BoundField DataField="CourseID" HeaderText="ID" />
                <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                <asp:BoundField DataField="CourseCode" HeaderText="Code" />
                <asp:BoundField DataField="Credits" HeaderText="Credits" />
                <asp:BoundField DataField="DoctorName" HeaderText="Assigned Doctor" />
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>