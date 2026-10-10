using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using static System.Net.Mime.MediaTypeNames;

namespace UniversitySystem.Admin
{
    public partial class Dashboard : System.Web.UI.Page
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
            {
                LoadDashboard();
            }
        }

        private void CheckAdmin()
        {
            if (Session["UserID"] == null || Session["RoleID"].ToString() != "1")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadDashboard()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmdStudents = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE RoleID = 3", conn);

                lblStudents.Text = cmdStudents.ExecuteScalar().ToString();

                SqlCommand cmdDoctors = new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE RoleID = 2", conn);

                lblDoctors.Text = cmdDoctors.ExecuteScalar().ToString();

                SqlCommand cmdCourses = new SqlCommand(
                    "SELECT COUNT(*) FROM Courses", conn);

                lblCourses.Text = cmdCourses.ExecuteScalar().ToString();

                SqlCommand cmdRegistrations = new SqlCommand(
                    "SELECT COUNT(*) FROM Registrations", conn);

                lblRegistrations.Text = cmdRegistrations.ExecuteScalar().ToString();

                string query = @"
                SELECT TOP 5
                    U.FullName AS Student,
                    C.CourseName AS Course,
                    CONVERT(VARCHAR, R.RegDate, 120) AS Date
                FROM Registrations R
                INNER JOIN Users U ON R.StudentID = U.UserID
                INNER JOIN Courses C ON R.CourseID = C.CourseID
                ORDER BY R.RegID DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                DataTable dt = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                gvRecent.DataSource = dt;
                gvRecent.DataBind();

                conn.Close();
            }
        }
    }
}