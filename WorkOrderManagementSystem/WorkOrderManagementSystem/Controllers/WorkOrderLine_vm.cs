using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WorkOrderManagementSystem.Controllers
{
    public class WorkOrderLineAdd
    {
        public ServiceItemBase ServiceItem { get; set; }
        
        public int Quantity { get; set; }
        
        public double LineTotal { get; set; }
    }
  public class WorkOrderLineDetails
  {
    public ServiceItemBase ServiceItem { get; set; }

    public int Quantity { get; set; }

    public double LineTotal { get; set; }
  }

  public class WorkOrderLineAddForm
  {
    //public SelectList ServiceItemList { get; set; }
    public ICollection<int> ServiceItemIds;
    public int Quantity { get; set; }

    public double LineTotal { get; set; }

  }

  public class WorkOrderLineEdit
  {
    public int ServiceItemId { get; set; }
    public int Quantity { get; set; }

    public double LineTotal { get; set; }

  }

  public class WorkOrderLineBase : WorkOrderLineAdd
    {
        public int Id { get; set; }
    }
}