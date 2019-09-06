using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QC.MF.Web.Controllers
{
    public class DocumentController : Controller
    {
        private const string DocumentPath = "\\App_data\\Documents";
        private const string DefaultDocument = "\\1. 目录.md";
        // GET: Document
        public ActionResult Index(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = DefaultDocument;
            }
            else
            {
                path = HttpUtility.UrlDecode(path);
            }
            var realpath = Server.MapPath(DocumentPath+path);
            using (var inputStream = System.IO.File.Open(realpath, FileMode.Open))
            {
                var reader = new StreamReader(inputStream);
                ViewBag.Content = reader.ReadToEnd();
            }
            ViewBag.Title=Path.GetFileName(path);
            return View();
        }
    }
}
