using BloodBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BloodBank
{
    public partial class BB_Dashboard : System.Web.UI.Page
    {
        private Database_Connections db = new Database_Connections();
        public string[] Labels { get; set; }
        public int[] Data { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(Session["LOGIN"]))
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Page.IsPostBack)
            {
                bloodbank bb = Session["bloodbank"] as bloodbank;
                //Set Username
                username.InnerText = bb.BB_USERNAME;
                PopulateDashboardObjects();
            }
        }

        public void PopulateDashboardObjects()
        {
            //Populate Number of Users
            string query = "select count(*) as USER_COUNT from user_account where UACC_STATUS=true;";
            TotalNumberUser.InnerText = db.GetCount(query).ToString();

            query = "select count(distinct BREQ_UACC_ID) as REQ_COUNT from blood_request join user_account on UACC_ID=BREQ_UACC_ID where UACC_STATUS=true;";
            NumberRequestor.InnerText = db.GetCount(query).ToString();

            query = "select count(distinct BD_UACC_ID) as DON_COUNT from blood_donation join user_account on UACC_ID=BD_UACC_ID where (BD_SURVEY_STATUS = true and BD_BLOOD_STATUS = true) and BD_REQ_STATUS=true and UACC_STATUS=true;";
            NumberDonor.InnerText = db.GetCount(query).ToString();


            //Populate Transaction NUmbers
            query = @"select ((select count(*) from blood_donation where (BD_SURVEY_STATUS = false or BD_BLOOD_STATUS = false) and BD_REQ_STATUS=true)
                        + (select count(*) from blood_request where (BREQ_SURVEY_STATUS = false or BREQ_BLOOD_STATUS = false) and BREQ_REQ_STATUS=true)) as Total;";
            TotalNumberTransaction.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_request where (BREQ_SURVEY_STATUS = false or BREQ_BLOOD_STATUS = false) and BREQ_REQ_STATUS=true;";
            NumberRequestTransaction.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_donation where (BD_SURVEY_STATUS = false or BD_BLOOD_STATUS = false) and BD_REQ_STATUS=true;";
            NumberDonationTransaction.InnerText = db.GetCount(query).ToString();

            int[] transactions = new int[3];
            string[] lbl = new string[3];

            lbl[0] = "Approved";
            lbl[1] = "Rejected";
            lbl[2] = "Pending";

            query = @"select ((select count(*) from blood_request where (BREQ_SURVEY_STATUS = true and BREQ_BLOOD_STATUS = true) and BREQ_REQ_STATUS=true) 
                    + (select count(*) from blood_donation where (BD_SURVEY_STATUS = true and BD_BLOOD_STATUS = true) and BD_REQ_STATUS=true)) as Total;";
            transactions[0] = db.GetCount(query);

            query = @"select ((select count(*) from blood_request where BREQ_REQ_STATUS=false) 
                    + (select count(*) from blood_donation where BD_REQ_STATUS=false)) as Total;";
            transactions[1] = db.GetCount(query);

            transactions[2] = Convert.ToInt32(TotalNumberTransaction.InnerText);

            Labels = lbl;
            Data = transactions;
        }

        protected void BtnLogout_ServerClick(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }
    }
}