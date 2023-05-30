using Player;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{ 
    [SerializeField] private PlayerMovement player;

    public override void InstallBindings()
    {
        Container.Bind<PlayerMovement>().FromInstance(player).AsSingle().NonLazy();
        Container.QueueForInject(player);
    }
}
