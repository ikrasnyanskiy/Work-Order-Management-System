using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
    public class ReportsController : Controller
    {
        private Manager m = new Manager();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        [Route("reports/serviceitempopularity")]
        public ActionResult ServiceItemPopularity()
        {
            var sipopularityreport = m.ReportServiceItemPopularity();
            return View(sipopularityreport);
        }
    public ActionResult ManufacturerPopularity()
    {
      var manupopularityreport = m.ReportManufacturerPopularity();
      return View(manupopularityreport);
    }

    [Route("reports/mechanicreports")]
    public ActionResult MechanicReports()
    {
      var list = m.ReportMechAsgntWeek();
      return View(list);
    }

    [Route("reports/MechAsgtWeekly")]
    public ActionResult MechAsgtWeekly()
    {
      var list = m.ReportMechAsgntWeek();
      return PartialView("_MechAsgtWeek", list);
    }
    [Route("reports/MechAsgtMonthly")]
    public ActionResult MechAsgtMonthly()
    {
      var list = m.ReportMechAsgntMonth();
      return PartialView("_MechAsgtWeek", list);
    }
      
    [Route("reports/MechAsgtYearly")]
    public ActionResult MechAsgtYearly()
    {
      var list = m.ReportMechAsgntYear();
    return PartialView("_MechAsgtWeek", list);

  }

    public ActionResult InvoiceReports()
    {
      var invReport = m.ReportOnInvoices();
      return View(invReport);
    }
  //// GET: Reports/Details/5
  //public ActionResult Details(int id)
  //{
  //    return View();
  //}

  //// GET: Reports/Create
  //public ActionResult Create()
  //{
  //    return View();
  //}

  //// POST: Reports/Create
  //[HttpPost]
  //public ActionResult Create(FormCollection collection)
  //{
  //    try
  //    {
  //        // TODO: Add insert logic here

  //        return RedirectToAction("Index");
  //    }
  //    catch
  //    {
  //        return View();
  //    }
  //}

  //// GET: Reports/Edit/5
  //public ActionResult Edit(int id)
  //{
  //    return View();
  //}

  //// POST: Reports/Edit/5
  //[HttpPost]
  //public ActionResult Edit(int id, FormCollection collection)
  //{
  //    try
  //    {
  //        // TODO: Add update logic here

  //        return RedirectToAction("Index");
  //    }
  //    catch
  //    {
  //        return View();
  //    }
  //}

  //// GET: Reports/Delete/5
  //public ActionResult Delete(int id)
  //{
  //    return View();
  //}

  //// POST: Reports/Delete/5
  //[HttpPost]
  //public ActionResult Delete(int id, FormCollection collection)
  //{
  //    try
  //    {
  //        // TODO: Add delete logic here

  //        return RedirectToAction("Index");
  //    }
  //    catch
  //    {
  //        return View();
  //    }
  //}
}
}
