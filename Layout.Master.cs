

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PTT
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //Checks if the user is logged in, redirects to login page if not
            if (Session["UTD_ID"] == null)
            {
                Response.Redirect("Login Page.aspx");
            }
            
            //Changes the name on the nav sidebar to the user that is logged in
            StudentName.Text = "Hello, " + Session["Fname"].ToString(); 
        }
    }
}