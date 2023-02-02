using System;

using GameModel;

namespace GameController
{
    public interface IGameView
    {
        public event Action<float> Tick;
        public event Action<int> DestroyEnemy;
        public event Action<int> DestroyAsteroid;
    }

    public interface IInputReader
    {
        public float GetMoveInput();
        public float GetRotationInput();
        public ProjectileType GetShootInput();
    }
}