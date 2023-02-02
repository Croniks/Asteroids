using Unity.VisualScripting;

using UnityEngine;


public abstract class DestructionLogic : MonoBehaviour
{
    public bool IsDestroyed { get; private set; }


    private void OnEnable()
    {
        IsDestroyed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsDestroyed == true)
        {
            return;
        }

        if (other.TryGetComponent(out DestructionLogic otherLogic))
        {
            IsDestroyed = CheckDestruction(otherLogic);
        }
    }

    public abstract void EnableCollider(bool on);
    protected abstract bool CheckDestruction(DestructionLogic otherLogic);
}