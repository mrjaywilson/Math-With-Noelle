using UnityEngine;
using Zenject;

public class BasicInstaller : MonoInstaller<BasicInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayer>().FromInstance(new Player()).AsTransient().NonLazy();
    }
}