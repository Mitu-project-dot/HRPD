using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chevron.HRPD.BusinessEntities
{
    /// <summary>
    /// Base persistable entity
    /// </summary>
    /// 
    [Serializable]
    public abstract class BusinessEntity
    {
        #region Properties

        public virtual int? ID { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (obj != null && obj is BusinessEntity)
            {
                BusinessEntity objectToCompare = (BusinessEntity)obj;

                if (objectToCompare.ID.HasValue && ID.HasValue)
                {
                    return objectToCompare.ID.Equals(ID);
                }

                return false;
            }

            return false;
        }

        #endregion


    }
}
