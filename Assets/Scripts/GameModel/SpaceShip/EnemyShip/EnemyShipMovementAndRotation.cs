using System;
using System.Numerics;

using GameModel.PlayerShipModel;

namespace GameModel.EnemyShipModel
{
    internal class EnemyShipMovementAndRotation
    {
        private float _screenHeightToLengthRatio = 1f;
        private float _maxSpeed = 0.5f;
        
        private PlayerShip _playerShip;
        
        public Vector2 BasisXDirection { get => _basisXDirection; }
        private Vector2 _basisXDirection = Vector2.UnitX;

        public Vector2 Position { get => _position; }
        private Vector2 _position = new Vector2(0.5f, 0.5f);


        public EnemyShipMovementAndRotation(float screenHeightToLengthRatio, Settings settings, PlayerShip playerShip, Vector2 defaultPosition)
        {
            _screenHeightToLengthRatio = screenHeightToLengthRatio;
            _position = defaultPosition;
            _maxSpeed = settings.maxSpeed;
            _playerShip = playerShip;
        }

        public TransformInfo MoveAndRotate(float deltaTime)
        {
            Vector2 targetBasisXDirection = Vector2.Normalize(_playerShip.Position - _position);
            _basisXDirection = Vector2.Lerp(_basisXDirection, targetBasisXDirection, deltaTime * 2f);
            
            Vector2 offset = Vector2.Multiply(_maxSpeed * deltaTime, _basisXDirection);
            
            _position += offset;
            _position = Utilities.RepeatVectorWithinScreenBorders(_position, _screenHeightToLengthRatio);

            return new TransformInfo(_basisXDirection, _position);
        }
    }
}