using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem.Student
{
    public partial class MyCourses : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";

        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckStudent();

            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void CheckStudent()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "3")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadCourses()
        {
            int studentId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        C.CourseName,
                        P.ProgramName,
                        C.CourseCode,
                        C.Credits,
                        U.FullName AS Doctor
                    FROM Registrations R
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    INNER JOIN Programs P ON C.ProgramID = P.ProgramID
                    LEFT JOIN Users U ON C.TeacherID = U.UserID
                    WHERE R.StudentID=@StudentID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();

                conn.Close();
            }
        }
    }
}