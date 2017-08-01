using Eason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Eason.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Create()
        {
            rtProjectManagement rt = new rtProjectManagement();

            return File(Encoding.UTF8.GetBytes(rt.FinalContent), "application/ms-excel", rt.WorkbookName);
        }
    }
}
