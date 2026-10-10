<%@ Page Title="Students" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="UniversitySystem.Admin.Students" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Students Management</h2>

    <div class="card">
        <div class="card-title">Add / Edit Student</div>

        <asp:HiddenField ID="hfUserID" runat="server" />

        <asp:HiddenField ID="hfAnnouncementID" runat="server" />
        
        
        <div style="display: grid; grid-template-columns: 1fr 1fr ; gap: 20px;">
            <div>
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="input" />

                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input" />

                <label>Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input" />
            </div>
            <div>
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="input" OnTextChanged="txtPassword_TextChanged" />

                <label>Phone</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="input" />
            </div>
        </div>

        <div style="margin-top: 15px;">
            <asp:Button ID="btnSave" runat="server" Text="Save Student" CssClass="btn btn-green" OnClick="btnSave_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn" OnClick="btnClear_Click" />
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="error" style="display:block; margin-top:10px;" />
    </div>

    <div class="card">
        <div class="card-title">Students List</div>

        <div style="display: flex; gap: 10px; margin-bottom: 20px; background: #f9f9f9; padding: 15px; border-radius: 8px; align-items: flex-end;">
            <div style="flex: 2;">
                <label>Search Students</label>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="input" placeholder="Search by ID, Name, Email, or Phone..." AutoPostBack="true" OnTextChanged="btnSearch_Click" />
            </div>
            <div style="flex: 1;">
                <label>Sort By</label>
                <asp:DropDownList ID="ddlSort" runat="server" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                    <asp:ListItem Text="Newest First (ID)" Value="UserID DESC" />
                    <asp:ListItem Text="Oldest First (ID)" Value="UserID ASC" />
                    <asp:ListItem Text="Name (A-Z)" Value="FullName ASC" />
                    <asp:ListItem Text="Name (Z-A)" Value="FullName DESC" />
                </asp:DropDownList>
            </div>
        </div>

        <asp:GridView ID="gvStudents" runat="server" CssClass="grid"
            AutoGenerateColumns="False"
            DataKeyNames="UserID"
            OnRowCommand="gvStudents_RowCommand">

            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="ID" />
                <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Username" HeaderText="Username" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn"
                            CommandName="EditRow" CommandArgument='<%# Eval("UserID") %>' />

                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-red"
                            CommandName="DeleteRow" CommandArgument='<%# Eval("UserID") %>'
                            OnClientClick="return confirm('Are you sure you want to delete this student?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="card" style="border-top: 4px solid #28a745;">
        <div class="card-title">📢 Broadcast Message</div>
        <p style="font-size: 0.9em; color: #666;">This message will be visible to all students on their dashboard.</p>
        
        <asp:TextBox ID="txtBulkMessage" runat="server" TextMode="MultiLine" Rows="3" CssClass="input" placeholder="Enter announcement content here..." />
        
        <div style="margin-top: 10px;">
          <asp:Button ID="btnSendBulk" runat="server" Text="Post Announcement" CssClass="btn btn-green" OnClick="btnSendBulk_Click" />
            <asp:Button ID="btnClearAnnouncement" runat="server" Text="Clear" CssClass="btn"
                OnClick="btnClearAnnouncement_Click" />
        </div>

        <br />
         <asp:GridView ID="gvAnnouncements"
        runat="server"
        CssClass="grid"
        AutoGenerateColumns="False"
        DataKeyNames="ID"
        OnRowCommand="gvAnnouncements_RowCommand">

        <Columns>

            <asp:BoundField DataField="ID" HeaderText="ID" />
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="MessageContent" HeaderText="Message" />
            <asp:BoundField DataField="PostDate" HeaderText="Date" />

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>

                    <asp:Button ID="btnEditAnnouncement"
                        runat="server"
                        Text="Edit"
                        CssClass="btn"
                        CommandName="EditAnnouncement"
                        CommandArgument='<%# Eval("ID") %>' />

                    <asp:Button ID="btnDeleteAnnouncement"
                        runat="server"
                        Text="Delete"
                        CssClass="btn btn-red"
                        CommandName="DeleteAnnouncement"
                        CommandArgument='<%# Eval("ID") %>'
                        OnClientClick="return confirm('Are you sure?');" />

                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>
        
        <asp:Label ID="lblA" runat="server" CssClass="error" style="display:block; margin-top:10px;" />
    </div>

</asp:Content>