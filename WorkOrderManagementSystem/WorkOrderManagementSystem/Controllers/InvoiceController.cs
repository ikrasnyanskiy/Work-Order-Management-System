using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
    public class InvoiceController : Controller
    {
        private Manager m = new Manager();

        // GET: Invoice
        public ActionResult Index()
        {
            var b = m.GetAllInvoices();
            return View(b);
        }

        //Get: All unpaid Invoices
        public ActionResult GetAllUnPaidInvoices()
        {
            return View(m.GetAllUnpaidInvoices());
        }

        //Get: All paid Invoices
        public ActionResult GetAllPaidInvoices() {
            return View(m.GetAllPaidInvoices());
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            //create a method to get Invoice Details by Id and then change this up
            var it = m.InvoiceDetailsById(id.GetValueOrDefault());
            if (it == null)
            {

                return HttpNotFound();
            }
            else
            {
                return View(it);
            }
        }

        // GET: Invoice/Create <--use this to create invoices
        //need to call action method
        [Route ("Invoice/Create/{id}")]
        public ActionResult Create(int? id)
        {

            //checks for oldInvoice - if there is one, return it is one option. 
            //var oldInv = m.InvoiceDetailsById(id.GetValueOrDefault());
            if (id ==null || id <=0)//used to check modelState
            {
                //return the user to the work order details of that work order instead
                return RedirectToAction("../WorkOrders/GetAllCompletedWO");
            }

            
            //manager method first checks if workorder exists, and may return existing instead of creating new one
            //TODO: need to pass error messages to new redirectpage if it returns null
            var createdInvoice = m.InvoiceCreateFromCompletedWO(id.Value);
            
            if (createdInvoice.Id == 0)
            {
                
                //return to list of invoices 
                //make invoice details a partial and call it from the view. 
                //will allow it to refresh easier
              return RedirectToAction("_InvoiceList");//needs error message on page
            }
            else
            {
                
                        return RedirectToAction("Details", new { id = createdInvoice.Id });
                
            }

            
        }

        // POST: Invoice/Create
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

        // GET: Invoice/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Invoice/Edit/5
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

        // GET: Invoice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Invoice/Delete/5
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

        [Route("Invoice/changepaidstatus/{id}")]
        public ActionResult ChangePaidStatus(int id) {

            //need different method here to call from manager to change the paid stat on ds.
            var theOne = m.InvoiceChangePaidStatusById(id);
            
        return PartialView("_InvoiceMoreDetailed", theOne);
            
        }
    }
}
