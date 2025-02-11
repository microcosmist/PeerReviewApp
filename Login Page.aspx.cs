

using MySql.Data.MySqlClient; 
using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.WebControls;
using static PTT.Login_Page;


namespace PTT
{
    public partial class Login_Page : System.Web.UI.Page
    {
        //saves the database connection to variable
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //function that authenticates login credentials against the database
        protected void Login_Authentication(object sender, AuthenticateEventArgs e)
        {
            //store user input
            string utdId = StudentLogin.UserName;
            string password = StudentLogin.Password;

            //use connection to validate input
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                string queryString = "SELECT first_name, last_name, team_id FROM Student WHERE username = @UtdId AND password = @Password";


                //pass query to command object
                using (MySqlCommand command = new MySqlCommand(queryString, connection))
                {

                    //use parameters to store data to prevent sql injection attacks
                    command.Parameters.AddWithValue("@UtdId", utdId);
                    command.Parameters.AddWithValue("@Password", password);

                    //opens connection to database
                    connection.Open();

                    //run the query against the database and retrieve data
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            // store first name and last name and team id to variables
                            string fname = reader["first_name"].ToString();
                            string lname = reader["last_name"].ToString();
                            int teamId = Convert.ToInt32(reader["team_id"]);


                            // Store important user data for the current session
                            Session["UTD_ID"] = utdId; 
                            Session["Team_ID"] = teamId; 
                            Session["Fname"] = fname; 
                            Session["Lname"] = lname;


                            // Authentication was successful 
                            e.Authenticated = true;

                            // Redirect to calendar page upon successful login
                            Response.Redirect("Peer_Review.aspx"); 
                        }
                        else
                        {
                            // Authentication failed, shows error message
                            e.Authenticated = false; 
                        }
                    }
                }
            }
        }
    }
}