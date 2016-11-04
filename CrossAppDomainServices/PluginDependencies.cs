using System;

namespace CrossAppDomainServices
{
	public class PluginDependencies : MarshalByRefObject
	{
		public IService Service { get; set; }

		public void Init(ApplicationDependencies applicationDependencies)
		{
			Service = applicationDependencies.Service;
		}
	}
}