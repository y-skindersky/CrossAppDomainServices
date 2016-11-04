using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CrossAppDomainServices
{
	class Program
	{
		private const int MaxPluginCount = 10;

		private static ApplicationDependencies _applicationDependencies;

		static void Main(string[] args)
		{
			_applicationDependencies = new ApplicationDependencies();
			_applicationDependencies.Service = new CrossAppDomainService();

			InitializePlugins();
		}

		private static void InitializePlugins()
		{
			for (int i = 0; i < MaxPluginCount; i++)
			{
				var assembly = Assembly.GetCallingAssembly();

				var pluginName = "Plugin" + i;

				Console.WriteLine($"Start '{pluginName}'...");

				Task.Factory.StartNew(() => InitializePlugin(assembly, pluginName));
			}

			Console.ReadKey();
		}

		private static void InitializePlugin(Assembly assembly, string pluginName)
		{
			try
			{
				Console.WriteLine($"Creating AppDomain for '{pluginName}'...");
				var appDomain = AppDomain.CreateDomain(pluginName);

				Console.WriteLine($"Creating '{pluginName}'...");
				var plugin = (Plugin)appDomain.CreateInstanceAndUnwrap(assembly.GetName().Name, typeof(Plugin).FullName);

				Console.WriteLine($"Creating '{pluginName}' dependencies...");
				var pluginDependencies = (PluginDependencies)appDomain.CreateInstanceAndUnwrap(assembly.GetName().Name, typeof(PluginDependencies).FullName);

				Console.WriteLine($"Initializing dependecnies for '{pluginName}'...");
				pluginDependencies.Init(_applicationDependencies);

				Console.WriteLine($"Initializing '{pluginName}'...");
				plugin.Init(pluginDependencies);

				Console.WriteLine($"'{pluginName}' initialized.");
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine($"{pluginName} error: {ex.Message}\n{ex.StackTrace}");
			}
		}
	}
}
