using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
    public class ModelController : Controller
    {
        private Manager m = new Manager();
        // GET: Model
        public ActionResult Index()
        {
            return View(m.ModelsGetAll());
        }

        // GET: Model/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Model/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Model/Create
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

        // GET: Manufacturer/Edit/5
        public ActionResult Edit(int? id)
        {
            var form = m.ModelGetByIdForEdit(id);

            if (form == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(form);
            }
        }

        // POST: Manufacturer/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, ModelEdit newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("edit", new { id = newItem.Id });
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {
                return RedirectToAction("index");
            }

            var editedItem = m.ModelEdit(newItem);

            if (editedItem == null)
            {
                return RedirectToAction("edit", new { id = newItem.Id });
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        // GET: Model/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Model/Delete/5
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
