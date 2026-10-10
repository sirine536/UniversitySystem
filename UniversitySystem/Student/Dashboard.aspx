<%@ Page Title="Student Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="UniversitySystem.Student.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 20px;">
        <h2>Welcome to your Dashboard</h2>

        <div class="card" style="background: #e3f2fd; border-left: 5px solid #2196f3; margin-bottom: 25px; padding: 15px; border-radius: 8px;">
            <h3 style="margin-top: 0;">📢 University News</h3>
            <asp:GridView ID="gvAnnouncements" runat="server" CssClass="grid" AutoGenerateColumns="False" 
                GridLines="None" ShowHeader="False" EmptyDataText="No new announcements from Admin.">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div style="margin-bottom: 10px; border-bottom: 1px solid #ddd; padding-bottom: 5px;">
                                <span style="font-size: 0.8em; color: #666;"><%# Eval("PostDate", "{0:dd MMM yyyy}") %></span><br />
                                <span style="font-weight: bold; color: #333;"><%# Eval("MessageContent") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 20px; margin-bottom: 25px;">
            <div class="stat-card" style="background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); text-align: center;">
                <h3 style="font-size: 2em; margin: 0; color: #2196f3;"><asp:Label ID="lblCourses" runat="server" Text="0" /></h3>
                <p style="color: #666; margin: 5px 0 0;">Active Courses</p>
            </div>
          
            <div class="stat-card" style="background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); text-align: center;">
                <h3 style="font-size: 2em; margin: 0; color: #ff9800;"><asp:Label ID="lblCompleted" runat="server" Text="0" /></h3>
                <p style="color: #666; margin: 5px 0 0;">Completed</p>
            </div>
        </div>

        <div class="card" style="background: #fff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 5px rgba(0,0,0,0.1);">
            <div class="card-title" style="font-weight: bold; margin-bottom: 15px; font-size: 1.2em;">My Registered Courses</div>
            <asp:GridView ID="gvCourses" runat="server" CssClass="grid" AutoGenerateColumns="true" Width="100%"></asp:GridView>
        </div>
    </div>
</asp:Content>