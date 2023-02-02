using System;
using System.Numerics;

using GameModel.PlayerShipModel;

namespace GameModel.EnemyShipModel
{
    public class EnemyShip 
    {
        private static int _lastId = -1;

        public int ID => _id;
        private readonly int _id;

        private EnemyShipMovementAndRotation _shipMovementAndRotation;


        public EnemyShip(float screenHeightToLengthRatio, Settings settings, PlayerShip playerShip, Vector2 defaultPosition = default)
        {
            _id = EnemyShip.GetID();

            _shipMovementAndRotation = new EnemyShipMovementAndRotation(screenHeightToLengthRatio, settings, playerShip, defaultPosition);
        }

        public TransformInfo MoveAndRotate(float deltaTime)
        {
            return _shipMovementAndRotation.MoveAndRotate(deltaTime);
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