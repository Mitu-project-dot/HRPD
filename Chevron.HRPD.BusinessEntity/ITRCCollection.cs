using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Chevron.HRPD.BusinessEntities
{
    /// <summary>
    /// Custom collection that provides lazy loading functionality and the ability to keep track of objects removed from the collection.
    /// </summary>
    [Serializable]
    public class ITRCCollection<T> : ICollection<T>,ICollection where T : BusinessEntity
    {
        #region Private Members

        private readonly List<T> currentObjects;
        
        private readonly List<T> deletedObjects;

        private bool isLazy = false;
        
        public delegate ICollection<T> FindByParent(int parentID);

        [NonSerialized]
        private FindByParent findByParentDelegate;

        private int parentID;

        private bool collectionInitialized = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor. Does not provide lazy loading functionality
        /// </summary>
        public ITRCCollection()
        {
            currentObjects = new List<T>();

            deletedObjects = new List<T>();
        }

        /// <summary>
        /// Constructor that takes a delegate to load the collection, and the ID of the object that "owns" this collection
        /// </summary>
        public ITRCCollection(FindByParent lazyMethod, int parentID) : this()
        {
            findByParentDelegate = lazyMethod;

            isLazy = true;

            this.parentID = parentID;
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the current list of objects. Does not check for duplicates
        /// </summary>
        public void Add(T item)
        {
            GetEnumerator();

            currentObjects.Add(item);
        }

        /// <summary>
        /// Searches for an item in the current list of objects that matches the specified predicate
        /// </summary>
        public T Find(Predicate<T> predicate)
        {
            GetEnumerator();

            return currentObjects.Find(predicate);
        }

        /// <summary>
        /// Removes all items from the current list of objects and moves them to the "deleted" list
        /// </summary>
        public void Clear()
        {
            GetEnumerator();

            foreach (var item in currentObjects)
            {
                if (!deletedObjects.Contains(item))
                {
                    deletedObjects.Add(item);
                }
            }

            currentObjects.Clear();
        }

        /// <summary>
        /// Checks wether the current list of objects contains the specified item
        /// </summary>
        public bool Contains(T item)
        {
            GetEnumerator();

            return currentObjects.Contains(item);
        }

        /// <summary>
        /// Copies the specified list to the current list of objects
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            GetEnumerator();

            currentObjects.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the amount of objects present in the current list
        /// </summary>
        public int Count
        {
            get
            {
                GetEnumerator();

                return currentObjects.Count;
            }
        }

        /// <summary>
        /// Returns wether the current list of objects is read only
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an item from the current list and moves it to the "Deleted" list
        /// </summary>
        public bool Remove(T item)
        {
            GetEnumerator();

            if (currentObjects.Remove(item))
            {
                if (!deletedObjects.Contains(item))
                {
                    deletedObjects.Add(item);
                }

                return true;
            }
            
            return false;
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator of type T to iterate over the current list of objects
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (!isLazy || collectionInitialized)
            {
                return currentObjects.GetEnumerator();
            }
            else
            {
                if (findByParentDelegate != null)
                {
                    currentObjects.AddRange(findByParentDelegate(parentID));

                    collectionInitialized = true;

                    return currentObjects.GetEnumerator();
                }

                throw new Exception("Delegate to load collection was not specified"); 
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Non generic implementation of GetEnumerator
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the list of object that were removed from the current list
        /// </summary>
        public ICollection<T> DeletedObjects
        {
            get
            {
                GetEnumerator();

                return deletedObjects;
            }
        }

        /// <summary>
        /// Indexer to access a specific item in the current list by its position in the list
        /// </summary>
        public T this[int index]
        {
            get
            {
                return currentObjects[index];
            }
            set 
            {
                currentObjects[index] = value;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            currentObjects.CopyTo(array.OfType<T>().ToArray(), index);
        }

        public bool IsSynchronized
        {
            get 
            {
                return true;
            }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
