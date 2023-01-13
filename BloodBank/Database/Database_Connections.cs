﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BloodBank.Database
{
    public class Database_Connections
    {
        public static string connectionType = "GCloud"; //Local or GCloud or SmarterASP
        readonly string path = ConfigurationManager.ConnectionStrings[connectionType].ConnectionString;

        private MySqlConnection con;
        private MySqlCommand cmd;
        private MySqlDataReader rdr;
        private MySqlDataAdapter da;

        public void DB_Connect()
        {
            try
            {
                con = new MySqlConnection(path);
            }
            catch (Exception ex)
            {
                Debug.Print("Connection Error : " + ex.Message);
            }
        }

        //For bloodbank Login
        public bloodbank BloodbankLogin(string query)
        {
            bloodbank bb = new bloodbank();
            try
            {
                DB_Connect();
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = query;
                rdr = cmd.ExecuteReader();
                if(rdr.Read() && !rdr.IsDBNull(0))
                {
                    bb.BB_ID = rdr["BB_ID"].ToString();
                    bb.BB_USERNAME = rdr["BB_USERNAME"].ToString();
                    bb.BB_PASSWORD = rdr["BB_PASSWORD"].ToString();
                    bb.BB_RANK = Convert.ToBoolean(rdr["BB_RANK"]);
                    bb.BB_STATUS = Convert.ToBoolean(rdr["BB_STATUS"]);
                }
                rdr.Close();
                con.Close();
            }
            catch(Exception ex)
            {
                Debug.Print("Login Error : " + ex.Message);
            }
            return bb;
        }

        //For Inserting Data on Bloodbank Logs Table
        public bool InsertBloodBankLogs(string query)
        {
            bool res = false;

            try
            {
                DB_Connect();
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = query;
                int x = cmd.ExecuteNonQuery();
                if(x > 0)
                {
                    res = true;
                }
                con.Close();
            }
            catch(Exception ex )
            {
                Debug.Print("Action Logs Error : " + ex.Message);
            }

            return res;
        }

        //If pang populate sa GridView DataTable jud ang datatype nga gamiton
        public DataTable GetBloodBankLogsTableData()
        {
            DataTable dt = new DataTable();
            try
            {
                DB_Connect();
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = "select * from bloodbanklogs order by BL_DATE desc;";
                da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                con.Close();
            }
            catch(Exception ex)
            {
                Debug.Print("Get Action Logs Error : " + ex.Message);
            }
            return dt;
        }

        //Update BloodBank Password
        public int UpdateProfileInfo(string uname, string npword, string opword)
        {
            //1 if Success
            //-2 if Old Password is not the one on record
            //-1 if Database

            int res = -1;
            try
            {
                DB_Connect();
                con.Open();
                cmd = con.CreateCommand();
                //Check Old Password 
                cmd.CommandText = string.Format("select count(*) as CountRow from bloodbank where binary BB_USERNAME='{0}' and binary BB_PASSWORD='{1}';", uname, opword);
                int check = Convert.ToInt32(cmd.ExecuteScalar());
                if(check >= 1)
                {
                    //Meaning the Old Password is right
                    //Try to Update
                    cmd.CommandText = string.Format("update bloodbank set BB_PASSWORD='{0}' where binary BB_USERNAME='{1}' and binary BB_PASSWORD='{2}';", npword, uname, opword);
                    int x = cmd.ExecuteNonQuery();
                    if(x > 0)
                    {
                        //Successful Update
                        res = 1;
                    }
                }
                else
                {
                    //Old Password does not match
                    res = -2;
                }
                con.Close();
            }
            catch(Exception ex)
            {
                Debug.Print("Information Update Error : " + ex.Message);
            }
            return res;
        }
    }
}