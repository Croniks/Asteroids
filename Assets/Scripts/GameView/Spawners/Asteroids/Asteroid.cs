using System;

using UnityEngine;

using Spawners;


public class Asteroid : PoolObject 
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private DestructionLogic _asteroidDestructionLogic;
    [SerializeField] private Explosion _explosion;


    private void OnEnable()
    {
        _meshRenderer.enabled = true;
        _asteroidDestructionLogic.EnableCollider(true);
    }
    
    public bool CheckDestruction()
    {
        return _asteroidDestructionLogic.IsDestroyed;
    }

    public void DoExplosionEffect(Action afterAction)
    {
        _meshRenderer.enabled = false;
        _asteroidDestructionLogic.EnableCollider(false);

        _explosion.DoExplosion(afterAction);
    }
}