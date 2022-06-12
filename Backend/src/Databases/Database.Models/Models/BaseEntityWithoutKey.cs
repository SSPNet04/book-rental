using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    public class BaseEntityWithoutKey : ISoftDeleteEntity
    {
        public int Order { get; set; }

        public virtual DateTime? Created { get; set; }

        public virtual DateTime? Updated { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }
    }
}
