

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.X509;
using static PTT.Review_WebForm;


namespace PTT
{
    public partial class Review_WebForm : System.Web.UI.Page
    {

        //store database connection 
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Set correct page title
                Label pageTitle = (Label)this.Master.FindControl($"PageTitle");
                pageTitle.Text = "Peer Review Form";
            }

            // check if the user is logged in, if not redirect to login page
            if (Session["UTD_ID"] == null)
            {
                Response.Redirect("Login Page.aspx");
            }

            //store user id 
            string utd_id = Session["UTD_ID"].ToString();


            //check if there is a review period active, redirect to message page if not
            if (IsReviewPeriodActivated() == false)
            {
                Response.Redirect("PeriodNotSet.aspx");
            }


            //call function that checks if the student has submitted the form already, redirect to message page if they have
            if (HasSubmittedPeerReview(utd_id) == true)
            {
                Response.Redirect("FormSubmissionMessage.aspx");
            }

            //calls a function that dynamically creates the table
            CreateTable();

        }


        //function to check if the student has already submitted a review for the current review period
        private bool HasSubmittedPeerReview(string reviewerId)
        {
            //by default, submission is false
            bool hasSubmitted = false;

            //string to hold the correct attribute name based on the period number
            //works based on the assumption that we will only ever have two review periods
            string review_number = "";

            int period_number = GetPeriodNumber();

            if (period_number == 1)
            {
                review_number = "review_1_submitted";
            }
            else if (period_number == 2)
            {
                review_number = "review_2_submitted";
            }


            //check the database to see if the review has been submitted by using the correct attribute name

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT " + review_number + " FROM student WHERE id = @reviewerId;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReviewerId", reviewerId);
                    connection.Open();

                    //convert bit value from attribute to boolean result
                    hasSubmitted = Convert.ToBoolean(command.ExecuteScalar());
                }
            }
            return hasSubmitted;
        }




        //check the database to see if there is a available review period
        private bool IsReviewPeriodActivated()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT start_date, end_date FROM review_period";


                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        //reads all rows in the table
                        while (reader.Read()) 
                        {

                            //variables to hold the start and end date for the current row
                            DateTime startDate = reader.GetDateTime(0); 
                            DateTime endDate = reader.GetDateTime(1);

                            //check if todays date is between the start and end date of the current row
                            if (DateTime.Today >= startDate && DateTime.Today <= endDate)
                            {
                                //debugging to check if logic works
                                System.Diagnostics.Debug.WriteLine($"Review period is active");

                                //return true if active period found
                                return true;
                            }
                        }
                    }
                }
            }

            //debugging to check if logic works
            System.Diagnostics.Debug.WriteLine($"Review period is not active");

            //if the current date is not within any of the dates of each review period return false
            return false;
        }



        //Dynamically creates table with dropdown boxes for ratings and a comment box
        private void CreateTable()
        {

            //get team id 
            int teamId = (int)Session["Team_ID"];

            //create and retrieve the list of team members
            List<Member> teamMembers = GetTeamMembers(teamId);


            // create a row in the table for each team member
            for (int i = 0; i < teamMembers.Count; i++)
            {
                //create new row 
                TableRow row = new TableRow();

               // create cell that will hold team member name
                TableCell memberCell = new TableCell { Text = $"{teamMembers[i].Fname} {teamMembers[i].Lname}", HorizontalAlign = HorizontalAlign.Center};

                //add member cell to current row
                row.Cells.Add(memberCell);
                

                // cells that will hold dropdowns for ratings
                for (int j = 0; j < 6; j++)
                {

                    //create new cell
                    TableCell cell = new TableCell { HorizontalAlign = HorizontalAlign.Center };

                    //if we are on the criteria columns add dropdown box to the cell
                    if (j < 5)
                    {
                        //create dropdown box
                        DropDownList dropdown = new DropDownList { CssClass = "ratings", ID = $"member{i}_rating{j}" };

                        //debugging to see how each rating is assigned an id
                        System.Diagnostics.Debug.WriteLine($"dropdown created for: member{i} rating{j}");


                        //add 0-5 values in the dropdown box
                        for (int k = 0; k <= 5; k++)
                        {

                            //sets the text and value from k num
                            dropdown.Items.Add(new ListItem(k.ToString(), k.ToString()));
                        }

                        //add dropdown to cell
                        cell.Controls.Add(dropdown);

                    }

                    // else if we are on the last column (the comments column) add a text box
                    else  
                    {

                       TextBox CommentsTextBox = new TextBox { CssClass = "commentBox", ID = $"member{i}_comment", TextMode=TextBoxMode.MultiLine };
                       cell.Controls.Add(CommentsTextBox);

                    }
                    //add cell to row
                    row.Cells.Add(cell);
                }
                //add row to table
                ReviewTable.Rows.Add(row);
            }

            //Create a single row for the submit button
            TableRow submitRow = new TableRow();

            //create cell for the button
            TableCell submitCell = new TableCell { ColumnSpan = 7, HorizontalAlign = HorizontalAlign.Center };

            //create button
            Button submitButton = new Button { Text = "Submit", CssClass = "submit-button" };

            // add event listener for button 
            submitButton.Click += SubmitButtonClick;

            //add submit button to cell
            submitCell.Controls.Add(submitButton);

            //add cell to submit row
            submitRow.Cells.Add(submitCell);

            //add row to table
            ReviewTable.Rows.Add(submitRow);
        }



        //Creates a list of member objects that will hold the first name, last name, and utd id of each student within a team
        //This list is necessary for:
        //Saving the id of the student that is being reviewed and pushing that info to the database.
        //Setting the first and last name of each team member within our dynamically created table.
        private List<Member> GetTeamMembers(int teamId)
        {

            //creates a list that will hold a member object
            List<Member> teamMembers = new List<Member>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                string query = "SELECT first_name, last_name, username FROM Student WHERE team_id = @TeamId";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeamId", teamId);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        //creates a new instance of the Member class for every student in the team
                        while (reader.Read())
                        {
                            teamMembers.Add(new Member
                            {
                                Fname = reader["first_name"].ToString(),
                                Lname = reader["last_name"].ToString(),
                                UTD_ID = reader["username"].ToString()
                            });
                        }
                    }
                }
            }
            return teamMembers;
        }


        //Member class that will store the the full name and utd_id of each student in a team
        public class Member
        {
            public string Fname { get; set; }
            public string Lname { get; set; }
            public string UTD_ID { get; set; }
        }



        //Pushes data to the database when the submit button is clicked
        protected void SubmitButtonClick(object sender, EventArgs e)
        {

            //debugging
            System.Diagnostics.Debug.WriteLine("Submit button clicked");

            // retrieve reviewer info from session
            string reviewer_name = Session["Fname"].ToString() + " " + Session["Lname"].ToString();
            string reviewer_id = Session["UTD_ID"].ToString();

            // get team info
            int team_id = (int)Session["Team_ID"];
            List<Member> teamMembers = GetTeamMembers(team_id);

            //get the review period
            int period_number = GetPeriodNumber();
            

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //for each member in the list of team members collect the ratings for each criteria and the comment
                foreach (var member in teamMembers)
                {
                    //for the current member set reviewee details
                    string reviewee_name = $"{member.Fname} {member.Lname}";
                    string reviewee_id = member.UTD_ID;

                    // initialize ratings and comment variables
                    int quality = 0;
                    int timeliness = 0;
                    int teamwork = 0;
                    int eff_and_part = 0;
                    int communication = 0;
                    string comment = "";

                    // Record each rating and the comment
                    for (int i = 0; i < 6; i++)
                    {

                        // if we are saving dropdown values
                        if (i < 5)
                        {

                            //each dropdown was assigned an id when it was created, find corresponding dropdown through id
                            //retrieve member index from the member list 

                            DropDownList dropdown = (DropDownList)ReviewTable.FindControl($"member{teamMembers.IndexOf(member)}_rating{i}");



                            //debugging for finding dropdown data
                            System.Diagnostics.Debug.WriteLine($"member i: {teamMembers.IndexOf(member)}");
                            System.Diagnostics.Debug.WriteLine($"rating j: {i}");

                            if (dropdown == null)
                            {
                                System.Diagnostics.Debug.WriteLine($"Dropdown not found for member {teamMembers.IndexOf(member)} and rating {i}");
                            }



                            //if we successfully find dropdown data
                            if (dropdown != null)
                            {
                                //store selected dropdown value
                                int rating = int.Parse(dropdown.SelectedValue);

                                //debugging to see if we got the correct selected value
                                System.Diagnostics.Debug.WriteLine($"selected value: {rating}");

                                //sets the dropdown value to the correct variable based on the current column
                                switch (i)
                                {
                                    case 0: quality = rating; break;
                                    case 1: timeliness = rating; break;
                                    case 2: teamwork = rating; break;
                                    case 3: eff_and_part = rating; break;
                                    case 4: communication = rating; break;
                                }
                            }
                        }

                        // else if we are saving comment input from the comment column
                        else
                        {
                            //use same method from dropdown to retrieve the id of the comment 
                            TextBox CommentsTextBox = (TextBox)ReviewTable.FindControl($"member{teamMembers.IndexOf(member)}_comment");

                            //debugging to see the comment id
                            System.Diagnostics.Debug.WriteLine($"member{teamMembers.IndexOf(member)}_comment");


                            //if the text box contains input, save it to our variable
                            if (CommentsTextBox != null)
                            {
                                comment = CommentsTextBox.Text.Trim();
                            }

                            // else do nothing, uses default value
                        }
                    }

                    // Insert review into the database
                    //uses old version of table with reviewee/er names for debugging purposes
                    string query = "INSERT INTO peer_review (review_period_id, reviewer_id,  reviewee_id, qual_of_work_rating, timeliness_rating, teamwork_rating, eff_and_part_rating, communication_rating, comment) " +
                                   "VALUES (@PeriodNumber, @ReviewerId,  @RevieweeId, @Quality, @Timeliness, @Teamwork, @EffAndPart, @Communication, @Comment)";

                 

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PeriodNumber", period_number);
                        command.Parameters.AddWithValue("@ReviewerId", reviewer_id);
                        command.Parameters.AddWithValue("@RevieweeId", reviewee_id);
                        command.Parameters.AddWithValue("@Quality", quality);
                        command.Parameters.AddWithValue("@Timeliness", timeliness);
                        command.Parameters.AddWithValue("@Teamwork", teamwork);
                        command.Parameters.AddWithValue("@EffAndPart", eff_and_part);
                        command.Parameters.AddWithValue("@Communication", communication);
                        command.Parameters.AddWithValue("@Comment", comment);

                        command.ExecuteNonQuery();
                    }
                }
            }

            //After pushing data to database, call function to mark review as submitted
            MarkReviewAsSubmitted(reviewer_id, period_number);
        }




        //marks the review as submitted by updating the bit value
        private void MarkReviewAsSubmitted(string reviewerId, int period_number)
        {
            //uses period number to make sure we mark the correct period as submitted
            //works based on the assumption that we will only ever have two review periods

            string review_number = "";

            if (period_number == 1)
            {
                review_number = "review_1_submitted";
            }
            else if (period_number == 2)
            {
                review_number = "review_2_submitted";
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE student SET " + review_number + " = 1 WHERE username = @ReviewerId";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReviewerId", reviewerId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            //once review is marked as submitted in database, redirect the user to submission confirmation page
            Response.Redirect("FormSubmissionMessage.aspx");
        }



        //used to retrieve correct period number based on current date
        private int GetPeriodNumber()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                

                string query = "SELECT review_id FROM review_period WHERE start_date <= CURDATE() AND end_date >= CURDATE()";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();

                    return (int)command.ExecuteScalar(); 

                }
            }
        }

    }
}