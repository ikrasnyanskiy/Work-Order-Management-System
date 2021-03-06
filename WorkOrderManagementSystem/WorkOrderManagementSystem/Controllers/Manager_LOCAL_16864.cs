﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WorkOrderManagementSystem.Models;
using System.Security.Claims;

namespace WorkOrderManagementSystem.Controllers
{
  public class Manager
  {

    ApplicationDbContext ds = new ApplicationDbContext();


    MapperConfiguration config;
    public IMapper mapper;


    public Manager()
    {
      config = new MapperConfiguration(cfg =>
      {
        //Mappings
        cfg.CreateMap<WorkOrder, WorkOrderBase>();
        cfg.CreateMap<WorkOrderAdd, WorkOrder>();
        cfg.CreateMap<WorkOrderAdd, WorkOrder>();
        cfg.CreateMap<WorkOrderAddForm, WorkOrderAdd>();
        cfg.CreateMap<WorkOrderAdd, WorkOrderAddForm>();

        cfg.CreateMap<WorkOrder, WorkOrderGeneral>();
        cfg.CreateMap<WorkOrderEditForm, WorkOrderEdit>();
        cfg.CreateMap<WorkOrderEdit, WorkOrder>();
        cfg.CreateMap<WorkOrder, WorkOrderEditForm>();
        cfg.CreateMap<WorkOrder, WorkOrderDetails>();
        cfg.CreateMap<WorkOrder, WorkOrderForMechDetail>();

        cfg.CreateMap<WorkLog, WorkLogBase>();



        cfg.CreateMap<Customer, CustomerBase>();
        cfg.CreateMap<Customer, CustomerGeneral>();
        cfg.CreateMap<Customer, CustomerWithDetails>();
        cfg.CreateMap<CustomerAdd, Customer>();     //will use this to add new customer
        cfg.CreateMap<CustomerAdd, BicycleAddNewCustomer>(); //trying to figure the mapper issue - not sure if this is correct
        cfg.CreateMap<CustomerEditForm, CustomerEdit>();
        cfg.CreateMap<CustomerEdit, Customer>();
        cfg.CreateMap<Customer, CustomerEditForm>();

        cfg.CreateMap<Bicycle, BicycleBase>();
        cfg.CreateMap<BicycleAdd, Bicycle>();
        cfg.CreateMap<Bicycle, BicycleWithAssoc>();
        cfg.CreateMap<Bicycle, BicycleAddNewCustomer>();


        cfg.CreateMap<ServiceItem, ServiceItemBase>();
        cfg.CreateMap<ServiceItemAdd, ServiceItem>();

        cfg.CreateMap<WorkOrderLine, WorkOrderLineBase>();
        cfg.CreateMap<WorkOrderLineAdd, WorkOrderLine>();
        cfg.CreateMap<WorkOrderLine, WorkOrderLineDetails>();
        //need map from work order to mechanic?
        cfg.CreateMap<MechanicBase, WorkOrderDetails>();
        cfg.CreateMap<Mechanic, MechanicBase>();
        cfg.CreateMap<Mechanic, MechanicWithWorkOrders>();

        cfg.CreateMap<MechanicAdd, MechanicBase>();

        cfg.CreateMap<Model, ModelBase>();
        cfg.CreateMap<ModelAdd, Model>();

        cfg.CreateMap<Manufacturer, ManufacturerBase>();
        cfg.CreateMap<ManufacturerAdd, Manufacturer>();

      });


      mapper = config.CreateMapper();
      ds.Configuration.ProxyCreationEnabled = false;

      ds.Configuration.LazyLoadingEnabled = false;
    }


    // Methods

    //does not include a list of previous mechanics
    public IEnumerable<WorkOrderGeneral> WorkOrdersGetAll()//must handle inactive mechs
    {

      var returnMe = new List<WorkOrderGeneral>();
      //on construction, completion time is also set to DateTime.Now()!
      var dsWOList = ds.WorkOrders.Include("Bicycle.Customer").Include("WorkLogs.Mechanic").OrderBy(w => w.IsCompleted).ThenBy(w => !w.HighPriority).ThenBy(w => w.CreationTime);

      foreach (var item in dsWOList)
      {
        WorkOrderGeneral addMe = mapper.Map<WorkOrderGeneral>(item);
        // var assocMech = dsMechList.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes pointer in use exception
        foreach (var wl in item.WorkLogs)
        {
          if (wl.IsActiveOnOrder)
          {
            addMe.ActiveMechanic = mapper.Map<MechanicBase>(wl.Mechanic);
          }
        }
        returnMe.Add(addMe);
      }
      return returnMe;
    }
    public IEnumerable<WorkOrderGeneral> WorkOrdersGetActive()
    {
      //TODO: TODO: TODO: (not actually to do, but this process is useful to refer back to)
      // create a new empty WorkOrderGeneral collection
      // do the query, var source = ds.WorkOrders... as you have below
      // for each in source...
      // create a new WorkOrderGeneral object, auto mapping from the current source object
      // do additional query / queries, to fetch data that's missing in the WorkOrderGeneral object\
      // return the WorkOrderGeneral collection

      var returnMe = new List<WorkOrderGeneral>();
      var dsWOList = ds.WorkOrders.Include("Bicycle.Customer").Include("WorkLogs.Mechanic").Where(w => w.IsCompleted == false).OrderBy(w => !w.HighPriority).ThenBy(w => w.CreationTime);
      List<MechanicBase> dsMechList = mapper.Map<List<MechanicBase>>(ds.Mechanics.OrderBy(m => m.LastName));//NOTE: TRYING TO DO DB QUERIES WITHIN FOR LOOP CAUSES "ALREADY AN OPEN DATA READER" EXCEPTION!!!
      foreach (var item in dsWOList)
      {
        WorkOrderGeneral addMe = mapper.Map<WorkOrderGeneral>(item);
        // var assocMech = dsMechList.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes "already an open data reader" exception
        foreach (var wl in item.WorkLogs)
        {
          if (wl.IsActiveOnOrder)
          {
            addMe.ActiveMechanic = mapper.Map<MechanicBase>(wl.Mechanic);//TODO there should probably be a safeguard here to check if multiple worklogs have IsActive (which is bad)
          }
        }
        returnMe.Add(addMe);

      }
      return returnMe;
      //return mapper.Map<IEnumerable<WorkOrder>>(ds.WorkOrders);
      //need to modify still to help with view
      // return mapper.Map<IEnumerable<WorkOrderGeneral>>(ds.WorkOrders.Include("Bicycle.Customer").Include("Mechanic").Where(w => w.IsCompleted == false).OrderBy(w => !w.HighPriority).ThenBy(w => w.CreationTime));
    }
    //for list views--does not include a collection of previous mechanics
    public IEnumerable<WorkOrderGeneral> WorkOrdersGetCompleted()
    {
      var returnMe = new List<WorkOrderGeneral>();

      //return with status set to false
      var wos = ds.WorkOrders.Include("Bicycle.Customer").Include("WorkLogs.Mechanic").Where(w => w.IsCompleted == true).OrderByDescending(w => w.CompletionTime).ThenBy(w => !w.HighPriority);
      foreach (var item in wos)
      {
        WorkOrderGeneral addMe = mapper.Map<WorkOrderGeneral>(item);
        // var assocMech = dsMechList.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes "already an open data reader" exception
        foreach (var wl in item.WorkLogs)
        {
          if (wl.IsActiveOnOrder)
          {
            addMe.ActiveMechanic = mapper.Map<MechanicBase>(wl.Mechanic);//TODO there should probably be a safeguard here to check if multiple worklogs have IsActive (which is bad)
          }
        }
        returnMe.Add(addMe);
      }

      return returnMe;
    }


    //CUSTOMER METHODS--------------------------------------------------
    public CustomerBase CustomerAddNew(CustomerAdd newItem)
    {
      //attempt to add a new item but it crashes here because maybe this is 
      //tryint to access something that isn't there or I did the mapper incorrectly. 
      //not sure I made the dummy correctly either. 
      var addNewCust = ds.Customers.Add(mapper.Map<Customer>(newItem));
      ds.SaveChanges();

      //if successful, return the added item, mapped to a veew mode
      return (addNewCust == null) ? null : mapper.Map<CustomerBase>(addNewCust);
    }

    public IEnumerable<CustomerBase> CustomersGetAll()
    {
      return mapper.Map<IEnumerable<CustomerBase>>(ds.Customers);
    }
    public IEnumerable<CustomerGeneral> CustomersGetAllWithBicycles()
    {
      return mapper.Map<IEnumerable<CustomerGeneral>>(ds.Customers.Include("Bicycles"));
    }
    public CustomerWithDetails CustomerDetailsById(int id)
    {
      var c = ds.Customers.Include("Bicycles.Manufacturer").Include("Bicycles.Model").SingleOrDefault(cu => cu.Id == id);
      return (c == null) ? null : mapper.Map<CustomerWithDetails>(c);
    }

    public CustomerBase CustomerGetById(int id)
    {
      var o = ds.Customers.Find(id);

      return (o == null) ? null : mapper.Map<CustomerBase>(o);
    }

    public CustomerEditForm CustomerGetByIdForEdit(int? id)
    {
      // Beyond POC 2, may need to include bicycles

      var found = ds.Customers.SingleOrDefault(c => c.Id == id);

      return (found == null) ? null : mapper.Map<CustomerEditForm>(found);
    }

    public CustomerBase CustomerEdit(CustomerEdit newItem)
    {
      // Attempt to fetch the object
      var o = ds.Customers.Find(newItem.Id);

      if (o == null)
      {
        // Problem - item was not found, so return
        return null;
      }
      else
      {
        // Update the object with the incoming values
        ds.Entry(o).CurrentValues.SetValues(newItem);
        ds.SaveChanges();

        // Prepare and return the object
        return mapper.Map<CustomerBase>(o);
      }
    }
    //======---------------------------------


    public IEnumerable<BicycleBase> BicyclesGetAll()
    {
      return mapper.Map<IEnumerable<BicycleBase>>(ds.Bicycles);
    }
    public IEnumerable<BicycleWithAssoc> BicyclesGetWithManuModel()
    {
      return mapper.Map<IEnumerable<BicycleWithAssoc>>(ds.Bicycles.Include("Manufacturer").Include("Model").OrderBy(m => m.Manufacturer.Name));
    }
    public BicycleBase BicycleGetbyIdWithCustomer(int id)
    {
      var b = ds.Bicycles.Include("Customer").SingleOrDefault(bi => bi.Id == id);
      return (b == null) ? null : mapper.Map<BicycleBase>(b);
    }


    public IEnumerable<MechanicBase> MechanicsGetAll()
    {
      return mapper.Map<IEnumerable<MechanicBase>>(ds.Mechanics);
    }
    public IEnumerable<MechanicWithWorkOrders> MechanicsGetWithWorkOrders()//fix for mechanic views (doing includes in mechanic base breaks other manager functions that actually try to retrive MechanicBase (ie with no associated data) objects 
    {
      // var c = ds.Mechanics.Include("WorkOrder").OrderBy(m => m.LastName);//TAG change to previous work orders?
      //POC2: 0:1 to 0:1 --use reference Ids rather than nav properties
      //POC1-1: implemented worklog bridge table

      List<MechanicWithWorkOrders> returnMe = new List<MechanicWithWorkOrders>();
      var dsMechanics = ds.Mechanics.Include("WorkLogs.WorkOrder").OrderBy(m => m.LastName);
      //List<WorkOrderForMechDetail> dsWOList = mapper.Map<List<WorkOrderForMechDetail>>(ds.WorkOrders);//NOTE: TRYING TO DO DB QUERIES WITHIN FOR LOOP CAUSES "ALREADY AN OPEN DATA READER" EXCEPTION!!!

      foreach (var item in dsMechanics)//activeworkorders previouwsworkorders
      {
        MechanicWithWorkOrders addMe = mapper.Map<MechanicWithWorkOrders>(item);
        // var assocWO = dsWOList.SingleOrDefault(w => w.Id == item.WorkOrderId);
        List<WorkOrderForMechDetail> activeWOs = new List<WorkOrderForMechDetail>();
        List<WorkOrderForMechDetail> prevWOs = new List<WorkOrderForMechDetail>();
        foreach (var wl in item.WorkLogs)
        {
          if (wl.IsActiveOnOrder)
          {
            activeWOs.Add(mapper.Map<WorkOrderForMechDetail>(wl.WorkOrder));
          }
          else
          {
            prevWOs.Add(mapper.Map<WorkOrderForMechDetail>(wl.WorkOrder));

          }
        }
        addMe.ActiveWorkOrders = activeWOs;
        addMe.PreviousWorkOrders = prevWOs;
        returnMe.Add(addMe);
      }
      // return mapper.Map<IEnumerable<MechanicWithWorkOrders>>(c);
      return returnMe;
    }

    public IEnumerable<ServiceItemBase> ServiceItemsGetAll()
    {
      return mapper.Map<IEnumerable<ServiceItemBase>>(ds.ServiceItems.OrderBy(s => s.Description).ThenBy(s => s.Price));
    }

    public WorkOrderGeneral WorkOrderGenGetById(int Id)
    {

      // var d = ds.WorkOrders.Include("Bicycle.Customer").Include("Mechanic").SingleOrDefault(w => w.Id == Id);
      var d = ds.WorkOrders.Include("Bicycle.Customer").Include("WorkLogs.Mechanic").SingleOrDefault(w => w.Id == Id);

      if (d == null)
      {
        return null;
      }
      else
      {
        WorkOrderGeneral returnMe = mapper.Map<WorkOrderGeneral>(d);
        List<MechanicBase> prevMechs = new List<MechanicBase>();
        foreach (var item in d.WorkLogs)
        {
          if (item.IsActiveOnOrder)
          {
            returnMe.ActiveMechanic = mapper.Map<MechanicBase>(item.Mechanic);
          }
          else
          {
            prevMechs.Add(mapper.Map<MechanicBase>(item.Mechanic));
          }
        }
        returnMe.InactiveMechanics = prevMechs;
        return returnMe;
      }
    }



    public WorkOrderDetails WorkOrderDetailsById(int id)
    {

      var d = ds.WorkOrders.Include("Bicycle.Customer").Include("WorkOrderLines.ServiceItem").Include("WorkLogs.Mechanic").SingleOrDefault(w => w.Id == id);


      if (d == null)
      {
        return null;
      }
      else
      {
        WorkOrderDetails returnMe = mapper.Map<WorkOrderDetails>(d);
        List<MechanicBase> prevMechs = new List<MechanicBase>();
        foreach (var item in d.WorkLogs)
        {
          if (item.IsActiveOnOrder)
          {
            returnMe.ActiveMechanic = mapper.Map<MechanicBase>(item.Mechanic);
          }
          else
          {
            prevMechs.Add(mapper.Map<MechanicBase>(item.Mechanic));
          }

        }
        returnMe.InactiveMechanics = prevMechs;
        return returnMe;
      }

    }
    public WorkOrderEditForm WorkOrderGetByIdForEdit(int id)//TODO: MAKE ME POPULATE OLD MECH SELECTLIST
    {

      var found = ds.WorkOrders.Include("Bicycle").Include("WorkOrderLines.ServiceItem").Include("WorkLogs.Mechanic").SingleOrDefault(w => w.Id == id);

      if (found == null)
      {
        return null;
      }
      else
      {
        int? activeMechId=null;
        foreach (var wl in found.WorkLogs)
        {
          if (wl.IsActiveOnOrder)
          {
            activeMechId = wl.Mechanic.Id;
          }
        }
        var returnMe=mapper.Map<WorkOrderEditForm>(found);
        returnMe.ActiveMechanicId = activeMechId;
        return returnMe;
      }
    }


    /*
    public MechanicBase MechanicGetById(int? id)
    {
      var o = ds.Mechanics.Include("WorkOrder.Bicycle.Customer").Include("WorkOrder.Bicycle.Customer").SingleOrDefault(m => m.Id == id);

      return (o == null) ? null : mapper.Map<MechanicBase>(o);
    }*/

      //used for mechanic details view:
      //Retrieves a mechanic and related workorders through worklogs bridge table as 2 collections: active and inactive.
      //Formats this into a single mechanicWithWorkOrders view model object
    public MechanicWithWorkOrders MechanicGetByIdWithWorkOrders(int? id)
    {
      
      var o = ds.Mechanics.Include("WorkLogs.WorkOrder.Bicycle.Customer").SingleOrDefault(m => m.Id == id);
      //var o = ds.Mechanics.Include("WorkOrder.Bicycle.Customer").SingleOrDefault(m => m.Id == id);

      if (o == null)
      {
        return null;
      }
      //map database mechanic to workorder viewmodel,
      //then manually check worklogs to parse out lists of
      //active and inactive workorders
      else
      {
        var returnMe = mapper.Map<MechanicWithWorkOrders>(o);
        //prepare empty lists to populate with workorder objects--
        //will be assigned to returnMe lists for active and inactive worklogs/orders
        List<WorkOrderForMechDetail> activeOrders = new List<WorkOrderForMechDetail>();
        List<WorkOrderForMechDetail> prevOrders = new List<WorkOrderForMechDetail>();
        //parse out each list of workorders
        foreach (var wl in o.WorkLogs)
        {
          if (wl.IsActiveOnOrder)
          {
            var addMe = mapper.Map<WorkOrderForMechDetail>(wl.WorkOrder);
            addMe.AssignedTime = wl.TimeStarted;
            addMe.UnassignedTime = wl.TimeStopped;
            activeOrders.Add(addMe);
          }
          else
          {
            var addMe = mapper.Map<WorkOrderForMechDetail>(wl.WorkOrder);
            addMe.AssignedTime = wl.TimeStarted;
            addMe.UnassignedTime = wl.TimeStopped;
            prevOrders.Add(addMe);

          }
        }
        //SORT THE WORK ORDER LISTS:
        //order active orders to put most recently assigned first
        var sortedActiveOrders = activeOrders.OrderByDescending(ao => ao.AssignedTime);
        //order inactive orders to put most recently un-assigned first
        var sortedPreviousOrders = prevOrders.OrderByDescending(po => po.UnassignedTime);

        returnMe.ActiveWorkOrders = sortedActiveOrders;
        returnMe.PreviousWorkOrders = sortedPreviousOrders;
        return returnMe;
      }
    }


    public BicycleBase BicycleGetById(int id)
    {
      var o = ds.Bicycles.Find(id);

      return (o == null) ? null : mapper.Map<BicycleBase>(o);
    }



    public ServiceItemBase ServiceItemGetById(int id)
    {
      var o = ds.ServiceItems.Find(id);

      return (o == null) ? null : mapper.Map<ServiceItemBase>(o);
    }

    public WorkOrderBase WorkOrderAdd(WorkOrderAdd newItem)
    {
      Mechanic dbMechanic = null;

      bool addMechFlag = false;
      if (newItem == null)
      {
        return null;
      }
      else
      {
        //look for and attempt to add associated items (from work order form id's)   
        //var dbCustomer = ds.Customers.Find(newItem.CustomerId);
        var dbBicycle = ds.Bicycles.Find(newItem.BicycleId);

        if (dbBicycle == null)
        {
          return null;
        }
        else
        {
          WorkOrder addedItem = mapper.Map<WorkOrder>(newItem);

          addedItem.Bicycle = dbBicycle;
          //addedItem.Bicycle.Customer = dbCustomer;//TODO: Our design model class implementation may be screwy: bicycle constructor has blank customer, can't add blank customers to datastore, so
          //must also add associated customer to associated bicycle object

          //check if a mechanic was selected on the form
          if (newItem.MechanicId != null && newItem.MechanicId != 0)
          {
            dbMechanic = ds.Mechanics.Find(newItem.MechanicId);
            if (dbMechanic == null)
            {
              //bad form data, so abort workorder creation
              return null;
            }
            else
            {
              //set flag to tell subsequent logic we want to add the found mechanic
              addMechFlag = true;
            }
          }

          //add addedItem to the datastore to give it an Id
          ds.WorkOrders.Add(addedItem);
          ds.SaveChanges();
          if (addedItem.IsCompleted)//POC1-1: if workorder is completed, but a mechanic has been selected, make the mechanic part of the previous mechanics on the workorder
          {
            if (addMechFlag)//if we're trying to add a mechanic to the completed workorder
            {
              WorkLog createdCompleted = new WorkLog
              {
                Mechanic = dbMechanic,

                MechanicId = dbMechanic.Id,
                WorkOrder = addedItem,
                WorkOrderId = addedItem.Id,
                IsActiveOnOrder = false
              };
              ds.WorkLogs.Add(createdCompleted);
              ds.SaveChanges();
              addedItem.WorkLogs.Add(createdCompleted);

            }
      
            //ds.WorkOrders.Add(addedItem);

          }
          else//workorder is active (not completed)
          {
            //we're assigning an active mechanic
            if (addMechFlag)//create appropriate worklog for an active order
            {
              WorkLog createdCompleted = new WorkLog
              {
                Mechanic = dbMechanic,

                MechanicId = dbMechanic.Id,
                WorkOrder = addedItem,
                WorkOrderId = addedItem.Id,
                //IsActiveOnOrder's default value is true
              };
              ds.WorkLogs.Add(createdCompleted);
              ds.SaveChanges();
              addedItem.WorkLogs.Add(createdCompleted);

            }
          }
          ds.SaveChanges();
          //prepare WorkOrderBase for view returned by controller
          WorkOrderBase returnMe = mapper.Map<WorkOrderBase>(addedItem);

          if (returnMe.IsCompleted)
          {
            returnMe.MechanicId = null;//TODO: check that edit form handles this
          }
          else
          {
            //This is unwieldy, but tries to ensure that the appropriate (and only, since it's a new workorder) mechanic id has been set
            returnMe.MechanicId = addedItem.WorkLogs.ElementAt(0).MechanicId;
          }
          return mapper.Map<WorkOrderBase>(addedItem);
        }
      }
    }


    public WorkOrderDetails WorkOrderEdit(WorkOrderEdit newItem)
    {
      //Fetch the object and related data necessary for editing
      //(worklogs needed to check whether we're creating a new workLog, or if the form mechanic is the same as the current mechanic, etc)
      var wo = ds.WorkOrders.Include("WorkOrderLines").Include("WorkLogs.Mechanic").SingleOrDefault(w => w.Id == newItem.Id);

      var dbBicycle = ds.Bicycles.Find(newItem.BicycleId);
      Mechanic dbNewMechanic;
      Mechanic dbOldMechanic;
      WorkLog oldActiveLog = null;
      WorkLog sameMech = null;

      //--handle bicycle and mechanic/workLogs----------------------------------------

      //check if a mechanic is currently active on the workorder.
      //Do this first because there probably aren't a ton of worklogs, and
      //oldActiveLog is used through various logic branches
      foreach (var wl in wo.WorkLogs)
      {
        if (wl.IsActiveOnOrder)
        {
          //TODO: consider throwing an exception if multiple active
          oldActiveLog = wl;//currently will reassign if multiple active
        }
        if (newItem.ActiveMechanicId == wl.Mechanic.Id)
        {
          sameMech = wl;
        }
      }
      if (dbBicycle == null)
      {
        return null;
      }
      else
      {
        wo.Bicycle = dbBicycle;
        //check if mechanic specified on the form actually exists, then do checks to update workorder
        if (newItem.ActiveMechanicId != 0)//if an active mechanic was selected on the form
        {
          dbNewMechanic = ds.Mechanics.Find(newItem.ActiveMechanicId);
          if (dbNewMechanic == null)//return null if user is trying to add a mechanic but id doesn't exist in db
          {
            return null; // null later
          }
          else
          {

            //if there is currently an active mechanic assigned to the workorder:
            if (oldActiveLog != null)
            {
              //assigning new mechanic as active, so unassign old one, settign appropriate flags in workLogs, etc
              if (oldActiveLog.Mechanic.Id != dbNewMechanic.Id)//if the active mechanic has changed
              {
                //set old work log as not active, and set its stop time:
                oldActiveLog.IsActiveOnOrder = false;
                oldActiveLog.TimeStopped = DateTime.Now;
                //create a new WorkLog for the new mechanic
                //TODO: FIX WORKLOG TABLES TO ALLOW SAME MECHANIC TO HAVE MULTIPLE WORKLOGS FOR SAME WORKORDER!
                if (sameMech != null)
                {
                  sameMech.IsActiveOnOrder = true;//put old worklog for same mechanic as active
                }
                else
                {
                  WorkLog newActiveLog = new WorkLog()
                  {
                    Mechanic = dbNewMechanic,
                    MechanicId = dbNewMechanic.Id,
                    WorkOrder = wo,
                    WorkOrderId = wo.Id
                    //start and end time are automatically initialized to now.
                    //IsOnActiveOrder defaults to true on construction
                  };
                  ds.WorkLogs.Add(newActiveLog);
                }
                ds.SaveChanges();
              }

            }
            //this may seem redundant, but we checked that the active mechanic had changed above
            else
            {
              //TODO: Temp  worklog pk is currently woId and mechId--change table design to allow mechanic to have multiple logs for same order!
              if (sameMech != null)
              {
                sameMech.IsActiveOnOrder = true;//put old worklog for same mechanic as active
              }
              else
              {
                WorkLog newActiveLog = new WorkLog()
                {
                  Mechanic = dbNewMechanic,
                  MechanicId = dbNewMechanic.Id,
                  WorkOrder = wo,
                  WorkOrderId = wo.Id
                  //start and end time are automatically initialized to now.
                  //IsOnActiveOrder defaults to true on construction
                };
                ds.WorkLogs.Add(newActiveLog);
              }
              ds.SaveChanges();
            }
          }
        }
        //no mechanic was selected, so clear the assigned mechanic if present
        else
        {
          //make active log inactive if applicable        
          if (oldActiveLog != null)
          {
            //set old work log as not active, and set its stop time:
            oldActiveLog.IsActiveOnOrder = false;
            oldActiveLog.TimeStopped = DateTime.Now;
          }
        }
        ds.Entry(wo).CurrentValues.SetValues(newItem);
        // ds.SaveChanges();

        //------Handle WORKORDERLINES and SERVICEITEMS-------------------------------------
        //int wolcounter = 0;
        // WorkOrderLine li = wo.WorkOrderLines.ElementAt(0);
        while (wo.WorkOrderLines.Count() > 0)//issue with indexing: we were trying to increase index while removing items so index actually increases twice as fast (don't increment index at all; we're popping from collection)
        {
          WorkOrderLine li = wo.WorkOrderLines.ElementAt(0);//is of type ICollection, which does not support an idexer/iterator?
          ds.WorkOrderLines.Remove(li);
          //wolcounter++;
        }

        // wo.WorkOrderLines.Clear();
        //  ds.SaveChanges();

        int count = 0;
        foreach (var siId in newItem.ServiceItemIds)
        {

          var si = ds.ServiceItems.Find(siId);
          if (si == null)
          {
            break;
          }
          else
          {
            if (newItem.Quantities.ElementAt(count) != 0)//do not add lines that have quantity of 0 (especially since one workorder line is always rendered in the form and we may not want to add a service)
            {
              var wol = ds.WorkOrderLines.Add(new WorkOrderLine
              {
                ServiceItem = si,
                Quantity = newItem.Quantities.ElementAt(count),
                LineTotal = (newItem.Quantities.ElementAt(count) * si.Price),
                WorkOrder = wo
              });

              int i = 1; // for setting breakpoint with wol in scope
            }
          }
          count++;


        }


      }
      //final filter: unset any mechanic if the workorder has been marked as completed:
      //we probably want to log that a mechanic worked on a bike even if start time is v/ close to end time.
      //NOTE: end time is intentially set to be slightly after start time to ensure that start time != end time
      //whenever an order is marked completed
      if (wo.IsCompleted)
      {
        var targetLog = ds.WorkLogs.SingleOrDefault(wl => wl.WorkOrder.Id == wo.Id && wl.IsActiveOnOrder == true);
        if (targetLog != null)
        {
          targetLog.IsActiveOnOrder = false;
          //add 10 seconds so that if the user changed the mechanic and marked
          //the order completed, start time != stop time
          targetLog.TimeStopped = DateTime.Now.AddSeconds(10);
        }
        wo.CompletionTime = DateTime.Now.AddSeconds(10);
      }

      ds.SaveChanges();
      return mapper.Map<WorkOrderDetails>(wo);
    }
    //AJAX support functions:++++++++++++++++++++++++++++++++++++

    //searching ACTIVE workorders with related data
    public IEnumerable<WorkOrderGeneral> WorkOrderGetByString(string search)
    {
      string searchStr = search.Trim().ToLower();
      //Attenttion: .Select(m=>m.Id) create a collection of mechanicId's (this query just returns the mechanic id's instead of a collection of full mechanic objects)
      IEnumerable<int?> foundMechanicIds = ds.Mechanics.Where(m => m.FirstName.ToLower().Contains(searchStr) || m.LastName.ToLower().Contains(searchStr) ||
      m.Email.ToLower().Contains(searchStr) || (m.FirstName.ToLower() + " " + m.LastName.ToLower() + " " + m.Phone.ToLower() + " " + m.Email.ToLower() + " " + m.LastName.ToLower() + " " + m.FirstName.ToLower()).Contains(searchStr)).Select(m => (int?)m.Id);
      //find all workorders we found mechanic stuff for based on the string:
      IEnumerable<int> woIdsForFoundMechs = ds.WorkLogs.Include("Mechanic").Where(wl => foundMechanicIds.Contains(wl.MechanicId)).Select(wl => wl.WorkOrderId);
      //create a collection of found workorders for the mechanic criteria
      var woForMech = ds.WorkOrders.Include("Bicycle.Customer").Include("Bicycle.Manufacturer").Include("Bicycle.Model").Include("WorkLogs.Mechanic").Where(wo => woIdsForFoundMechs.Contains(wo.Id));
      //Create a collection of workorders for all other search criteria
      //Attention: using "foundMechanicIds.Contains(c.MechanicId)" in LINQ to return a colleciton that contains Ids of mechanics that matched the above search for mechanics;
      var found = ds.WorkOrders.Include("Bicycle.Customer").Include("Bicycle.Manufacturer").Include("Bicycle.Model").Include("WorkLogs.Mechanic")
          .Where(w => w.IsCompleted == false).Where(c => c.Notes.ToLower().Contains(searchStr) || c.Bicycle.Customer.FirstName.ToLower().Contains(searchStr) || c.Bicycle.Customer.LastName.ToLower().Contains(searchStr) ||
          c.Bicycle.Customer.Phone.ToLower().Contains(searchStr) || c.Bicycle.Customer.Email.ToLower().Contains(searchStr) || c.Bicycle.Description.ToLower().Contains(searchStr) ||
          c.Bicycle.Manufacturer.Name.ToLower().Contains(searchStr) || c.Bicycle.Model.Name.ToLower().Contains(searchStr) || c.Bicycle.Model.Info.ToLower().Contains(searchStr)
          || (c.Bicycle.Customer.FirstName.ToLower() + " " + c.Bicycle.Customer.LastName.ToLower() + " " + c.Bicycle.Customer.Phone.ToLower() + " " + c.Bicycle.Customer.Email.ToLower() + " " + c.Bicycle.Customer.LastName.ToLower() + " " + c.Bicycle.Customer.FirstName.ToLower()).Contains(searchStr))
          .OrderBy(w => !w.HighPriority).ThenBy(w => w.CreationTime);

      //combine the two collections of workorders, ensuring that there are no duplications:
      var returnMe = found.Concat(woForMech);
      //Group by id, then pull the first element by that id out (therefore only selecting workorders of the same id once, ensuring distinct workorders)
      //to make sure that there are no duplicate workorders returned in the collection.
      //returnMe = returnMe.GroupBy(wo => wo.Id).Select(w => w.FirstOrDefault());
      //
      /*||
      c.Mechanic.FirstName.ToLower().Contains(searchStr) || c.Mechanic.LastName.ToLower().Contains(searchStr) || c.Mechanic.Phone.ToLower().Contains(searchStr) ||
      c.Mechanic.Email.ToLower().Contains(searchStr));*/


      if (found == null)
      {
        //return mapper.Map<IEnumerable<WorkOrderGeneral>>(ds.WorkOrders.Include("Bicycle.Customer").OrderBy(w => !w.HighPriority).ThenBy(w => w.CreationTime));
        return WorkOrdersGetAll();
      }
      else
      {
        /*FROM LAST SEMESTER 0-1 to 0-1--------
        List<WorkOrderGeneral> returnMeDefault = new List<WorkOrderGeneral>();
        List<MechanicBase> dbMechs = mapper.Map<List<MechanicBase>>(ds.Mechanics);
        foreach (var item in found)
        {
          WorkOrderGeneral addMe = mapper.Map<WorkOrderGeneral>(item);
          var assocMech = dbMechs.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes pointer in use exception
          addMe.Mechanic = mapper.Map<MechanicBase>(assocMech);
          returnMe.Add(addMe);
        }*/

        //attach active and previous mechanics to workorderGeneral:
        List<WorkOrderGeneral> returnMeMapped = new List<WorkOrderGeneral>();
        foreach (var item in returnMe)
        {
          WorkOrderGeneral addMe = mapper.Map<WorkOrderGeneral>(item);
          // var assocMech = dsMechList.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes "already an open data reader" exception
          foreach (var wl in item.WorkLogs)
          {
            if (wl.IsActiveOnOrder)
            {
              addMe.ActiveMechanic = mapper.Map<MechanicBase>(wl.Mechanic);//TODO there should probably be a safeguard here to check if multiple worklogs have IsActive (which is bad)
            }
          }
          returnMeMapped.Add(addMe);

        }
        return returnMeMapped;
      }
    }
    public IEnumerable<WorkOrderGeneral> WorkOrderGetCompletedByString(string search)
    {
      string searchStr = search.Trim().ToLower();
      // .Select(m=>m.Id) create a collection of mechanicId's (this query just returns the mechanic id's instead of a collection of full mechanic objects)
      IEnumerable<int?> foundMechanicIds = ds.Mechanics.Where(m => m.FirstName.ToLower().Contains(searchStr) || m.LastName.ToLower().Contains(searchStr) ||
      m.Email.ToLower().Contains(searchStr) || (m.FirstName.ToLower() + " " + m.LastName.ToLower() + " " + m.Phone.ToLower() + " " + m.Email.ToLower() + " " + m.LastName.ToLower() + " " + m.FirstName.ToLower()).Contains(searchStr)).Select(m => (int?)m.Id);
      //find all workorders we found mechanic stuff for based on the string:
      IEnumerable<int> woIdsForFoundMechs = ds.WorkLogs.Include("Mechanic").Where(wl => foundMechanicIds.Contains(wl.MechanicId)).Select(wl => wl.WorkOrderId);
      //create a collection of found workorders for the mechanic criteria
      var woForMech = ds.WorkOrders.Include("Bicycle.Customer").Include("Bicycle.Manufacturer").Include("Bicycle.Model").Include("WorkLogs.Mechanic").Where(wo => woIdsForFoundMechs.Contains(wo.Id));

      //Create a collection of workorders for all other search criteria
      var found = ds.WorkOrders.Include("Bicycle.Customer").Include("Bicycle.Manufacturer").Include("Bicycle.Model").Include("WorkLogs.Mechanic")
          .Where(w => w.IsCompleted == true).Where(c => c.Notes.ToLower().Contains(searchStr) || c.Bicycle.Customer.FirstName.ToLower().Contains(searchStr) || c.Bicycle.Customer.LastName.ToLower().Contains(searchStr) ||
          c.Bicycle.Customer.Phone.ToLower().Contains(searchStr) || c.Bicycle.Customer.Email.ToLower().Contains(searchStr) || c.Bicycle.Description.ToLower().Contains(searchStr) ||
          c.Bicycle.Manufacturer.Name.ToLower().Contains(searchStr) || c.Bicycle.Model.Name.ToLower().Contains(searchStr) || c.Bicycle.Model.Info.ToLower().Contains(searchStr)
          || (c.Bicycle.Customer.FirstName.ToLower() + " " + c.Bicycle.Customer.LastName.ToLower() + " " + c.Bicycle.Customer.Phone.ToLower() + " " + c.Bicycle.Customer.Email.ToLower() + " " + c.Bicycle.Customer.LastName.ToLower() + " " + c.Bicycle.Customer.FirstName.ToLower()).Contains(searchStr))
          .OrderByDescending(w => w.CompletionTime).ThenBy(w => !w.HighPriority);

      //combine the two collections of workorders, ensuring that there are no duplications:
      var returnMe = found.Concat(woForMech);
      //Group by id, then pull the first element by that id out (therefore only selecting workorders of the same id once, ensuring distinct workorders)
      //to make sure that there are no duplicate workorders returned in the collection.
      //returnMe = returnMe.GroupBy(wo => wo.Id).Select(w => w.FirstOrDefault());

      //POC1-1:  need to incorporate mechanic information because completed workorders now not have mechanics assigned to them as previous


      if (found == null)
      {
        return WorkOrdersGetCompleted();
        //returnMe= mapper.Map<IEnumerable<WorkOrderGeneral>>(ds.WorkOrders.Include("Bicycle.Customer").Include("WorkLogs.Mechanic").Where(w => w.IsCompleted == true).OrderBy(w => !w.HighPriority).ThenBy(w => w.CreationTime));

      }
      else
      {
        List<WorkOrderGeneral> returnMeMapped = new List<WorkOrderGeneral>();
        foreach (var item in returnMe)
        {
          WorkOrderGeneral addMe = mapper.Map<WorkOrderGeneral>(item);
          // var assocMech = dsMechList.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes "already an open data reader" exception
          foreach (var wl in item.WorkLogs)
          {
            if (wl.IsActiveOnOrder)
            {
              addMe.ActiveMechanic = mapper.Map<MechanicBase>(wl.Mechanic);//TODO there should probably be a safeguard here to check if multiple worklogs have IsActive (which is bad)
            }
          }
          returnMeMapped.Add(addMe);

        }
        return returnMeMapped;
      }
    }

    public IEnumerable<CustomerGeneral> CustomerGetBySearchString(string search)
    {
      string searchStr = search.Trim().ToLower();

      var found = ds.Customers.Include("Bicycles")
          .Where(c => c.FirstName.ToLower().Contains(searchStr) || c.LastName.ToLower().Contains(searchStr) || c.Phone.ToLower().Contains(searchStr) || c.Email.ToLower().Contains(searchStr))
          .OrderBy(c => c.LastName).ThenBy(c => c.LastName);

      if (found == null)
      {
        return mapper.Map<IEnumerable<CustomerGeneral>>(ds.Customers.Include("Bicycles").OrderBy(c => c.LastName).ThenBy(w => w.FirstName));
      }
      else
      {
        return mapper.Map<IEnumerable<CustomerGeneral>>(found.OrderBy(c => c.LastName).ThenBy(w => w.FirstName));
      }
    }

    public IEnumerable<WorkOrderGeneral> WorkOrderGetAllByString(string search)
    {
      string searchStr = search.Trim().ToLower();
      //Attenttion: .Select(m=>m.Id) create a collection of mechanicId's (this query just returns the mechanic id's instead of a collection of full mechanic objects)
      IEnumerable<int?> foundMechanicIds = ds.Mechanics.Where(m => m.FirstName.ToLower().Contains(searchStr) || m.LastName.ToLower().Contains(searchStr) ||
      m.Email.ToLower().Contains(searchStr) || (m.FirstName.ToLower() + " " + m.LastName.ToLower() + " " + m.Phone.ToLower() + " " + m.Email.ToLower() + " " + m.LastName.ToLower() + " " + m.FirstName.ToLower()).Contains(searchStr)).Select(m => (int?)m.Id);
      //find all workorders we found mechanic stuff for based on the string:
      IEnumerable<int> woIdsForFoundMechs = ds.WorkLogs.Include("Mechanic").Where(wl => foundMechanicIds.Contains(wl.MechanicId)).Select(wl => wl.WorkOrderId);
      //create a collection of found workorders for the mechanic criteria
      var woForMech = ds.WorkOrders.Include("Bicycle.Customer").Include("Bicycle.Manufacturer").Include("Bicycle.Model").Include("WorkLogs.Mechanic").Where(wo => woIdsForFoundMechs.Contains(wo.Id));

      //Attention: using "foundMechanicIds.Contains(c.MechanicId)" in LINQ to return a colleciton that contains Ids of mechanics that matched the above search for mechanics;
      var found = ds.WorkOrders.Include("Bicycle.Customer").Include("Bicycle.Manufacturer").Include("Bicycle.Model")
          .Where(c => c.Notes.ToLower().Contains(searchStr) || c.Bicycle.Customer.FirstName.ToLower().Contains(searchStr) || c.Bicycle.Customer.LastName.ToLower().Contains(searchStr) ||
          c.Bicycle.Customer.Phone.ToLower().Contains(searchStr) || c.Bicycle.Customer.Email.ToLower().Contains(searchStr) || c.Bicycle.Description.ToLower().Contains(searchStr) ||
          c.Bicycle.Manufacturer.Name.ToLower().Contains(searchStr) || c.Bicycle.Model.Name.ToLower().Contains(searchStr) || c.Bicycle.Model.Info.ToLower().Contains(searchStr)
          || (c.Bicycle.Customer.FirstName.ToLower() + " " + c.Bicycle.Customer.LastName.ToLower() + " " + c.Bicycle.Customer.Phone.ToLower() + " " + c.Bicycle.Customer.Email.ToLower() + " " + c.Bicycle.Customer.LastName.ToLower() + " " + c.Bicycle.Customer.FirstName.ToLower()).Contains(searchStr))
          .OrderBy(w => w.IsCompleted).ThenBy(w => !w.HighPriority).ThenBy(w => w.CreationTime);
      /*||
      c.Mechanic.FirstName.ToLower().Contains(searchStr) || c.Mechanic.LastName.ToLower().Contains(searchStr) || c.Mechanic.Phone.ToLower().Contains(searchStr) ||
      c.Mechanic.Email.ToLower().Contains(searchStr));*/

      //combine the two collections of workorders, ensuring that there are no duplications:
      var returnMe = found.Concat(woForMech);
      //Group by id, then pull the first element by that id out (therefore only selecting workorders of the same id once, ensuring distinct workorders)
      //to make sure that there are no duplicate workorders returned in the collection.
     // returnMe = returnMe.GroupBy(wo => wo.Id).Select(w => w.FirstOrDefault());

      if (found == null)
      {
        //return mapper.Map<IEnumerable<WorkOrderGeneral>>(ds.WorkOrders.Include("Bicycle.Customer").OrderBy(w => !w.HighPriority).ThenBy(w => w.CreationTime));
        return WorkOrdersGetAll();
      }
      else
      {
        List<WorkOrderGeneral> returnMeMapped = new List<WorkOrderGeneral>();
        foreach (var item in returnMe)
        {
          WorkOrderGeneral addMe = mapper.Map<WorkOrderGeneral>(item);
          // var assocMech = dsMechList.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes "already an open data reader" exception
          foreach (var wl in item.WorkLogs)
          {
            if (wl.IsActiveOnOrder)
            {
              addMe.ActiveMechanic = mapper.Map<MechanicBase>(wl.Mechanic);//TODO there should probably be a safeguard here to check if multiple worklogs have IsActive (which is bad)
            }
          }
          returnMeMapped.Add(addMe);

        }
        return returnMeMapped;
      }
    }
    //for searching customer select lists
    public IEnumerable<CustomerBase> CustomerGetByString(string search)
    {
      string searchStr = search.Trim().ToLower();

      //.Contains() will give you partial matches (ie not ==), and
      //is Case sensitive, so filter w/ ToLower()
      var found = ds.Customers.Where(c => c.FirstName.ToLower().Contains(searchStr) || c.LastName.ToLower().Contains(searchStr) || c.Phone.ToLower().Contains(searchStr) || c.Email.ToLower().Contains(searchStr)
        || (c.FirstName.ToLower() + " " + c.LastName.ToLower() + " " + c.Phone.ToLower() + " " + c.Email.ToLower() + " " + c.LastName.ToLower() + " " + c.FirstName.ToLower()).Contains(searchStr));

      if (found == null) { return null; }//TODO: may want to change this to a get all type thing because if no results are found, the search box will become empty
                                         //at any rate, some method for repopulating albums to the original full list should be available on a form (think customer search list for our app)
      else
      {
        return mapper.Map<IEnumerable<CustomerBase>>(found);
      }
    }
    //for populating bicycle select lists based on customer id
    public IEnumerable<BicycleWithAssoc> BicycleGetByCustId(int id)
    {
      var found = ds.Bicycles.Include("Model").Include("Manufacturer").Where(b => b.Customer.Id == id);
      if (found == null) { return null; }

      else
      {
        return mapper.Map<IEnumerable<BicycleWithAssoc>>(found);
      }
    }

    public IEnumerable<MechanicWithWorkOrders> MechanicGetByString(string search)
    {
      string searchStr = search.Trim().ToLower();

      var found = ds.Mechanics.Include("WorkLogs.WorkOrder").Where(c => c.FirstName.ToLower().Contains(searchStr) || c.LastName.ToLower().Contains(searchStr) || c.Phone.ToLower().Contains(searchStr) || c.Email.ToLower().Contains(searchStr));//.Contains() will give you partial matches (ie not ==), and
                                                                                                                                                                                                                                                 //is Case sensitive, so filter w/ ToLower()
      if (found == null) { return null; }//TODO: may want to change this to a get all type thing because if no results are found, the search box will become empty
                                         //at any rate, some method for repopulating albums to the original full list should be available on a form (think customer search list for our app)
      else
      {
        List<MechanicWithWorkOrders> returnMeMapped = new List<MechanicWithWorkOrders>();
        foreach (var item in found)
        {
          MechanicWithWorkOrders addMe = mapper.Map<MechanicWithWorkOrders>(item);
          List<WorkOrderForMechDetail> ActiveOrders = new List<WorkOrderForMechDetail>();
          List<WorkOrderForMechDetail> InactiveOrders = new List<WorkOrderForMechDetail>();

          // var assocMech = dsMechList.SingleOrDefault(m => m.Id == item.MechanicId);//NOTE: querying the db directly here causes "already an open data reader" exception
          foreach (var wl in item.WorkLogs)
          {
            if (wl.IsActiveOnOrder)
            {
              ActiveOrders.Add(mapper.Map<WorkOrderForMechDetail>(wl.WorkOrder));//TODO there should probably be a safeguard here to check if multiple worklogs have IsActive (which is bad)
            }
            else
            {
              InactiveOrders.Add(mapper.Map<WorkOrderForMechDetail>(wl.WorkOrder));//TODO there should probably be a safeguard here to check if multiple worklogs have IsActive (which is bad)

            }
          }
          addMe.ActiveWorkOrders = ActiveOrders;
          addMe.PreviousWorkOrders = InactiveOrders;
          returnMeMapped.Add(addMe);

        }
        return returnMeMapped;
      }
    }

    //method to change the status of the work order
    public WorkOrderGeneral WorkOrdersStatusById(int Id)
    {
      //grab the correct object
      var gotIt = ds.WorkOrders.Include("Bicycle.Customer").Include("WorkLogs.Mechanic").SingleOrDefault(w => w.Id == Id);
      if (gotIt == null)
      {
        return null;
      }
      else
      {
        //if it is found, change the status of the found item, and return it
        gotIt.IsCompleted = !gotIt.IsCompleted;//NOTE: we just flipped the status (important for if logic below)

        //handle activemechanics and previous mechanics associations\
        if (gotIt.IsCompleted == true)//going from active to completed (completed workorders should not have an active mechanic)
        {
          //if (gotIt.ActiveMechanic != null)
          WorkLog activeLog = null;
          foreach (var log in gotIt.WorkLogs)
          {
            if (log.IsActiveOnOrder)
            {
              activeLog = log;
            }
          }
          if (activeLog != null)

          {

            activeLog.IsActiveOnOrder = false;
            activeLog.TimeStopped = DateTime.Now;
          }
          gotIt.CompletionTime = DateTime.Now;//set workorder's completion time
        }
      }
      if (gotIt.IsCompleted == false)//going from completed to active
      {
        //find the most recently assigned mechanic/workLog:
        WorkLog lastLog = null;
        DateTime compareMe = new DateTime();//lowest possible init value
        if (gotIt.WorkLogs != null && gotIt.WorkLogs.Count > 0)
        {
          //find most recent worklog based on TimeStarted
          foreach (var wl in gotIt.WorkLogs)
          {
            if (wl.TimeStarted > compareMe)
            {
              compareMe = wl.TimeStarted;
              lastLog = wl;
            }
          }
          //re-activate order (don't change start time, and end time will be set again when marked completed)
          lastLog.IsActiveOnOrder = true;
        }
        gotIt.CreationTime = DateTime.Now;//although this is not "created" we need to set this to make the workorder appear back at the top of the active list
        gotIt.CompletionTime = DateTime.Now;//we set creation and completion time equal on uncompleted workders (mimics workorder constructor logic)
        //

        //}
      }

      //update the object with the incoming values
      ds.Entry(gotIt).CurrentValues.SetValues(Id);
      //Save the changes
      ds.SaveChanges();

      //prepare and return the object
      return mapper.Map<WorkOrderGeneral>(gotIt);
    }
  

  //method to get price for specific serviceitems and return as string for AJAX to set innerHtml of Price element on forms
  public string GetPriceByServiceItemId(int id)
  {
    var foundItem = ds.ServiceItems.Find(id);
    if (foundItem == null)
    {
      return "";
    }
    else
    {
      return foundItem.Price.ToString();
    }
  }



  //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


  //PRELOAD DATA METHODS  ====================================================================
  public bool LoadData()
  {

    //var user = user.name //TODO: Add user object to more easily retrieve role claims
    bool done = false;
    if (ds.RoleClaims.Count() == 0)
    {
      ds.RoleClaims.Add(new RoleClaim { Name = "Mechanic" });
      ds.RoleClaims.Add(new RoleClaim { Name = "Manager" });
      ds.SaveChanges();
      done = true;
    }
    //ADD SERVICE ITEMS=====================================================
    //load service items
    if (ds.ServiceItems.Count() == 0)
    {
      done = false;

      ds.ServiceItems.Add(new ServiceItem { Description = "Flat tire fix with tube", Price = 15.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Fender installation", Price = 10.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Rear Wheel replacement", Price = 20.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Rear wheel replacement with respacing", Price = 30.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Repack Bottom Bracket", Price = 25.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Replace shift cable", Price = 15.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Replace brake cable", Price = 10.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Replace brake pads", Price = 5.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Base Tuneup Labour", Price = 50.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "wrap new bartape", Price = 10.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Install rear derailleur with new cable", Price = 25.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Install front derailleur with new cable", Price = 20.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Repack rear hub", Price = 15.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Repack front hub", Price = 10.00 });
      ds.ServiceItems.Add(new ServiceItem { Description = "Repack Headset", Price = 15.00 });
      //ds.ServiceItems.Add(new ServiceItem { Description = "Discount", Price = 0.00 }); TODO: a named service item where we can change the workorderline total manually for discounts (ie want to say what work, but don't want to charge)

      ds.SaveChanges();
      done = true;
    }
    //ADD CUSTOMERS===================================================
    //load customers
    if (ds.Customers.Count() == 0)
    {
      done = false;

      ds.Customers.Add(new Models.Customer { FirstName = "Robert", LastName = "Arctur", Phone = "647-555-5525", Email = "bob@investigations.example.com" });
      ds.Customers.Add(new Models.Customer { FirstName = "Michael", LastName = "Overall", Phone = "416-555-5555", Email = "michael@example.com" });
      ds.Customers.Add(new Models.Customer { FirstName = "Tanvir", LastName = "Sarkar", Phone = "647-222-2254", Email = "tanvir@example.com" });
      ds.Customers.Add(new Models.Customer { FirstName = "Igor", LastName = "Krasnyanskiy", Phone = "905-222-2222", Email = "Krasnyanskiy@example.com" });
      ds.Customers.Add(new Models.Customer { FirstName = "Dan", LastName = "Foster", Phone = "905-452-5526", Email = "dan@example.com" });
      ds.Customers.Add(new Models.Customer { FirstName = "Richard", LastName = "Barris", Phone = "905-442-1526", Email = "richard@example.com" });

      ds.SaveChanges();
      done = true;
    }
    //ADD MANUFACTURERS==============================================
    //manufacturer then model, which is dependent on manufacturer, then bike which is dependent on manufacturer
    if (ds.Manufacturers.Count() == 0)
    {
      done = false;

      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Peugeot" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Specialized" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Giant" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Norco" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "CCM" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Raleigh" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Supercycle" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Univega" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Surly" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Bianchi" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Colnago" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "GT" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Huffy" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Miele" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Miyata" });
      ds.Manufacturers.Add(new Models.Manufacturer { Name = "Unspecified" });


      ds.SaveChanges();
      done = true;

    }
    //ADD MODELS============================================
    //load models
    if (ds.Models.Count() == 0)
    {
      done = false;

      var peugeot = ds.Manufacturers.SingleOrDefault(b => b.Name == "Peugeot");
      var specialized = ds.Manufacturers.SingleOrDefault(b => b.Name == "Specialized");
      var giant = ds.Manufacturers.SingleOrDefault(b => b.Name == "Giant");
      var norco = ds.Manufacturers.SingleOrDefault(n => n.Name == "Norco");
      var miyata = ds.Manufacturers.SingleOrDefault(n => n.Name == "Miyata");
      var unspecified = ds.Manufacturers.SingleOrDefault(n => n.Name == "Unspecified");

      ds.Models.Add(new Models.Model
      {
        Manufacturer = unspecified,
        Name = "No Model",
        Info = "This is a dummy empty bicycle object"
      });

      ds.Models.Add(new Models.Model
      {
        Manufacturer = peugeot,
        Name = "Sport 10",
        Info = "A terrible bicycle with French standard components (22.0 stem, etc)"
      });
      ds.Models.Add(new Models.Model
      {
        Manufacturer = miyata,
        Name = "Sport 10",
        Info = "Has downtube braze-on to hold clamps for oldschool downtube shifters. Don't use stem shifters if downtube shifters are available (unless customer says otherwise)"
      });
      ds.Models.Add(new Models.Model
      {
        Manufacturer = peugeot,
        Name = "terrible",
        Info = "A terrible bicycle with French standard components and cottered cranks (22.0 stem, etc)"
      });
      ds.Models.Add(new Models.Model
      {
        Manufacturer = norco,
        Name = "Monterey",
        Info = "rear derailleur hanger replacement is Live To Play part# 220-3345-00"
      });
      ds.Models.Add(new Models.Model
      {
        Manufacturer = norco,
        Name = "bigfoot",
        Info = "Standard fork replacement: lambert #200-409-2287"
      });

      ds.Models.Add(new Models.Model
      {
        Manufacturer = specialized,
        Name = "Rock Hopper"
      });

      ds.Models.Add(new Models.Model
      {
        Manufacturer = specialized,
        Name = "Stump Jumper",
        Info = "Frame setup requires rear Cantilever brakes (has pipe cable guide brazed on to seat tube)"
      });

      ds.Models.Add(new Models.Model
      {
        Manufacturer = giant,
        Name = "Rapid",
        Info = ""
      });

      ds.SaveChanges();
      done = true;
    }

    // var temp = ds.Manufacturers.Where(m => m.Name == "Peugeot"); put breakpoint here to see if peugeot manufacturer loaded w/ associated models

    //ADD BICYCLES==================================================
    //load bikes for customers
    if (ds.Customers.Count() != 0 && ds.Bicycles.Count() == 0)
    {
      done = false;
      var robert = ds.Customers.SingleOrDefault(r => r.FirstName == "Robert");
      var richard = ds.Customers.SingleOrDefault(r => r.FirstName == "Richard");
      var michael = ds.Customers.SingleOrDefault(c => c.FirstName == "Michael");
      var tanvir = ds.Customers.SingleOrDefault(c => c.FirstName == "Tanvir");
      var igor = ds.Customers.SingleOrDefault(c => c.FirstName == "Igor");
      var dan = ds.Customers.SingleOrDefault(c => c.FirstName == "Dan");


      var peugeot = ds.Manufacturers.SingleOrDefault(m => m.Name == "Peugeot");
      var peugS10 = ds.Models.SingleOrDefault(mo => mo.Name == "Sport 10" && mo.Manufacturer.Name == "Peugeot");
      var peugT = ds.Models.SingleOrDefault(mo => mo.Name == "terrible");

      var norco = ds.Manufacturers.SingleOrDefault(n => n.Name == "Norco");
      var norcoMont = ds.Models.SingleOrDefault(nm => nm.Name == "Monterey");
      var norcoB = ds.Models.SingleOrDefault(nm => nm.Name == "bigfoot");

      var miyata = ds.Manufacturers.SingleOrDefault(n => n.Name == "Miyata");
      var miyS10 = ds.Models.SingleOrDefault(mo => mo.Name == "Sport 10" && mo.Manufacturer.Name == "Miyata");


      var specialized = ds.Manufacturers.SingleOrDefault(n => n.Name == "Specialized");
      var rHopper = ds.Models.SingleOrDefault(rh => rh.Name == "Rock Hopper");
      var sJumper = ds.Models.SingleOrDefault(rh => rh.Name == "Stump Jumper");


      var giant = ds.Manufacturers.SingleOrDefault(n => n.Name == "Giant");
      var rapid = ds.Models.SingleOrDefault(rh => rh.Name == "Rapid");






      //NOTE: since this load uses premade objects it does not take into consideration the addNew pattern and bicycleAdd view model class
      //Ie trying to associate an existing model with a bicycle may require that we have the id of the model we want to associate so we can
      //find it (form data includes model id, search db for model, then add it to the new object as it is being created)
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = robert,
        Manufacturer = peugeot,
        Model = peugS10,
        Description = "red Peugeot Sport 10 with black bartape and a silver rear rack"
      });
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = robert,
        Manufacturer = norco,
        Model = norcoB,
        Description = "Olive green Mountainbike with large knobby tires and dual suspension"
      });
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = richard,
        Manufacturer = norco,
        Model = norcoB,
        Description = "Bright orange Mountainbike with large knobby tires and dual suspension"
      });
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = richard,
        Manufacturer = norco,
        Model = norcoMont,
        Description = "Blue roadbike with 9 speed sora groupset and white bartape"
      });

      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = michael,
        Manufacturer = miyata,
        Model = miyS10,
        Description = "red Miyata Sport 10 with black bartape and a black rear rack"
      });
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = tanvir,
        Manufacturer = specialized,
        Model = rHopper,
        Description = "Black Specialized Rock Hopper mtb with a silver rotary bell"
      });
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = tanvir,
        Manufacturer = specialized,
        Model = sJumper,
        Description = "Green Specialized Stump Jumper MTB; All stock accessories"
      });
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = tanvir,
        Manufacturer = miyata,
        Model = miyS10,
        Description = "Blue Miyata Sport 10 with orange bartape"
      });
      ds.Bicycles.Add(new Models.Bicycle
      {
        Customer = dan,
        Manufacturer = giant,
        Model = rapid,
        Description = "black and grey Giant hybrid with semislick tires"
      });

      ds.SaveChanges();
      done = true;
    }

    if (ds.Mechanics.Count() == 0)
    {
      done = false;
      ds.Mechanics.Add(new Models.Mechanic { FirstName = "Sheldon", LastName = "Brown", Email = "sheldon@example.com", Phone = "555-554-5545" });
      ds.Mechanics.Add(new Models.Mechanic { FirstName = "Gerd", LastName = "Shraner", Email = "strangeName@example.com", Phone = "565-564-6645" });
      ds.Mechanics.Add(new Models.Mechanic { FirstName = "Jon", LastName = "Song", Email = "song@example.com", Phone = "905-564-6645" });
      ds.Mechanics.Add(new Models.Mechanic { FirstName = "Darius", LastName = "Chovermann", Email = "chovermann_d@example.com", Phone = "416-620-1545" });
      ds.Mechanics.Add(new Models.Mechanic { FirstName = "Jens", LastName = "Robertson", Email = "jens@example.com", Phone = "647-250-1965" });
      ds.Mechanics.Add(new Models.Mechanic { FirstName = "Kelsey", LastName = "Gray", Email = "kel.g@example.com", Phone = "416-220-9495" });



      ds.SaveChanges();
      done = true;
    }

    //ADD WORKORDERS===============================
    bool needPrevMechanics = false;
    bool needWorkOrderLines = false;//flag to tell whether to add work order lines to preloaded work orders
    if (ds.WorkOrders.Count() == 0)
    {
      done = false;
      needPrevMechanics = true;
      //get bicycles for loading
      var robBike1 = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Robert" && b.Model.Name == "bigfoot");
      var robBike2 = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Robert" && b.Model.Name == "Sport 10");

      var richBike1 = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Richard" && b.Model.Name == "bigfoot");
      var richBike2 = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Richard" && b.Model.Name == "Monterey");

      var mikeBike = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Michael" && b.Model.Name == "Sport 10");//Attention: there are now 2 bikes in the datastore with the same model name, but different manufacturers and customers

      var tanBik1e = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Tanvir" && b.Model.Name == "Rock Hopper");//Attention: there are now 2 bikes in the datastore with the same model name, but different manufacturers and customers
      var tanBike2 = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Tanvir" && b.Model.Name == "Stump Jumper");//Attention: there are now 2 bikes in the datastore with the same model name, but different manufacturers and customers
      var tanBike3 = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Tanvir" && b.Model.Name == "Sport 10");//Attention: there are now 2 bikes in the datastore with the same model name, but different manufacturers and customers

      var danBike = ds.Bicycles.SingleOrDefault(b => b.Customer.FirstName == "Dan" && b.Model.Name == "Rapid");//Attention: there are now 2 bikes in the datastore with the same model name, but different manufacturers and customers


      //get customers for loading
      var robert = ds.Customers.SingleOrDefault(c => c.FirstName == "Robert");
      var richard = ds.Customers.SingleOrDefault(c => c.FirstName == "Richard");
      var michael = ds.Customers.SingleOrDefault(c => c.FirstName == "Michael");
      var tanvir = ds.Customers.SingleOrDefault(c => c.FirstName == "Tanvir");
      var dan = ds.Customers.SingleOrDefault(c => c.FirstName == "Dan");

      //get mechanics for loading
      var sheldon = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Sheldon");
      var gerd = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Gerd");
      var jon = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Jon");
      var darius = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Darius");
      var jens = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Jens");



        //NOTE: does not include work order lines!
        //NOTE: Customers are now accessed from workorders through the workorder's associated Bicycle (otherwise database bursts into flames if workorder is attached to both bike and customer)
        var w1 = new Models.WorkOrder
        {
          Notes = "repack bottom bracket and grind replacement cotter pins into custom taper for french spindle",
          //Customer = robert,
          Bicycle = robBike2,
          CreationTime = DateTime.Now,
          IsCompleted = false,//ie not completed
                              //ActiveMechanic = sheldon,
          

          // Mechanics = new List<Mechanic> {sheldon, jens, jon, darius }
        };

      ds.WorkOrders.Add(w1);
        ds.SaveChanges();
        WorkLog wl1_1 = new WorkLog()
        {
          WorkOrder = w1,
          WorkOrderId = w1.Id,
          Mechanic = sheldon,
          MechanicId = sheldon.Id
        };
        ds.WorkLogs.Add(wl1_1);
        ds.SaveChanges();

      ds.WorkOrders.Add(new Models.WorkOrder
      {
        Notes = "fix front and rear flat tires--also replace rear tire with 700x23c Continental, wants new black bartape, do tuneup ",
        // Customer = richard,
        Bicycle = richBike2,
        CreationTime = DateTime.Now,
        IsCompleted = true//completed
      });
      ds.WorkOrders.Add(new Models.WorkOrder
      {
        Notes = "Look at bike and fill in work order for tuneup",
        // Customer = robert,
        Bicycle = robBike1,
        CreationTime = DateTime.Now,
        IsCompleted = false//ie not completed
      });

        var w4 = new Models.WorkOrder
        {
          Notes = "Just picked up for a flat fix. Customer rode about a block and it went flat again. Fix ASAP and do not charge again for the work. Do not make any work order lines b/c no charge",
          //  Customer = michael,
          Bicycle = mikeBike,
          CreationTime = DateTime.Now,
          IsCompleted = false,//ie not completed
          HighPriority = true,
          //Mechanics= new List<Mechanic> { gerd}

        };
        ds.WorkOrders.Add(w4);
        ds.SaveChanges();
        var wl4_1 = new WorkLog()
        {
          WorkOrder = w4,
          WorkOrderId = w4.Id,
          Mechanic = gerd,
          MechanicId = gerd.Id,
          TimeStarted = DateTime.Now.AddDays(-3),
          TimeStopped = DateTime.Now.AddDays(-1),
          IsActiveOnOrder = false
        };

      ds.WorkLogs.Add(wl4_1);

      ds.SaveChanges();
      needWorkOrderLines = true;
      done = true;
    }
    /*
          //add workorders to mechanics' previous workorders List (being careful that this matches the previousMechanics for workorders loaded above)
          if (needPrevMechanics)
          {
            done = false;
            //retrieve mechanics from database:
            var sheldon = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Sheldon");
            var gerd = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Gerd");
            var jon = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Jon");
            var darius = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Darius");
            var jens = ds.Mechanics.SingleOrDefault(m => m.FirstName == "Jens");
            //ATTENTTION: this may break if not preloading all data at once (because I got lazy below)!!!:
            //retrieve relevant workorders (and get lazy, realizing that entity id numbering starts from 1 if preloading) 
            var wo1 = ds.WorkOrders.Find(1);
            var wo4 = ds.WorkOrders.Find(4);
            //update mechanic's list of previous workorders:
            sheldon.PreviousWorkOrders.Add(wo1);
            jens.PreviousWorkOrders.Add(wo1);
            jon.PreviousWorkOrders.Add(wo1);
            darius.PreviousWorkOrders.Add(wo1);
            gerd.PreviousWorkOrders.Add(wo4);
            ds.SaveChanges();
            done = true;
          }
          */
    //add some work order lines with service items to some work orders
    if (needWorkOrderLines)
    {
      //
      var order1 = ds.WorkOrders.SingleOrDefault(o => o.Bicycle.Customer.FirstName == "Robert" && o.Bicycle.Model.Name == "Sport 10");

      var servBBRepack = ds.ServiceItems.SingleOrDefault(s => s.Description == "Repack Bottom Bracket");
      //

      //
      var order2 = ds.WorkOrders.SingleOrDefault(o => o.Bicycle.Customer.FirstName == "Richard" && o.Bicycle.Model.Name == "Monterey");

      var servFlat = ds.ServiceItems.SingleOrDefault(s => s.Description == "Flat tire fix with tube");
      var servBartape = ds.ServiceItems.SingleOrDefault(s => s.Description == "wrap new bartape");
      var servTU = ds.ServiceItems.SingleOrDefault(s => s.Description == "Base Tuneup Labour");
      //

      var o1L1 = new Models.WorkOrderLine { ServiceItem = servBBRepack, Quantity = 1, LineTotal = (servBBRepack.Price * 1) };
      order1.WorkOrderLines.Add(o1L1);

      var o2L1 = new Models.WorkOrderLine { ServiceItem = servFlat, Quantity = 2, LineTotal = (servFlat.Price * 2) };//is there a way to do price * Quantity during construction?
      order2.WorkOrderLines.Add(o2L1);
      var o2L2 = new Models.WorkOrderLine { ServiceItem = servBartape, Quantity = 1, LineTotal = (servBartape.Price * 1) };
      order2.WorkOrderLines.Add(o2L2);
      var o2L3 = new Models.WorkOrderLine { ServiceItem = servTU, Quantity = 1, LineTotal = (servTU.Price * 1) };
      order2.WorkOrderLines.Add(o2L3);

      ds.SaveChanges();
      done = true;
    }



    //variable for checking success of workorder addition (put breakpoint at 'return done'):
    var checkOrders = ds.WorkOrders;
    // var checkCustomers = ds.Customers;
    return done;
  }


  public bool RemoveData()
  {
    try
    {
      foreach (var r in ds.RoleClaims)
      {
        ds.Entry(r).State = System.Data.Entity.EntityState.Deleted;
      }
      ds.SaveChanges();

      foreach (var sl in ds.ServiceItems)
      {
        ds.Entry(sl).State = System.Data.Entity.EntityState.Deleted;
        ds.SaveChanges();
      }
      return true;
    }
    catch (Exception)
    {
      return false;//consider retrurning the actual exception for error handling...
    }
  }

  public bool RemoveDatabase()
  {
    try
    {
      return ds.Database.Delete();
    }
    catch (Exception)
    {
      return false;
    }
  }


}
}
