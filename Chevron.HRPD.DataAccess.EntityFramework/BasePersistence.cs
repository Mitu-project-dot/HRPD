using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.HRPD.Common.Interfaces;
using Chevron.HRPD.BusinessEntities;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Chevron.HRPD.DataAccess
{
    public abstract class BasePersistence<T> : IPersistence<T> where T : BusinessEntity, new()
    {
        #region IPersistence<T> Members

        /// <summary>
        /// Saves specified entity to DB
        /// </summary>
        /// <param name="entity">Entity to be saved</param>
        public virtual void Save(T entity)
        {
            using (BaseContext context = new BaseContext())
            {
                context.Set<T>().Add(entity);
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the specified entity. This marks ALL properties as modified, which may not be the intended use. 
        /// If that's the case, override this method and implement custom functionality. Please note that this method will only update the scalar properties of the entity, for related entities this method needs to be overwritten.
        /// </summary>
        /// <param name="entity">Entity being updated</param>
        public virtual void Update(T entity)
        {
            using (BaseContext context = new BaseContext())
            {

                context.Entry(entity).State = System.Data.EntityState.Modified;

                context.ChangeTracker.DetectChanges();

                context.SaveChanges();


            }
        }

        /// <summary>
        /// Deletes the specified T entity
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        public virtual void Delete(T entity)
        {
            using (BaseContext context = new BaseContext())
            {
                context.Set<T>().Attach(entity);

                context.Set<T>().Remove(entity);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes an entity of type T with the specified identifier
        /// </summary>
        /// <param name="ID">Entity identifier</param>
        public virtual void Delete(int ID)
        {
            using (BaseContext context = new BaseContext())
            {
                //Get the object from the DB
                var entity = (from c in context.Set<T>() where c.ID == ID select c).FirstOrDefault();

                if (entity != null)
                {
                    context.Set<T>().Remove(entity);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Return an entity of type T with the specified identifier. This method does not load any related types
        /// </summary>
        /// <param name="ID">Entity identifier</param>
        public virtual T FindByID(int ID)
        {
            using (BaseContext context = new BaseContext())
            {
                return (from c in context.Set<T>() where c.ID == ID select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Returns a list of all the present entities of type T in the database. This method does not load any related entities
        /// </summary>
        public virtual List<T> Find()
        {
            using (BaseContext context = new BaseContext())
            {
                return context.Set<T>().ToList();
            }
        }

        protected bool IsEntityNew(BusinessEntity entity)
        {
            return !(entity != null && entity.ID.HasValue);
        }

        #endregion
    }
}
