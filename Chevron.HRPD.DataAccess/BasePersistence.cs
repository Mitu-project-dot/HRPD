using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using Chevron.HRPD.Common.Interfaces;
using Chevron.HRPD.BusinessEntities;

namespace Chevron.HRPD.DataAccess.EntLib
{
    public abstract class BasePersistence <T> : IPersistence<T> where T : BusinessEntity,  new ()
    {
        #region Private Members

        private Database _db;

        #endregion

        #region IPersistence<T> Members

        /// <summary>
        /// Inserts a new record in the DB
        /// </summary>
        public virtual void Save(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity paramenter cannot be null");
            }

            DbCommand tempCommand = InsertCommand;

            MapEntity(entity, DB, tempCommand);

            entity.ID = Convert.ToInt32(DB.ExecuteScalar(tempCommand).ToString());
        }

        /// <summary>
        /// Updates an existing record in the DB
        /// </summary>
        public virtual void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity paramenter cannot be null");
            }

            DbCommand tempCommand = UpdateCommand;

            MapEntity(entity, DB, tempCommand);

            DB.AddInParameter(tempCommand, "ID", DbType.Int32, entity.ID);

            DB.ExecuteNonQuery(tempCommand);
        }

        /// <summary>
        /// Deletes the entity from the DB. Assumes the DeleteCommand has a parameter called "ID"
        /// </summary>
        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity paramenter cannot be null");
            }

            DbCommand tempCommand = DeleteCommand;

            DB.AddInParameter(tempCommand, "ID", DbType.Int32, entity.ID);

            DB.ExecuteNonQuery(tempCommand);
        }

        /// <summary>
        /// Deletes the entity from the DB. Assumes the DeleteCommand has a parameter called "ID"
        /// </summary>
        public virtual void Delete(int ID)
        {
            DbCommand tempCommand = DeleteCommand;

            DB.AddInParameter(tempCommand, "ID", DbType.Int32, ID);

            DB.ExecuteNonQuery(tempCommand);
        }

        /// <summary>
        /// Finds an entity by it's ID in the database. Assumes to the FindByIdCommand has a parameter called "ID"
        /// </summary>
        public virtual T FindByID(int ID)
        {
            DbCommand tempCommand = FindByIdCommand;

            DB.AddInParameter(tempCommand, "ID", DbType.Int32, ID);

            using (IDataReader dr = DB.ExecuteReader(tempCommand))
            {
                if (dr.Read())
                {
                    return MapFields(dr);
                }

                return null;
            }
        }

        /// <summary>
        /// Returns all the object of type T present in the database
        /// </summary>
        public virtual List<T> Find()
        {
            List<T> newList = new List<T>();

            using (IDataReader dr = DB.ExecuteReader(FindAllCommand))
            {
                while(dr.Read())
                {
                    newList.Add(MapFields(dr));
                }
            }

            return newList;

        }

        #endregion
        
        #region Abstract Properties

        /// <summary>
        /// Returns a DB Command with the Query / Stored procedure name that inserts a new T record in the Database.
        /// </summary>
        protected abstract DbCommand InsertCommand { get; }
        
        /// <summary>
        /// Returns a DB Command with the Query / Stored procedure name that updates an existing T record in the Database. 
        /// Assumes there's a parameter called "ID"
        /// </summary>
        protected abstract DbCommand UpdateCommand { get; }

        /// <summary>
        /// Returns a DB Command with the Query / Stored procedure name that deletes an existing T record from the Database. 
        /// Assumes there's a parameter called "ID"
        /// </summary>
        protected abstract DbCommand DeleteCommand { get; }

        /// <summary>
        /// Returns a DB Command with the Query / Stored procedure name that searches for an existing T record with the specified ID. 
        /// Assumes there's a parameter called "ID"
        /// </summary>
        protected abstract DbCommand FindByIdCommand { get; }
        
        /// <summary>
        /// Returns a DB Command with the Query / Stored procedure name that returns all existing records in the DB of type T
        /// </summary>
        protected abstract DbCommand FindAllCommand { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Enterprise library database object to execute statements against the DB. This property returns the DEFAULT database. Please refer to the GetDatabase Method if a DB other than the default one needs to be used
        /// </summary>
        protected virtual Database DB 
        { 
            get 
            {
                if (_db == null)
                {
                    _db = DatabaseFactory.CreateDatabase();
                }

                return _db;
            } 
        }
        
        #endregion

        #region Abstract Methods

        /// <summary>
        /// Maps a datareader row to a T object
        /// </summary>
        protected abstract T MapFields(IDataReader dr);

        /// <summary>
        /// Maps an entity to a database command, usually to be inserted / updated
        /// </summary>
        protected abstract void MapEntity(T entity,Database db, DbCommand DbCom); 
        
        #endregion

        #region Methods

        /// <summary>
        /// Returns a database object using a specific connection name
        /// </summary>
        protected Database GetDatabase(string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
            {
                throw new ArgumentException("A database name needs to be specified");
            }

            return DatabaseFactory.CreateDatabase(dbName);
        }

        #endregion
    }
}
