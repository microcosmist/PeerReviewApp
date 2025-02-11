

<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Performance_Report.aspx.cs" Inherits="PTT.Performance_WebForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <title>Performance Report</title>
 <link rel="stylesheet" href="performance_report.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="parentholder" runat="server">
    <!--container that holds all criteria and rating tables-->
     <div class="report-container">      
          <!--container that holds qual of work table-->
        <div class="qual-of-work"> 
            <div class="average-grade">
                <div class="avg-text">Quality of Work</div>
                <div class="avg-num"><asp:Label ID="qowAvg" runat="server" Text="Label"></asp:Label></div>
            </div>
            <div class="actual-grade">
                <asp:Table ID="QowTable"  style="width: 13vw; height: 27vh; padding: 5%;" runat="server"></asp:Table>
            </div>
        </div>

         <!--container that holds timeliness table-->
        <div class="timeliness">
            <div class="average-grade">
                <div class="avg-text">Timeliness</div>
                <div class="avg-num"><asp:Label ID="timeAvg" runat="server" Text="Label"></asp:Label></div>
            </div>
            <div class="actual-grade">
                <asp:Table ID="TimeTable" style="width: 13vw; height: 27vh; padding: 5%;" runat="server"></asp:Table>
            </div>
        </div>

         <!--container that holds communication table-->
        <div class="communication">
            <div class="average-grade">
                <div class="avg-text">Teamwork</div>
                <div class="avg-num"><asp:Label ID="teamAvg" runat="server" Text="Label"></asp:Label></div>
            </div>
            <div class="actual-grade">
                <asp:Table ID="TeamworkTable" style="width: 13vw; height: 27vh; padding: 5%;" runat="server"></asp:Table>
            </div>
        </div>

         <!--container that holds teamwork table-->
        <div class="teamwork">
            <div class="average-grade">
                <div class="avg-text">Effort and Particpation</div>
                <div class="avg-num"><asp:Label ID="eapAvg" runat="server" Text="Label"></asp:Label></div>
            </div>
            <div class="actual-grade">
               <asp:Table ID="EaPTable" style="width: 13vw; height: 27vh; padding: 5%;" runat="server"></asp:Table>
            </div>
        </div>

           <!--container that holds effort and participation table-->
        <div class="effort-part">
            <div class="average-grade">
                <div class="avg-text">Communication</div>
                <div class="avg-num"><asp:Label ID="commAvg" runat="server" Text="Label"></asp:Label></div>
            </div>
            <div class="actual-grade">
                <asp:Table ID="CommTable" style="width: 13vw; height: 27vh; padding: 5%;" runat="server"></asp:Table>
            </div>
        </div>
    </div> 

</asp:Content>
