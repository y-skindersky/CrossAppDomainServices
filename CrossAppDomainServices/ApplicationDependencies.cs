using System;

namespace CrossAppDomainServices
{
	public class ApplicationDependencies : MarshalByRefObject
	{
		public IService Service { get; set; }
	}
}