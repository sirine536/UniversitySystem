using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem.Doctor
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
            CheckDoctor();

            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void CheckDoctor()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "2")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadCourses()
        {
            int doctorId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        CourseID,
                        CourseName,
                        CourseCode,
                        Credits
                    FROM Courses
                    WHERE TeacherID=@DoctorID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);

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