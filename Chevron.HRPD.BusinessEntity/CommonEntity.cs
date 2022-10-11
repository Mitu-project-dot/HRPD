

/// <summary>     
///  Added by       : Soumitra Bain
///  Date           : 04-July-2015
///  Purpose        : This class is going to hold common value for each and every class like Created_By, Created_Time and so on..
///  
///  Updated By     : 
///  Date           : 
///  Purpose        :
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.HRPD.BusinessEntities
{
    [Serializable]
    public abstract class CommonEntity : BusinessEntity
    {
        //Added on 27-07-2015

        public virtual string Created_By { get; set; }
        public virtual DateTime? Created_Time { get; set; }
        public virtual string Updated_By { get; set; }
        public virtual DateTime? Updated_Time { get; set; }
    }
}
