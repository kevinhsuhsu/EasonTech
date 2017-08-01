using Eason.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Eason.Controllers
{
    public class CustomerController : Controller
    {
        public CustomerController()
        {
            UserInfo.Function = "Customer";
        }
        public ActionResult Index()
        {
            Customer vCust = new Customer();
            List<Customer> CustList = vCust.View<Customer>(vCust.getSQLStatement());
            return View(CustList);
        }
        public JsonResultPro LoadData()
        {
            Customer vCst = new Customer();
            List<Customer> CstList = vCst.View<Customer>(vCst.getSQLStatement());

            return new JsonResultPro(
                    new { data = CstList },
                    JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Customer vCust = new Customer();

                if(TryUpdateModel(vCust))
                {
                    vCust.Insert<Customer>(collection);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(vCust);
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(string serno)
        {
            Customer vCust = new Customer();
            vCust.Conditions = vCust.getCondition(Customer.ncConditions.CST_SERNO.ToString(), serno);
            List<Customer> CustList = vCust.View<Customer>(vCust.getSQLStatement());

            return View(CustList.First());
        }

        [HttpPost]
        public ActionResult Edit(string serno, FormCollection collection)
        {
            try
            {
                Customer vCust = new Customer();

                if (TryUpdateModel(vCust))
                {
                    vCust.Insert<Customer>(collection);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(vCust);
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
                Customer vCust = new Customer();
                vCust.Delete<Customer>(serno, collection);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
