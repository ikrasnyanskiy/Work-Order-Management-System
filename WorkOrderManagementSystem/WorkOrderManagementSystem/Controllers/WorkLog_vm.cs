using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkOrderManagementSystem.Controllers
{
  public class WorkLogBase//can this really be a base object if it has a composite key??
  {
    public int MechanicId { get; set; }//should be configured w/ fluent api so no data annotations?
    // [Key, ForeignKey("WorkOrder")]
    public int WorkOrderId { get; set; }

    public DateTime TimeStarted { get; set; }
    public DateTime TimeStopped { get; set; }
    public bool IsActiveOnOrder { get; set; }
  }

  public class WorkLogWithMechanic : WorkLogBase//this could be completely wrong
  {
    public MechanicBase Mechanic { get; set; }
  }
  public class WorkLogWithWorkOrder : WorkLogBase//this could be completely wrong
  {
    public WorkOrderBase WorkOrder { get; set; }//should this include other data?
  }
}