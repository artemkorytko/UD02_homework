using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AI;

//[RequireComponent(typeof(Movable))]
public class Ship : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
    //private Movable _movable;
    private NavMeshAgent _navMeshAgent;
    private bool _isMoving;
    private Vector3 _movingToPoint;

    private void Awake()
    {
        //_movable = GetComponent<Movable>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public async UniTask StartMove()
    {
        //await _movable.Move(startPoint.position);
        await Move(startPoint);
    }

    public async UniTask EndMove()
    {
        //await _movable.Move(endPoint.position);
        await Move(endPoint);
    }

    private async UniTask Move(Transform targetPoint)
    {
        _isMoving = true;
        _movingToPoint = targetPoint.position;
        _navMeshAgent.SetDestination(_movingToPoint);
        await UniTask.WaitUntil(() => _isMoving == false);
        targetPoint.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_isMoving) return;
        
        if (other.gameObject.transform.position == _movingToPoint)
        {
            _isMoving = false;
        }
    }
}
