using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers
{
    public class BicycleAdd
    {
        public BicycleAdd(){

            Deactivated = false;
        }
    [Required, StringLength(100)]
    public string Description { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public CustomerBase Customer { get; set; }
    
    public bool Deactivated { get; set; }
  }


    //in trying to solve the dummy bike issue without creating a circ reference, 
    //went with this. please modify to make workable if we got this route, or 
    //delete it out right if it is crap.
    public class BicycleAddNewCustomer : BicycleAdd {
        //added a constructor to build a dummy bike on creation of new customer...
        //constructor takes a string of bike info
        public BicycleAddNewCustomer(String bikeInfo)
        {

            BikeName = bikeInfo;

        }
        //copied and modified to remove required on manufac name
        [StringLength(100)]
        public string BikeName { get; set; }


        //[StringLength(100)]
        //removed to make adding a dummy bike easier
        //public string ManufacturerName { get; set; }
    }


  public class BicycleBase : BicycleAdd
  {
        
    public int Id { get; set; }
    
  }

  public class BicycleWithAssoc : BicycleBase
  {
    [StringLength(100)]
    public string ModelName { get; set; }


    [Required, StringLength(100)]
    public string ManufacturerName { get; set; }

    public string BikeListLabel { get { return string.Format("{0} {1} | {2} |", ManufacturerName, ModelName, Description); } }//another hack job selectlist labelling thing

    public string CustomerFirstName { get; set; }

    public string CustomerLastName { get; set; }

    }

    // temp addforcreate class
    // old add had no associations and base inherited from it, bicyclewithassoc inherited from that
    // rewriting isn't high priority right now
    public class BicycleAddForCreate 
    {
        //public ManufacturerBase Manufacturer { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Display(Name = "Manufacturer")]
        [Required, StringLength(100)]
        public string ManufacturerName { get; set; }

        [Display(Name = "Model")]
        [Required, StringLength(100)]
        public string ModelName { get; set; }
    }

    public class BicycleAddForCreateForm : BicycleAddForCreate
    {
        //Customer First Name
        public string CustomerFirstName { get; set; }
        //Customer Last Name
        public string CustomerLastName { get; set; }
        //Customer Email
        public string CustomerEmail { get; set; }
        //Customer Phone
        public string CustomerPhone { get; set; }
    }

    public class BicycleGeneral : BicycleAddForCreateForm
    {
        public int Id { get; set; }
    }

    public class BicycleEditForm
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public CustomerBase Customer { get; set; }

        // public ManufacturerBase Manufacturer { get; set; }

        // public ModelBase Model { get; set; }

        [Display(Name = "Manufacturer")]
        [Required, StringLength(100)]
        public string ManufacturerName { get; set; }

        [Display(Name = "Model")]
        [Required, StringLength(100)]
        public string ModelName { get; set; }

        //Customer First Name
        public string CustomerFirstName { get; set; }
        //Customer Last Name
        public string CustomerLastName { get; set; }
        //Customer Email
        public string CustomerEmail { get; set; }
        //Customer Phone
        public string CustomerPhone { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }
    }

    public class BicycleEdit
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        [Required, StringLength(100)]
        public string ManufacturerName { get; set; }

        [Required, StringLength(100)]
        public string ModelName { get; set; }
    }
}