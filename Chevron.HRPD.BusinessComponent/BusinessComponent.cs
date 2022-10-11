using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.Common.Helpers;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Chevron.HRPD.Common.Exceptions;

namespace Chevron.HRPD.BusinessComponent
{
    public abstract class BusinessComponent<T> where T : BusinessEntity
    {
        #region Private Members
    
        Validator<T> entityValidator;
        
        ValidatorFactory valFactory;
        
        #endregion

        #region Methods

        /// <summary>
        /// Finds an entity by its database ID. Returns null if the object is not found
        /// </summary>
        /// <param name="id">Entity's ID in the database</param>
        /// <returns></returns>
        public virtual T FindByID(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("ID parameter is invalid");
            }

            return Persistence.FindByID(id);
        }

        /// <summary>
        /// Inserts or updates an entity in the DB. If the entity has a value for its ID property, it assumes it exists and tries to update.
        /// Otherwise, a new record is created.
        /// </summary>
        /// <param name="entity">Entity to be inserted/updated</param>
        public void Persist(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity parameter cannot be null");
            }

            ////Validate the entity
            //ValidationResults validationResults = Validate(entity);

            //if (validationResults.IsValid)
            //{
                //Create a new transaction
                using (TransactionWrapper transaction = new TransactionWrapper())
                {
                    //If the entity has an ID, we assume it exists in the DB
                    if (entity.ID.HasValue)
                    {
                        DoUpdate(entity, null);
                    }
                    else
                    {
                        DoInsert(entity, null);
                    }

                    //Commit the transaction
                    transaction.Complete();
                }
            //}
            //else 
            //{
            //    throw new ValidationException(validationResults);
            //}
        }

        /// <summary>
        /// Protected virtual method that calls the Persistence layer to insert the entity
        /// </summary>
        protected virtual void DoInsert(T entity, TransactionWrapper transaction)
        {
            Persistence.Save(entity);
        }
        

        /// <summary>
        /// Protected virtual method that calls the Persistence layer to update an existing entity
        /// </summary>
        protected virtual void DoUpdate(T entity, TransactionWrapper transaction)
        {
            Persistence.Update(entity);
        }

        /// <summary>
        /// Protected virtual method that will be implemented in child classes for specific validation rules
        /// </summary>
        public virtual ValidationResults DoValidate(T entity)
        {
            return new ValidationResults();
        }

        /// <summary>
        /// Validates an entity to make sure its propertie's value are valid within the business domain
        /// </summary>
        /// <param name="entity">Entity being validated</param>
        public virtual ValidationResults Validate(T entity)
        {
            ValidationResults valResults = new ValidationResults();

            if (entity == null)
            {
                throw new ArgumentNullException("Entity parameter cannot be null");
            }

            if (EntityValidator != null)
            {
                valResults = EntityValidator.Validate(entity);
            }

            valResults.AddAllResults(DoValidate(entity)); //Add any custom validations that come from the inheriting class
     
            return valResults;
        }

        /// <summary>
        /// Returns a list of all the objects of a particular type stored in the DB
        /// </summary>
        public virtual List<T> Find()
        {
            return Persistence.Find();
        }

        /// <summary>
        /// Deletes an entity from the DB.
        /// </summary>
        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity parameter cannot be null");
            }

            Persistence.Delete(entity);
        }

        /// <summary>
        /// Deletes an entity from the DB searching for it by its identifier
        /// </summary>
        public virtual void Delete(int ID)
        {
            if (ID < 0)
            {
                throw new ArgumentOutOfRangeException("ID parameter is invalid");
            }

            Persistence.Delete(ID);
        }

        /// <summary>
        /// Retrieves a validator for the specified type. If the specified type is T, the EntityValidator property should be used
        /// </summary>
        protected Validator GetValidator(Type type)
        {
            return ValidatorFactory.CreateValidator(type);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Resolves an instance of a validator object capable of validating an object of type T
        /// </summary>
        private ValidatorFactory ValidatorFactory
        {
            get 
            {
                if (valFactory == null)
                {
                    //valFactory = EnterpriseLibraryContainer.Current.GetInstance<ValidatorFactory>();
                }

                return valFactory;
            }
        }

        /// <summary>
        /// Protected property. Returns an entity validator object for the specified T
        /// </summary>
        protected Validator<T> EntityValidator
        {
            get
            {
                return ValidatorFactory.CreateValidator<T>();

            }
        }

        /// <summary>
        /// Persistence object retrieved 
        /// </summary>
        protected virtual HRPD.Common.Interfaces.IPersistence<T> Persistence
        {
            get
            {
                return UnityContainerHelper.Resolve<HRPD.Common.Interfaces.IPersistence<T>>();
            }
        }

        #endregion
    }
}
