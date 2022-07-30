using UnityEngine;
using Cysharp.Threading.Tasks;

public class Ship : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Movable _movable;

    public async UniTask StartMove()
    {
        await _movable.Move(_startPoint.position);
    }

    public async UniTask EndMove()
    {
        await _movable.Move(_endPoint.position);
    }
}
