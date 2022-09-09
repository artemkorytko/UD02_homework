using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] private Transform objectTransform;
    
    private Vector3 _offset;

    private void Awake()
    {
        _offset = objectTransform.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.position = objectTransform.position - _offset;
    }
}
