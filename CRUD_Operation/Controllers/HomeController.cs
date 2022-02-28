using CRUD_Operation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD_Operation.Pagination;

namespace CRUD_Operation.Controllers
{
     
    public class HomeController : Controller
    {

        DEMOEntities db = new DEMOEntities();
        public ActionResult Index()
        {
            List<Category> CatList = db.Categories.ToList();
            ViewBag.ListOfCategory = new SelectList(CatList, "CategoryId", "CategoryName");
            return View();

        }

        public JsonResult GetProductList(int pageNumber=1,int pageSize=20)
        {
            List<ProductViewModel> ProdList = db.Products.Select(x => new ProductViewModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                CategoryId = x.Category.CategoryId,
                CategoryName = x.Category.CategoryName
            }).ToList();

            var PagedData = Pagination.Pagination.PagedResult(ProdList, pageNumber, pageSize);

            return Json(PagedData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductById(int ProductId)
        {
            Product model = db.Products.Where(x => x.ProductId == ProductId).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveProductDetails(ProductViewModel model)
        {
            var result = false;
            try
            {
                if (model.ProductId > 0)
                {
                    Product Prod = db.Products.SingleOrDefault(x => x.ProductId == model.ProductId);
                    Prod.ProductName = model.ProductName;
                    Prod.CategoryId = model.CategoryId;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    Product prod = new Product();
                    prod.ProductName = model.ProductName;
                    prod.CategoryId = model.CategoryId;
                    db.Products.Add(prod);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProductDetails(int ProductId)
        {
            bool result = false;
            Product prod = db.Products.SingleOrDefault(x => x.ProductId == ProductId);
            if (prod != null)
            {
                db.Products.Remove(prod);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}