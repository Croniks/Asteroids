using UnityEngine;

using TMPro;

public class GroupElement : MonoBehaviour
{
    public GroupElementType ElementType => _elementType;
    [SerializeField] private GroupElementType _elementType;

    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _message;

    [SerializeField, Space] private string _defaultMessage = string.Empty;


    private void Awake()
    {
        _label.text = $"{_elementType}:";
        _message.text = _defaultMessage;
    }

    public void SetMessage(string text)
    {
        _message.text = text;
    }
}

public enum GroupElementType
{
    Position,
    RotationAngle,
    Velocity,
    LaserCharges,
    LaserCooldown,
    Move,
    Rotate,
    ShootBullet,
    ShootLasser
}