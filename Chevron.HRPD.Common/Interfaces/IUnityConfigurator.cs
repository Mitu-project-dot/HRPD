using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace Chevron.HRPD.Common.Interfaces
{
	/// <summary>
	/// IUnityConfigurator
	/// </summary>
	public interface IUnityConfigurator
	{
		/// <summary>
		/// Configures a unity container
		/// </summary>
		void Configure(IUnityContainer container);
	}
}
