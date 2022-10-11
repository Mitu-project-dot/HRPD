using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chevron.HRPD.BusinessEntities;

namespace Chevron.HRPD.Common.Interfaces
{
    /// Describes User functions performed by the data acess layer
    /// 
    public interface IEmployee_PD_InfoPersistence : IPersistence<Employee_PD_Info>
    {
        //UserLogInLog FindByCAI(string CAI);
    }
}
