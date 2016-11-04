using System;

namespace CrossAppDomainServices
{
	public class Plugin : MarshalByRefObject
	{
		private IService _service;

		public void Init(PluginDependencies pluginDependencies)
		{
			_service = pluginDependencies.Service;
		}
	}
}