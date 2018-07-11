using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkOrderManagementSystem.Controllers
{
  public class LogEntryBase
  {
    public LogEntryBase()
    {
      mechStarted = DateTime.Now;
      mechStopped = DateTime.Now;
    }
    //PK unique identifier
    public int Id { get; set; }
    //using specifically structured LINQ queries instead (see comments on top of this class declaration)
    //public int LookupId { get; set; }
    public DateTime mechStarted { get; set; }
    public DateTime mechStopped { get; set; }
    //flag determines if mechanic still working (avoid confusion since mechStarted==mechStopped for new LogEntries)
    public bool inProgress { get; set; }
  }
}