using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 2f; 
    
    private Joystick _joystick;
    private bool _isCanMove = false;
    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
        _joystick = FindObjectOfType<Joystick>();

        if (_joystick is null)
        {
            throw new NullReferenceException(nameof(Joystick));
        }
    }

    private void Update()
    {
        if (!_isCanMove) return;

        var movingDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);

        if (movingDirection == Vector3.zero) return;
        
        _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(movingDirection), speed * Time.deltaTime);
        _transform.position += _transform.forward * (movingDirection.magnitude * speed * Time.deltaTime);
    }

    public void ActivateMoving()
    {
        _isCanMove = true;
    }
}
