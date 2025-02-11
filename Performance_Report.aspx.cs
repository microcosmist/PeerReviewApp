

using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static PTT.Performance_WebForm;
using static PTT.Review_WebForm;
using Table = System.Web.UI.WebControls.Table;




namespace PTT
{
    public partial class Performance_WebForm : System.Web.UI.Page
    {
        //stores database connection
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
                pageTitle.Text = "Performance Report";
            }



            //Store utd_id 
            string utd_id = Session["UTD_ID"].ToString();
            int period_number = GetMostRecentPeriodNumber();
            System.Diagnostics.Debug.WriteLine($"periodNum: {period_number}");

            if (IsReviewPeriodOver(period_number) == false)
            {
                Response.Redirect("PeriodStillActive.aspx");
            }

            var ratingsList = GetTeamRatings(period_number, utd_id);

            List<string> TableID = new List<string> { "QowTable", "TimeTable", "TeamworkTable", "EaPTable", "CommTable" };


            foreach (string tableID in TableID)
            {
                CreateTables(ratingsList, tableID);
                GetAverages(ratingsList, tableID);
            }
        }

        //uses the database to retrieve the ratings for the current team member
        private List<Ratings> GetTeamRatings(int period_number, string utd_id)
        {

            //creates a list that will hold a member object
            List<Ratings> teamRatings = new List<Ratings>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                string query = @"SELECT 
                                S.first_name, 
                                S.last_name, 
                                R.qual_of_work_rating, 
                                R.timeliness_rating, 
                                R.teamwork_rating, 
                                R.eff_and_part_rating, 
                                R.communication_rating 
                                FROM Student S
                                JOIN peer_review R on R.reviewer_id = S.username
                                WHERE R.review_period_id = @PeriodNumber AND R.reviewee_id = @UTD_ID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PeriodNumber", period_number);
                    command.Parameters.AddWithValue("@UTD_ID", utd_id);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        //creates a new instance of the Member class for every student in the team
                        while (reader.Read())
                        {
                            Ratings rating = new Ratings
                            {
                                Fname = reader["first_name"].ToString(),
                                Lname = reader["last_name"].ToString(),
                                QoW = Convert.ToInt32(reader["qual_of_work_rating"]),
                                Time = Convert.ToInt32(reader["timeliness_rating"]),
                                Team = Convert.ToInt32(reader["teamwork_rating"]),
                                EaP = Convert.ToInt32(reader["eff_and_part_rating"]),
                                Comm = Convert.ToInt32(reader["communication_rating"]),
                            };
                            teamRatings.Add(rating);
                        }
                    }
                }
            }

            return teamRatings;
        }

        //class will hold the individual ratings from every team member that graded the user
        public class Ratings
        {
            public string Fname { get; set; }
            public string Lname { get; set; }
            public int QoW { get; set; }
            public int Time { get; set; }
            public int Team { get; set; }
            public int EaP { get; set; }
            public int Comm { get; set; }

        }


        // Function to get the most recent period number,
        // there's some complicated logic 
        // if the review period is over we aren't able to query for the period number
        // because the query's success relies on the todays date falling within the review period start and end date
        //instead I hardcoded the period number based on the start date of period 2, its a temporary solution 
        //works on the assumption that we will only ever have 2 review periods
        private int GetMostRecentPeriodNumber()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                string query = "SELECT start_date FROM review_period WHERE review_id = 2";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    DateTime start_date = Convert.ToDateTime(command.ExecuteScalar());

                    if (DateTime.Today < start_date)
                    {
                        return 1;
                    }
                    return 2;
                }
            }
        }


        //creates the table depending on the table id 
        private void CreateTables(List<Ratings> ratingsList, string tableID)
        {

            // create a row in the table for each team member
            for (int i = 0; i < ratingsList.Count; i++)
            {
                int rating = 0;

                //create new row 
                TableRow row = new TableRow();

                // create cell that will hold team member name
                TableCell memberCell = new TableCell { Text = $"{ratingsList[i].Fname} {ratingsList[i].Lname}" };

                //add member cell to current row
                row.Cells.Add(memberCell);

                //depending on which criteria table we are working on, add the correct rating to the table
                switch (tableID)
                {
                    case "QowTable":
                        rating = ratingsList[i].QoW;
                        TableCell qowCell = new TableCell { Text = rating.ToString(), HorizontalAlign = HorizontalAlign.Right };
                        row.Cells.Add(qowCell);
                        QowTable.Rows.Add(row);
                        break;
                    case "TimeTable":
                        rating = ratingsList[i].Time;
                        TableCell timeCell = new TableCell { Text = rating.ToString(), HorizontalAlign = HorizontalAlign.Right };
                        row.Cells.Add(timeCell);
                        TimeTable.Rows.Add(row);
                        break;
                    case "TeamworkTable":
                        rating = ratingsList[i].Team;
                        TableCell teamCell = new TableCell { Text = rating.ToString(), HorizontalAlign = HorizontalAlign.Right };
                        row.Cells.Add(teamCell);
                        TeamworkTable.Rows.Add(row);
                        break;
                    case "EaPTable":
                        rating = ratingsList[i].EaP;
                        TableCell eapCell = new TableCell { Text = rating.ToString(), HorizontalAlign = HorizontalAlign.Right };
                        row.Cells.Add(eapCell);
                        EaPTable.Rows.Add(row);
                        break;
                    case "CommTable":
                        rating = ratingsList[i].Comm;
                        TableCell commCell = new TableCell { Text = rating.ToString(), HorizontalAlign = HorizontalAlign.Right };
                        row.Cells.Add(commCell);
                        CommTable.Rows.Add(row);
                        break;
                    default:
                        rating = 0;
                        break;
                }
            }
        }

        //calculates the averages from each instance of the class Rating and displays this value on the page
        private void GetAverages(List<Ratings> ratingsList, string tableID)
        {

            double avgQoW = ratingsList.Any() ? ratingsList.Average(r => r.QoW) : 0;
            double avgTime = ratingsList.Any() ? ratingsList.Average(r => r.Time) : 0;
            double avgTeam = ratingsList.Any() ? ratingsList.Average(r => r.Team) : 0;
            double avgEaP = ratingsList.Any() ? ratingsList.Average(r => r.EaP) : 0;
            double avgComm = ratingsList.Any() ? ratingsList.Average(r => r.Comm) : 0;


            qowAvg.Text = avgQoW.ToString("F1");
            timeAvg.Text = avgTime.ToString("F1");
            teamAvg.Text = avgTeam.ToString("F1");
            eapAvg.Text = avgEaP.ToString("F1");
            commAvg.Text = avgComm.ToString("F1");

        }



        //check the database to see if the review period is over
        private bool IsReviewPeriodOver(int period_number)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT end_date FROM review_period WHERE review_id = @PeriodNumber";


                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PeriodNumber", period_number);
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            //variable to hold the end date 
                            DateTime endDate = reader.GetDateTime(0);

                            //check if todays date is after the end date 
                            if (DateTime.Today > endDate)
                            {
                                //debugging to check if logic works
                                System.Diagnostics.Debug.WriteLine($"Review period is over");

                                //return true when the review period is over
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }


    }
}

  
