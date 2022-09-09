using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    public event Action OnFinish;
    
    public bool IsMoving { get; private set; }
    public bool IsFinished { get; private set; }
    public RewardComponent Reward { get; private set; }

    private Joystick _joystick;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private bool _isCanMove;
    private bool _isRewarded;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!_isCanMove) return;

        var movingDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);

        if (movingDirection != Vector3.zero)
        {
            IsMoving = true;
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(movingDirection), speed * Time.deltaTime);
            var velocity = _transform.forward * (movingDirection.magnitude * speed * Time.deltaTime);
            _rigidbody.velocity = velocity;
        }
        else
        {
            IsMoving = false;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void ActivateMoving()
    {
        _joystick = FindObjectOfType<Joystick>();
        
        if (_joystick is null)
        {
            throw new NullReferenceException(nameof(Joystick));
        }
        
        _isCanMove = true;
    }
    
    public void DeactivateMoving()
    {
        _isCanMove = false;
        IsMoving = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.ReturnToPool();
        }
        else if (_isRewarded == false)
        {
            if (!other.gameObject.TryGetComponent(out RewardComponent reward)) return;

            var door = FindObjectOfType<Door>();
            door.GetPoint(reward);

            var rewardGameObject = reward.gameObject;
            Vector3 rewardPosition = GetComponentInChildren<RewardTransformComponent>().
                                       gameObject.transform.position;

            rewardGameObject.transform.SetParent(gameObject.transform);
            rewardGameObject.GetComponent<Collider>().enabled = false;
            rewardGameObject.transform.position = rewardPosition;
            rewardGameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            _isRewarded = true;
            
            Reward = reward;
        }
        else if (IsFinished == false)
        {
            if (other.gameObject.GetComponent<Ship>() == null) return;

            IsFinished = true;
            OnFinish?.Invoke();
        }
    }
}
