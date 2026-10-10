<%@ Page Title="Doctors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Doctors.aspx.cs" Inherits="UniversitySystem.Admin.Doctors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Doctors Management</h2>

    <div class="card">
        <div class="card-title">Add / Edit Doctor</div>

        <asp:HiddenField ID="hfUserID" runat="server" />

        <label>Full Name</label>
        <asp:TextBox ID="txtFullName" runat="server" CssClass="input" />

        <label>Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="input" />

        <label>Username</label>
        <asp:TextBox ID="txtUsername" runat="server" CssClass="input" />

        <label>Password</label>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="input"  />

        <label>Phone</label>
        <asp:TextBox ID="txtPhone" runat="server" CssClass="input" />

        <label>Department</label>
        <asp:TextBox ID="txtDepartement" runat="server" CssClass="input" />

        <asp:Button ID="btnSave" runat="server" Text="Save Doctor" CssClass="btn btn-green" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnClear_Click" />

        <asp:Label ID="lblMsg" runat="server" CssClass="error" />
    </div>

    <div class="card">
        <div class="card-title">Doctors List</div>

        <asp:GridView ID="gvDoctors" runat="server" CssClass="grid"
            AutoGenerateColumns="False"
            DataKeyNames="UserID"
            OnRowCommand="gvDoctors_RowCommand">

            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="ID" />
                <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Username" HeaderText="Username" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                <asp:BoundField DataField="Departement" HeaderText="Departement" />

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn"
                            CommandName="EditRow" CommandArgument='<%# Eval("UserID") %>' />

                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-red"
                            CommandName="DeleteRow" CommandArgument='<%# Eval("UserID") %>'
                            OnClientClick="return confirm('Delete this doctor?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>

</asp:Content>