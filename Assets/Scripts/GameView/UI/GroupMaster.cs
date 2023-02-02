using System.Collections.Generic;
using UnityEngine;


public class GroupMaster : MonoBehaviour
{
    [SerializeField] private List<GroupElement> _elements;

    private Dictionary<GroupElementType, GroupElement> _elementsDict = new Dictionary<GroupElementType, GroupElement>();

    private void Awake()
    {
        _elements.ForEach(e => _elementsDict.Add(e.ElementType, e));
    }

    public void SetMessage(GroupElementType elementType, string message)
    {
        if (_elementsDict.ContainsKey(elementType))
        {
            _elementsDict[elementType].SetMessage(message);
        }
    }
}