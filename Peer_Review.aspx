

<%@ Page Title="Peer Review" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Peer_Review.aspx.cs" Inherits="PTT.Review_WebForm"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Peer Review Form 1</title>
    <link rel="stylesheet" href="peer_review.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="parentholder" runat="server">
    <!--Container holds the content of the entire page, important for inserting into layout/master page-->
    <div class="body-container">
        <!--Container for the instructions box-->
        <div class="form-info">
            <!--the actual text within the instructions-->
		    <p class="info-title"><u>Instructions</u></p>
	
		    Please rate each of your team members and yourself based on the 5 criteria listed below.

		    <br><br><mark>You are only allowed one submission</mark>, after that you may not be able to change your ratings without emailing Professor Cole.
		    <br><br>
            <strong>Quality of Work:</strong> How well did the team members complete
            their tasks? Was the work accurate, thorough, and up to the required
            standard?
		    <br><br>
            <strong>Timeliness:</strong> Did the team member meet deadlines
            and submit their work on time?
		    <br><br>
            <strong>Communication:</strong> How effectively did the team
            member communicate with the group? Did they provide clear updates and
            contribute to discussions?
		    <br><br>
            <strong>Teamwork:</strong> How well did the team members work with
            others? Did they show respect, help solve problems, and contribute
            positively to team dynamics?
		    <br><br>
            <strong>Effort and Participation:</strong> How much effort did the
            team member put into the project? Were they actively involved in all
            stages of the work?
            <br><br>
            <strong>Comments:</strong> You may input any comments if you want to leave feedback about 
            your teammate, but they are <i>optional</i>.

	      </div>

        <!--The actual review table, which initializes the header row, the code behind populates all other cells-->
            <asp:Table ID="ReviewTable" runat="server" Border="1" CellSpacing="1" Width="100%" CellPadding="10" EnableViewState="true" HorizontalAlign="Center" >
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Team Member</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Quality of Work</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Timeliness</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Communication</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Teamwork</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Effort and Participation</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Comments</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>


        </div>
  

</asp:Content>
