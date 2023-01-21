using BloodBank.Database;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BloodBank
{
    public partial class BB_Request_Survey : System.Web.UI.Page
    {
        private Database_Connections db = new Database_Connections();
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
                PopulateFormInputs();
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


        private void PopulateFormInputs()
        {
            blood_request br = Session["BloodRequest"] as blood_request;
            request_survey_form rq = JsonConvert.DeserializeObject<request_survey_form>(br.BREQ_JSON_SURVEY_FORM);

            familyname.Text = rq.lname;
            firstname.Text = rq.fname;
            middlename.Text = rq.mname;
            gender.Text = rq.gender;
            dateofbirth.Text = rq.dob;
            Age.Text = rq.age;
            bloobredtyperequest.Text = rq.brequest;
            resaddress.Text = rq.raddress;
            posaddress.Text = rq.paddress;
            Home.Text = rq.home;
            Mobile.Text = rq.mobile;
            Email.Text = rq.email;

            DisableInputs();

            if(!br.BREQ_REQ_STATUS)
            {
                SurveyGroup.Style.Add("display", "none");
                BloodGroup.Style.Add("display", "none");
            }
            else if (!br.BREQ_SURVEY_STATUS)
            {
                SurveyGroup.Style.Add("display", "");
                BloodGroup.Style.Add("display", "none");
            }
            else if(!br.BREQ_BLOOD_STATUS)
            {
                SurveyGroup.Style.Add("display", "none");
                BloodGroup.Style.Add("display", "");
            }
        }

        private void DisableInputs()
        {
            familyname.Enabled = false;
            firstname.Enabled = false;
            middlename.Enabled = false;
            gender.Enabled = false;
            dateofbirth.Enabled = false;
            Age.Enabled = false;
            bloobredtyperequest.Enabled = false;
            resaddress.Enabled = false;
            posaddress.Enabled = false;
            Home.Enabled = false;
            Mobile.Enabled = false;
            Email.Enabled = false;
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BB_BloodTransaction.aspx");
        }

        protected void ApproveSurveyBtn_Click(object sender, EventArgs e)
        {
            UserRequestSurveyResponse(true);
        }

        protected void RejectSurveyBtn_Click(object sender, EventArgs e)
        {
            UserRequestSurveyResponse(false);
        }

        private void UserRequestSurveyResponse(bool res)
        {
            blood_request br = Session["BloodRequest"] as blood_request;
            bloodbank bb = Session["bloodbank"] as bloodbank;
            string query = "";
            DateTime vDate = DateTime.Now.AddDays(2);
            if (res)
            {
                query = string.Format(@"update blood_request set BREQ_SURVEY_STATUS={0}, BREQ_VISIT_DATE='{1}' where BREQ_ID={2}", res, vDate, br.BREQ_ID);
                Debug.Print(query);
                if (db.UpdateBloodRequestStatus(query))
                {
                    //Create Logs
                    string description = string.Format("{0} Accepted User {1} ( ",bb.BB_USERNAME, br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ') Initial Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID, "BloodBank", br.BREQ_UACC_ID);
                    Debug.Print(query);
                    bool x = db.InsertBloodBankLogs(query);
                    //If Not Successfully Inserted Logs
                    if (!x)
                    {
                        Debug.Print("BloodBank Logs Not Inserted");
                    }

                    //Send Notification
                    string sbj = "Blood Request Form Accepted";
                    string msg = MySqlHelper.EscapeString(string.Format(@"Your Request ID {0}
Your request has been approved you may now proceed to claim your request.
                                                    
Please bring the following with you:
Any valid ID
Doctor's consent for blood bag request with Doctor's name and signature
Ice bucket filled with ice
Processing fee: P1,500.00
                                                    
*Please keep in mind that you can only claim your request until the following date: {1}
*Note: Show your Request ID to the bloodbank.", br.BREQ_ID, vDate));
                    query = string.Format(@"insert into notifications(NTF_SUBJECT, NTF_MESSAGE, NTF_RECEIVER_ID, NTF_SENDER_ID) 
                                                values('{0}', '{1}', {2}, {3})", sbj, msg, br.BREQ_UACC_ID, bb.BB_ID);
                    if(!db.InsertToNotification(query))
                    {
                        Debug.Print("Notification was not sent.");
                    }

                    //Success
                    Response.Write(string.Format("<script>alert('User {0} blood request survey was successfully approved.')</script>", br.BREQ_UACC_ID));

                    SurveyGroup.Style.Add("display", "none");
                    BloodGroup.Style.Add("display", "");
                }
            }
            else
            {
                query = string.Format(@"update blood_request set BREQ_SURVEY_STATUS=false, BREQ_BLOOD_STATUS=false, BREQ_REQ_STATUS={0} where BREQ_ID={1}", res, br.BREQ_ID);
                if (db.UpdateBloodRequestStatus(query))
                {
                    //Create Login Logs
                    string description = string.Format("{0} Rejected User {1} ( ", bb.BB_USERNAME, br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ') Initial Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID, "BloodBank", br.BREQ_UACC_ID);

                    Debug.Print(query);
                    bool x = db.InsertBloodBankLogs(query);
                    //If Not Successfully Inserted Logs
                    if (!x)
                    {
                        Debug.Print("BloodBank Logs Not Inserted");
                    }

                    //Send Notification
                    string sbj = "Blood Request Form Rejected";
                    string msg = string.Format(@"Your Request ID {0}
Your request has been rejected.", br.BREQ_ID);
                    query = string.Format(@"insert into notifications(NTF_SUBJECT, NTF_MESSAGE, NTF_RECEIVER_ID, NTF_SENDER_ID) 
                                                values('{0}', '{1}', {2}, {3})", sbj, msg, br.BREQ_UACC_ID, bb.BB_ID);
                    if (!db.InsertToNotification(query))
                    {
                        Debug.Print("Notification was not sent.");
                    }

                    //Success
                    Response.Write(string.Format("<script>alert('User {0} blood request survey was successfully rejected.')</script>", br.BREQ_UACC_ID));

                    SurveyGroup.Style.Add("display", "none");
                    BloodGroup.Style.Add("display", "none");
                }
            }

        }

        protected void ApproveBloodBtn_Click(object sender, EventArgs e)
        {
            UserRequestBloodResponse(true);
        }

        protected void RejectBloodBtn_Click(object sender, EventArgs e)
        {
            UserRequestBloodResponse(false);
        }

        private void UserRequestBloodResponse(bool res)
        {
            blood_request br = Session["BloodRequest"] as blood_request;
            bloodbank bb = Session["bloodbank"] as bloodbank;
            string query = "";

            if (res)
            {
                query = string.Format(@"update blood_request set BREQ_BLOOD_STATUS={0} where BREQ_ID={1}", res, br.BREQ_ID);
                if (db.UpdateBloodRequestStatus(query))
                {
                    //Create Login Logs
                    string description = string.Format("{0} Accepted User {1} ( ", bb.BB_USERNAME, br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ' ) Final Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID,"BloodBank", br.BREQ_UACC_ID);

                    Debug.Print(query);
                    bool x = db.InsertBloodBankLogs(query);
                    //If Not Successfully Inserted Logs
                    if (!x)
                    {
                        Debug.Print("BloodBank Logs Not Inserted");
                    }
                    //Success
                    Response.Write(string.Format("<script>alert('User {0} blood request was successfully approved.')</script>", br.BREQ_UACC_ID));

                    SurveyGroup.Style.Add("display", "none");
                    BloodGroup.Style.Add("display", "none");
                }
            }
            else
            {
                query = string.Format(@"update blood_request set BREQ_BLOOD_STATUS=false, BREQ_REQ_STATUS={0} where BREQ_ID={1}", res, br.BREQ_ID);
                if (db.UpdateBloodRequestStatus(query))
                {
                    //Create Login Logs
                    string description = string.Format("{0} Rejected User {1} ( ", bb.BB_USERNAME, br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ' ) Final Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID, "BloodBank", br.BREQ_UACC_ID);


                    Debug.Print(query);
                    bool x = db.InsertBloodBankLogs(query);
                    //If Not Successfully Inserted Logs
                    if (!x)
                    {
                        Debug.Print("BloodBank Logs Not Inserted");
                    }
                    //Success
                    Response.Write(string.Format("<script>alert('User {0} blood request was successfully rejected.')</script>", br.BREQ_UACC_ID));

                    SurveyGroup.Style.Add("display", "none");
                    BloodGroup.Style.Add("display", "none");
                }
            }
        }

        protected void BtnLogout_ServerClick(object sender, EventArgs e)
        {

            Session.Clear();
            Session.RemoveAll();
            Server.Transfer("~/Default.aspx");
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

    }
}