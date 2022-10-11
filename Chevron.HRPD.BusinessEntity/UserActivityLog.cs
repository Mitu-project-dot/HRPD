


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
    public class UserActivityLog : BusinessEntity
    {
       
        public string CAI { get; set; }

        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string SessionID { get; set; }

        public DateTime ActivityTime { get; set; }

        public string Remarks { get; set; }
    }
}
