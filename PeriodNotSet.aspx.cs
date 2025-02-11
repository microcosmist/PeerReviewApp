

using Mysqlx.Notice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PTT
{
    public partial class ReviewWarning_WebForm : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Set correct page title
                Label pageTitle = (Label)this.Master.FindControl($"PageTitle");
                pageTitle.Text = "Come Back Later";
            }
        }
    }
}