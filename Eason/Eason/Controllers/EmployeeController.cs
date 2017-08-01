using Eason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Eason.Utils;

namespace Eason.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        DropDownList TitleList;
        public EmployeeController()
        {
            UserInfo.Function = "Employee";
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResultPro LoadData()
        {
            Employee vEmp = new Employee();
            List<Employee> Emp = vEmp.View<Employee>(vEmp.getSQLStatement());
            // 不顯示 ADMIN
            Emp.Remove(Emp.Single(model => model.EMP_TITLE == Eason.Enums.Title.ADMIN.ToString()));

            return new JsonResultPro( 
                    new { data = Emp }, 
                    JsonRequestBehavior.AllowGet
                );
        }

        [AjaxOnly]
        public PartialViewResult Create()
        {
            ddlInit();
            return PartialView("_Create");
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Encrypt encrypt = new Encrypt(collection["EMP_PASSWORD"]);
                collection["EMP_PASSWORD"] = encrypt.Result;

                Employee vEmp = new Employee();
                if(TryUpdateModel(vEmp))
                {

                    vEmp.Insert<Employee>(collection);

                    return RedirectToAction("Index");
                }
                else 
                {
                    return View(vEmp);
                }
            }
            catch
            {
                return View();
            }
        }


        public PartialViewResult Edit(string serno)
        {
            ddlInit();

            if (string.IsNullOrEmpty(serno))
                serno = UserInfo.UserSerno;

            Employee vEmp = new Employee();
            vEmp.Conditions = vEmp.getCondition(Employee.ncConditions.EMP_SERNO.ToString(), serno);

            List<Employee> EmpList = vEmp.View<Employee>(vEmp.getSQLStatement());

            return PartialView("_Edit",EmpList.First());
        }

        [HttpPost]
        public ActionResult Edit(string serno, FormCollection collection)
        {
            try
            {
                if (string.IsNullOrEmpty(serno))
                    serno = UserInfo.UserSerno;

                Encrypt encrypt = new Encrypt(collection["EMP_PASSWORD"]);
                collection["EMP_PASSWORD"] = encrypt.Result;

                Employee Emp = new Employee();
                if(TryUpdateModel(Emp))
                {
                    
                    Emp.Update<Employee>(serno,collection);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(Emp);
                }

            }
            catch
            {
                return View();
            }
        }

        public PartialViewResult Delete(string serno)
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Delete(string serno, FormCollection collection)
        {
            try
            {
                Employee vEmp = new Employee();
                vEmp.Delete<Employee>(serno, collection);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        private void ddlInit()
        {
            #region Title
            Dictionary<string, string> titles = new Dictionary<string, string>();
            foreach(string title in Enum.GetNames(typeof(Enums.Title)))
            {
                titles.Add(title,title);
            }
            titles.Remove("ADMIN");
            TitleList = new DropDownList(titles);
            ViewBag.ddlTitle = TitleList.SelectListItem; 
            #endregion
        }
    }
}
