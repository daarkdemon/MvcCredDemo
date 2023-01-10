using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MvcTestApp.Models;

namespace MvcTestApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private CategoryDBHandle dbHandler = new CategoryDBHandle();

        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }

        // GET: View Category
        public ActionResult ViewCategory()
        {
            return View(dbHandler.GetCategory());
        }

        // GET: View Category
        public ActionResult AddCategory()
        {
            return View();
        }

        //POST: Add Category
        [HttpPost]
        public ActionResult AddCategory(CategoryModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!dbHandler.IfCategoryExist(category.Name))
                    {
                        if (!dbHandler.AddCategory(category))
                        {
                            ViewBag.Message = "Category Details Added Successfully";
                            ModelState.Clear();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Category already exists";
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }

        //Get Edit Category
        public ActionResult EditCategory(int id)
        {
            return View(dbHandler.GetCategory().Find(category => category.CategoryId == id));
        }

        //POST: Edit Category
        [HttpPost]
        public ActionResult EditCategory(int id, CategoryModel category)
        {
            try
            {
                dbHandler.UpdateCategoryDetails(category, id);
                ViewBag.Message = "Data updated successfully";
                return RedirectToAction("ViewCategory");
            }
            catch
            {
                ViewBag.Message = "Something went wrong";
                return View();
            }
        }

        //GET: Delet Category
        public ActionResult Delete(int id)
        {
            try
            {
                if (dbHandler.DeleteCategory(id))
                {
                    ViewBag.AlertMsg = "Category Deleted Successfully";
                }
                return RedirectToAction("ViewCategory");
            }
            catch
            {
                return View();
            }
        }

        //Export as CSV
        [HttpPost]
        public ActionResult ExportCsv()
        {
            StringBuilder sb = new StringBuilder();
            List<CategoryModel> categorylist = dbHandler.GetCategory();
            CategoryModel pd = new CategoryModel();
            sb.Append("Name, Description");
            foreach (CategoryModel category in categorylist)
            {
                sb.AppendLine();
                sb.AppendFormat("{0},{1}",
                    category.Name,
                    category.Description
                );
            }
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Headers.Add("Content-Disposition", "attachment;filename=CategoryList.csv");
            Response.Write(sb.ToString());
            Response.End();
            return View();
        }
    }
}