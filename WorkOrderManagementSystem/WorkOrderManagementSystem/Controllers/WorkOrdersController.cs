using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{

  public class WorkOrdersController : Controller

  {
    private Manager m = new Manager();
    // GET: Work All Active General Orders
    public ActionResult Index()
    {
      return View(m.WorkOrdersGetActive());
    }

    // GET: All active and inactive Work Orders
    public ActionResult GetAllWorkorders()
    {
      return View(m.WorkOrdersGetAll());
    }

    public ActionResult GetAllCompletedWO()
    {
      return View(m.WorkOrdersGetCompleted());
    }



    // GET: WorkOrders/Details/5
    public ActionResult Details(int? Id)
    {
      var it = m.WorkOrderDetailsById(Id.GetValueOrDefault());
      if (it == null)
      {

        return HttpNotFound();
      }
      else
      {
        return View(it);
      }
    }

    //--controllers to service AJAX requests in workorder creation/edit forms [works with view accessed through Create() method below this section]--------------------

    //UPDATE WorkOrder GET ALL list
    [Route("workorders/workorderSearch/{searchExp}")]
    public ActionResult WorkOrderSearch(string searchExp)
    {
      var found = m.WorkOrderGetByString(searchExp);
      if (found == null) { found = new List<WorkOrderGeneral>(); }
      return PartialView("_WorkOrderIndexTable", found);
    }
    [Route("workorders/workorderCompletedSearch/{searchExp}")]
    public ActionResult WorkOrderCompletedSearch(string searchExp)
    {
      var found = m.WorkOrderGetCompletedByString(searchExp);
      if (found == null) { found = new List<WorkOrderGeneral>(); }
      return PartialView("_WorkOrderCompletedTable", found);
    }
    [Route("workorders/workorderAllSearch/{searchExp}")]
    public ActionResult WorkOrderAllSearch(string searchExp)
    {
      var found = m.WorkOrderGetAllByString(searchExp);
      if (found == null) { found = new List<WorkOrderGeneral>(); }
      return PartialView("_WorkOrderAllTable", found);
    }

    //UPDATE customer select lists:
    [Route("workorders/customerSearch/{searchExp}")]
    public ActionResult CustomerSearch(string searchExp = "")
    {
      var found = m.CustomerGetByString(searchExp);
      if (found == null) { found = m.CustomersGetAll(); }//TODO: check if safe when search box empty
                                                         // Create a new selectlist (this is a partial view)
      var newList = new WorkOrderPartial();//uses the same view model class as the view we called the ajax function from
      newList.CustomerSearchList = new SelectList(found, dataValueField: "Id", dataTextField: "CustListLabel");//set the partial view's selectList to hold our search results

      return PartialView("_CustList", newList);
    }
    //UPDATE bicycle select list for customer of specified id
    [Route("workorders/bicycleSearch/{custId}")]
    public ActionResult BicycleSearch(int? custId)
    {
      var found = m.BicycleGetByCustId(custId.GetValueOrDefault());
      if (found == null) { found = m.BicyclesGetWithManuModel(); }//TODO: check if safe when search box empty
                                                                  // Create a new selectlist (this is a partial view)
      var newList = new WorkOrderAddForm();//uses the same view model class as the view we called the ajax function from
      newList.BicycleSearchList = new SelectList(found, dataValueField: "Id", dataTextField: "BikeListLabel");//set the partial view's selectList to hold our search results

      return PartialView("_BikeList", newList);
    }

    //UPDATE mechanic select lists:
    [Route("workorders/mechanicSearch/{searchExp}")]
    public ActionResult MechanicSearch(string searchExp = "")
    {
      IEnumerable<MechanicWithWorkOrders> found = m.MechanicGetByString(searchExp);
      if (found == null) { found = m.MechanicsGetWithWorkOrders(); }//TODO: consider changing this so customer list is repopulated to being empty on workorder form
                                                         // Create a new selectlist (this is a partial view)
      var newList = new WorkOrderPartial();//uses the same view model class as the view we called the ajax function from
      
      newList.MechanicSearchList = new SelectList(found, dataValueField: "Id", dataTextField: "MechListLabel");//set the partial view's selectList to hold our search results

      return PartialView("_MechList", newList);
    }

    //Change Status by Id
    [Route("workorders/changeStatus/{Id}")]
    public ActionResult ChangeStatus(int Id)
    {
      var theOne = m.WorkOrdersStatusById(Id);

      if (theOne == null)
      {   //if the update did not work, return the error message partial view
        var unchanged = m.WorkOrdersGetActive();
        return PartialView("_WorkOrderError", unchanged);
      }
      else
      {
        IEnumerable<WorkOrderGeneral> changedGroup;
        //Choose which collection to return (all completed or all active) based on the workorder's initial status
        if (theOne.IsCompleted)//was changed from completed to active (false means it's now active)
        {
           changedGroup = m.WorkOrdersGetActive();
        }
        else
        {
          changedGroup = m.WorkOrdersGetCompleted();
        }
       
        return PartialView("_WorkOrderIndexTable", changedGroup);
      }
    }
    //Change Status by Id (from workorder all view -- needed because other function will return either collections of only active, or only completed)
    [Route("workorders/changeStatusFromAllView/{Id}")]
    public ActionResult ChangeStatusFromAllView(int Id)
    {
      var theOne = m.WorkOrdersStatusById(Id);

      if (theOne == null)
      {   //if the update did not work, return the error message partial view
        var unchanged = m.WorkOrdersGetActive();
        return PartialView("_WorkOrderError", unchanged);
      }
      else
      {
        IEnumerable<WorkOrderGeneral> changedGroup;
        changedGroup = m.WorkOrdersGetAll();

        return PartialView("_WorkOrderAllTable", changedGroup);
      }
    }
    /*UNUSED: (mech detail view checkboxes to toggle status need a dedicated manager function
     * to not reassign most recent mechanic when toggling ( WorkOrdersStatusById does this right now)
     * 
    //Change Status by Id from a mechanic details view.
    //This view has 2 tables: one for a mechanic's active orders and one
    //for previously assigned ones, so the partial view populates these
    //USES PARTIAL VIEW: _MechDetailWorkOrders
    [Route("workorders/ChangeStatusFromMechView/{mechId}{workOrderId}")]
    public ActionResult ChangeStatusFromMechView(int mechId, int workOrderId)
    {
      //try to change the workorder's status:
      var theWorkOrder = m.WorkOrdersStatusById(workOrderId);

      //Get the mechanic details view object with formatted workorders
      //(just grab the whole object, and use what's needed in the partial view).
      //If anything's changed, it will be returned as changed, and if it hasn't,
      //the partial view will be the same as the original view.
      var returnMe = m.MechanicGetByIdWithWorkOrders(mechId);

      //set an error message if the workorder was not changed
      if (theWorkOrder == null)
      {   //if the update did not work, return the error message partial view
        returnMe.Error = "Error retrieving target work order";
      }
      return PartialView("_MechDetailWorkOrders", returnMe);
    }*/
    //---------------------------------------------

    // GET: WorkOrders/Create : Render a workorder Creation form
    public ActionResult Create()
    {
      var form = new WorkOrderAddForm();
      form.CustomerSearchList = new SelectList(m.CustomersGetAll(), "Id", "CustListLabel");//TODO improve by displaying First and last name, phone and email
      form.BicycleSearchList = new SelectList(new List<BicycleBase>());// empty list
      form.MechanicSearchList = new SelectList(m.MechanicsGetWithWorkOrders(), "Id", "MechListLabel");

      return View(form);
    }

    // POST: WorkOrders/Create
    [HttpPost]
    public ActionResult Create(WorkOrderAdd newOrder)//Automapper conversion from POST of WorkOrderAddForm
    {//TODO: implement form feedback for trying to select a mechanic that is already assigned to a workorder

      //this would perhaps be handled better client side?
      if (!ModelState.IsValid)
      {
        //TODO: repopulate the form select lists, return a form object that contains the user's previous selections
        //supposedly, you can get modelstate to be invalid through custom validation?
        //modelstate.addmodelerror()
        return View(newOrder);
      }
      var addedOrder = m.WorkOrderAdd(newOrder);
      if (addedOrder == null)
      {
        //repopulate with previous form data before returning view!!!
        return View(newOrder);
      }
      else
      {
        return RedirectToAction("edit", new { id = addedOrder.Id });// TODO:  implement detail view and uncomment this line
      }
    }

    // GET
    [Route("workorders/createCustomerThroughWO")]
    public ActionResult CreateCustomerThroughWO()
    {
        return PartialView("_CreateCustomer");
    }

    // POST: Customers/Create ***modified to add customer
    [HttpPost]
    public ActionResult CreateCustomerThroughWO(CustomerAdd newItem)
    {
        //validate the input
        if (!ModelState.IsValid) {
            // return View(newItem);
            return PartialView("_CreateCustomer", newItem);
        }

        //process the inputted data from form
        var addedCust = m.CustomerAddNew(newItem);

        if (addedCust == null)
        {

            //return View(newItem);
            return PartialView("_CreateCustomer");

        }
        else {

           // return RedirectToAction("DetailsCustomer", new { id = addedCust.Id });
            return PartialView("_DetailsCustomer", addedCust);
        }

    }
    
    public ActionResult DetailsCustomer(int? Id)
    {
        var it = m.CustomerDetailsById(Id.GetValueOrDefault());
        if (it == null)
        {

            return HttpNotFound();
        }
        else
        {
            //return View(it);
            return PartialView("_DetailsCustomer", it);
        }
    }

    // GET
    [Route("workorders/createBicycleThroughWO/{id}")]
    public ActionResult CreateBicycleThroughWO(int? id)
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
            return PartialView("_CreateBicycle", form);
        }
    }

    // POST: Customers/Create ***modified to add customer
    [HttpPost]
    public ActionResult CreateBicycleThroughWO(BicycleAddForCreate newItem)
    {
        //validate the input
        if (!ModelState.IsValid) {
            // return View(newItem);
            return PartialView("_CreateBicycle", newItem);
        }

        //process the inputted data from form
        var addedBike = m.BicycleAddNew(newItem);

        if (addedBike == null)
        {
            return PartialView("_CreateBicycle");
        }
        else {
            return PartialView("_DetailsBicycle", addedBike);
        }

    }
    
    public ActionResult DetailsBicycle(int? Id)
    {
        var it = m.BicycleGetByIdWithCustManuModel(Id.GetValueOrDefault());
        if (it == null)
        {
            return HttpNotFound();
        }
        else
        {
            return PartialView("_DetailsBicycle", it);
        }
    }

    // GET: WorkOrders/Edit/5
    public ActionResult Edit(int id)
    {
      var form = m.WorkOrderGetByIdForEdit(id);
      if (form == null)
      {
        return HttpNotFound();
      }
      else
      {
        var bike = m.BicycleGetbyIdWithCustomer(form.BicycleId);
        form.CustomerSearchList = new SelectList(m.CustomersGetAll(), "Id", "CustListLabel", bike.CustomerId);                                                                               
        form.BicycleSearchList = new SelectList(m.BicycleGetByCustId(bike.CustomerId), "Id", "BikeListLabel", form.BicycleId);
        form.MechanicSearchList = new SelectList(m.MechanicsGetWithWorkOrders(), "Id", "MechListLabel", form.ActiveMechanicId);
        form.ServiceItemList = new SelectList(m.ServiceItemsGetAll(), "Id", "Description");
        return View(form);
      }
    }

    // POST: WorkOrders/Edit/5
    [HttpPost]
    public ActionResult Edit(int? id, WorkOrderEdit newItem)
    {
      if (!ModelState.IsValid)
      {//Ivalidator.errorMessage
        string errorMsgs = "";
        foreach (ModelState modelState in ViewData.ModelState.Values)
        {
          foreach (ModelError error in modelState.Errors)
          {
            
            errorMsgs+=error.ErrorMessage + "\n";
          }
        }
        @TempData["SubmitErrorMsg"] = errorMsgs;
        return RedirectToAction("edit", new { id = newItem.Id });//TODO: preserve workorder lines in progress!
      }
      /*was used in POC2:
      //check that a workorder form mechanic is not already assigned
      var mechanics = m.MechanicsGetWithWorkOrders();
      var mechanic = mechanics.SingleOrDefault(m => m.Id == newItem.ActiveMechanicId);
      if (mechanic != null)
      {
        if (mechanic.WorkOrder != null)
        {
          if (mechanic.WorkOrder.Id != newItem.Id)
          {
            @TempData["SubmitErrorMsg"] = mechanic.FirstName + " " + mechanic.LastName + " is already assigned to Work Order #" + mechanic.WorkOrder.Id;
            return RedirectToAction("edit", new { id = newItem.Id });//TODO: preserve workorder lines in progress!
          }
        }
      }
      */

      if (id.GetValueOrDefault() != newItem.Id)
      {
        // This appears to be data tampering, so redirect the user away
        return RedirectToAction("index");
      }

      // Attempt to do the update
      var editedItem = m.WorkOrderEdit(newItem);
      
      //TODO: check for last mechanic
      
      if (editedItem == null)
      {
        return RedirectToAction("edit", new { id = newItem.Id });
      }
      else
      {
        return RedirectToAction("details", new { id = newItem.Id });
      }

      // return RedirectToAction("index");

    }

    // GET: WorkOrders/Delete/5
    public ActionResult Delete(int id)
    {
      return View();
    }

    // POST: WorkOrders/Delete/5
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
