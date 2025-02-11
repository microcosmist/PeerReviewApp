using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


// Page Credit: Samantha Ekanem

namespace PTT
{
    public partial class FormSubmissionMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            // Check if the user is logged in
            if (Session["UTD_ID"] == null)
            {
                // User is not logged in, redirect to login page
                Response.Redirect("Login Page.aspx");
            }

            if (!IsPostBack) 
            {
                //Set correct page title 
                Label pageTitle = (Label)this.Master.FindControl($"PageTitle");
                pageTitle.Text = "Submission Confirmation";
            }
        }

    }
}