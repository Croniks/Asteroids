using UnityEngine;

public class BulletDestructionLogic : DestructionLogic
{
    [SerializeField] private BoxCollider _collider;

    
    public override void EnableCollider(bool on)
    {
        _collider.enabled = on;
    }

    protected override bool CheckDestruction(DestructionLogic otherLogic)
    {
        if(otherLogic is AsteroidDestructionLogic || otherLogic is EnemyDestructionLogic)
        {
            return true;
        }

        return false;
    }
}