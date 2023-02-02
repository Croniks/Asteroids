using System;
using System.Collections.Generic;
using Vector2DotNet = System.Numerics.Vector2;

using UnityEngine;

using GameController;
using GameModel;


namespace AsteroidsGame
{
    public class GameView : MonoBehaviour, IGameView
    {
        public event Action<float> Tick;
        public event Action<int> DestroyEnemy;
        public event Action<int> DestroyAsteroid;

        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private UIController _ui;
        [SerializeField, Space] private PlayerSpaceShipFacade _playerSpaceShipFacade;

        [SerializeField, Space] private LasserPool _lasserPool;
        [SerializeField] private BulletPool _bulletPool;
        [SerializeField] private AsteroidsPool _asteroidsPool;
        [SerializeField] private EnemyPool _enemyPool;

        private float _screenHeightToLengthRatio = 1f;
        private InputProvider _inputProvider;
        private GameController.GameController _gameController;

        private Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();
        private Dictionary<int, Asteroid> _asteroids = new Dictionary<int, Asteroid>();

        private List<int> _idsForDelete = new List<int>(8);

        #region UnityCalls

        private void Awake()
        {
            _screenHeightToLengthRatio = ((float)Screen.height / (float)Screen.width);

            _inputProvider = new InputProvider();
            _gameController = new GameController.GameController
            (
                _screenHeightToLengthRatio,
                this,
                _inputProvider,
                _gameSettings.Settings                                            
            );

            _gameController.MoveAndRotatePlayerShip += MoveAndRotatePlayerShipHandler;
            _gameController.ShootFromPlayerShip += ShootFromPlayerShipHandler;

            _gameController.SpawnEnemyShip += SpawnEnemyShipHandler;
            _gameController.MoveEnemyShip += MoveEnemyShipHandler;
            _gameController.DestroyEnemyShip += DestroyEnemyShipHandler;

            _gameController.SpawnAsteroid += SpawnAsteroidHandler;
            _gameController.MoveAsteroid += MoveAsteroidHandler;
            _gameController.DestroyAsteroid += DestroyAsteroidHandler;

            _playerSpaceShipFacade.Setup(_lasserPool, _bulletPool);
            
            _ui.RestartButton.onClick.AddListener(OnRestartButton);
            _ui.EnableGameUI(true);
        }
        
        private void Update()
        {
            CheckPlayerCollisions();
            CheckEnemyCollisions();
            CheckAsteroidCollisions();

            Tick?.Invoke(Time.deltaTime);
        }

        private void OnEnable()
        {
            _gameController.Enable();
            _inputProvider.Enable();

            _ui.EnableGameUI(true);
        }

        private void OnDisable()
        {
            _gameController.Disable();
            _inputProvider.Disable();
        }

        #endregion

        #region CollisionsLogic

        private void CheckPlayerCollisions()
        {
            if(_playerSpaceShipFacade.CheckDestruction() == true)
            {
                _playerSpaceShipFacade.DoExplosionEffect(() => { _playerSpaceShipFacade.gameObject.SetActive(false); });
                enabled = false;

                _ui.EnableGameUI(false);
            }
        }

        private void CheckEnemyCollisions()
        {
            foreach(var enemy in _enemies)
            {
                if(enemy.Value.EnemySpaceShip.CheckDestruction() == true)
                {
                    _idsForDelete.Add(enemy.Key);
                    DestroyEnemy?.Invoke(enemy.Key);
                }
            }

            _idsForDelete.ForEach(id => { _enemies.Remove(id); });
            _idsForDelete.Clear();
        }

        private void CheckAsteroidCollisions()
        {
            foreach (var asteroid in _asteroids)
            {
                if (asteroid.Value.CheckDestruction() == true)
                {
                    _idsForDelete.Add(asteroid.Key);
                    DestroyAsteroid?.Invoke(asteroid.Key);
                }
            }

            _idsForDelete.ForEach(id => { _asteroids.Remove(id); });
            _idsForDelete.Clear();
        }

        #endregion

        #region GameControllerEventHandlers

        private void MoveAndRotatePlayerShipHandler(TransformInfo transformInfo)
        {
            var tuple = GetBasisAndPositionFromTransformInfo(transformInfo);

            _ui.SetMessage(GroupElementType.Position, tuple.Item2.ToString());
            _ui.SetMessage(GroupElementType.RotationAngle, transformInfo.Angle.ToString());
            _ui.SetMessage(GroupElementType.Velocity, transformInfo.Speed.ToString());

            _playerSpaceShipFacade.SetBasisX(tuple.Item1);
            _playerSpaceShipFacade.SetPosition(tuple.Item2);
        }

        private void ShootFromPlayerShipHandler(Vector2DotNet shootDirection, ProjectileType projectileType)
        {
            _playerSpaceShipFacade.Shoot(shootDirection.ConvertToVector2Unity(), projectileType);
        }

        private void SpawnEnemyShipHandler(int id)
        {
            Enemy enemy = _enemyPool.Spawn();
            _enemies.Add(id, enemy);
        }

        private void MoveEnemyShipHandler(int id, TransformInfo transformInfo)
        {
            if(_enemies.Count > 0 && _enemies.ContainsKey(id) == true)
            {
                Enemy enemy = _enemies[id];

                var tuple = GetBasisAndPositionFromTransformInfo(transformInfo);

                enemy.EnemySpaceShip.SetBasisX(tuple.Item1);
                enemy.EnemySpaceShip.SetPosition(tuple.Item2);
            }
        }

        private void DestroyEnemyShipHandler(int id)
        {
            if (_enemies.ContainsKey(id) == true)
            {
                var enemy = _enemies[id];
                enemy.EnemySpaceShip.DoExplosionEffect(() => { enemy.ReturnToPool(); });
            }
        }

        private void SpawnAsteroidHandler(int id)
        {
            Asteroid asteroid = _asteroidsPool.Spawn();
            _asteroids.Add(id, asteroid);
        }

        private void MoveAsteroidHandler(int id, Vector2DotNet position)
        {
            if (_asteroids.Count > 0 && _asteroids.ContainsKey(id) == true)
            {
                Asteroid asteroid = _asteroids[id];

                Vector2 newPosition = position.ConvertToVector2Unity();

                Vector2 screenPoint = new Vector2(newPosition.x * Screen.width, (newPosition.y * Screen.height) / _screenHeightToLengthRatio);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);

                asteroid.transform.position = worldPosition;
            }
        }
        
        private void DestroyAsteroidHandler(int id)
        {
            if (_asteroids.ContainsKey(id) == true)
            {
                var asteroid = _asteroids[id];
                asteroid.DoExplosionEffect(() => { asteroid.ReturnToPool(); });
            }
        }

        #endregion

        private (Vector2, Vector2) GetBasisAndPositionFromTransformInfo(TransformInfo transformInfo)
        {
            Vector2 normalizedPosition = transformInfo.Position.ConvertToVector2Unity();
            Vector2 screenPoint = new Vector2(normalizedPosition.x * Screen.width, (normalizedPosition.y * Screen.height) / _screenHeightToLengthRatio);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);

            Vector2 newBasisX = transformInfo.BasisX.ConvertToVector2Unity();

            return (newBasisX, worldPosition);
        }

        private void OnRestartButton()
        {
            _gameController.Restart();
            _playerSpaceShipFacade.ResetPlayer();
            enabled = true;
        }
    }
}