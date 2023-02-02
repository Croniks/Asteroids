using UnityEngine;

public class LasserDestructionLogic : DestructionLogic
{
    [SerializeField] private BoxCollider _collider;

    
    public override void EnableCollider(bool on)
    {
        _collider.enabled = on;
    }

    protected override bool CheckDestruction(DestructionLogic otherLogic)
    {
        return false;
    }
}