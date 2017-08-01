using Eason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Eason.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        List<SelectListItem> progress;
        public HomeController()
        {
            UserInfo.Function = "Bulletin";
        }
        public ActionResult GoogleMap()
        {
            UserInfo.Function = "GoogleMap";

            return View();
        }
        public ActionResult Index()
        {
            return View();
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

                    return RedirectToAction("Index");
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
        }
        public JsonResultPro LoadData(string frm_data)
        {
            Dictionary<string, string> collection = new JsonToDictionary(frm_data).Dictionary;

            ProjectItem vProj = new ProjectItem();
            vProj.Conditions = " 1=1 ";
            vProj.Conditions += " AND " + vProj.getCondition(ProjectItem.ncConditions.PJI_EMPNAME.ToString(), UserInfo.UserName);

            List<ProjectItem> ProjList = vProj.View<ProjectItem>(vProj.getSQLStatement());

            return new JsonResultPro(
                            new { data = ProjList },
                            JsonResultPro.DateTimeFormatType.ShortDateTime,
                            JsonRequestBehavior.DenyGet
                        );
        }

        #region Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(FormCollection loginModel)
        {               
            Employee systemuser = new Employee();

            if (!TryUpdateModel(systemuser))
            {
                return View();
            }

            systemuser.Conditions = systemuser.getCondition(Employee.ncConditions.EMP_NAME.ToString(), loginModel["EMP_NAME"]);

            List<Employee> EmpList = systemuser.View<Employee>(systemuser.getSQLStatement());

            if (EmpList.Count != 0)
            {
                systemuser = EmpList.First();
            }
            else
            {
                systemuser = null;
            }

            if (systemuser == null)
            {
                ModelState.AddModelError("", "請輸入正確的帳號或密碼!");
                return View();
            }

            Encrypt encrypt = new Encrypt(loginModel["EMP_PASSWORD"]);
            loginModel["EMP_PASSWORD"] = encrypt.Result;

            if (systemuser.EMP_PASSWORD.Equals(loginModel["EMP_PASSWORD"]))
            {
                this.LoginProcess(systemuser, loginModel);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "請輸入正確的帳號或密碼!");
                return View();
            }
        }
        private void LoginProcess(Employee user, FormCollection loginModel)
        {
            var now = DateTime.Now;

            var ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.EMP_NAME.ToString(),
                issueDate: now,
                expiration: now.AddMinutes(30),
                isPersistent: false,
                userData: "",
                cookiePath: FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);


            user.Update<Employee>(user.EMP_SERNO, loginModel);

            UserInfo.LoginDTTM = DateTime.Parse(loginModel["EMP_LASTLOGINDTTM"]);
            UserInfo.UserName = user.EMP_NAME;
            UserInfo.Title = user.EMP_TITLE;
            UserInfo.UserSerno = user.EMP_SERNO;
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            //清除所有的 session
            Session.RemoveAll();

            //建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            FormCollection collection = new FormCollection();
            collection.Add("EMP_LASTLOGOUTDTTM",DateTime.Now.ToString());

            Employee vEmp = new Employee();
            vEmp.Update<Employee>(UserInfo.UserSerno, collection);

            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}