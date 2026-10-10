<%@ Page Title="Manage Programs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Programs.aspx.cs" Inherits="UniversitySystem.Admin.Programs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Program & Curriculum Management</h2>

    <div class="card">
        <div class="card-title">Step 1: Create Program</div>
        <label>Program Name</label>
        <asp:TextBox ID="txtProgramName" runat="server" CssClass="input" placeholder="e.g. Computer Science" />
        
        <label>Number of Courses in this Program</label>
        <asp:TextBox ID="txtCourseCount" runat="server" CssClass="input" TextMode="Number" placeholder="How many courses?" />
        
        <div style="margin-top: 15px;">
            <asp:Button ID="btnGenerateFields" runat="server" Text="Next: Define Courses" CssClass="btn btn-green" OnClick="btnGenerateFields_Click" />
        </div>
    </div>

    <asp:Panel ID="pnlCourseInputs" runat="server" Visible="false" CssClass="card">
        <div class="card-title">Step 2: Enter Course Details</div>
        <p class="text-muted">Please enter the details for the <b><asp:Literal ID="litCount" runat="server"></asp:Literal></b> courses:</p>
        
        <asp:PlaceHolder ID="phCourseFields" runat="server"></asp:PlaceHolder>

        <div style="margin-top: 20px;">
            <asp:Button ID="btnFinalSave" runat="server" Text="Finish & Save Everything" CssClass="btn btn-green" OnClick="btnFinalSave_Click" />
        </div>
    </asp:Panel>

    <div class="card">
        <div class="card-title">Existing Academic Programs</div>
        <asp:GridView ID="gvPrograms" runat="server" CssClass="grid" AutoGenerateColumns="False" DataKeyNames="ProgramID" OnRowDeleting="gvPrograms_RowDeleting">
            <Columns>
                <asp:BoundField DataField="ProgramID" HeaderText="ID" />
                <asp:BoundField DataField="ProgramName" HeaderText="Program Name" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CssClass="btn btn-red" OnClientClick="return confirm('Warning: This will delete the program and all its courses. Continue?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <asp:Label ID="lblMsg" runat="server" CssClass="error" />
</asp:Content>