using BloodBank.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                GetUnreadNotif();
            }
        }

        protected void NotificationNavList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewNotif")
            {
                string id = e.CommandArgument.ToString();
                Session["IsViewing"] = true;
                Session["NTF_ID"] = id;
                Response.Redirect("~/BB_Notification.aspx");
            }
        }

        public void PopulateDashboardObjects()
        {
            //Populate Number of Users
            string query = "select count(*) as USER_COUNT from user_account where UACC_STATUS=true;";
            TotalNumberUser.InnerText = db.GetCount(query).ToString();

            query = "select count(*) as USER_COUNT from user_account where UACC_REQUESTOR=true";
            NumberRequestor.InnerText = db.GetCount(query).ToString();

            query = "select count(*) as USER_COUNT from user_account where UACC_DONOR=true";
            NumberDonor.InnerText = db.GetCount(query).ToString();

            //Populate Pending Initial Transaction NUmbers
            query = @"select ((select count(*) from blood_donation where BD_SURVEY_STATUS=false and BD_BLOOD_STATUS=false and BD_REQ_STATUS=true)
                        + (select count(*) from blood_request where BREQ_SURVEY_STATUS=false and BREQ_BLOOD_STATUS=false and BREQ_REQ_STATUS=true)) as Total;";
            TotalNumberTransaction.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_request where BREQ_SURVEY_STATUS = false and BREQ_BLOOD_STATUS=false and BREQ_REQ_STATUS=true;";
            NumberRequestTransaction.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_donation where BD_SURVEY_STATUS = false and BD_BLOOD_STATUS=false and BD_REQ_STATUS=true;";
            NumberDonationTransaction.InnerText = db.GetCount(query).ToString();



            //Populate Approved Initial Transaction NUmbers
            query = @"select ((select count(*) from blood_donation where BD_SURVEY_STATUS = true and (BD_BLOOD_STATUS=false or BD_BLOOD_STATUS=true) and BD_REQ_STATUS=true)
                        + (select count(*) from blood_request where BREQ_SURVEY_STATUS = true and (BREQ_BLOOD_STATUS=false or BREQ_BLOOD_STATUS=true) and BREQ_REQ_STATUS=true)) as Total;";
           TotalApproved.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_request where BREQ_SURVEY_STATUS = true and (BREQ_BLOOD_STATUS=false or BREQ_BLOOD_STATUS=true) and BREQ_REQ_STATUS=true;";
           BR_Approved.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_donation where BD_SURVEY_STATUS = true and (BD_BLOOD_STATUS=false or BD_BLOOD_STATUS=true) and BD_REQ_STATUS=true;";
           BD_Approved.InnerText = db.GetCount(query).ToString();


            //Populate Completef Transaction NUmbers
            query = @"select ((select count(*) from blood_donation where (BD_SURVEY_STATUS = true or BD_BLOOD_STATUS = true) and BD_REQ_STATUS=true)
                        + (select count(*) from blood_request where (BREQ_SURVEY_STATUS = true or BREQ_BLOOD_STATUS = false) and BREQ_REQ_STATUS=true)) as Total;";
           // TotalNumberTransaction.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_request where (BREQ_SURVEY_STATUS = true or BREQ_BLOOD_STATUS = true) and BREQ_REQ_STATUS=true;";
           // NumberRequestTransaction.InnerText = db.GetCount(query).ToString();

            query = @"select count(*) from blood_donation where (BD_SURVEY_STATUS = true or BD_BLOOD_STATUS = true) and BD_REQ_STATUS=true;";
           // NumberDonationTransaction.InnerText = db.GetCount(query).ToString();



            object sender = new object();
            EventArgs e = new EventArgs();
            PieOption_SelectedIndexChanged(sender, e);


        }

        protected void BtnLogout_ServerClick(object sender, EventArgs e)
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }


        private void GetUnreadNotif()
        {
            bloodbank bb = Session["bloodbank"] as bloodbank;

            //Get Unread COunt
            string query = string.Format(@"select count(*) from notifications where NTF_RECEIVER_ID={0} and NTF_STATUS=false;", bb.BB_ID);
            int count = db.GetUnreadNotificationCount(query);

            if (count <= 9)
            {
                UnreadCount.InnerText = count.ToString();
            }
            else
            {
                UnreadCount.InnerText = "9+";
            }
            Debug.Print("Unread Count : " + count);

            query = string.Format(@"select * from notifications where NTF_RECEIVER_ID={0} order by NTF_STATUS, NTF_DATE desc", bb.BB_ID);
            List<notifications> nList = db.GetNotifications(query);
            if (nList != null && nList[0].NTF_ID != null)
            {
                List<notifications> unread = nList.Where(x => x.NTF_STATUS == false).Select(g => g).ToList();
                if (unread != null)
                {
                    int rows = 0;
                    if (count > 5)
                    {
                        rows = 5;
                    }
                    else
                    {
                        rows = unread.Count;
                    }
                    List<notifications> newUnread = new List<notifications>();
                    for (int i = 0; i < rows; i++)
                    {
                        newUnread.Add(unread[i]);
                    }

                    NotificationNavList.DataSource = null;
                    NotificationNavList.DataSource = newUnread;
                    NotificationNavList.DataBind();
                }
            }
        }

        protected void PieOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            int[] transactions = new int[3];
            string[] lbl = new string[3];
            string query = "";

            lbl[0] = "Completed";
            lbl[1] = "Rejected";
            lbl[2] = "Pending";

            switch (PieOption.SelectedValue)
            {
                case "0":
                    //Blood Requests
                    PieTitle.InnerText = "Requests";
                    query = @"select count(*) as Total from blood_request where (BREQ_SURVEY_STATUS = true and BREQ_BLOOD_STATUS = true) and BREQ_REQ_STATUS=true;";
                    transactions[0] = db.GetCount(query);

                    query = @"select count(*) as Total from blood_request where BREQ_REQ_STATUS=false;";
                    transactions[1] = db.GetCount(query);

                    query = @"select count(*) as Total from blood_request where (BREQ_SURVEY_STATUS = false or BREQ_BLOOD_STATUS = false) and BREQ_REQ_STATUS=true;";
                    transactions[2] = db.GetCount(query);
                    break;
                case "1":
                    //Blood Donations
                    PieTitle.InnerText = "Donations";
                    query = @"select count(*) as Total from blood_donation where (BD_SURVEY_STATUS = true and BD_BLOOD_STATUS = true) and BD_REQ_STATUS=true;";
                    transactions[0] = db.GetCount(query);

                    query = @"select count(*) as Total from blood_donation where BD_REQ_STATUS=false;";
                    transactions[1] = db.GetCount(query);

                    query = @"select count(*) as Total from blood_donation where (BD_SURVEY_STATUS = false or BD_BLOOD_STATUS = false) and BD_REQ_STATUS=true;";
                    transactions[2] = db.GetCount(query);
                    break;
            }

            Labels = lbl;
            Data = transactions;
        }
    }
}