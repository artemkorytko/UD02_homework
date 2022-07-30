using UnityEngine;
using Cysharp.Threading.Tasks;

public class Movable : MonoBehaviour
{
    [SerializeField] private float _speed;

    private bool _isMoving;

    public bool IsMoving => _isMoving;

    public async UniTask Move(Vector3 point)
    {
        _isMoving = true;

        while (transform.position != point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, _speed * Time.deltaTime);
            await UniTask.Yield();
        }

        _isMoving = false;
    }
}
