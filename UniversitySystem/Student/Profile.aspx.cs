using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem.Student
{
    public partial class Profile : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";
        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        int studentId;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckStudent();

            studentId = Convert.ToInt32(Session["UserID"]);

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void CheckStudent()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "3")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadProfile()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT * 
                    FROM Users 
                    WHERE UserID=@UserID", conn);

                cmd.Parameters.AddWithValue("@UserID", studentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    txtFullName.Text = dt.Rows[0]["FullName"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();
                    txtPhone.Text = dt.Rows[0]["Phone"].ToString();
                    txtDepartment.Text = dt.Rows[0]["Department"].ToString();
                    txtPassword.Text = dt.Rows[0]["Password"].ToString();
                }

                conn.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Users SET
                        Email=@Email,
                        Phone=@Phone,
                        Password=@Password
                    WHERE UserID=@UserID", conn);

                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                cmd.Parameters.AddWithValue("@UserID", studentId);

                cmd.ExecuteNonQuery();

                conn.Close();
            }

            lblMsg.Text = "Profile updated successfully.";
        }
    }
}