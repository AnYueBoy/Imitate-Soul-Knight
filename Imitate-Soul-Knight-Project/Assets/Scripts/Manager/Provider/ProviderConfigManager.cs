using UFramework.Core;
public class ProviderConfigManager : IServiceProvider {
    public void Init () {
        App.Make<IConfigManager> ().Init ();
    }

    public void Register () {
        App.Singleton<IConfigManager, ConfigManager> ();
    }
}