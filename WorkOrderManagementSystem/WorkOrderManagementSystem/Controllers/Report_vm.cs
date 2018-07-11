using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers
{
    public class ReportServiceItemPopularity
    {
        public String ServiceItemDesc { get; set; }
        public double ServiceItemPrice { get; set; }
        public int ServiceItemOccurences { get; set; }
        public double RevenueGenerated { get; set; }
    }

    public class ReportManufacturerPopularity
    {
        public String ManufacturerName { get; set; }
        public int ManufacturerOccurences { get; set; }
        public double AvgRevenueGenerated { get; set; }
    }

  public class ReportMechanicOrderTotals
  {
    public MechanicBase mechanic { get; set; }
    public int inProgress { get; set; }
    public int completed { get; set; }
  }

  public class InvoiceReport
  {
    public double avgRevenueThisYear { get; set; }
    public int numThisYear { get; set; }
    public double avgRevenueThisWeek { get; set; }
    public int numThisWeek { get; set; }
    public double avgRevenueThisMonth { get; set; }
    public int numThisMonth { get; set; }
    public int numOutstandingInvoices { get; set; }
    public double moneyOutstanding { get; set; }
  }
}