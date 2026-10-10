<%@ Page Title="Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="UniversitySystem.Admin.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Student Registration</h2>

    <div class="card">
        <div class="card-title">Register Student To Course</div>

        <label>Student</label>
        <asp:DropDownList ID="ddlStudent" runat="server" CssClass="input"></asp:DropDownList>

        <label>Academic Program</label>
        <asp:DropDownList ID="ddlProgram" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged">
        </asp:DropDownList>

        <label>Course</label>
        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="input"></asp:DropDownList>

        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-green" OnClick="btnRegister_Click" />

        <asp:Label ID="lblMsg" runat="server" CssClass="error" />
    </div>

    <div class="card">
        <div class="card-title">Registered Courses</div>

        <asp:GridView ID="gvRegistration" runat="server" CssClass="grid"
            AutoGenerateColumns="False"
            DataKeyNames="RegID"
            OnRowCommand="gvRegistration_RowCommand">

            <Columns>
                <asp:BoundField DataField="RegID" HeaderText="ID" />
                <asp:BoundField DataField="StudentName" HeaderText="Student" />
                <asp:BoundField DataField="CourseName" HeaderText="Course" />
                <asp:BoundField DataField="RegDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-red"
                            CommandName="DeleteRow" CommandArgument='<%# Eval("RegID") %>'
                            OnClientClick="return confirm('Delete this registration?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>