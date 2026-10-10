using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem.Doctor
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";
        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        private int doctorId;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckDoctor();

            doctorId = Convert.ToInt32(Session["UserID"]);

            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        private void CheckDoctor()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "2")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadDashboard()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd1 = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM Courses 
                    WHERE TeacherID=@DoctorID", conn);

                cmd1.Parameters.AddWithValue("@DoctorID", doctorId);
                lblCourses.Text = cmd1.ExecuteScalar().ToString();

                SqlCommand cmd2 = new SqlCommand(@"
                    SELECT COUNT(DISTINCT R.StudentID)
                    FROM Registrations R
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    WHERE C.TeacherID=@DoctorID", conn);

                cmd2.Parameters.AddWithValue("@DoctorID", doctorId);
                lblStudents.Text = cmd2.ExecuteScalar().ToString();

                SqlCommand cmd3 = new SqlCommand(@"
                    SELECT COUNT(*)
                    FROM Grades G
                    INNER JOIN Registrations R ON G.RegID = R.RegID
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    WHERE C.TeacherID=@DoctorID", conn);

                cmd3.Parameters.AddWithValue("@DoctorID", doctorId);
                lblGrades.Text = cmd3.ExecuteScalar().ToString();

                SqlCommand cmd4 = new SqlCommand(@"
                    SELECT CourseName, CourseCode, Credits
                    FROM Courses
                    WHERE TeacherID=@DoctorID", conn);

                cmd4.Parameters.AddWithValue("@DoctorID", doctorId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd4);
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();

                conn.Close();
            }
        }

        protected void gvCourses_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}