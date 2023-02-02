using System;
using System.Numerics;

namespace GameModel.PlayerShipModel
{
    public class PlayerShip
    {
        public Vector2 Position => _shipMovementAndRotation.Position;

        private PlayerShipMovementAndRotation _shipMovementAndRotation;
        private PlayerShipShooting _shipShooting;

        
        public PlayerShip(float screenHeightToLengthRatio, Settings settings)
        {
            _shipMovementAndRotation = new PlayerShipMovementAndRotation(screenHeightToLengthRatio, settings);
            _shipShooting = new PlayerShipShooting();
        }
        
        public TransformInfo MoveAndRotate(float rotationInput, float moveInput, float deltaTime)
        {
            return _shipMovementAndRotation.MoveAndRotate(rotationInput, moveInput, deltaTime);
        }

        public Vector2 Shoot(ProjectileType projectileType)
        {
            if(projectileType == ProjectileType.Bullet)
            {
                return _shipShooting.ShootBullet(_shipMovementAndRotation.BasisXDirection); 
            }
            else
            {
                return _shipShooting.ShootLasser(_shipMovementAndRotation.BasisXDirection);
            }
        }

        public void Restart()
        {
            _shipMovementAndRotation.Restart();
        }
    }

    [Serializable]
    public class Settings
    {
        public float rotationSpeed = 180f;
        public float maxSpeed = 0.5f;
        public float deccelerationTime = 6f;
        public float accelerationTime = 3f;
    }
}