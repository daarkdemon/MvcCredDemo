using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MvcTestApp.Models
{
    //Database handler for Category records
    public class CategoryDBHandle
    {
        private SqlConnection con;
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            con = new SqlConnection(constr);
        }

        //Add Category records
        public bool AddCategory(CategoryModel category)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("AddCategory", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", category.Name);
            cmd.Parameters.AddWithValue("@Description", category.Description);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
            if (i >= 1)
                return true;
            else
                return false;
        }

        //Get Category recors
        public List<CategoryModel> GetCategory()
        {
            Connection();
            List<CategoryModel> categorylist = new List<CategoryModel>();
            SqlCommand cmd = new SqlCommand("GetCategory", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();
            cmd.Dispose();

            foreach (DataRow dr in dt.Rows)
            {
                categorylist.Add(
                    new CategoryModel
                    {
                        CategoryId = Convert.ToInt32(dr["CategoryID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Description = Convert.ToString(dr["Description"])
                    }
                );
            }
            return categorylist;
        }

        //Update Category records
        public bool UpdateCategoryDetails(CategoryModel category, int id)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("UpdateCategoryDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", category.Name);
            cmd.Parameters.AddWithValue("@Description", category.Description);
            cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool DeleteCategory(int id)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("DeleteCategory", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@CategoryId", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool IfCategoryExist(string name)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("IfCategoryExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", name);
            con.Open();
            object i = cmd.ExecuteScalar();
            con.Close();
            cmd.Dispose();
            if (i != null)
                return true;
            else
                return false;
        }

        //Get Category records in DataSet
        public DataSet GetCategoryList()
        {
            Connection();
            DataSet dataSet = new DataSet();
            SqlCommand cmd = new SqlCommand("GetCategory", con)
            {
                CommandType = CommandType.StoredProcedure

            };
            SqlDataAdapter sda = new SqlDataAdapter(cmd)
            {
                SelectCommand = cmd
            };
            sda.Fill(dataSet);
            cmd.Dispose();
            return dataSet;
        }
    }
}