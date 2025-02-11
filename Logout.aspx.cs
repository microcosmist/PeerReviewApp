

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PTT
{
    public partial class Logout_WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //destroy the session and all the saved data
            Session.Abandon();

            //redirect to login page
            Response.Redirect("Login Page.aspx");
        }
    }
}