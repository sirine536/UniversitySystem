using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace UniversitySystem.Admin
{
    public partial class Grades : System.Web.UI.Page
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
                LoadGrades();
            }
        }

        private void CheckAdmin()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "1")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadGrades()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        U.FullName AS Student,
                        C.CourseName AS Course,
                        G.Midterm,
                        G.Final,
                        G.Total,
                        G.LetterGrade
                    FROM Grades G
                    INNER JOIN Registrations R ON G.RegID = R.RegID
                    INNER JOIN Users U ON R.StudentID = U.UserID
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    ORDER BY U.FullName";

                SqlCommand cmd = new SqlCommand(query, conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvGrades.DataSource = dt;
                gvGrades.DataBind();

                conn.Close();
            }
        }
    }
}