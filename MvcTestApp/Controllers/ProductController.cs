using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcTestApp.Models;

namespace MvcTestApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private ProductDBHandle dbHandler = new ProductDBHandle();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        // GET: View Product
        public ActionResult ViewProduct()
        {

            return View(dbHandler.GetProduct());
        }

        // GET: Add Product
        public ActionResult AddProduct()
        {

            ViewBag.Category = dbHandler.GetCategoryList();
            return View();
        }

   
        //POST: Add Product
        [HttpPost]
        public ActionResult AddProduct([Bind(Include= "Name, CategoryId, Quantity, Price")] ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!dbHandler.IfProductExist(product.Name))
                    {
                        if (dbHandler.AddProduct(product))
                        {
                            ViewBag.Message = "Product Details Added Successfully";
                            ModelState.Clear();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Product Already exists";
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            ViewBag.Category = dbHandler.GetCategoryList();
            return View();
        }

        //GET: Edit Product/{id}/
        public ActionResult EditProduct(int id)
        {
            ViewBag.Category = dbHandler.GetCategoryList();
            List<ProductModel> products = new List<ProductModel>();
            foreach(ProductListModel product in dbHandler.GetProduct())
            {
                products.Add(
                    new ProductModel
                    {
                        ProductId = product.Product.ProductId,
                        Name = product.Product.Name,
                        CategoryId = product.Product.CategoryId,
                        Quantity = product.Product.Quantity,
                        Price = product.Product.Price,
                    }
                );
            }
            return View(products.Find(product => product.ProductId == id));
        }

        //POST: Edit Product/{id}/
        [HttpPost]
        public ActionResult EditProduct(int id, ProductModel product)
        {
            try
            {
                dbHandler.UpdateProductDetails(product, id);
                ViewBag.Category = dbHandler.GetCategoryList();
                return RedirectToAction("ViewProduct");
            }
            catch
            {
                return View();
            }
        }

        // GET: Delete Product
        public ActionResult Delete(int id)
        {
            try
            {
                if (dbHandler.DeleteProduct(id))
                {
                    ViewBag.AlertMsg = "Student Deleted Successfully";
                }
                return RedirectToAction("ViewProduct");
            }
            catch
            {
                ViewBag.AlertMsg = "Error deleting record";
                return View();
            }
        }

        // POST: Export as Excel
        [HttpPost]
        public ActionResult ExportExcel()
        {
            DataSet ds = dbHandler.GetProductList();
            StringBuilder sb = new StringBuilder();

            sb.Append("<table>");

            //Get column names
            var columnName = ds.Tables[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            sb.Append("<tr>");
            //Insert column names
            foreach (var col in columnName)
                sb.Append("<td>" + col + "</td>");
            sb.Append("</tr>");

            //Insert table recors
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    sb.Append("<td>" + dr[dc] + "</td>");
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            //Writing StringBuilder content to an excel file.
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=ProductList.xls");
            Response.Write(sb.ToString());
            Response.Flush();
            Response.Close();
            return View();
        }

        //POST: Export CSV
        [HttpPost]
        public ActionResult ExportCsv()
        {
            StringBuilder sb = new StringBuilder();
            List<ProductListModel> productlist = dbHandler.GetProduct().ToList<ProductListModel>();
            ProductListModel pd = new ProductListModel(); 
            sb.Append("Product Name, Quantity, Price, Category, Category Description");
            foreach (ProductListModel product in productlist)
            {
                sb.AppendLine();
                sb.AppendFormat("{0},{1},{2},{3},{4}",
                    product.Product.Name,
                    product.Product.Quantity,
                    product.Product.Price,
                    product.Category.Name,
                    product.Category.Description
                );
            }
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Headers.Add("Content-Disposition", "attachment;filename=ProductList.csv");
            Response.Write(sb.ToString());
            Response.End();
            return View();
        }
    }
}