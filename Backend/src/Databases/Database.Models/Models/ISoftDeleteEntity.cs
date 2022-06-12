using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public interface ISoftDeleteEntity
    {
        bool IsDeleted { get; set; }
    }
}
