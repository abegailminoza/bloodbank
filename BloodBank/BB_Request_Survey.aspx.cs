using BloodBank.Database;
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

            if (res)
            {
                query = string.Format(@"update blood_request set BREQ_SURVEY_STATUS={0} where BREQ_ID={1}", res, br.BREQ_ID);
                if(db.UpdateBloodRequestStatus(query))
                {
                    //Create Login Logs
                    string description = string.Format("BloodBank Accepted User {0} ( ", br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ') Initial Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID, bb.BB_USERNAME, br.BREQ_UACC_ID);
                    Debug.Print(query);
                    bool x = db.InsertBloodBankLogs(query);
                    //If Not Successfully Inserted Logs
                    if (!x)
                    {
                        Debug.Print("BloodBank Logs Not Inserted");
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
                    string description = string.Format("BloodBank Rejected User {0} ( ", br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ' ) Initial Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID, bb.BB_USERNAME, br.BREQ_UACC_ID);

                    Debug.Print(query);
                    bool x = db.InsertBloodBankLogs(query);
                    //If Not Successfully Inserted Logs
                    if (!x)
                    {
                        Debug.Print("BloodBank Logs Not Inserted");
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
                    string description = string.Format("BloodBank Accepted User {0} ( ", br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ' ) Final Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID, bb.BB_USERNAME, br.BREQ_UACC_ID);

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
                    string description = string.Format("BloodBank Rejected User {0} ( ", br.BREQ_UACC_ID);
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME)
                                            select concat('{0}', UACC_FIRST, ' ', UACC_LAST, ' ) Final Blood Request Form'), {1}, '{2}' from user_account
                                            where UACC_ID={3};", description, bb.BB_ID, bb.BB_USERNAME, br.BREQ_UACC_ID);

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
    }
}