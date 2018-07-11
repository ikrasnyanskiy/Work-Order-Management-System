using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
    public class BicyclesController : Controller
    {
        private Manager m = new Manager();

        // GET: Bicycles
        public ActionResult Index()
        {
            return View();
        }

        // GET: Bicycles/Details/5
        public ActionResult Details(int? id)
        {
            // Attempt to get the matching object
            var o = m.BicycleGetByIdWithCustManuModel(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(o);
            }
        }

        //// GET: Bicycles/Create
        //[Route("Bicycles/Create/{custId}")]
        //public ActionResult Create(int? custId)
        //{
        //    //return View();

        //    //var form = m.CustomerGetByIdForEdit(custId);

        //    //if (form == null)
        //    //{
        //    //    return HttpNotFound();
        //    //}
        //    //else
        //    //{
        //    //    return View(form);
        //    //}
        //    int a = 5;

        //    if (custId == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        //// POST: Bicycles/Create
        //[HttpPost]
        //public ActionResult Create(BicycleAddForCreate newItem)
        //{
           
        //    //validate the input
        //    if (!ModelState.IsValid)
        //    {
        //        return View(newItem);
        //    }

        //    //process the inputted data from form
        //    var addedBike = m.BicycleAddNew(newItem);

        //    if (addedBike == null)
        //    {
        //        return View(newItem);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Details", new { id = addedBike.Id });
        //    }
        //}

        // GET: Bicycles/Edit/5
        public ActionResult Edit(int? id)
        {
            var form = m.BicycleGetByIdForEdit(id);

            if (form == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(form);
            }
        }

        // POST: Bicycles/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, BicycleEdit newItem)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("edit", new { id = newItem.Id });
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {
                // This appears to be data tampering, so redirect the user away
                return RedirectToAction("index");
            }

            // Attempt to do the update
            var editedItem = m.BicycleEdit(newItem);

            if (editedItem == null)
            {
                // There was a problem updating the object
                // Our "version 1" approach is to display the "edit form" again
                return RedirectToAction("edit", new { id = newItem.Id });
            }
            else
            {
                // Show the details view, which will have the updated data
                return RedirectToAction("details", new { id = newItem.Id });
            }
        }
        public ActionResult Deactivate(int id)
        {
            string url = this.Request.UrlReferrer.AbsolutePath;
            try
            {
                m.BicycleDeactivate(id);

                return Redirect(url);
            }
            catch
            {

                return Redirect(url);
            }
        }

        public ActionResult Activate(int id)
        {
            string url = this.Request.UrlReferrer.AbsolutePath;
            try
            {
                m.BicycleActivate(id);

                return Redirect(url);
            }
            catch
            {
                return Redirect(url);
            }
        }
        // GET: Bicycles/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Bicycles/Delete/5
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
