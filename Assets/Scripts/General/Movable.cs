using UnityEngine;
using Cysharp.Threading.Tasks;

public class Movable : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private const int REDUCE_ROTATION_SPEED_COEFFICIENT = 100;
    private Vector3 _movingToPoint = Vector3.zero;
    private Transform _transform;

    public bool IsMoving { get; private set; }

    public async UniTask Move(Vector3 point)
    {
        _movingToPoint = point;
        _transform = transform;
        
        IsMoving = true;

        var movingDirection = (point - _transform.position).normalized;

        while (IsMoving)
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(movingDirection), speed / REDUCE_ROTATION_SPEED_COEFFICIENT * Time.deltaTime);
            _transform.position = Vector3.MoveTowards(_transform.position, point, speed * Time.deltaTime);
            await UniTask.Yield();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!IsMoving) return;
        
        if (other.gameObject.transform.position == _movingToPoint)
        {
            IsMoving = false;
        }
    }
}
