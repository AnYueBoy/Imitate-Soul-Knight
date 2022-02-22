using UFramework.Core;
public class ProviderEnemyManager : IServiceProvider {
    public void Init () {
    }

    public void Register () {
        App.Singleton<IEnemyManager, EnemyManager> ();
    }
}