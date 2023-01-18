using BloodBank.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BloodBank
{
    public partial class BB_Profile : System.Web.UI.Page
    {
        Database_Connections db = new Database_Connections();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bloodbank bb = Session["bloodbank"] as bloodbank;
                //Set Username
                username.InnerText = bb.BB_USERNAME;
                Profile_Username.Value = bb.BB_USERNAME;
            }
        }

        //Update Info (this is from the button)
        protected void UpdateInfo_Click(object sender, EventArgs e)
        {
            string uname = username.InnerText;
            string npword = Profile_NPassword.Value;
            string opword = Profile_OPassword.Value;

            bool isSuccess = false;
            if (npword == "" || opword == "")
            {
                //Missing Fields
                Response.Write("<script>alert('Missing Fields.')</script>");
            }
            else
            {
                int res = db.UpdateProfileInfo(uname, npword, opword);
                //1 if Success
                //-2 if Old Password is not the one on record
                //-1 if Database
                switch (res)
                {
                    case 1:
                        Response.Write("<script>alert('Profile Password Updated Successfully.')</script>");
                        isSuccess = true;
                        break;
                    case -2:
                        Response.Write("<script>alert('Old Profile Password does not match the one on record.')</script>");
                        break;
                    case -1:
                        Response.Write("<script>alert('Database Error.')</script>");
                        break;
                }

            }


            if (isSuccess)
            {
                Session.Clear();
                Response.Redirect("~/Default.aspx");
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