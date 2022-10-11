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
    public class UserLogInLog : BusinessEntity
    {
       
        public string CAI { get; set; }


       
        public DateTime LoginDate { get; set; }

        //public bool SessionActive { get; set; }

        public string IPAddress { get; set; }


        public string SessionID { get; set; }


        public DateTime LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }
        
        

        //public int loginNumber { get; set; }

        public DateTime SessionLastHit { get; set; }

        public DateTime SessionExpires { get; set; }
    }
}
