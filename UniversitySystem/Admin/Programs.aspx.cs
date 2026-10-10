using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UniversitySystem.Admin
{
    public partial class Programs : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";

        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPrograms();
            }

            if (Session["Count"] != null)
            {
                CreateCourseFields(int.Parse(Session["Count"].ToString()));
                litCount.Text = Session["Count"].ToString();
            }
        }

        private void LoadPrograms()
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Programs ORDER BY ProgramID DESC", conn);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                gvPrograms.DataSource = dt;
                gvPrograms.DataBind();

                conn.Close();
            }
        }

        protected void btnGenerateFields_Click(object sender, EventArgs e)
        {
            int count;

            if (int.TryParse(txtCourseCount.Text, out count) && count > 0)
            {
                Session["Count"] = count;
                litCount.Text = count.ToString();

                pnlCourseInputs.Visible = true;
                CreateCourseFields(count);

                lblMsg.Text = "";
            }
            else
            {
                lblMsg.Text = "Please enter a valid number of courses.";
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void CreateCourseFields(int count)
        {
            phCourseFields.Controls.Clear();

            for (int i = 1; i <= count; i++)
            {
                phCourseFields.Controls.Add(new Literal
                {
                    Text = $"<div style='border:1px solid #ccc;padding:15px;margin-bottom:15px;background:#f9f9f9;'>"
                });

                phCourseFields.Controls.Add(new Literal { Text = $"<h4>Course #{i}</h4>" });

                phCourseFields.Controls.Add(new Literal { Text = "Course Name:" });
                phCourseFields.Controls.Add(new TextBox { ID = "txtName_" + i });

                phCourseFields.Controls.Add(new Literal { Text = "<br/>Course Code:" });
                phCourseFields.Controls.Add(new TextBox { ID = "txtCode_" + i });

                phCourseFields.Controls.Add(new Literal { Text = "<br/>Credits:" });
                phCourseFields.Controls.Add(new TextBox { ID = "txtCredits_" + i, TextMode = TextBoxMode.Number });

                phCourseFields.Controls.Add(new Literal { Text = "</div>" });
            }
        }

        protected void btnFinalSave_Click(object sender, EventArgs e)
        {
            try
            {
                string programName = txtProgramName.Text.Trim();

                if (string.IsNullOrEmpty(programName))
                {
                    lblMsg.Text = "Program Name is required.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();

                    SqlCommand checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Programs WHERE ProgramName=@Name", conn);

                    checkCmd.Parameters.AddWithValue("@Name", programName);

                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        lblMsg.Text = "Error: Program already exists!";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    SqlCommand insertProg = new SqlCommand(@"
                        INSERT INTO Programs (ProgramName)
                        VALUES (@Name);
                        SELECT SCOPE_IDENTITY();", conn);

                    insertProg.Parameters.AddWithValue("@Name", programName);

                    int newProgramID = Convert.ToInt32(insertProg.ExecuteScalar());

                    int count = int.Parse(Session["Count"].ToString());

                    for (int i = 1; i <= count; i++)
                    {
                        TextBox txtN = (TextBox)phCourseFields.FindControl("txtName_" + i);
                        TextBox txtC = (TextBox)phCourseFields.FindControl("txtCode_" + i);
                        TextBox txtCr = (TextBox)phCourseFields.FindControl("txtCredits_" + i);

                        if (txtN != null && !string.IsNullOrWhiteSpace(txtN.Text))
                        {
                            SqlCommand cmdCourse = new SqlCommand(@"
                                INSERT INTO Courses (CourseName, CourseCode, Credits, ProgramID)
                                VALUES (@Name, @Code, @Credits, @PID)", conn);

                            cmdCourse.Parameters.AddWithValue("@Name", txtN.Text.Trim());
                            cmdCourse.Parameters.AddWithValue("@Code", txtC.Text.Trim());
                            cmdCourse.Parameters.AddWithValue("@Credits", txtCr.Text.Trim());
                            cmdCourse.Parameters.AddWithValue("@PID", newProgramID);

                            cmdCourse.ExecuteNonQuery();
                        }
                    }

                    conn.Close();
                }

                lblMsg.Text = "Success! Program and courses saved.";
                lblMsg.ForeColor = System.Drawing.Color.Green;

                Session.Remove("Count");
                pnlCourseInputs.Visible = false;

                txtProgramName.Text = "";
                txtCourseCount.Text = "";

                LoadPrograms();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void gvPrograms_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int progId = Convert.ToInt32(gvPrograms.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                SqlCommand cmd1 = new SqlCommand(@"
                    DELETE FROM Grades
                    WHERE RegID IN (
                        SELECT RegID FROM Registrations
                        WHERE CourseID IN (
                            SELECT CourseID FROM Courses WHERE ProgramID=@PID
                        )
                    )", conn);
                cmd1.Parameters.AddWithValue("@PID", progId);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd2 = new SqlCommand(@"
                    DELETE FROM Registrations
                    WHERE CourseID IN (
                        SELECT CourseID FROM Courses WHERE ProgramID=@PID
                    )", conn);
                cmd2.Parameters.AddWithValue("@PID", progId);
                cmd2.ExecuteNonQuery();

                SqlCommand cmd3 = new SqlCommand(
                    "DELETE FROM Courses WHERE ProgramID=@PID", conn);
                cmd3.Parameters.AddWithValue("@PID", progId);
                cmd3.ExecuteNonQuery();

                SqlCommand cmd4 = new SqlCommand(
                    "DELETE FROM Programs WHERE ProgramID=@PID", conn);
                cmd4.Parameters.AddWithValue("@PID", progId);
                cmd4.ExecuteNonQuery();

                conn.Close();
            }

            LoadPrograms();
        }
    }
}
