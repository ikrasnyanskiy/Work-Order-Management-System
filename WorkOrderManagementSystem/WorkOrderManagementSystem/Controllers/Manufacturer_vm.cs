using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers
{
    public class ManufacturerAdd
    {

        public ICollection<ModelBase> Models { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class ManufacturerBase : ManufacturerAdd
    {
        public int Id { get; set; }

    }

    public class ManufacturerEditForm
    {
        public int Id { get; set; }
        
        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class ManufacturerEdit
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

    }
}