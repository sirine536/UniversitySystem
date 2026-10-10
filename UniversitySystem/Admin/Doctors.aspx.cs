using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

namespace UniversitySystem.Admin
{
    public partial class Doctors : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";

        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckAdmin();

            if (!IsPostBack)
                LoadDoctors();
        }

        private void CheckAdmin()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "1")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadDoctors()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT UserID, FullName, Email, Username, Phone, Departement
                    FROM Users
                    WHERE RoleID = 2
                    ORDER BY UserID DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvDoctors.DataSource = dt;
                gvDoctors.DataBind();

                conn.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtFullName.Text))
                    {
                        Response.Write("<script>alert('Please enter Full Name')</script>");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                        !txtEmail.Text.Contains("@") ||
                        !txtEmail.Text.Contains(".") ||
                        txtEmail.Text.IndexOf("@") > txtEmail.Text.LastIndexOf("."))
                    {
                        Response.Write("<script>alert('Please enter a valid Email')</script>");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtUsername.Text))
                    {
                        Response.Write("<script>alert('Please enter Username')</script>");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 3 )
                    {
                        Response.Write("<script>alert('Password must be at least 4 characters')</script>");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtPhone.Text) ||
                        txtPhone.Text.Length < 6 ||
                        !txtPhone.Text.All(char.IsDigit))
                    {
                        Response.Write("<script>alert('Phone number must contain only numbers and be at least 6 digits')</script>");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txtDepartement.Text))
                    {
                        Response.Write("<script>alert('Please enter Departement')</script>");
                        return;
                    }

                    SqlCommand checkCmd = new SqlCommand(@"
                        SELECT COUNT(*) 
                        FROM Users
                        WHERE FullName=@FullName
                        AND Email=@Email
                        AND Username=@Username
                        AND Phone=@Phone
                        AND RoleID = 2", conn);

                    checkCmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                    checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    checkCmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    checkCmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());

                    conn.Open();
                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (hfUserID.Value == "" && exists > 0)
                    {
                        lblMsg.Text = "This doctor is already exists.";
                        return;
                    }


                    if (hfUserID.Value == "")
                    {
                        SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Users
                        (FullName, Email, Username, Password, RoleID, Phone, Departement)
                        VALUES
                        (@FullName, @Email, @Username, @Password, 2, @Phone, @Departement)", conn);

                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Departement", txtDepartement.Text.Trim());

                        cmd.ExecuteNonQuery();

                        lblMsg.Text = "Doctor added successfully.";
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand(@"
                        UPDATE Users
                        SET FullName=@FullName,
                            Email=@Email,
                            Username=@Username,
                            Password=@Password,
                            Phone=@Phone,
                            Departement=@Departement
                        WHERE UserID=@UserID", conn);

                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Departement", txtDepartement.Text.Trim());
                        cmd.Parameters.AddWithValue("@UserID", hfUserID.Value);

                        cmd.ExecuteNonQuery();

                        lblMsg.Text = "Doctor updated successfully.";
                    }

                    conn.Close();


                    ClearForm();
                    LoadDoctors();

                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "')</script>");
                }
            }
        }

        protected void gvDoctors_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null || string.IsNullOrEmpty(e.CommandArgument.ToString()))
                return;

            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRow")
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE UserID=@UserID", conn);
                    cmd.Parameters.AddWithValue("@UserID", id);

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
                        txtDepartement.Text = dt.Rows[0]["Departement"].ToString();
                    }

                    conn.Close();
                }
            }

            else if (e.CommandName == "DeleteRow")
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand(
                        "UPDATE Courses SET TeacherID=NULL WHERE TeacherID=@UserID", conn);
                    cmd1.Parameters.AddWithValue("@UserID", id);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand(
                        "DELETE FROM Users WHERE UserID=@UserID", conn);
                    cmd2.Parameters.AddWithValue("@UserID", id);
                    cmd2.ExecuteNonQuery();

                    conn.Close();
                }

                LoadDoctors();
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hfUserID.Value = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtPhone.Text = "";
            txtDepartement.Text = "";
            lblMsg.Text = "";
        }
    }
}