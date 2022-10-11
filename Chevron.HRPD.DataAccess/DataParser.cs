using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chevron.HRPD.DataAccess.EntLib
{
    internal static class DataParser
    {
        /// <summary>
        /// Converts an object to the type specified in the generic parameter
        /// </summary>
        internal static T GetValue<T>(this object value)
        {
            Type type = typeof(T);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>).GetGenericTypeDefinition())
            {
                return GetValue<T>(value, true); //If this overload is called and a business rule says this field is not nullable when the underlying data type is, the other overload should be used
            }

            return GetValue<T>(value, false);
        }

        /// <summary>
        /// Converts an object to the type specified in the generic parameter
        /// </summary>
        internal static T GetValue<T>(this object value, bool isNullable)
        {
            if (!isNullable)
            {
                if (value == null || value == DBNull.Value)
                {
                    throw new ArgumentNullException("Cannot set null to non-nullable field");
                }
            }

            if (value == DBNull.Value)
            {
                return default(T);
            }

            //return (T)Convert.ChangeType(value, typeof(T));

            return (T)value;
        }

        internal static int? SafeGetRelatedEntity<T>(this T entity) where T : BusinessEntities.BusinessEntity
        {
            return entity != null ? entity.ID : null;
        }
    }
}
