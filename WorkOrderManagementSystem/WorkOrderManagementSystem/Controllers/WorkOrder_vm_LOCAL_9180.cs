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

    public class WorkOrderGeneral
    {

        //Work Order Id
        public int Id { get; set; }
        //Work Order Note length changed to max of 50 chars
        [Required, StringLength(50)]
        public string Notes { get; set; }
        //Work Order Status
        [Required]
        public bool Status { get; set; }


        //Customer First Name
        public string BicycleCustomerFirstName { get; set; }
        //Customer Last Name
        public string BicycleCustomerLastName { get; set; }
        //Customer Email
        public string BicycleCustomerEmail { get; set; }
        //Customer Phone
        public string BicycleCustomerPhone { get; set; }
        //Active Mechanic for the Work Order
        public MechanicBase ActiveMechanic { get; set; }

        //Bicycle Id
        public int BicycleId { get; set; }
        //Bicycle Description
        public string BicycleDescription { get; set; }
        //Work Order Priority
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
        public int BicycleId { get; set; }//used for strong typing validation in dropdownlistfor
        public int ActiveMechanicId { get; set; }
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
        public bool Status { get; set; }

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
        public ICollection<int> Quantities { get; set; }

        [Required]
        public bool Status { get; set; }

        public bool HighPriority { get; set; }
    }
  public class WorkOrderDetails //should not inherit from workorderGeneral because it puts a 50 char limit on the notes that are rendered
  {
        //missing constructor for list of WorkOrderLines?
        public WorkOrderDetails() {
            WorkOrderLines = new List<WorkOrderLineDetails>();
        }
    //Work Order Id
    public int Id { get; set; }
    //Work Order Note length max as set in design model class
    [Required, StringLength(350)]
    public string Notes { get; set; }
    //Work Order Status
    [Required]
    public bool Status { get; set; }


    //Customer First Name
    public string BicycleCustomerFirstName { get; set; }
    //Customer Last Name
    public string BicycleCustomerLastName { get; set; }
    //Customer Email
    public string BicycleCustomerEmail { get; set; }
    //Customer Phone
    public string BicycleCustomerPhone { get; set; }
    //Active Mechanic for the Work Order
    public MechanicBase ActiveMechanic { get; set; }

    //Bicycle Id
    public int BicycleId { get; set; }
    //Bicycle Description
    public string BicycleDescription { get; set; }
    //Work Order Priority
    public bool HighPriority { get; set; }
    public IEnumerable<WorkOrderLineDetails> WorkOrderLines { get; set; }//used workorderLineAdd because had relevant properties

  }
}

