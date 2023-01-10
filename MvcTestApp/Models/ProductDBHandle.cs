using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;

namespace MvcTestApp.Models
{
    //Database handler for Product records
    public class ProductDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ToString();
            con = new SqlConnection(constr);
        }

        //Add Product details
        public bool AddProduct(ProductModel product)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddProduct", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@CategoryID", product.CategoryId);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            con.Open();
            object i = cmd.ExecuteNonQuery();
            con.Close();
            if (i != null)
                return true;
            else
                return false;
        }

        //View Product details
        public List<ProductListModel> GetProduct()
        {
            connection();
            List<CategoryModel> categories = new List<CategoryModel>();
            List<ProductModel> products = new List<ProductModel>();
            List<ProductListModel> productlist = new List<ProductListModel>();
            SqlCommand cmd = new SqlCommand("GetProduct", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                productlist.Add(
                    new ProductListModel
                    {
                        Product = new ProductModel {
                            ProductId = Convert.ToInt32(dr["ProductID"]),
                            Name = Convert.ToString(dr["Name"]),
                            Quantity = Convert.ToInt32(dr["Quantity"]),
                            Price = Convert.ToInt32(dr["Price"]),
                        },
                        Category = new CategoryModel {
                            Name = Convert.ToString(dr["Name"]),
                            Description = Convert.ToString(dr["Description"])
                        }
                    }
                );
            }
            return productlist;

        }

        //Dopdownlist for categories
        public SelectList GetCategoryList()
        {
            connection();
            List<SelectListItem> categorylist = new List<SelectListItem>();
            SqlCommand cmd = new SqlCommand("GetCategoryList", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    categorylist.Add(
                        new SelectListItem
                        {
                            Value = Convert.ToString(dr["CategoryID"]),
                            Text = Convert.ToString(dr["Name"])
                        }
                    );
                }
            }
            else
            {
                categorylist.Add(new SelectListItem() { Text = "--none--", Value = "" });
            }

            return new SelectList(categorylist, "Value", "Text", null);
        }

        //Update Product detils
        public bool UpdateProductDetails(ProductModel product, int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateProductDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ProductID", id);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@CategoryID", product.CategoryId);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Delete Product records
        public bool DeleteProduct(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteProduct", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProductID", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Check if Product already exists
        public bool IfProductExist(string name)
        {
            connection();
            SqlCommand cmd = new SqlCommand("IfProductExist", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", name);
            con.Open();
            int i = (int)cmd.ExecuteScalar();
            con.Close();
            cmd.Dispose();
            
            if (i > 0)
                return true;
            else
                return false;
        }

        //Get product records in DataSet
        public DataSet GetProductList()
        {
            connection();
            DataSet dataSet = new DataSet();
            SqlCommand cmd = new SqlCommand("GetProduct", con)
            {
                CommandType = CommandType.StoredProcedure

            };
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.SelectCommand = cmd;
            sda.Fill(dataSet);
            return dataSet;
        }
    }
}