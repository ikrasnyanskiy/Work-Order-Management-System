using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkOrderManagementSystem.Controllers
{
  public class InvoiceLineBase
  {
    public int Id { get; set; }

    public string ServiceDescription { get; set; }
    public double ServicePrice { get; set; }

    public int Quantity { get; set; }

    public double LineTotal { get; set; }
  }
}