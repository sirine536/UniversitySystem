using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem.Student
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";

        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        int studentId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "3")
            {
                Response.Redirect("~/Login.aspx");
            }

            studentId = Convert.ToInt32(Session["UserID"]);

            if (!IsPostBack)
            {
                LoadDashboard();
            }
        }

        private void LoadDashboard()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmdCourses = new SqlCommand(@"
                        SELECT COUNT(*) 
                        FROM Registrations 
                        WHERE StudentID=@ID", conn);

                    cmdCourses.Parameters.AddWithValue("@ID", studentId);
                    lblCourses.Text = cmdCourses.ExecuteScalar().ToString();

                    SqlCommand cmdCompleted = new SqlCommand(@"
                        SELECT COUNT(*) 
                        FROM Grades G
                        INNER JOIN Registrations R ON G.RegID = R.RegID
                        WHERE R.StudentID=@ID", conn);

                    cmdCompleted.Parameters.AddWithValue("@ID", studentId);
                    lblCompleted.Text = cmdCompleted.ExecuteScalar().ToString();

                    SqlCommand cmdAnn = new SqlCommand(@"
                        SELECT TOP 5 MessageContent, PostDate 
                        FROM Announcements 
                        ORDER BY PostDate DESC", conn);

                    DataTable dtAnn = new DataTable();
                    SqlDataAdapter daAnn = new SqlDataAdapter(cmdAnn);
                    daAnn.Fill(dtAnn);

                    gvAnnouncements.DataSource = dtAnn;
                    gvAnnouncements.DataBind();

                    SqlCommand cmdGrid = new SqlCommand(@"
                        SELECT C.CourseName, C.CourseCode, C.Credits
                        FROM Courses C
                        INNER JOIN Registrations R ON R.CourseID = C.CourseID
                        WHERE R.StudentID=@ID", conn);

                    cmdGrid.Parameters.AddWithValue("@ID", studentId);

                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmdGrid);
                    da.Fill(dt);

                    gvCourses.DataSource = dt;
                    gvCourses.DataBind();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }
    }
}