using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Chevron.HRPD.Common.Interfaces;

namespace Chevron.HRPD.Common.Helpers
{
/// <summary>
    /// Config Unity Configurator
    /// </summary>
	public class UnityConfiguratorHelper : IUnityConfigurator
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="ConfigUnityConfigurator"/>.
		/// </summary>
		public UnityConfiguratorHelper()
		{
		}

		#endregion

		#region IUnityConfigurator Members

		/// <summary>
		/// Configures a unity container
		/// </summary>
		public void Configure(IUnityContainer container)
		{
			object section = ConfigurationManager.GetSection("unity");

			UnityConfigurationSection localSection = section as UnityConfigurationSection;

			if (localSection != null)
			{
				localSection.Configure(container);
			}
		}

		#endregion
	}
}
