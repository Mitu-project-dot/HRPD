using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chevron.HRPD.Common.Exceptions
{
    //TODO: Shouldn't this class name end with "Exception"?
	public class MissingUnityContainerReference : ApplicationException
	{
		public MissingUnityContainerReference() : base() { }

		public MissingUnityContainerReference(string message) : base(message){}
	}
}
