using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkOrderManagementSystem.Models
{
  //for account purposes
  public class RoleClaim
  {
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }
  }


  public class Mechanic
  {
    public Mechanic()
    {
      WorkLogs = new List<WorkLog>();
    }


    //Attetnion: POC2: 1:0 to 1:0 relation: just use a nullable int Id for associated workorder rather than use a nav property
    // public int? WorkOrderId { get; set; }
    //POC2: a mechanic may appear as active on only one workorder. previous mechanics are not recorded
    // public WorkOrder WorkOrder { get; set; }
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string FirstName { get; set; }

    [Required, StringLength(100)]
    public string LastName { get; set; }

    [Required, StringLength(20)]
    public string Phone { get; set; }

    public string Email { get; set; }

    public ICollection<WorkLog> WorkLogs { get; set; }
    //public ICollection<WorkOrder> ActiveWorkOrders { get; set; }
    //public ICollection<WorkOrder> WorkOrderList { get; set; } //THIS IS PreviousWorkOrders
    //public ICollection<WorkOrder> PreviousWorkOrders { get; set; }
    //public string MechListLabel { get { return string.Format("{0} {1} {2} {3}", FirstName, LastName, Phone, Email); } }

  }

  public class WorkLog
  {
    public WorkLog()
    {
      TimeStarted = DateTime.Now;
      TimeStopped = DateTime.Now;
      IsActiveOnOrder = true;//Attention: set true on construction because we assume that a new workLog is added when a mechanic is added to a workorder
    }
    //[Key, ForeignKey("Mechanic")]
    public int MechanicId { get; set; }//should be configured w/ fluent api so no data annotations?
    // [Key, ForeignKey("WorkOrder")]
    public int WorkOrderId { get; set; }

    public DateTime TimeStarted { get; set; }
    public DateTime TimeStopped { get; set; }
    public bool IsActiveOnOrder { get; set; }//boolean flag denoting whether mechanic is active on the work order
                                             //--mechanics can be active on many workorders but a work order may only have one active mechanic (need to manually enforce in program logic)
    public Mechanic Mechanic { get; set; }
    public WorkOrder WorkOrder { get; set; }
    //nav property--LogEntries allows multiple start-stop times on a work order for the same mechanic
    public ICollection<LogEntry> LogEntries { get; set; }
  }

  //Use ds.LogEntries.Include("WorkLog").OrderByDescending(l=>l.Id).FirstOrDefault().Where(l=>l.WorkLog.Id==id) to get latest logEntry for a workLog
  //Use ds.LogEntries.Include("WorkLog").OrderByAscending(l=>l.Id).FirstOrDefault().Where(l=>l.WorkLog.Id==id) to get latest logEntry for a WorkLog
  //or OrderBy mechStopped if not inProgress?
  public class LogEntry
  {
    public LogEntry()
    {
      mechStarted = DateTime.Now;
      mechStopped = DateTime.Now;
    }
    //PK unique identifier
    public int Id { get; set; }
    //using specifically structured LINQ queries instead (see comments on top of this class declaration)
    //public int LookupId { get; set; }
    public DateTime mechStarted { get; set; }
    public DateTime mechStopped { get; set; }
    //flag determines if mechanic still working (avoid confusion since mechStarted==mechStopped for new LogEntries)
    public bool inProgress { get; set; }
    [Required]
    public WorkLog WorkLog { get; set; }
  }

  public class Customer
  {
    public Customer()
    {
      Bicycles = new List<Bicycle>();
      //  WorkOrders = new List<WorkOrder>();
    }

    public int Id { get; set; }

    [Required, StringLength(100)]
    public string FirstName { get; set; }

    [Required, StringLength(100)]
    public string LastName { get; set; }

    [Required, StringLength(20)]
    public string Phone { get; set; }

    public string Email { get; set; }

    public ICollection<Bicycle> Bicycles { get; set; }
    // public ICollection<WorkOrder> WorkOrders { get; set; }

    // public string CustListLabel { get { return string.Format("{0} {1} {2} {3}", FirstName, LastName, Phone, Email); } } //used for generating selectlists with nicely formattedlabels (from https://stackoverflow.com/questions/4868627/display-two-properties-in-the-dropdownlist)

  }

  public class Bicycle
  {

    public Bicycle()
    {
      WorkOrders = new List<WorkOrder>();

      Deactivated = false;
    }
    public int Id { get; set; }
    [Required]
    public Model Model { get; set; }
    //[Required]
    public Manufacturer Manufacturer { get; set; }

    [Required, StringLength(100)]
    public string Description { get; set; }

    public int CustomerId { get; set; }

    public bool Deactivated { get; set; }


    //public string ModelName { get; set; }
    [Required]
    public Customer Customer { get; set; }
    //public string BikeListLabel { get { return string.Format("{0} {1} | {2} |", this.Manufacturer.Name, this.Model.Name, (Description.Substring(0, 30) + "...")); } }//hacktastic selectList labelling
    public ICollection<WorkOrder> WorkOrders { get; set; }

  }

  public class ServiceItem
  {
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Description { get; set; }

    //It data annotations? It might be good to allow negative values for use cases like discounts (ie negative value that subtracts from work order total?)
    public double Price { get; set; }

  }

  public class WorkOrderLine
  {

    public int Id { get; set; }

    public ServiceItem ServiceItem { get; set; }

    public int Quantity { get; set; }

    public double LineTotal { get; set; }
    [Required]
    public WorkOrder WorkOrder { get; set; }//Attention: added for association
  }

  public class WorkOrder
  {
    public WorkOrder()
    {

      WorkOrderLines = new List<WorkOrderLine>();
      WorkLogs = new List<WorkLog>();
      //single active mechanic for POC2 (no previous, no multiple mechanics on multiple workorders
      // Mechanics = new List<Mechanic>();//this works fine because we are trying to construct a list of mechanics, not the mechanics themselves.

      CreationTime = DateTime.Now;

      CompletionTime = DateTime.Now;
    }
    [Key]
    public int Id { get; set; }


    [Required, StringLength(350)]
    public string Notes { get; set; }

    public ICollection<WorkOrderLine> WorkOrderLines { get; set; }
    
    //invoice Id
    public int InvoiceId { get; set; }
    // [Required]
    // public Customer Customer { get; set; } //Attention: NOW ACCESSED THROUGH BICYCLE OBJECT ASSOCIATION (INCUDING BOTH IN WORKORDERS CAUSES CIRCULAR REFERENCE AND STACK OVERFLOW)

    //public Mechanic Mechanic { get; set; }

    //POC2: use nullable Id for lookup and not a nav property (0:1 to 0:1 association)
    //public int? MechanicId { get; set; }
    //  public ICollection<Mechanic> Mechanics { get; set; }
    [Required]
    public Bicycle Bicycle { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime CompletionTime { get; set; }

    [Required]
    public bool IsCompleted { get; set; }

    public bool HighPriority { get; set; }
    public bool IsRejected { get; set; }//true indicates workorder was rejected by customer (implies setting rejected should also set completed to true?)
    public ICollection<WorkLog> WorkLogs { get; set; }
  }

  public class Model
  {
    public Model()
    {
      Bicycles = new List<Bicycle>();
    }

    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }
    [Required]
    public Manufacturer Manufacturer { get; set; }

    [StringLength(300)]
    public string Info { get; set; }

    public string PhotoContentType { get; set; }

    public byte[] Photo { get; set; }

    public ICollection<Bicycle> Bicycles { get; set; }
  }

  public class Manufacturer
  {
    public Manufacturer()
    {
      Models = new List<Model>();
      Bicycles = new List<Bicycle>();
    }

    public int Id { get; set; }

    public ICollection<Model> Models { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }
    public ICollection<Bicycle> Bicycles { get; set; }

  }

  public class Invoice
  {
        public Invoice()
        {

            InvoiceLines = new List<InvoiceLine>();
            CreationTime = DateTime.Now;
            CompletionTime = DateTime.Now.AddDays(-25);
        }

    [Key]
    public int Id { get; set; }

    public bool IsPaid { get; set; }
    [Required]
    public int  WorkOrderId { get; set; }
   //Dedicated class to ensure that invoice data is static regardless of whether or not classes
   //it was generated with change (ie service item prices)
    public ICollection<InvoiceLine> InvoiceLines { get; set; }

    [Required]
    //USE FOR CURRENT customer info LOOKUP (ds.customers.find(inv.CustomerId)
    public int CustomerId { get; set; }
    //Strings to store customer info when the invoice was created
    public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhoneNumber { get; set; }
    [Required]
    public string BicycleDescription { get; set; }
    [Required]
    //set this  from Bicycle->Model->Name
    public string BicycleModelName { get; set; }
    [Required]
    //set this from Bicycle->Model->Manufactuer -> Name
    public string BicycleModelManufacturerName { get; set; }

    
    public DateTime CreationTime { get; set; }
    
    //need to use in paid/unpaid logic
    public DateTime CompletionTime { get; set; }

   
  }

  public class InvoiceLine
  {
    public int Id { get; set; }

    public string ServiceDescription { get; set; }
    public double ServicePrice { get; set; }

    public int Quantity { get; set; }
    //can be calculated
   // public double LineTotal { get; set; }
    [Required]
    public Invoice Invoice { get; set; }
  }

}