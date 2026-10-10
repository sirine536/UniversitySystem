using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace UniversitySystem.Admin
{
    public partial class Courses : System.Web.UI.Page
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
                LoadPrograms();
                LoadDoctors();
            }
        }

        private void CheckAdmin()
        {
            if (Session["UserID"] == null || Session["RoleID"] == null || Session["RoleID"].ToString() != "1")
            {
                Response.Redirect("~/Login.aspx");
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

        private void LoadDoctors()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT UserID, FullName FROM Users WHERE RoleID = 2", conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                ddlDoctor.DataSource = dt;
                ddlDoctor.DataTextField = "FullName";
                ddlDoctor.DataValueField = "UserID";
                ddlDoctor.DataBind();

                ddlDoctor.Items.Insert(0, new ListItem("-- Select Doctor --", ""));

                conn.Close();
            }
        }

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
        {
            string programId = ddlProgram.SelectedValue;

            if (programId != "0")
            {
                LoadCourses(programId);
                LoadGridByProgram(programId);
            }
            else
            {
                ddlCourse.Items.Clear();
                gvCourses.DataSource = null;
                gvCourses.DataBind();
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

                ddlCourse.Items.Insert(0, new ListItem("-- Select Course --", "0"));

                conn.Close();
            }
        }

        private void LoadGridByProgram(string programId)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT C.CourseID, C.CourseName, C.CourseCode, C.Credits,
                           ISNULL(U.FullName, 'Not Assigned') AS DoctorName
                    FROM Courses C
                    LEFT JOIN Users U ON C.TeacherID = U.UserID
                    WHERE C.ProgramID = @PID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PID", programId);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();

                conn.Close();
            }
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            if (ddlCourse.SelectedValue == "0" || ddlDoctor.SelectedValue == "")
            {
                lblMsg.Text = "Please select both a Course and a Doctor.";
                return;
            }

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand checkCourse = new SqlCommand(@"
                        SELECT TeacherID 
                        FROM Courses 
                        WHERE CourseID = @CID", conn);

                checkCourse.Parameters.AddWithValue("@CID", ddlCourse.SelectedValue);

                object currentTeacher = checkCourse.ExecuteScalar();



                if (currentTeacher != DBNull.Value && currentTeacher != null)
                {
                    string existingTeacher = currentTeacher.ToString();

                    if (existingTeacher != ddlDoctor.SelectedValue)
                    {
                        lblMsg.Text = "This course is already assigned to another doctor.";
                        return;
                    }
                }



                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Courses
                    SET TeacherID = @TID
                    WHERE CourseID = @CID AND TeacherID IS NULL", conn);

                cmd.Parameters.AddWithValue("@TID", ddlDoctor.SelectedValue);
                cmd.Parameters.AddWithValue("@CID", ddlCourse.SelectedValue);

                int rows = cmd.ExecuteNonQuery();

                conn.Close();

                if (rows > 0)
                    lblMsg.Text = "Doctor assigned successfully!";
                else
                    lblMsg.Text = "Assignment failed. Course already has a doctor.";
            }
        }


         

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlProgram.SelectedIndex = 0;
            ddlCourse.Items.Clear();
            ddlDoctor.SelectedIndex = 0;

            gvCourses.DataSource = null;
            gvCourses.DataBind();

            lblMsg.Text = "";
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}