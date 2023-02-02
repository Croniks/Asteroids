using System;
using System.Numerics;

namespace GameModel.PlayerShipModel
{
    internal class PlayerShipMovementAndRotation
    {
        private float _screenHeightToLengthRatio = 1f;

        private float _rotationSpeed = 180f;
        private float _maxSpeed = 0.5f;
        private float _deccelerationTime = 6f;
        private float _accelerationTime = 3f;

        private float _rotaionAngle = 0f;
        private float _speedModule = 0f;
        private float _resistAcceleration = 0f;
        private float _speedAcceleration = 0f;
        
        private Vector2 _speedDirection = Vector2.UnitX;

        public Vector2 BasisXDirection { get => _basisXDirection; }
        private Vector2 _basisXDirection = Vector2.UnitX;

        public Vector2 Position { get => _position; }
        private Vector2 _position = new Vector2(0.5f, 0.5f);


        public PlayerShipMovementAndRotation(float screenHeightToLengthRatio, Settings settings)
        {
            _screenHeightToLengthRatio = screenHeightToLengthRatio;
            _position = new Vector2(0.5f, 0.5f * _screenHeightToLengthRatio);

            _rotationSpeed = settings.rotationSpeed;
            _maxSpeed = settings.maxSpeed;
            _deccelerationTime = settings.deccelerationTime;
            _accelerationTime = settings.accelerationTime;

            _resistAcceleration = _maxSpeed / _deccelerationTime;
            _speedAcceleration = (_maxSpeed / _accelerationTime) + _resistAcceleration;
        }

        public TransformInfo MoveAndRotate(float rotationInput, float moveInput, float deltaTime)
        {
            int angle = Utilities.RepeatAngleWithinRangingFrom0To360(_rotaionAngle);

            if (rotationInput != 0f)
            {
                _rotaionAngle += _rotationSpeed * deltaTime * rotationInput;
                angle = Utilities.RepeatAngleWithinRangingFrom0To360(_rotaionAngle);

                Matrix4x4 rotationMatrix = Utilities.GetRotationMatrix(angle);
                _basisXDirection = Vector2.Transform(Vector2.UnitX, rotationMatrix);
            }

            Vector2 offset = Vector2.Zero;

            if (moveInput > 0f)
            {
                Vector2 speedVector = Vector2.Multiply(_speedDirection, _speedModule);
                Vector2 accelerationVector = Vector2.Multiply(_basisXDirection, _speedAcceleration * deltaTime);

                Vector2 newSpeedVector = Vector2.Add(accelerationVector, speedVector);

                float speedVectorLength = newSpeedVector.Length();

                _speedDirection = Vector2.Divide(newSpeedVector, speedVectorLength);
                _speedModule = speedVectorLength - (_resistAcceleration * deltaTime);
            }
            else
            {
                _speedModule -= _resistAcceleration * deltaTime;
            }

            _speedModule = Math.Clamp(_speedModule, 0f, _maxSpeed);
            offset = Vector2.Multiply(_speedModule * deltaTime, _speedDirection);
            
            _position += offset;
            _position = Utilities.RepeatVectorWithinScreenBorders(_position, _screenHeightToLengthRatio);

            return new TransformInfo(_basisXDirection, _position, angle, _speedModule);
        }

        public void Restart()
        {
            _position = new Vector2(0.5f, 0.5f * _screenHeightToLengthRatio);
            _basisXDirection = Vector2.UnitX;
            _rotaionAngle = 0f;
            _speedModule = 0f;
        }
    }
}