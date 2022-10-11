


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chevron.HRPD.BusinessEntities
{
    [Serializable]
    public class User_Role : BusinessEntity
    {
        
        public string CAI { get; set; }
        public string Role { get; set; }
        public string Role_Description { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string Created_By { get; set; }
        public DateTime? Created_Time { get; set; }
        public string Updated_By { get; set; }
        public DateTime? Updated_Time { get; set; }        
    }
}
