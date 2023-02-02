using UnityEngine;

public class SpaceShipMoveAndRotation : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    public Vector2 Position
    {
        get { return _transform.position; }
        set { _transform.position = value; }
    }

    public Vector2 BasisX
    {
        get { return _transform.right; }
        set { _transform.right = value; }
    }
}