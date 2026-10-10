<%@ Page Title="Enter Grades" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnterGrades.aspx.cs" Inherits="UniversitySystem.Doctor.EnterGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Enter Grades</h2>

    <div class="card">
        <div class="card-title">Select Course</div>

        <label>Course</label>
        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged"></asp:DropDownList>

        <asp:Label ID="lblMsg" runat="server" CssClass="error"></asp:Label>
    </div>

    <div class="card">
        <div class="card-title">Students Grades</div>

        <asp:GridView ID="gvGrades" runat="server" CssClass="grid"
            AutoGenerateColumns="False"
            DataKeyNames="RegID"
            OnRowCommand="gvGrades_RowCommand">

            <Columns>
                <asp:BoundField DataField="RegID" HeaderText="Reg ID" />
                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />

                <asp:TemplateField HeaderText="Midterm">
                    <ItemTemplate>
                        <asp:TextBox ID="txtMidterm" runat="server" CssClass="input"
                            Text='<%# Eval("Midterm") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Final">
                    <ItemTemplate>
                        <asp:TextBox ID="txtFinal" runat="server" CssClass="input"
                            Text='<%# Eval("Final") %>'></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Total" HeaderText="Total" />
                <asp:BoundField DataField="LetterGrade" HeaderText="Grade" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-green"
                            CommandName="SaveGrade"
                            CommandArgument='<%# Eval("RegID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>

</asp:Content>