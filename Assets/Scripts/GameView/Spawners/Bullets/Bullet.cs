using UnityEngine;
using Spawners;


public class Bullet : PoolObject 
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxLifeTime;
    [SerializeField] private DestructionLogic _bulletDestructionLogic;

    private float _currentLifeTime = 0f;


    private void OnEnable()
    {
        _currentLifeTime = 0f;
    }
    
    public void Update()
    {
        if(_currentLifeTime >= _maxLifeTime || CheckDestruction() == true)
        {
            ReturnToPool();
        }

        transform.position += transform.right * _speed * Time.deltaTime;
        _currentLifeTime += Time.deltaTime;
    }

    public bool CheckDestruction()
    {
        return _bulletDestructionLogic.IsDestroyed;
    }
}