using Services.Input;
using StateMachine;
using UnityEngine;
using Zenject;

namespace Logic.Levels
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameStateMachine _gameStateMachine;
        
        public override void InstallBindings()
        {
            BindStateMachine();
            BindInputService();
        }

        private void BindStateMachine() =>
            Container.BindInstance(_gameStateMachine).AsSingle();

        private void BindInputService()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WebGLPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                IInputService inputService = new StandaloneInputService();
                Container.BindInstance(inputService).AsSingle();
            }
            else
            {
                IInputService inputService = new MobileInputService();
                Container.BindInstance(inputService).AsSingle();
            }
        }
    }
}