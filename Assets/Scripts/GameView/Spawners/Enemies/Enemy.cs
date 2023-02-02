using UnityEngine;

using Spawners;


public class Enemy : PoolObject
{
    [SerializeField] private EnemySpaceShipFacade _enemySpaceShip;
    public EnemySpaceShipFacade EnemySpaceShip => _enemySpaceShip;
}