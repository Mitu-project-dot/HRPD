

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chevron.HRPD.BusinessEntities;

namespace Chevron.HRPD.Common.Interfaces
{
    /// Describes User functions performed by the data acess layer
    /// 
    public interface IUser_RolePersistence : IPersistence<User_Role>
    {
        //UserLogInLog FindByCAI(string CAI);
    }
}
