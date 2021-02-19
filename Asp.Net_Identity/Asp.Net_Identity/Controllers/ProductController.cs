using Asp.Net_Identity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.ProductService;
using DataConnection;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;

namespace Asp.Net_Identity.Controllers
{
    public class ProductController : Controller
    {
        private IProductServiceLayer _ProductService;

        public ProductController(IProductServiceLayer ProductService)
        {
            _ProductService = ProductService;
        }
        // GET: Profile
        [Authorize]
        public ActionResult Index(string SearchText = "")
        {
            ViewBag.ListOfProducts = _ProductService.ReadProducts(SearchText);
            ViewBag.SearchText = SearchText;
            return View();
        }

        public ActionResult AddProduct(Product product, string ErrorMessage = "")
        {
            ViewBag.ErrorMessage = ErrorMessage;
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProduct(Product product)
        {
            if(product.Name == null || product.Price == 0)
            {
                return RedirectToAction("AddProduct", new { ErrorMessage = "Name and Price Cannot Be Empty"});
            }
            string BaseDomain = AppDomain.CurrentDomain.BaseDirectory;
            if (Request.Files["files"].ContentLength > 0)
            {
                string path = @"/Uploads/ProductPhotos" + (DateTime.Now.ToString("dd_MM_yyy_HH_mm_ss_ms")).ToString();
                string CompletelyPath = BaseDomain + path;
                bool FileExists = System.IO.Directory.Exists(CompletelyPath);
                string host = "";
                if (string.IsNullOrEmpty(host))
                {
                    var uri = HttpContext.Request.Url;
                    host = uri.GetLeftPart(UriPartial.Authority);
                }

                if (!FileExists)
                    System.IO.Directory.CreateDirectory(CompletelyPath);
                string FileName;
                string FileUrl;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    FileName = Path.GetFileName(Request.Files[i].FileName);
                    FileUrl = host + path + "/" + FileName.Replace(" ", "");
                    Request.Files[i].SaveAs(Server.MapPath(path + "/" + FileName.Replace(" ", "")));
                    product.Photo = FileUrl;
                }
            }
            var Result = _ProductService.SaveProduct(product);
            if (Result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Content("Failed");
            }
        }

        public ActionResult DeleteProduct(Product product)
        {
            var Result = _ProductService.DeleteProduct(product);
            if (Result)
            {
                return RedirectToAction("Index");
            }else
            {
                return Content("Failed");
            }
        }


        public ActionResult ViewProduct(Product product)
        {
            return View(product);
        }

        public string GetUserId()
        {
            return System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId()).Id;
        }

        public ActionResult ExportToExcel(string SearchText ="")
        {
            var Products = _ProductService.ReadProducts(SearchText);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Name";
            Sheet.Cells["B1"].Value = "Price";
            Sheet.Cells["C1"].Value = "Photo";
            Sheet.Cells["D1"].Value = "Last Updated";
            int row = 2;
            foreach (var item in Products)
            {
                Sheet.Cells[string.Format("A{0}", row)].Value = item.Name;
                Sheet.Cells[string.Format("B{0}", row)].Value = item.Price;
                Sheet.Cells[string.Format("C{0}", row)].Value = item.Photo;
                Sheet.Cells[string.Format("D{0}", row)].Value = item.LastUpdated;
                row++;
            }


            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
            return Content("");
        }
    }
}