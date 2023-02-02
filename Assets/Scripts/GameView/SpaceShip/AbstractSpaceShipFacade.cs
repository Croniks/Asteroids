using UnityEngine;

public abstract class AbstractSpaceShipFacade : MonoBehaviour
{
    [SerializeField] protected SpaceShipMoveAndRotation spaceShipMoveAndRotation;

    public void SetPosition(Vector2 position)
    {
        spaceShipMoveAndRotation.Position = position;
    }

    public void SetBasisX(Vector2 basisX)
    {
        spaceShipMoveAndRotation.BasisX = basisX;
    }
}