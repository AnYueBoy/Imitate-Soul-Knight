using UFramework.Core;
public class ProviderTransformManager : IServiceProvider {
    public void Init () {
    }

    public void Register () {
        App.Singleton<ITransfromManager, TransformManager> ();
    }
}