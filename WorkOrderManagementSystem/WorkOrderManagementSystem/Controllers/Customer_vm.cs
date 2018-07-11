using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers
{
    public class CustomerAdd
    {
        //Customer constructor - populates a dummy bike for each customer on creation
        public CustomerAdd()
        {
            //create a list of bikes for the customer
            List<BicycleAddNewCustomer> Bicycles = new List<BicycleAddNewCustomer>();
            //populate a bicycle object with dummyBikeName and dummyManufacturer
            String dummyBike = "no bike yet";
            BicycleAddNewCustomer dummy = new BicycleAddNewCustomer(dummyBike);
            Bicycles.Add(dummy);


        }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a valid number, with no special characters or spaces")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})", ErrorMessage = "Please enter a valid number, with no special characters or spaces")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email must be in the form: me@site.com")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ICollection<BicycleBase> Bicycles { get; set; }

        public ICollection<BicycleAddNewCustomer> NewCustBike { get; set; }

    }

    public class CustomerGeneral : CustomerAdd
    {
        public int Id { get; set; }
        public string CustListLabel { get { return string.Format("{0} {1} {2} {3}", FirstName, LastName, Phone, Email); } }//hack for giving selectlists nice labels
    }

    public class CustomerBase
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(20)]
        public string Phone { get; set; }

        public string Email { get; set; }

        public int Id { get; set; }
        public string CustListLabel { get { return string.Format("{0} {1} {2} {3}", FirstName, LastName, Phone, Email); } }//hack for giving selectlists nice labels
    }

    public class CustomerWithDetails
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(20)]
        public string Phone { get; set; }

        public string Email { get; set; }

        public ICollection<BicycleWithAssoc> Bicycles { get; set; }

        public int Id { get; set; }
        public string CustListLabel { get { return string.Format("{0} {1} {2} {3}", FirstName, LastName, Phone, Email); } }//hack for giving selectlists nice labels
    }

    public class CustomerEditForm
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a valid number, with no special characters or spaces")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})", ErrorMessage = "Please enter a valid number, with no special characters or spaces")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email must be in the form: me@site.com")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class CustomerEdit
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }
    }
}