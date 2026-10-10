using System;
using System.Data;
using System.Data.SqlClient;

namespace UniversitySystem
{
    public partial class Login : System.Web.UI.Page
    {
        private string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UniversityDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True";
        private SqlConnection GetConnection()
        {
            return new SqlConnection(cs);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT UserID, FullName, RoleID 
                    FROM Users 
                    WHERE Username=@Username AND Password=@Password";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                    Session["FullName"] = dt.Rows[0]["FullName"].ToString();
                    Session["RoleID"] = dt.Rows[0]["RoleID"].ToString();

                    string role = Session["RoleID"].ToString();

                    if (role == "1")
                        Response.Redirect("~/Admin/Dashboard.aspx");
                    else if (role == "2")
                        Response.Redirect("~/Doctor/Dashboard.aspx");
                    else if (role == "3")
                        Response.Redirect("~/Student/Dashboard.aspx");
                }
                else
                {
                    lblMsg.Text = "Invalid username or password";
                }
            }
        }

        protected void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}