using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    private Controls _controls;

    [SerializeField]
    private SceneController _controller;
    public override void InstallBindings()
    {
        _controls = new Controls();
        _controls.Game.Enable();
    }
}