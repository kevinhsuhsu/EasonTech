using Eason.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Eason.ViewModels;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Eason.Controllers
{
    [Authorize]
    public class ProjectListController : Controller
    {
        private List<SelectListItem> progress;
        private DropDownList<Employee> EmployeeList;
        private DropDownList<Customer> CustList;
        private DropDownList typeList;

        public ProjectListController()
        {
            UserInfo.Function = "Project";
        }

        public ActionResult Index(ProjectListViewModel viewModel)
        {
            ddlInit();
            
            var result = new ProjectListViewModel
            {
                SearchParameter = viewModel.SearchParameter,
                PJT_CUSTOMER = CustList.SelectListItem,
                PJT_PARTICIPANT = EmployeeList.SelectListItem,
                PJT_TYPE = typeList.SelectListItem
            };

            return View(result);
        }
        [HttpPost]
        public JsonResultPro LoadData(string frm_data)
        {
            Dictionary<string, string> collection = new JsonToDictionary(frm_data).Dictionary;

            ProjectList vProj = new ProjectList();
            vProj.Conditions = " 1=1 ";
            if (!string.IsNullOrEmpty(collection["PJT_NAME"]))
                vProj.Conditions += " AND " + vProj.getCondition(ProjectList.ncConditions.PJT_NAME.ToString(), (string)collection["PJT_NAME"]);
            if (!string.IsNullOrEmpty(collection["PJT_TYPE"]))
                vProj.Conditions += " AND " + vProj.getCondition(ProjectList.ncConditions.PJT_TYPE.ToString(), (string)collection["PJT_TYPE"]);
            if (!string.IsNullOrEmpty(collection["PJT_PARTICIPANT"]))
                vProj.Conditions += " AND " + vProj.getCondition(ProjectList.ncConditions.PJT_PARTICIPANTS.ToString(), (string)collection["PJT_PARTICIPANT"]);
            if (!string.IsNullOrEmpty(collection["PJT_CUSTOMER"]))
                vProj.Conditions += " AND " + vProj.getCondition(ProjectList.ncConditions.PJT_CUSTOMER.ToString(), (string)collection["PJT_CUSTOMER"]);

            List<ProjectList> ProjList = vProj.View<ProjectList>(vProj.getSQLStatement());

            return new JsonResultPro(
                            new { data = ProjList }, 
                            JsonResultPro.DateTimeFormatType.ShortDateTime, 
                            JsonRequestBehavior.DenyGet
                        );
        }
        public ActionResult Details(string serno)
        {
            return RedirectToAction("Index", "ProjectItem", new { PJT_Serno = serno });
        }
        public ActionResult Create()
        {
            ddlInit();

            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        { 
            try
            {
                ProjectList ProjList = new ProjectList();

                if (TryUpdateModel(ProjList))
                {
                    ProjList.Insert<ProjectList>(collection);
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ddlInit();

                    return View(ProjList);
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(string serno)
        {
            
            ProjectList vProj = new ProjectList();
            vProj.Conditions = vProj.getCondition(ProjectList.ncConditions.PJT_SERNO.ToString(), serno);
            List<ProjectList> ProjList = vProj.View<ProjectList>(vProj.getSQLStatement());

            ddlInit();

            return View(ProjList.First());
        }
        [HttpPost]
        public ActionResult Edit(string serno, FormCollection collection)
        {
            try
            {
                ProjectList ProjList = new ProjectList();

                if (TryUpdateModel(ProjList))
                {

                    ProjList.Update<ProjectList>(serno, collection);

                    return RedirectToAction("Index");
                }
                else
                {
                    ddlInit();

                    return View(ProjList);
                }

            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(string serno)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(string serno, FormCollection collection)
        {
            try
            {
                ProjectList vProj = new ProjectList();
                vProj.Delete<ProjectList>(serno, collection);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public JsonResultPro GetCustomerCode(string CustomerName)
        {
            Customer vCst = new Customer();
            vCst.Conditions = vCst.getCondition(Customer.ncConditions.CST_NAME.ToString(), CustomerName);
            List<Customer> CstList = vCst.View<Customer>(vCst.getSQLStatement());

            return new JsonResultPro(
                new { data = CstList.First().CST_CODE },
                JsonRequestBehavior.DenyGet
                );
        }
        private void ddlInit()
        {
            #region 進度
            progress = new List<SelectListItem>();

            for (int i = 0; i <= 100; i += 5)
            {
                progress.Add(new SelectListItem()
                {
                    Text = string.Format("{0}%", i),
                    Value = i.ToString()
                });
            }
            ViewBag.ProgressItem = progress;
            #endregion

            #region 參與人員
            EmployeeList = new DropDownList<Employee>(new Employee(), "EMP_NAME", "EMP_NAME");
            ViewBag.ddlParticipant = EmployeeList.SelectListItem;
            #endregion

            #region 客戶
            CustList = new DropDownList<Customer>(new Customer(), "CST_NAME", "CST_NAME");
            ViewBag.ddlCustomer = CustList.SelectListItem;
            #endregion

            #region 專案類型
            Dictionary<string, string> types = new Dictionary<string, string>();
            types.Add("需求", "需求");
            types.Add("問題", "問題");
            typeList = new DropDownList(types);
            ViewBag.ddlProjectType = typeList.SelectListItem;
            #endregion
        }
    }
}
