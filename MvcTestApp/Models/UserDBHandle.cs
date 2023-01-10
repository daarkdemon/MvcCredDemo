using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcTestApp.Models
{
    public class UserDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ToString();
            con = new SqlConnection(constr);
        }

        public bool SignUp(UserModel user)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
            cmd.Parameters.AddWithValue("@LastName", user.LastName);
            cmd.Parameters.AddWithValue("@EmailAddress", user.EmailAddress);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 1)
                return true;
            else
                return false;
        }

        public bool IfUserExist(string email)
        {
            connection();
            SqlCommand cmd = new SqlCommand("IfUserExist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmailAddress", email);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i > 0)
                return true;
            else
                return false;
        }

        public bool IsValidUser(string email, string password)
        {
            connection();
            bool IsValid = false;
            SqlCommand cmd = new SqlCommand("IsValidUser", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@EmailAddress", email);
            cmd.Parameters.AddWithValue("@Password", password);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (dt.Rows.Count > 0)
            {
                IsValid = true;
            }
            return IsValid;
        }
    }
}