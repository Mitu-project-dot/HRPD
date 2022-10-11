using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chevron.HRPD.BusinessEntities;

namespace Chevron.HRPD.Common.Interfaces
{
    /// <summary>
    /// Describes basic functions performed by the data acess layer
    /// </summary>
    public interface IPersistence<T> where T : BusinessEntity 
    {
        void Save(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int ID);

        T FindByID(int ID);

        List<T> Find();

        //TODO: FindByCriteria?
    }
}
