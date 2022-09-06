using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _offset;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<Player>().gameObject.transform;
        _offset = _playerTransform.position - transform.position;
    }

    private void LateUpdate()
    {
        transform.position = _playerTransform.position - _offset;
    }
}
