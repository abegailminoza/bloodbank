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
    public partial class _Default : Page
    {
        private Database_Connections db = new Database_Connections();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_Click(object sender, EventArgs e)
        {
            string uname = Username.Text;
            string pword = Password.Text;

            //Check if Input Fields are Empty
            if(uname == "" || pword == "")
            {
                //Display Message

            }
            else
            {
                //Attempt to Login

                //Query or SQL Command
                string query = string.Format(@"select * from bloodbank where binary BB_USERNAME='{0}' and binary BB_PASSWORD='{1}';", uname, pword);

                //Try Query
                bloodbank bb = db.BloodbankLogin(query);

                Debug.Print(JsonConvert.SerializeObject(bb));

                //Check if it is not empty
                if(bb.BB_ID != null)
                {
                    //Save Login Information in Session
                    Session["LOGIN"] = true;
                    Session["bloodbank"] = bb;

                    //Create Login Logs
                    query = string.Format(@"insert into activity_logs(ACT_DESCRIPTION, ACT_UACC_ID, ACT_UNAME) 
                                            values('{0}', {1}, '{2}');", "User " + bb.BB_USERNAME + " Logged In", bb.BB_ID, bb.BB_USERNAME);
                    Debug.Print(query);
                    bool res = db.InsertBloodBankLogs(query);
                    //If Not Successfully Inserted Logs
                    if(!res)
                    {
                        Debug.Print("BloodBank Logs Not Inserted");
                    }
                    Debug.Print("BB Rank : " + bb.BB_RANK);
                    string msg = "";
                    if(bb.BB_RANK)
                    {
                        //Personnel Head
                        msg = "Personnel Head";
                    }
                    else
                    {
                        //Personnel (Bata-bata)
                        msg = "Personnel (Bata-bata)";
                    }

                    Response.Write("<script>alert('" + msg + "')</script>");
                    Response.Redirect("~/BB_Dashboard.aspx");
                }
                else
                {
                    Response.Write("<script>alert('Username and Password Mismatch.')</script>");
                }
                
            }
        }
    }
}