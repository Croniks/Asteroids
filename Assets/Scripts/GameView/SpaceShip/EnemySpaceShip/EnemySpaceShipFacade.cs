using System;

using UnityEngine;


public class EnemySpaceShipFacade : AbstractSpaceShipFacade
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private DestructionLogic _enemyDestructionLogic;
    [SerializeField] private Explosion _explosion;

    
    private void OnEnable()
    {
        _meshRenderer.enabled = true;
        _enemyDestructionLogic.EnableCollider(true);
    }

    public bool CheckDestruction()
    {
        return _enemyDestructionLogic.IsDestroyed;
    }

    public void DoExplosionEffect(Action afterAction)
    {
        _meshRenderer.enabled = false;
        _enemyDestructionLogic.EnableCollider(false);

        _explosion.DoExplosion(afterAction);
    }
}