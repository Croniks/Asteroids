using UnityEngine.InputSystem;

using GameController;
using GameModel;


namespace AsteroidsGame
{
    public class InputProvider : IInputReader
    {
        private StandartInput _input;
        private ProjectileType _currentProjectileType;

        public InputProvider()
        {
            _input = new StandartInput();
        }
        
        public void Enable()
        {
            _currentProjectileType = ProjectileType.None;

            _input.Enable();

            _input.KeyboardAndMouse.ShootBullets.started += OnShootBullets;
            _input.KeyboardAndMouse.ShootLasser.started += OnShootLasser;
        }

        public void Disable()
        {
            _input.Disable();

            _input.KeyboardAndMouse.ShootBullets.started -= OnShootBullets;
            _input.KeyboardAndMouse.ShootLasser.started -= OnShootLasser;
        }

        public float GetMoveInput()
        {
            return _input.KeyboardAndMouse.MoveForward.ReadValue<float>();
        }

        public float GetRotationInput()
        {
            return _input.KeyboardAndMouse.Rotation.ReadValue<float>();
        }

        public ProjectileType GetShootInput()
        {
            var projectileType = _currentProjectileType;
            _currentProjectileType = ProjectileType.None;
            return projectileType;
        }

        private void OnShootBullets(InputAction.CallbackContext context)
        {
            _currentProjectileType = ProjectileType.Bullet;
        }

        private void OnShootLasser(InputAction.CallbackContext context)
        {
            _currentProjectileType = ProjectileType.Lasser;
        }
    }
}