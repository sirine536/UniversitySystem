using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UniversitySystem.Doctor
{
    public partial class EnterGrades : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";

        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        int doctorId;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckDoctor();

            doctorId = Convert.ToInt32(Session["UserID"]);

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
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
                    SELECT CourseID, CourseName 
                    FROM Courses 
                    WHERE TeacherID=@DoctorID", conn);

                cmd.Parameters.AddWithValue("@DoctorID", doctorId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ddlCourse.DataSource = dt;
                ddlCourse.DataTextField = "CourseName";
                ddlCourse.DataValueField = "CourseID";
                ddlCourse.DataBind();

                ddlCourse.Items.Insert(0, new ListItem("-- Select Course --", ""));

                conn.Close();
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentsGrades();
        }

        private void LoadStudentsGrades()
        {
            if (ddlCourse.SelectedValue == "")
            {
                gvGrades.DataSource = null;
                gvGrades.DataBind();
                return;
            }

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        R.RegID,
                        U.FullName AS StudentName,
                        ISNULL(G.Midterm, 0) AS Midterm,
                        ISNULL(G.Final, 0) AS Final,
                        ISNULL(G.Total, 0) AS Total,
                        ISNULL(G.LetterGrade, '-') AS LetterGrade
                    FROM Registrations R
                    INNER JOIN Users U ON R.StudentID = U.UserID
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    LEFT JOIN Grades G ON R.RegID = G.RegID
                    WHERE R.CourseID=@CourseID
                    AND C.TeacherID=@DoctorID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", ddlCourse.SelectedValue);
                cmd.Parameters.AddWithValue("@DoctorID", doctorId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvGrades.DataSource = dt;
                gvGrades.DataBind();

                conn.Close();
            }
        }

        protected void gvGrades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SaveGrade")
            {
                int regId = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = ((Button)e.CommandSource).NamingContainer as GridViewRow;

                TextBox txtMidterm = row.FindControl("txtMidterm") as TextBox;
                TextBox txtFinal = row.FindControl("txtFinal") as TextBox;

                double midterm = 0;
                double final = 0;

                double.TryParse(txtMidterm.Text.Trim(), out midterm);
                double.TryParse(txtFinal.Text.Trim(), out final);

                if (midterm < 0 || midterm > 100)
                {
                    lblMsg.Text = "Midterm grade must be between 0 and 100.";
                    return;
                }

                if (final < 0 || final > 100)
                {
                    lblMsg.Text = "Final grade must be between 0 and 100.";
                    return;
                }


                int total = (int)Math.Round((0.30 * midterm) + (0.70 * final));
                string letter = GetLetterGrade(total);

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Grades WHERE RegID=@RegID", conn);

                    checkCmd.Parameters.AddWithValue("@RegID", regId);

                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        SqlCommand updateCmd = new SqlCommand(@"
                            UPDATE Grades SET 
                                Midterm=@Midterm,
                                Final=@Final,
                                Total=@Total,
                                LetterGrade=@LetterGrade
                            WHERE RegID=@RegID", conn);

                        updateCmd.Parameters.AddWithValue("@Midterm", midterm);
                        updateCmd.Parameters.AddWithValue("@Final", final);
                        updateCmd.Parameters.AddWithValue("@Total", total);
                        updateCmd.Parameters.AddWithValue("@LetterGrade", letter);
                        updateCmd.Parameters.AddWithValue("@RegID", regId);

                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand insertCmd = new SqlCommand(@"
                            INSERT INTO Grades (RegID, Midterm, Final, Total, LetterGrade)
                            VALUES (@RegID, @Midterm, @Final, @Total, @LetterGrade)", conn);

                        insertCmd.Parameters.AddWithValue("@RegID", regId);
                        insertCmd.Parameters.AddWithValue("@Midterm", midterm);
                        insertCmd.Parameters.AddWithValue("@Final", final);
                        insertCmd.Parameters.AddWithValue("@Total", total);
                        insertCmd.Parameters.AddWithValue("@LetterGrade", letter);

                        insertCmd.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                lblMsg.Text = "Grade saved successfully.";
                LoadStudentsGrades();
            }
        }

        private string GetLetterGrade(int total)
        {
            if (total >= 90) return "A";
            else if (total >= 80) return "B+";
            else if (total >= 70) return "B";
            else if (total >= 60) return "C";
            else if (total >= 50) return "D";
            else return "F";
        }
    }
}