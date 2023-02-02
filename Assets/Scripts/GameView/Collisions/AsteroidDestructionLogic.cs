using UnityEngine;

public class AsteroidDestructionLogic : DestructionLogic
{
    [SerializeField] private SphereCollider _collider;

    
    public override void EnableCollider(bool on)
    {
        _collider.enabled = on;
    }

    protected override bool CheckDestruction(DestructionLogic otherLogic)
    {
        if(otherLogic is BulletDestructionLogic || otherLogic is LasserDestructionLogic)
        {
            return true;
        }
        
        return false;
    }
}