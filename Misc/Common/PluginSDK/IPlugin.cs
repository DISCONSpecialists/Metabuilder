namespace MetaBuilder.PluginSDK
{
	/// <summary>
	/// A public interface to be used by all custom plugins
	/// </summary>
	public interface IPlugin
	{
		string Name{get;}
		//void PerformAction(IPluginContext context);
        bool PerformAction(IPluginContext context);
	}
}
