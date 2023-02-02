using Spawners;

using UnityEngine;

public class Lasser : PoolObject 
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxLength;
    [SerializeField] private DestructionLogic _lasserDestructionLogic;

    private float _currentLength = 0f;
    private Transform _firePointTransform;


    public void Setup(Transform firePointTransform)
    {
        _firePointTransform = firePointTransform;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
       _currentLength = 0f;

        transform.localScale = new Vector3(_maxLength, 1f, 1f);
    }
    
    public void Update()
    {
        if (_currentLength >= _maxLength)
        {
            ReturnToPool();
        }

        float lengthOffset = _speed * Time.deltaTime;
        _currentLength += lengthOffset;
        transform.localScale = new Vector3(_currentLength, 1f, 1f);
        transform.position += transform.right * (lengthOffset / 4f);
    }

    public bool CheckDestruction()
    {
        return _lasserDestructionLogic.IsDestroyed;
    }
}