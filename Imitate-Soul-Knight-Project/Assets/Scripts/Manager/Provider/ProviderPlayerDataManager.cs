using UFramework.Core;
public class ProviderPlayerDataManager : IServiceProvider {
    public void Init () {
    }

    public void Register () {
        App.Singleton<IPlayerDataManager, PlayerDataManager> ();
    }
}