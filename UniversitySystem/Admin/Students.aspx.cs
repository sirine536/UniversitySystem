using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UniversitySystem.Admin
{
    public partial class Students : System.Web.UI.Page
    {

        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";
        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["RoleID"].ToString() != "1")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStudents();
                LoadAnnouncements();
            }
        }

        //private void LoadStudents()
        //{
        //    using(SqlConnection conn = GetConnection())
        //    {
        //        conn.Open();
        //        TextBox ts = (TextBox)Master.FindControl("MainContent").FindControl("txtSearch");
        //        DropDownList ds = (DropDownList)Master.FindControl("MainContent").FindControl("ddlSort");

        //        string searchTerm = (ts != null) ? "%" + ts.Text.Trim() + "%" : "%%";
        //        string sortOrder = (ds != null && !string.IsNullOrEmpty(ds.SelectedValue))
        //                            ? ds.SelectedValue
        //                            : "UserID DESC";

        //        string query = @"
        //            SELECT UserID, FullName, Email, Username, Phone
        //            FROM Users
        //            WHERE RoleID = 3
        //            AND (
        //                FullName LIKE @search OR
        //                Email LIKE @search OR
        //                Username LIKE @search OR
        //                Phone LIKE @search OR
        //                CAST(UserID AS VARCHAR) LIKE @search
        //            )
        //            ORDER BY " + sortOrder;

        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@search", searchTerm);

        //        DataTable dt = new DataTable();
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //        gvStudents.DataSource = dt;
        //        gvStudents.DataBind();

        //        conn.Close();
        //    }
        //}

        private void LoadStudents()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string searchTerm = "%" + txtSearch.Text.Trim() + "%";
                string sortOrder = !string.IsNullOrEmpty(ddlSort.SelectedValue)
                                    ? ddlSort.SelectedValue
                                    : "UserID DESC";
                string query = @"
            SELECT UserID, FullName, Email, Username, Phone
            FROM Users
            WHERE RoleID = 3
            AND (
                FullName LIKE @search OR
                Email LIKE @search OR
                Username LIKE @search OR
                Phone LIKE @search OR
                CAST(UserID AS VARCHAR) LIKE @search
            )
            ORDER BY " + sortOrder;
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", searchTerm);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }
        private void LoadAnnouncements()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT ID, Title, MessageContent, PostDate
            FROM Announcements
            ORDER BY ID DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                gvAnnouncements.DataSource = dt;
                gvAnnouncements.DataBind();

                conn.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = GetConnection())
            { 

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                lblMsg.Text = "Full Name is required";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                !txtEmail.Text.Contains("@") ||
                !txtEmail.Text.Contains("."))
            {
                lblMsg.Text = "Invalid Email format";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblMsg.Text = "Username is required";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 3)
            {
                lblMsg.Text = "Password must be at least 6 characters";
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !txtPhone.Text.All(char.IsDigit))
            {
                lblMsg.Text = "Phone must contain only numbers";
                return;
            }
                SqlCommand checkCmd = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM Users
                    WHERE FullName=@FullName
                    AND Email=@Email
                    AND Username=@Username
                    AND Phone=@Phone
                    AND RoleID=3", conn);

                                          

                checkCmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                checkCmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());

                conn.Open();

                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists > 0 && hfUserID.Value == "")
                {
                    lblMsg.Text = "This student is already exists";
                    return;
                }

               

                if (hfUserID.Value == "")
                {
                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Users (FullName, Email, Username, Password, RoleID, Phone)
                        VALUES (@FullName, @Email, @Username, @Password, 3, @Phone)", conn);

                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());

                    cmd.ExecuteNonQuery();
                    lblMsg.Text = "Student added successfully";
                }
                else
                {
                    SqlCommand cmd = new SqlCommand(@"
                        UPDATE Users
                        SET FullName=@FullName,
                            Email=@Email,
                            Username=@Username,
                            Password=@Password,
                            Phone=@Phone
                        WHERE UserID=@UserID", conn);

                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserID", hfUserID.Value);

                    cmd.ExecuteNonQuery();
                    lblMsg.Text = "Student updated successfully";
                }

                conn.Close();
            }

            ClearForm();
            LoadStudents();
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString()))
                return;

            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE UserID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        hfUserID.Value = dt.Rows[0]["UserID"].ToString();
                        txtFullName.Text = dt.Rows[0]["FullName"].ToString();
                        txtEmail.Text = dt.Rows[0]["Email"].ToString();
                        txtUsername.Text = dt.Rows[0]["Username"].ToString();
                        txtPassword.Text = dt.Rows[0]["Password"].ToString();
                        txtPhone.Text = dt.Rows[0]["Phone"].ToString();
                    }
                }
            }
            else if (e.CommandName == "DeleteRow")
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand(@"
                        DELETE FROM Grades
                        WHERE RegID IN (
                            SELECT RegID FROM Registrations WHERE StudentID=@ID
                        )", conn);
                    cmd1.Parameters.AddWithValue("@ID", id);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("DELETE FROM Registrations WHERE StudentID=@ID", conn);
                    cmd2.Parameters.AddWithValue("@ID", id);
                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand("DELETE FROM Users WHERE UserID=@ID", conn);
                    cmd3.Parameters.AddWithValue("@ID", id);
                    cmd3.ExecuteNonQuery();

                    conn.Close();
                }

                LoadStudents();
            }
        }

        protected void gvAnnouncements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditAnnouncement")
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "SELECT * FROM Announcements WHERE ID=@ID", conn);

                    cmd.Parameters.AddWithValue("@ID", id);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        hfAnnouncementID.Value = dt.Rows[0]["ID"].ToString();

                        txtBulkMessage.Text =
                            dt.Rows[0]["MessageContent"].ToString();
                    }

                    conn.Close();
                }
            }

            else if (e.CommandName == "DeleteAnnouncement")
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Announcements WHERE ID=@ID", conn);

                    cmd.Parameters.AddWithValue("@ID", id);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }

                lblA.Text = "Announcement deleted successfully.";

                LoadAnnouncements();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e) => LoadStudents();
        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e) => LoadStudents();

        protected void btnSendBulk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBulkMessage.Text))
            {
                lblA.Text = "Please enter announcement message.";
                return;
            }

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                if (hfAnnouncementID.Value == "")
                {
                    SqlCommand cmd = new SqlCommand(@"
                INSERT INTO Announcements
                (Title, MessageContent, PostDate)
                VALUES
                (@Title, @Msg, GETDATE())", conn);

                    cmd.Parameters.AddWithValue("@Title", "Broadcast");
                    cmd.Parameters.AddWithValue("@Msg", txtBulkMessage.Text.Trim());

                    cmd.ExecuteNonQuery();

                    lblA.Text = "Announcement added successfully.";
                }
                else
                {
                    SqlCommand cmd = new SqlCommand(@"
                UPDATE Announcements
                SET MessageContent=@Msg,
                    PostDate=GETDATE()
                WHERE ID=@ID", conn);

                    cmd.Parameters.AddWithValue("@Msg", txtBulkMessage.Text.Trim());
                    cmd.Parameters.AddWithValue("@ID", hfAnnouncementID.Value);

                    cmd.ExecuteNonQuery();

                    lblA.Text = "Announcement updated successfully.";
                }

                conn.Close();
            }

            txtBulkMessage.Text = "";
            hfAnnouncementID.Value = "";

            LoadAnnouncements();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnClearAnnouncement_Click(object sender, EventArgs e)
        {
            hfAnnouncementID.Value = "";
            txtBulkMessage.Text = "";
        }

        private void ClearForm()
        {
            hfUserID.Value = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtPhone.Text = "";
            lblMsg.Text = "";
        }

        protected void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
