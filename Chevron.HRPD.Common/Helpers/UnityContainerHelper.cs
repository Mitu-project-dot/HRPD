using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Practices.Unity;
using Chevron.HRPD.Common.Interfaces;
using Chevron.HRPD.Common.Exceptions;

namespace Chevron.HRPD.Common.Helpers
{
	public static class UnityContainerHelper
	{
		#region Private Fields
        
		private static IUnityContainer _container; 
        private static readonly object _lockObject = new object();
		private const string UnityContextKey = "HTTPContext_UnityContainer";

		#endregion

		#region Public Properties

		/// <summary>
		/// The Unity container for the current application
		/// </summary>
		public static IUnityContainer Container
		{
			get
			{
				if (_container == null)
				{
					_container = GetContainer();
				}

				return _container;
			}
		}
		
		#endregion

        #region Methods

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        #endregion

        #region private members

        /// <summary>
		/// Look for unity container in each different context
		/// If exist, just returning it, if not creating a new one.
		/// </summary>
		/// <returns>Instance of a configured unity container.</returns>
		private static IUnityContainer GetContainer()
		{
			//Looking for unity container in HttpContext
			if (HttpContext.Current != null)
			{
				//if doesn't exist
				if (HttpContext.Current.Application[UnityContextKey] != null)
				{
					return HttpContext.Current.Application[UnityContextKey] as IUnityContainer;
				}
				else 
				{
					return CreateContainer();
						
				}
			}
			else if (AppDomain.CurrentDomain != null) //Looking for unity container in App Context
			{
				return CreateContainer();
			}
			else
			{
				throw new NullReferenceException("Application Context not found");
			}
		}

		/// <summary>
		/// Create new configured unity container
		/// </summary>
		/// <returns>Instance of a new configured unity container.</returns>
		private static IUnityContainer CreateContainer()
		{
			//create a new container
			if (_container == null)
			{
				lock (_lockObject)
				{
					_container = new UnityContainer();
					//Configure container
					IUnityConfigurator config = new UnityConfiguratorHelper();
					config.Configure(_container); 
				}
			}

			return _container;
		}

		#endregion

	}
}

