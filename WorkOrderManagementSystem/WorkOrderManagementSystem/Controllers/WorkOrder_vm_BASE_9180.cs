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

    public ICollection<WorkOrderLineBase> WorkOrderLines { get; set; }

    // [Required]
    // public int CustomerId { get; set; }//ATTENTION: Customer Selectlists exist in addForms now only to help find a list of bicycles to associate with the workorder!!!

    public int ActiveMechanicId { get; set; }
    [Required]
    public int BicycleId { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime CompletionTime { get; set; }

    [Required]
    public bool Status { get; set; }

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
    public bool Status { get; set; }

    public bool HighPriority { get; set; }
  }

  public class WorkOrderBase : WorkOrderAdd
  {
    public int Id { get; set; }
  }

  public class WorkOrderGeneral : WorkOrderBase
  {

    //work order id
    public int Id { get; set; }
    //work order note length changed to max of 50 chars
    [Required, StringLength(50)]
    public string Notes { get; set; }
    //work order status
    [Required]
    public bool Status { get; set; }
    /*
        //Customer Id
        public int CustomerId { get; set; }
        //Customer First Name
        public string CustomerFirstName { get; set; }
        //Customer Last Name
        public string CustomerLastName { get; set; }
        //Customer Email
        public string CustomerEmail { get; set; }
        //Customer Phone
        public int CustomerPhone { get; set; }
        */
    //  public CustomerBase Customer { get; set; }
    public string BicycleCustomerFirstName { get; set; }
    public string BicycleCustomerLastName { get; set; }
    public string BicycleCustomerEmail { get; set; }
    public string BicycleCustomerPhone { get; set; }

    public MechanicBase ActiveMechanic { get; set; }

    //Bicycle Id
    public int BicycleId { get; set; }
    //Bicycle Description
    public string BicycleDescription { get; set; }
    public bool HighPriority { get; set; }
  }
  public class WorkOrderEditForm
  {

    public int Id { get; set; }
    //select lists and list text info for searching customers, bikes, mechanics
    public SelectList CustomerSearchList { get; set; }//ATTENTION: Customer Selectlists exist in addForms now only to help find a list of bicycles to associate with the workorder!!!
    [Required(ErrorMessage = "You must select a bicycle for this work order")]
    //public int BicycleId { get; set; }
    public SelectList BicycleSearchList { get; set; }
    public SelectList MechanicSearchList { get; set; }
    public SelectList ServiceItemList { get; set; }


    [Required, StringLength(350)]
    public string Notes { get; set; }

    //public ICollection<WorkOrderLineAddForm> WorkOrderLines { get; set; }
    public ICollection<int> Quantities { get; set; }
    //public ICollection<double> Prices { get; set; }

    [Required]
    public bool Status { get; set; }

    public bool HighPriority { get; set; }
  }
  public class WorkOrderEdit
  {

    public int ActiveMechanicId { get; set; }
    [Required]
    public int BicycleId { get; set; }


    //public int BicycleId { get; set; }
    public SelectList BicycleSearchList { get; set; }
    public SelectList MechanicSearchList { get; set; }


    [Required, StringLength(350)]
    public string Notes { get; set; }

    public ICollection<WorkOrderLineAdd> WorkOrderLines { get; set; }

    [Required]
    public bool Status { get; set; }

    public bool HighPriority { get; set; }
  }
}