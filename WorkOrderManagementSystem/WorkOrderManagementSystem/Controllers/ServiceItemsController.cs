using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
    public class ServiceItemsController : Controller
    {
    Manager m = new Manager();
        // GET: ServiceItems
        public ActionResult Index()
        {

            return View(m.ServiceItemsGetAll());
        }

    [Route("ServiceItems/getPriceById/{id}")]
    public string GetPriceById(int? id)
    {
      return m.GetPriceByServiceItemId(id.Value);
    }

        // GET: ServiceItems/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ServiceItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceItems/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiceItems/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServiceItems/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ServiceItems/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServiceItems/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
