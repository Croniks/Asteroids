using UnityEngine;

using GameModel;
using System;

public class PlayerSpaceShipFacade : AbstractSpaceShipFacade
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private PlayerFireSystem _fireSystem;
    [SerializeField] private DestructionLogic _playerDestructionLogic;
    [SerializeField] private Explosion _explosion;
    
    
    public void Setup(LasserPool lasserPool, BulletPool bulletPool)
    {
        _fireSystem.Setup(lasserPool, bulletPool, transform);
    }

    public void ResetPlayer()
    {
        gameObject.SetActive(true);
        _playerDestructionLogic.EnableCollider(true);
        _meshRenderer.enabled = true;
    }

    public void Shoot(Vector2 direction, ProjectileType projectileType)
    {
        _fireSystem.Shoot(direction, projectileType);
    }

    public bool CheckDestruction()
    {
        return _playerDestructionLogic.IsDestroyed;
    }

    public void DoExplosionEffect(Action afterAction)
    {
        _meshRenderer.enabled = false;
        _playerDestructionLogic.EnableCollider(false);

        _explosion.DoExplosion(afterAction);
    }
}