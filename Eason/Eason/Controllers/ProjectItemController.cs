using Eason.Models;
using Eason.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eason.Controllers
{
    [Authorize]
    public class ProjectItemController : Controller
    {
        List<SelectListItem> progress;
        DropDownList<Employee> EmployeeList;
        public ActionResult Index(ProjectItemViewModel viewModel, string PJT_Serno)
        {
            if (viewModel.SearchParameter == null)
            {
                viewModel.SearchParameter = new ProjectItemSearchModel();
                viewModel.SearchParameter.PJT_SERNO = PJT_Serno;
            }
            var result = new ProjectItemViewModel
            {
                SearchParameter = viewModel.SearchParameter
            };

            return View(result);
        }
        [HttpPost]
        public JsonResultPro LoadData(string frm_data)
        {
            Dictionary<string, string> collection = new JsonToDictionary(frm_data).Dictionary;

            ProjectItem vProj = new ProjectItem();
            vProj.Conditions = " 1=1 ";
            if (!string.IsNullOrEmpty((string)collection["PJT_SERNO"]))
                vProj.Conditions += " AND " + vProj.getCondition(ProjectItem.ncConditions.PJT_SERNO.ToString(), (string)collection["PJT_SERNO"]);

            List<ProjectItem> ProjList = vProj.View<ProjectItem>(vProj.getSQLStatement());

            return new JsonResultPro(
                            new { data = ProjList },
                            JsonResultPro.DateTimeFormatType.ShortDateTime,
                            JsonRequestBehavior.DenyGet
                        );
        }

        public ActionResult Create(string PJT_Serno)
        {
            ProjectItem vProj = new ProjectItem
            {
                PJT_SERNO = PJT_Serno
            };

            ddlInit();
            
            return View(vProj);
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                ProjectItem vProj = new ProjectItem();

                if (TryUpdateModel(vProj))
                {

                    vProj.Insert<ProjectItem>(collection);

                    return RedirectToAction("Index", new { PJT_Serno = collection["PJT_SERNO"] });
                }
                else
                {
                    ddlInit();

                    return View(vProj);
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(string serno)
        {
            
            ProjectItem vProj = new ProjectItem();
            vProj.Conditions = vProj.getCondition(ProjectItem.ncConditions.PJI_SERNO.ToString(), serno);
            List<ProjectItem> ProjList = vProj.View<ProjectItem>(vProj.getSQLStatement());
            
            ddlInit();

            return View(ProjList.First());
        }
        [HttpPost]
        public ActionResult Edit(string serno, FormCollection collection)
        {
            try
            {
                ProjectItem vProj = new ProjectItem();

                if (TryUpdateModel(vProj))
                {
                    vProj.Update<ProjectItem>(serno, collection);

                    return RedirectToAction("Index", new { PJT_Serno = collection["PJT_SERNO"] });
                }
                else
                {
                    ddlInit();

                    return View(vProj);
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(string serno)
        {
            
            ProjectItem vProj = new ProjectItem();
            vProj.Conditions = vProj.getCondition(ProjectItem.ncConditions.PJI_SERNO.ToString(), serno);
            List<ProjectItem> ProjList = vProj.View<ProjectItem>(vProj.getSQLStatement());

            return View(ProjList.First());
        }
        [HttpPost]
        public ActionResult Delete(string serno, FormCollection collection)
        {
            try
            {
                ProjectItem vProj = new ProjectItem();
                vProj.Delete<ProjectItem>(serno, collection);

                return RedirectToAction("Index", new { PJT_Serno = TempData["PJT_SERNO"] });
            }
            catch
            {
                return View();
            }
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
        }
    }
}
