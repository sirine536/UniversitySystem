using System;

namespace UniversitySystem
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            lblName.Text = Session["FullName"].ToString();

            AdminNav.Visible = false;
            DoctorNav.Visible = false;
            StudentNav.Visible = false;

            string role = Session["RoleID"].ToString();

            if (role == "1")
                AdminNav.Visible = true;
            else if (role == "2")
                DoctorNav.Visible = true;
            else if (role == "3")
                StudentNav.Visible = true;
        }
    }
}