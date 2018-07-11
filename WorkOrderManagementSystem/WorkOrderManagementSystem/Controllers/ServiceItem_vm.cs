using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers
{
    public class ServiceItemAdd
    {
        [Required, StringLength(100)]
        public string Description { get; set; }
        
        public double Price { get; set; }
    }

    public class ServiceItemBase : ServiceItemAdd
    {
        public int Id { get; set; }
    }
}