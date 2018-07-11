using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WorkOrderManagementSystem.Models;

namespace WorkOrderManagementSystem.Controllers
{
  public class InvoiceBase
  {
        public InvoiceBase() {
            CreationTime = DateTime.Now;
            //set completion time for -25 days from today....
            //will check against this for completion and reporting. 
            CompletionTime = DateTime.Now.AddDays(-25);
        }

    //invoice Id
    public int Id { get; set; }

    public bool IsPaid { get; set; }
    [Required]
    public int WorkOrderId { get; set; }

   

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
    
    //set the display format for the printed invoice here?
    //[DisplayFormat(DataFormatString = "{O:MM/dd/yyyy}")]
    public DateTime CreationTime { get; set; }

    public DateTime CompletionTime { get; set; }
  }
  
  public class InvoiceWithDetails : InvoiceBase
  {
    public InvoiceWithDetails()
    {
      InvoiceLines = new List<InvoiceLineBase>();

      InvoiceCompletionTime = DateTime.Now.AddDays(-100);

    }
    public IEnumerable<InvoiceLineBase> InvoiceLines { get; set; }

    //[DisplayFormat (DataFormatString="{O:C}")]
    public double Subtotal { get; set; }

   //[DisplayFormat(DataFormatString = "{O:C}")]
   public double Total { get; set; }
   
   public DateTime InvoiceCompletionTime { get; set; }
  }
}