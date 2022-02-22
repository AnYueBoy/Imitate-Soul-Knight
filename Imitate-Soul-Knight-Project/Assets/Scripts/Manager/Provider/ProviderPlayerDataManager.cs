using UFramework.Core;
public class ProviderPlayerDataManager : IServiceProvider {
    public void Init () {
        App.Make<IPlayerDataManager> ().Init ();
    }

    public void Register () {
        App.Singleton<IPlayerDataManager, PlayerDataManager> ();
    }
}