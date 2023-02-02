using System;
using System.Numerics;


namespace GameModel.Asteroid
{
    public class Asteroid 
    {
        private static int _lastId = -1;

        public int ID => _id;
        private readonly int _id;

        private float _screenHeightToLengthRatio;
        private float _maxSpeed;
        private Vector2 _position;
        private Vector2 _randomDirection;


        public Asteroid(float screenHeightToLengthRatio, Settings settings, Vector2 defaultPosition = default)
        {
            _id = Asteroid.GetID();

            _screenHeightToLengthRatio = screenHeightToLengthRatio;
            _maxSpeed = settings.maxSpeed;
            _position = defaultPosition;

            Random random = new Random();
            int angle = random.Next(0, 359);
            Matrix4x4 rotationMatrix = Utilities.GetRotationMatrix(angle);

            _randomDirection = new Vector2(rotationMatrix.M11, rotationMatrix.M21);
        }

        public Vector2 Move(float deltaTime)
        {
            Vector2 offset = Vector2.Multiply(_maxSpeed * deltaTime, _randomDirection);

            _position += offset;
            _position = Utilities.RepeatVectorWithinScreenBorders(_position, _screenHeightToLengthRatio);

            return _position;
        }

        private static int GetID()
        {
            return ++_lastId;
        }
    }

    [Serializable]
    public class Settings
    {
        public float maxSpeed = 0.5f;
    }
}