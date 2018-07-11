using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
    public class CustomersController : Controller
    {
        private Manager m = new Manager();
        // GET: Customers
        public ActionResult Index()
        {
            return View(m.CustomersGetAllWithBicycles());
        }

        [Route("customers/customerCompletedSearch/{searchExp}")]
        public ActionResult CustomerSearch(string searchExp)
        {
            var found = m.CustomerGetBySearchString(searchExp);
            if (found == null) { found = new List<CustomerGeneral>(); }
            return PartialView("_CustomerIndexTable", found);
        }

        // GET: Customers/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        public ActionResult Details(int? Id)
        {
            var it = m.CustomerDetailsById(Id.GetValueOrDefault());
            if (it == null)
            {

                return HttpNotFound();
            }
            else
            {
                return View(it);
            }
        }

        // GET: Customers/Create *** modified to add customer
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create ***modified to add customer
        [HttpPost]
        public ActionResult Create(CustomerAdd newItem)
        {
            //validate the input
            if (!ModelState.IsValid) {
                return View(newItem);
            }

            //process the inputted data from form
            var addedCust = m.CustomerAddNew(newItem);

            if (addedCust == null)
            {

                return View(newItem);

            }
            else {

                return RedirectToAction("Details", new { id = addedCust.Id });
            }

            
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            // Attempt to fetch the matching object

            var form = m.CustomerGetByIdForEdit(id);

            if (form == null)
            {
                return HttpNotFound();
            }
            else
            {
               

                return View(form);
            }
        }

        // POST: Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, CustomerEdit newItem)
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
            var editedItem = m.CustomerEdit(newItem);

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

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Customers/Delete/5
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

        // GET: Bicycles/Create
        [Route("Customers/AddBicycle/{id}")]
        public ActionResult AddBicycle(int? id)
        {
            var c = m.CustomerGetByIdMaybe(id.GetValueOrDefault());

            if(c == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new BicycleAddForCreateForm();
                form.CustomerId = c.Id;
                form.CustomerFirstName = c.FirstName;
                form.CustomerLastName = c.LastName;
                form.CustomerEmail = c.Email;
                form.CustomerPhone = c.Phone;
                return View(form);
            }

        }

        // POST: Bicycles/Create
        [Route("Customers/AddBicycle/{id}")]
        [HttpPost]
        public ActionResult AddBicycle(BicycleAddForCreate newItem)
        {

            //validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            //process the inputted data from form
            var addedBike = m.BicycleAddNew(newItem);

            if (addedBike == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("Details", "Bicycles", new { id = addedBike.Id });
            }
        }

       [Route("Customers/getbikesforcustomer/{Id}")]
       public ActionResult GetBikesForCustomer(int? Id)
        {
            var foundThem = m.BicycleGetByCustIdForDetails(Id.Value);
                        if (foundThem == null)
                        { return HttpNotFound();
                        }
                      else {
                            return PartialView("_NBikeList", foundThem);
                        }
                  }

        [Route("Customers/getworkordersforcustomer/{Id}")]
        public ActionResult GetWorkOrdersForCustomer(int? Id)
        {
            var customerWorkOrders = m.WorkOrdersByCustomerId(Id.Value);
                       if (customerWorkOrders == null)
            {
                           return HttpNotFound();
                       }
                       else { return PartialView("_WorkOrdersByCustomer", customerWorkOrders); };
           
            }

        [Route("Customers/getinvoicesforcustomer/{Id}")]
        public ActionResult GetInvoicesForCustomer(int? Id) {
            var customerInvoices = m.GetInvoiceByCustId(Id.Value);
            if (customerInvoices == null)
            {
                return HttpNotFound();
            }
            else {
                //problem seems to be here < --- get internal server error here....
                return PartialView("_InvoicesForCustomer", customerInvoices);
            }
        }
    }
}
