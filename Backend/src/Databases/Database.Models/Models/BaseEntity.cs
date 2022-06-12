using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Database.Models
{
    public class BaseEntity : ISoftDeleteEntity
    {
        public BaseEntity()
        {
            if (string.IsNullOrEmpty(this.ID))
            {
                this.ID = Guid.NewGuid().ToString();
            }
        }

        [Key]
        [MaxLength(36)]
        public string ID { get; set; }

        public int Order { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Updated { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
