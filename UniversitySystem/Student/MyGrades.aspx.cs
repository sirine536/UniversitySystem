using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace UniversitySystem.Student
{
    public partial class MyGrades : System.Web.UI.Page
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
                LoadPrograms();
                LoadGrades();
            }
        }

        private void CheckStudent()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "3")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadPrograms()
        {
            int studentId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT DISTINCT P.ProgramID, P.ProgramName
                    FROM Programs P
                    INNER JOIN Courses C ON P.ProgramID = C.ProgramID
                    INNER JOIN Registrations R ON C.CourseID = R.CourseID
                    WHERE R.StudentID=@StudentID", conn);

                cmd.Parameters.AddWithValue("@StudentID", studentId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ddlPrograms.DataSource = dt;
                ddlPrograms.DataTextField = "ProgramName";
                ddlPrograms.DataValueField = "ProgramID";
                ddlPrograms.DataBind();

                ddlPrograms.Items.Insert(0, new ListItem("Choose Program...", "0"));

                conn.Close();
            }
        }

        protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrades();
        }

        private void LoadGrades()
        {
            int studentId = Convert.ToInt32(Session["UserID"]);
            int programId = Convert.ToInt32(ddlPrograms.SelectedValue);

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        P.ProgramName,
                        C.CourseName,
                        C.Credits,
                        G.Midterm,
                        G.Final,
                        G.Total,
                        G.LetterGrade
                    FROM Grades G
                    INNER JOIN Registrations R ON G.RegID = R.RegID
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    INNER JOIN Programs P ON C.ProgramID = P.ProgramID
                    WHERE R.StudentID=@StudentID";

                if (programId > 0)
                {
                    query += " AND P.ProgramID=@ProgramID";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                if (programId > 0)
                {
                    cmd.Parameters.AddWithValue("@ProgramID", programId);
                }

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvGrades.DataSource = dt;
                gvGrades.DataBind();

                conn.Close();
            }

            if (programId > 0)
            {
                CalculateGPA(studentId, programId);
            }
            else
            {
                lblGPA.Text = "";
            }
        }

        private void CalculateGPA(int studentId, int programId)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT 
                        SUM(CAST(G.Total AS FLOAT) * C.Credits) AS TotalPoints,
                        SUM(C.Credits) AS TotalCredits
                    FROM Grades G
                    INNER JOIN Registrations R ON G.RegID = R.RegID
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    WHERE R.StudentID=@StudentID AND C.ProgramID=@ProgramID", conn);

                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@ProgramID", programId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0 && dt.Rows[0]["TotalCredits"] != DBNull.Value)
                {
                    double totalPoints = Convert.ToDouble(dt.Rows[0]["TotalPoints"]);
                    int totalCredits = Convert.ToInt32(dt.Rows[0]["TotalCredits"]);

                    if (totalCredits > 0)
                    {
                        double gpa = totalPoints / totalCredits;

                        if (gpa > 100) gpa = 100;

                        lblGPA.Text = "Your Program GPA: " + gpa.ToString("0.00") + " %";
                    }
                    else
                    {
                        lblGPA.Text = "GPA: N/A (No credits assigned)";
                    }
                }
                else
                {
                    lblGPA.Text = "GPA: No grades recorded yet.";
                }

                conn.Close();
            }
        }
    }
}