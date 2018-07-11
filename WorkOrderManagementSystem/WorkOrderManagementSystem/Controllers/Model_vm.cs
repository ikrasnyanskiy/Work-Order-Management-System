using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WorkOrderManagementSystem.Controllers
{
    public class ModelAdd
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public ManufacturerBase Manufacturer { get; set; }

        [StringLength(100)]
        public string Info { get; set; }
    }

    public class ModelBase : ModelAdd
    {
        public int Id { get; set; }
        
    }

    public class ModelWithAssoc
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public ManufacturerBase Manufacturer { get; set; }

        [StringLength(100)]
        public string Info { get; set; }

        public int Id { get; set; }

    }

    public class ModelEdit
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Info { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }

    public class ModelEditForm
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Info { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }
    public class ModelWithPhotoInfo : ModelBase
    {
        public string PhotoContentType { get; set; }
    }

    public class ModelWithPhoto : ModelWithPhotoInfo
    {
        public byte[] Photo { get; set; }
    }
}