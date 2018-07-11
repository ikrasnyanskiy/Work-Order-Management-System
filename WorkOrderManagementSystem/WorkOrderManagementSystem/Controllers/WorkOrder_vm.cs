
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers

{
  public class WorkOrderAdd
  {
    public WorkOrderAdd()
    {
      CreationTime = DateTime.Now;
      CompletionTime = DateTime.Now;
    }
    [Required, StringLength(350)]
    public string Notes { get; set; }

    public IEnumerable<WorkOrderLineBase> WorkOrderLines { get; set; }
    //public IEnumerable<WorkLogBase> WorkLogs { get; set; }

    // [Required]
    // public int CustomerId { get; set; }//ATTENTION: Customer Selectlists exist in addForms now only to help find a list of bicycles to associate with the workorder!!!

    public int? MechanicId { get; set; }//form can be set with a chosen machanic
    [Required]
    public int BicycleId { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime CompletionTime { get; set; }

    [Required]
    public bool IsCompleted { get; set; }

    public bool HighPriority { get; set; }
  }

  public class WorkOrderPartial //for populating selectlist partial views
  {
    public SelectList CustomerSearchList { get; set; }
    public SelectList BicycleSearchList { get; set; }
    public SelectList MechanicSearchList { get; set; }
  }

  public class WorkOrderAddForm
  {
    //select lists and list text info for searching customers, bikes, mechanics
    public SelectList CustomerSearchList { get; set; }//ATTENTION: Customer Selectlists exist in addForms now only to help find a list of bicycles to associate with the workorder!!!
    [Required(ErrorMessage = "You must select a bicycle for this work order")]
    public int BicycleId { get; set; }
    public SelectList BicycleSearchList { get; set; }
    public SelectList MechanicSearchList { get; set; }


    [Required, StringLength(350)]
    public string Notes { get; set; }

    public ICollection<WorkOrderLineBase> WorkOrderLines { get; set; }

    [Required]
    public bool IsCompleted { get; set; }

    public bool HighPriority { get; set; }
  }

  public class WorkOrderBase : WorkOrderAdd
  {
    public int Id { get; set; }
    
    
  }

  public class WorkOrderGeneral
  {

    //Work Order Id
    public int Id { get; set; }
    //Work Order Note length changed to max of 50 chars
    [Required, StringLength(50)]
    public string Notes { get; set; }
    //Work Order Status
    [Required]
    public bool IsCompleted { get; set; }


    //Customer First Name
    public string BicycleCustomerFirstName { get; set; }
    //Customer Last Name
    public string BicycleCustomerLastName { get; set; }
    //Customer Email
    public string BicycleCustomerEmail { get; set; }
    //Customer Phone
    public string BicycleCustomerPhone { get; set; }
    //Active Mechanic for the Work Order
    public MechanicBase ActiveMechanic { get; set; }//needs to be set manually still
    public IEnumerable<MechanicBase> InactiveMechanics { get; set; }//need to set manually where worklog.IsActiveOnWorkorder==false


    //Bicycle Id
    public int BicycleId { get; set; }
    //Bicycle Description
    public string BicycleDescription { get; set; }
    //Work Order Priority
    public bool HighPriority { get; set; }

        public int InvoiceId { get; set; }
    }

  public class WorkOrderEditForm
  {

    public int Id { get; set; }
    //select lists and list text info for searching customers, bikes, mechanics
    public SelectList CustomerSearchList { get; set; }//ATTENTION: Customer Selectlists exist in addForms now only to help find a list of bicycles to associate with the workorder!!!
    [Required(ErrorMessage = "You must select a bicycle for this work order")]
    //public int BicycleId { get; set; }
    public SelectList BicycleSearchList { get; set; }
    public int BicycleId { get; set; }//used for strong typing validation in dropdownlistfor
    //for setting active mechanic
    public int? ActiveMechanicId { get; set; }//NOTE: the mechanic selectlist will automatically be set as required on the form if this is not set as nullable
    public SelectList MechanicSearchList { get; set; }
    public SelectList ServiceItemList { get; set; }


    [Required, StringLength(350)]
    public string Notes { get; set; }

    //public ICollection<WorkOrderLineAddForm> WorkOrderLines { get; set; }

    public ICollection<int> ServiceItemIds { get; set; }
    public ICollection<int> Quantities { get; set; }
    //public ICollection<double> Prices { get; set; }

    public ICollection<WorkOrderLineBase> WorkOrderLines { get; set; }

    [Required]
    public bool IsCompleted { get; set; }

    public bool HighPriority { get; set; }
  }
  public class WorkOrderEdit
  {

    public int Id { get; set; }
    public int ActiveMechanicId { get; set; }
    [Required]
    public int BicycleId { get; set; }


    //public int BicycleId { get; set; }
    public SelectList BicycleSearchList { get; set; }
    public SelectList MechanicSearchList { get; set; }


    [Required, StringLength(350)]
    public string Notes { get; set; }

    public ICollection<WorkOrderLineAdd> WorkOrderLines { get; set; }

    public ICollection<int> ServiceItemIds { get; set; }
    [ValidateLineQuantities(ErrorMessage = "Service item quantities must be between 0 and 9999 inclusive")]//this error message currently gets overridden for some reason
    public ICollection<int> Quantities { get; set; }

    [Required]
    public bool IsCompleted { get; set; }

    public bool HighPriority { get; set; }

    //custom validation class for quantities collection on form. Using [Range] data annotation on a collection
    //will always cause validation to fail (possibly becuase Range is based on the single object's value, and
    //the value of a collection may be some kind of object hash as in java?)
    public class ValidateLineQuantities : ValidationAttribute
    {
      public override bool IsValid(object value)
      {
        var list = value as IEnumerable<int>;
        bool validFlag = true;
        if (list != null)
        {
          foreach (var item in list)
          {
            if (item < 0 || item > 9999)
            {
              validFlag = false;
              
            }
          }

        }
        return validFlag;
      }
      
    }

  }
  public class WorkOrderDetails //should not inherit from workorderGeneral because it puts a 50 char limit on the notes that are rendered
  {
    public WorkOrderDetails()
    {
      WorkOrderLines = new List<WorkOrderLineDetails>();
      CreationTime = DateTime.Now;
      //CompletionTime = DateTime.Now;  //not sure if setting this at creation time is a good idea
      CreationTimeString = String.Format("{0:f}", CreationTime);
    }
    //accessibility setting is questionable...
    public class PrevLog
    {
     public PrevLog()
      {
        Mech = null;
        startTime = DateTime.Now;
        endTime = DateTime.Now;
      }
     public PrevLog(MechanicBase mech, DateTime start, DateTime end)
      {
        Mech = mech;
        startTime = start;
        endTime = end;
      }
     public MechanicBase Mech { get; set; }
      public DateTime startTime { get; set; }
      public DateTime endTime { get; set; }
    }

    //Work Order Id
    public int Id { get; set; }
    //Work Order Note length max as set in design model class
    [Required, StringLength(350)]
    public string Notes { get; set; }
    //Work Order Status
    [Required]
    public bool IsCompleted { get; set; }


    //Customer First Name
    public string BicycleCustomerFirstName { get; set; }
    //Customer Last Name
    public string BicycleCustomerLastName { get; set; }
    //Customer Email
    public string BicycleCustomerEmail { get; set; }
    //Customer Phone
    public string BicycleCustomerPhone { get; set; }
    //Active Mechanic for the Work Order
    public PrevLog ActiveMechanic { get; set; }
    public IEnumerable<PrevLog> InactiveMechanics { get; set; }

    //Last Mechanic For the Work Order
    //public IEnumerable<MechanicBase> Mechanics { get; set; }//POC2: not used

    //Bicycle Id
    public int BicycleId { get; set; }
    //Bicycle Description
    public string BicycleDescription { get; set; }
    //Work Order Priority
    public bool HighPriority { get; set; }
    public IEnumerable<WorkOrderLineDetails> WorkOrderLines { get; set; }//used workorderLineAdd because had relevant properties

    public DateTime CreationTime { get; set; }

    public String CreationTimeString { get; set; }

    public DateTime CompletionTime { get; set; }

  }
  public class WorkOrderForMechDetail
  {

    //Work Order Id
    public int Id { get; set; }
    //Work Order Note length changed to max of 50 chars
    [Required, StringLength(50)]
    public string Notes { get; set; }
    //Work Order IsCompleted
    [Required]
    public bool IsCompleted { get; set; }


    //Customer First Name
    public string BicycleCustomerFirstName { get; set; }
    //Customer Last Name
    public string BicycleCustomerLastName { get; set; }
    //Customer Email
    public string BicycleCustomerEmail { get; set; }
    //Customer Phone
    public string BicycleCustomerPhone { get; set; }
    //Active Mechanic for the Work Order
    //public MechanicBase Mechanic { get; set; }


    //Bicycle Id
    public int BicycleId { get; set; }
    //Bicycle Description
    public string BicycleDescription { get; set; }
    //Work Order Priority
    public bool HighPriority { get; set; }
    //assigned and unassigned times are populated from worklog TimeStarted
    //and from worklog StopTime
    public DateTime AssignedTime { get; set; }
    public DateTime UnassignedTime { get; set; }
  }
}

