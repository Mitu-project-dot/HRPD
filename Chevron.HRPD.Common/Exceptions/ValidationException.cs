using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Chevron.HRPD.Common.Exceptions
{
    /// <summary>
    /// Exception class used when a validation fails on an entity
    /// </summary>
    public class ValidationException : ApplicationException
    {
        #region Properties

        private ValidationResults Results { get; set; }

        /// <summary>
        /// Entity that contains the validation errors
        /// </summary>
        public BusinessEntities.BusinessEntity Entity { get; set; }

        /// <summary>
        /// If the results collection is empty or null, returns true. Other wise it's false
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (Results == null)
                {
                    return true;
                }

                return Results.IsValid;
            }
        }

        /// <summary>
        /// List of validation results
        /// </summary>
        public List<ValidationResult> ValidationResults
        {
            get 
            {
                if (Results == null)
                {
                    return new List<ValidationResult>();
                }

                return Results.ToList();
            }
        }

        /// <summary>
        /// Concatenates the error messages contained in the ValidationResults collection
        /// </summary>
        public string ErrorMessages
        {
            get 
            {
                if (Results == null)
                {
                    return string.Empty;
                }

                StringBuilder builder = new StringBuilder();

                foreach (ValidationResult result in Results)
                {
                    builder.AppendLine(result.Message + "<br />");
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Hides Message property to force consumers to use the ErrorMessages property
        /// </summary>
        private new string Message
        {
            get
            {
                return ErrorMessages;
            }
        }

        #endregion

        #region Constructor

        public ValidationException(ValidationResults results)
        {
            Results = results;
        }

        public ValidationException(ValidationResults results, BusinessEntities.BusinessEntity entity)
        {
            Results = results;

            Entity = entity;
        }

        #endregion
    }
}
