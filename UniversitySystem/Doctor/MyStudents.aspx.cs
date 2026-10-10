using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem.Doctor
{
    public partial class MyStudents : System.Web.UI.Page
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
                LoadStudents();
            }
        }

        private void CheckDoctor()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "2")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadStudents()
        {
            int doctorId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        U.UserID AS StudentID,
                        U.FullName AS StudentName,
                        U.Email,
                        U.Phone,
                        C.CourseName
                    FROM Registrations R
                    INNER JOIN Users U ON R.StudentID = U.UserID
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    WHERE C.TeacherID=@DoctorID
                    ORDER BY C.CourseName, U.FullName";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvStudents.DataSource = dt;
                gvStudents.DataBind();

                conn.Close();
            }
        }

        protected void gvStudents_SelectedIndexChanged()
        {

        }
    }
}