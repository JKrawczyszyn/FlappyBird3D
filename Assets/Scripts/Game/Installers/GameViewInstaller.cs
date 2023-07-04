using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller<GameViewInstaller>
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private WallsView wallsView;

    public override void InstallBindings()
    {
        Container.BindInstance(camera);
        Container.BindInstance(wallsView);

        Container.Bind<WallsAssetManager>().AsSingle();
    }
}
