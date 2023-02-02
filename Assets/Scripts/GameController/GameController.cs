using System;
using System.Numerics;
using System.Collections.Generic;

using GameModel;
using GameModel.EnemyShipModel;
using GameModel.PlayerShipModel;
using GameModel.Asteroid;
using static UnityEngine.EventSystems.EventTrigger;


namespace GameController
{
    public class GameController
    {
        public event Action<TransformInfo> MoveAndRotatePlayerShip;
        public event Action<Vector2, ProjectileType> ShootFromPlayerShip;

        public event Action<int> SpawnEnemyShip;
        public event Action<int, TransformInfo> MoveEnemyShip;
        public event Action<int> DestroyEnemyShip;

        public event Action<int> SpawnAsteroid;
        public event Action<int, Vector2> MoveAsteroid;
        public event Action<int> DestroyAsteroid;

        private float _screenHeightToLengthRatio;
        private IGameView _gameView;
        private IInputReader _input;
        private GameSettings _gameSettings;

        private PlayerShip _playerShip;

        private Dictionary<int, EnemyShip> _enemies = new Dictionary<int, EnemyShip>(); 
        private Dictionary<int, Vector2> _enemySpawnPositions = new Dictionary<int, Vector2>(); 
        private float _enemyAppearanceFrequency = 3f;
        private float _timeToEnemyAppearance = 0f;
        private int _currentEnemySpawnPlaceID = -1;

        private Dictionary<int, Asteroid> _asteroids = new Dictionary<int, Asteroid>();
        private Dictionary<int, Vector2> _asteroidsSpawnPositions = new Dictionary<int, Vector2>();
        private float _asteroidAppearanceFrequency = 1f;
        private float _timeToAsteroidAppearance = 1f;
        private int _currentAsteroidSpawnPlaceID = -1;

        private List<int> _idsForDelete = new List<int>(8);

        public GameController
        (
            float screenHeightToLengthRatio,
            IGameView gameView,
            IInputReader input,
            GameSettings gameSettings
        )
        {
            _screenHeightToLengthRatio = screenHeightToLengthRatio;
            _gameView = gameView;
            _input = input;
            _gameSettings = gameSettings;

            _playerShip = new PlayerShip(_screenHeightToLengthRatio, gameSettings.PlayerShipSettings);

            _enemySpawnPositions.Add(0, new Vector2(0f, screenHeightToLengthRatio / 2));
            _enemySpawnPositions.Add(1, new Vector2(0.5f, screenHeightToLengthRatio));
            _enemySpawnPositions.Add(2, new Vector2(0.9f, screenHeightToLengthRatio / 2));
            _enemySpawnPositions.Add(3, new Vector2(0.5f, 0f));

            _asteroidsSpawnPositions.Add(0, new Vector2(0f, 0f));
            _asteroidsSpawnPositions.Add(1, new Vector2(0f, screenHeightToLengthRatio));
            _asteroidsSpawnPositions.Add(2, new Vector2(0.9f, screenHeightToLengthRatio));
            _asteroidsSpawnPositions.Add(3, new Vector2(0.9f, 0f));
        }

        public void Enable()
        {
            _gameView.Tick += OnTick;

            _gameView.DestroyEnemy += OnEnemyDestroyed;
            _gameView.DestroyAsteroid += OnAsteroidDestroyed;
        }

        public void Disable()
        {
            _gameView.Tick -= OnTick;

            _gameView.DestroyEnemy -= OnEnemyDestroyed;
            _gameView.DestroyAsteroid -= OnAsteroidDestroyed;
        }

        public void Restart()
        {
            foreach (var enemy in _enemies)
            {
                _idsForDelete.Add(enemy.Key);
            }

            _idsForDelete.ForEach(id => { OnEnemyDestroyed(id); });
            _idsForDelete.Clear();

            foreach (var asteroid in _asteroids)
            {
                _idsForDelete.Add(asteroid.Key);
            }

            _idsForDelete.ForEach(id => { OnAsteroidDestroyed(id); });
            _idsForDelete.Clear();

            _playerShip.Restart();
        }

        private void OnTick(float deltaTime)
        {
            ProcessPlayer(deltaTime);
            ProcessEnemies(deltaTime);
            ProcessAsteroids(deltaTime);
        }

        private void ProcessPlayer(float deltaTime)
        {
            float rotationInput = _input.GetRotationInput();
            float moveInput = _input.GetMoveInput();
            
            TransformInfo newTransformInfo = _playerShip.MoveAndRotate(rotationInput, moveInput, deltaTime);
            MoveAndRotatePlayerShip?.Invoke(newTransformInfo);

            ProjectileType projectileType = _input.GetShootInput();

            if(projectileType != ProjectileType.None)
            {
                Vector2 shootDirection = _playerShip.Shoot(projectileType);
                ShootFromPlayerShip?.Invoke(shootDirection, projectileType);
            }
        }

        private void ProcessEnemies(float deltaTime)
        {
            _timeToEnemyAppearance += deltaTime;

            if(_timeToEnemyAppearance >= _enemyAppearanceFrequency && _enemies.Count < 2)
            {
                _timeToEnemyAppearance = 0f;

                _currentEnemySpawnPlaceID++;
                _currentEnemySpawnPlaceID = Utilities.RepeatInt(_currentEnemySpawnPlaceID, _enemySpawnPositions.Count - 1);
                Vector2 spawnPosition = _enemySpawnPositions[_currentEnemySpawnPlaceID];

                EnemyShip enemyShip = new EnemyShip(
                _screenHeightToLengthRatio,
                _gameSettings.EnemyShipSettings,
                _playerShip,
                spawnPosition);

                _enemies.Add(enemyShip.ID, enemyShip);

                SpawnEnemyShip?.Invoke(enemyShip.ID);
            }

            if(_enemies.Count > 0)
            {
                foreach(var enemy in _enemies)
                {
                    TransformInfo transformInfo = enemy.Value.MoveAndRotate(deltaTime);
                    MoveEnemyShip?.Invoke(enemy.Key, transformInfo);
                }
            }
        }

        private void ProcessAsteroids(float deltaTime)
        {
            _timeToAsteroidAppearance += deltaTime;

            if (_timeToAsteroidAppearance >= _asteroidAppearanceFrequency && _asteroids.Count < 4)
            {
                _timeToAsteroidAppearance = 0f;

                for (int i = _asteroids.Count; i < 4; i++)
                {
                    _currentAsteroidSpawnPlaceID++;
                    _currentAsteroidSpawnPlaceID = Utilities.RepeatInt(_currentAsteroidSpawnPlaceID, _asteroidsSpawnPositions.Count - 1);
                    Vector2 spawnPosition = _asteroidsSpawnPositions[_currentAsteroidSpawnPlaceID];

                    Asteroid asteroid = new Asteroid(
                    _screenHeightToLengthRatio,
                    _gameSettings.AsteroidSettings,
                    spawnPosition);

                    _asteroids.Add(asteroid.ID, asteroid);

                    SpawnAsteroid?.Invoke(asteroid.ID);
                }
            }

            if (_asteroids.Count > 0)
            {
                foreach (var asteroid in _asteroids)
                {
                    Vector2 position = asteroid.Value.Move(deltaTime);
                    MoveAsteroid?.Invoke(asteroid.Key, position);
                }
            }
        }

        private void OnEnemyDestroyed(int id)
        {
            if (_enemies.ContainsKey(id) == true)
            {
                _enemies.Remove(id);
                DestroyEnemyShip?.Invoke(id);
            }
        }

        private void OnAsteroidDestroyed(int id)
        {
            if (_asteroids.ContainsKey(id) == true)
            {
                _asteroids.Remove(id);
                DestroyAsteroid?.Invoke(id);
            }
        }
    }
}