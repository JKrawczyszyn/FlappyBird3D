using Game.Controllers;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    [SerializeField]
    private GameConfig gameConfig;

    public override void InstallBindings()
    {
        Container.BindInstance(gameConfig);
        Container.BindInstance(new GameControls());

        Container.BindInterfacesAndSelfTo<BirdController>().AsSingle();

        Application.targetFrameRate = gameConfig.frameRate;
        Physics.gravity = Vector3.down * gameConfig.gravity;
    }
}
