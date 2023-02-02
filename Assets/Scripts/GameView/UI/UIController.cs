using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform _gameInfoContainer;
    [SerializeField] private GroupMaster _gameInfoMaster;
    [SerializeField] private Button _restartButton;
    public Button RestartButton => _restartButton;

    public void EnableGameUI(bool on)
    {
        int scale = on == true ? 1 : 0;
        _gameInfoContainer.transform.localScale = Vector3.one * scale;

        scale = on == false ? 1 : 0;
        _restartButton.transform.localScale = Vector3.one * scale;
    }

    public void SetMessage(GroupElementType elementType, string message)
    {
        _gameInfoMaster.SetMessage(elementType, message);
    }
}