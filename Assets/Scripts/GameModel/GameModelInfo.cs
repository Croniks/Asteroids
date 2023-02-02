using System;
using System.Numerics;

using UnityEngine.UIElements;

namespace GameModel
{
    public enum ProjectileType { None, Bullet, Lasser }
    
    public struct TransformInfo
    {
        public Vector2 BasisX { get; private set; }
        public Vector2 Position { get; private set; }

        public float Angle { get; private set; }
        public float Speed { get; private set; }

        public TransformInfo(Vector2 newBasisX, Vector2 newPosition, float angle = 0f, float speed = 0f)
        {                                                             
            BasisX = newBasisX;
            Position = newPosition;
            Angle = angle;
            Speed = speed;
        }
    }

    [Serializable]
    public class GameSettings
    {
        public PlayerShipModel.Settings PlayerShipSettings;
        public EnemyShipModel.Settings EnemyShipSettings;
        public Asteroid.Settings AsteroidSettings;
    }
}