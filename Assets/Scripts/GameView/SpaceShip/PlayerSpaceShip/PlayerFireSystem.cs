using UnityEngine;

using GameModel;


public class PlayerFireSystem : MonoBehaviour
{
    [SerializeField] private float _bulletFrequency = 0.5f;
    [SerializeField] private float _lasserFrequency = 1f;

    private LasserPool _lasserPool;
    private BulletPool _bulletPool;
    private Transform _shipTransfrom;

    private float _timeToBulletShoot = 0f;
    private float _timeToLasserShoot = 0f;

    
    private void Update()
    {
        _timeToBulletShoot += Time.deltaTime;
        _timeToLasserShoot += Time.deltaTime;
    }

    public void Setup(LasserPool lasserPool, BulletPool bulletPool, Transform shipTransform)
    {
        _lasserPool = lasserPool;
        _bulletPool = bulletPool;
        _shipTransfrom = shipTransform;
    }

    public void Shoot(Vector2 direction, ProjectileType projectileType)
    {
        if (projectileType == ProjectileType.Bullet)
        {
            ShootBullet(direction);
        }
        else if(projectileType == ProjectileType.Lasser)
        {
            ShootLasser(direction);
        }
    }

    private void ShootBullet(Vector2 direction)
    {
        if (_timeToBulletShoot < _bulletFrequency)
        {
            return;
        }

        _timeToBulletShoot = 0f;

        Bullet bullet = _bulletPool.Spawn();
        bullet.transform.position = transform.position;
        bullet.transform.right = direction;
    }

    private void ShootLasser(Vector2 direction)
    {
        if (_timeToLasserShoot < _lasserFrequency)
        {
            return;
        }

        _timeToLasserShoot = 0f;

        Lasser lasser = _lasserPool.Spawn();

        lasser.transform.position = transform.position;
        lasser.transform.right = direction;
        lasser.Setup(transform);
    }
}