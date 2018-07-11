using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
  public class MechanicsController : Controller
  {
    private Manager m = new Manager();

    // GET: Mechanics
    public ActionResult Index()
    {
      return View(m.MechanicsGetWithWorkOrders());
    }

    // GET: Mechanics/Details/5
    public ActionResult Details(int? id)
    {
      var it = m.MechanicGetByIdWithWorkOrders(id.GetValueOrDefault());
      if (it == null)
      {

        return HttpNotFound();
      }
      else
      {
        return View(it);
      }
    }

    // GET: Mechanics/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Mechanics/Create
    [HttpPost]
    public ActionResult Create(MechanicBase newMech)
    {
            if (!ModelState.IsValid) {
                return View(newMech);
            }

            bool isUnique = m.isMechanicUnique(newMech);

            if (!isUnique)
            {
                string errorMsg = "Duplicate entry. That phone or email already exists.";
                @TempData["SubmitErrorMsg"] = errorMsg;
                return View(newMech);
            }

            var addedMech = m.MechanicAddNew(newMech);

            if (addedMech == null)
            {

                return View(newMech);
            }
            else {
                return RedirectToAction("Details", new { id = addedMech.Id });
            }
    }

    // GET: Mechanics/Edit/5
    public ActionResult Edit(int? id)
    {
            var o = m.MechanicGetById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            else {
                var editForm = m.mapper.Map<MechanicBase>(o);
                return View(editForm);
            }

    }

        // POST: Mechanics/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, MechanicBase newItem)
        {
            //validate the input
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id = newItem.Id });
            }

            if (id.GetValueOrDefault() != newItem.Id)
            {

                return RedirectToAction("index");
            }

            bool isUnique = m.isMechanicUnique(newItem);

            if (!isUnique)
            {
                string errorMsg = "Duplicate entry. That phone or email already exists. Resetting info.";
                @TempData["SubmitErrorMsg"] = errorMsg;
                return RedirectToAction("Edit", new { id = newItem.Id });
            }

            //attempt to do the update
            var editedItem = m.MechanicEdit(newItem);

            if (editedItem == null)
            {
                //there is a problem updating the object
                return RedirectToAction("Edit", new { id = newItem.Id });

            }
            else
            {
                return RedirectToAction("Details", new { id = newItem.Id });
            }
            
        }

        // GET: Mechanics/Delete/5
        public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: Mechanics/Delete/5
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
