

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chevron.HRPD.BusinessEntities;

namespace Chevron.HRPD.Common.Interfaces
{
    /// Describes User functions performed by the data acess layer
    /// 
    public interface IUserActivityLogPersistence : IPersistence<UserActivityLog>
    {
        //UserLogInLog FindByCAI(string CAI);
    }
}
