using BloodBank.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace BloodBank
{
    public partial class BB_Notification : System.Web.UI.Page
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
                if (Convert.ToBoolean(Session["IsViewing"]))
                {
                    ViewNotifWithID(Session["NTF_ID"].ToString());
                }
                else
                {

                    PopulateNotificationGridView();
                }

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


        public void PopulateNotificationGridView()
        {
            bloodbank ua= Session["bloodbank"] as bloodbank;


            DataTable data = db.GetNotificationTableData(ua);

            if (data != null)
            {
                NoDataMsg.Attributes.Add("display", "none");
                TableContainer.Attributes.Add("display", "");
                NotificationGrid.DataSource = null;
                NotificationGrid.DataSource = data;
                NotificationGrid.DataBind();
            }
            else
            {
                NoDataMsg.Attributes.Add("display", "");
                TableContainer.Attributes.Add("display", "none");
            }
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


        protected void NotificationGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = NotificationGrid.SelectedRow;
            string id = row.Cells[0].Text;
            notifications ntf = db.SearchNotification(id);

            if (ntf != null)
            {
                if (ntf.NTF_SENDER_ID != null)
                {
                    Panel1.Visible = true;
                    Subject.Text = ntf.NTF_SUBJECT;
                    Message.InnerText = ntf.NTF_MESSAGE;

                    //Update Status
                    if (!ntf.NTF_STATUS)
                    {
                        if (db.UpdateNotificationStatus(id))
                        {
                            Debug.Print("Success");
                            GetUnreadNotif();
                        }
                        PopulateNotificationGridView();
                    }
                }
            }
        }

        public void ViewNotifWithID(string id)
        {
            notifications ntf = db.SearchNotification(id);

            if (ntf != null)
            {
                if (ntf.NTF_SENDER_ID != null)
                {
                    Panel1.Visible = true;
                    Subject.Text = ntf.NTF_SUBJECT;
                    Message.InnerText = ntf.NTF_MESSAGE;

                    //Update Status
                    if (!ntf.NTF_STATUS)
                    {
                        if (db.UpdateNotificationStatus(id))
                        {
                            Debug.Print("Success");
                        }
                        PopulateNotificationGridView();
                        GetUnreadNotif();
                    }
                }
            }
        }

        protected void Close_Click1(object sender, ImageClickEventArgs e)
        {
            Panel1.Visible = false;
        }
    }
}