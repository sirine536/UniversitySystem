using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace UniversitySystem.Admin
{
    public partial class Registration : System.Web.UI.Page
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
                LoadStudents();
                LoadPrograms();
                LoadRegistrations();
            }
        }

        private void CheckAdmin()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "1")
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT UserID, FullName FROM Users WHERE RoleID = 3", conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ddlStudent.DataSource = dt;
                ddlStudent.DataTextField = "FullName";
                ddlStudent.DataValueField = "UserID";
                ddlStudent.DataBind();

                ddlStudent.Items.Insert(0, new ListItem("-- Select Student --", ""));

                conn.Close();
            }
        }

        private void LoadPrograms()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT ProgramID, ProgramName FROM Programs", conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ddlProgram.DataSource = dt;
                ddlProgram.DataTextField = "ProgramName";
                ddlProgram.DataValueField = "ProgramID";
                ddlProgram.DataBind();

                ddlProgram.Items.Insert(0, new ListItem("-- Select Program --", "0"));

                conn.Close();
            }
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            string programId = ddlProgram.SelectedValue;

            if (!string.IsNullOrEmpty(programId) && programId != "0")
            {
                LoadCourses(programId);
            }
            else
            {
                ddlCourse.Items.Clear();
                ddlCourse.Items.Insert(0, new ListItem("-- Select Course --", ""));
            }
        }

        private void LoadCourses(string programId)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT CourseID, CourseName FROM Courses WHERE ProgramID=@PID", conn);

                cmd.Parameters.AddWithValue("@PID", programId);

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

        private void LoadRegistrations()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        R.RegID,
                        U.FullName AS StudentName,
                        C.CourseName,
                        R.RegDate
                    FROM Registrations R
                    INNER JOIN Users U ON R.StudentID = U.UserID
                    INNER JOIN Courses C ON R.CourseID = C.CourseID
                    ORDER BY R.RegID DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvRegistration.DataSource = dt;
                gvRegistration.DataBind();

                conn.Close();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (ddlStudent.SelectedValue == "" || ddlCourse.SelectedValue == "")
            {
                lblMsg.Text = "Please select student and course.";
                return;
            }

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                // CHECK EXISTING
                SqlCommand checkCmd = new SqlCommand(@"
                    SELECT COUNT(*) 
                    FROM Registrations 
                    WHERE StudentID=@SID AND CourseID=@CID", conn);

                checkCmd.Parameters.AddWithValue("@SID", ddlStudent.SelectedValue);
                checkCmd.Parameters.AddWithValue("@CID", ddlCourse.SelectedValue);

                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    lblMsg.Text = "This student is already registered in this course.";
                    return;
                }

                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Registrations (StudentID, CourseID, RegDate)
                    VALUES (@SID, @CID, GETDATE())", conn);

                cmd.Parameters.AddWithValue("@SID", ddlStudent.SelectedValue);
                cmd.Parameters.AddWithValue("@CID", ddlCourse.SelectedValue);

                cmd.ExecuteNonQuery();

                conn.Close();
            }

            lblMsg.Text = "Student registered successfully.";
            LoadRegistrations();
        }

        protected void gvRegistration_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteRow")
            {
                int regId = Convert.ToInt32(e.CommandArgument);

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand cmd1 = new SqlCommand(
                        "DELETE FROM Grades WHERE RegID=@RID", conn);
                    cmd1.Parameters.AddWithValue("@RID", regId);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand(
                        "DELETE FROM Registrations WHERE RegID=@RID", conn);
                    cmd2.Parameters.AddWithValue("@RID", regId);
                    cmd2.ExecuteNonQuery();

                    conn.Close();
                }

                LoadRegistrations();
            }
        }
    }
}