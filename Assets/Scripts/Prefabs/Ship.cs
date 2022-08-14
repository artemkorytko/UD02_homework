using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Movable))]
public class Ship : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
    private Movable _movable;

    private void Awake()
    {
        _movable = GetComponent<Movable>();
    }

    public async UniTask StartMove()
    {
        await _movable.Move(startPoint.position);
        startPoint.gameObject.SetActive(false);
    }

    public async UniTask EndMove()
    {
        await _movable.Move(endPoint.position);
    }
}
