using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers
{
  public class MechanicAdd
  {
        
    [Required, StringLength(100)]
    public string FirstName { get; set; }

    [Required, StringLength(100)]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please enter a valid number, with no special characters or spaces")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})", ErrorMessage = "Please enter a valid number, with no special characters or spaces")]
    [StringLength(20)]
    public string Phone { get; set; }

    public string Email { get; set; }

    //public IEnumerable<WorkOrderBase> ActiveWorkOrders { get; set; }
    //public IEnumerable<WorkOrderBase> WorkOrderList { get; set; }//not used any more (PreviousWorkOrders does this)
    //public IEnumerable<WorkOrderGeneral> PreviousWorkOrders { get; set; }//This name replaced WorkOrderList (as a less ambiguous name)

    //POC2:
    public WorkOrderBase WorkOrder { get; set; }
  }

  public class MechanicBase : MechanicAdd
  {
    public int Id { get; set; }
    public string MechListLabel { get { return string.Format("{0} {1} {2} {3}", FirstName, LastName, Phone, Email); } }

  }


  public class MechanicWithWorkOrders
  {
    public int Id { get; set; }
    public string MechListLabel { get { return string.Format("{0} {1} {2} {3} {4}",  (ActiveWorkOrders.Count()==0)? "": ("|[On Order(s)|]"), FirstName, LastName, Phone, Email); } }
    [Required, StringLength(100)]
    public string FirstName { get; set; }


    [Required, StringLength(100)]
    public string LastName { get; set; }

    [Required, StringLength(20)]
    public string Phone { get; set; }

    public string Email { get; set; }

    public IEnumerable<WorkOrderForMechDetail> ActiveWorkOrders { get; set; }
    public IEnumerable<WorkOrderForMechDetail> PreviousWorkOrders { get; set; }//This name replaced WorkOrderList (as a less ambiguous name)
    //an error string that can be populated in the controller and presented in the view.
    public string Error { get; set; }

    //POC2:
    //public WorkOrderForMechDetail WorkOrder { get; set; }
  }
}