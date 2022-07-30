using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Viking : MonoBehaviour
{
    public string Name;
    private Rigidbody _rb;
    private readonly int _walkSpeed = 20;     //readonly))


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private bool _onFigntPoint = false;
    public bool OnFightPoint => _onFigntPoint;
 


    private bool _onPoint = false;
    public bool OnPoint => _onPoint;



    private bool _onGates = false;
    public bool OnGates => _onGates;
    

    private bool _onChurchPoint = false;
    public bool OnChurchPoint => _onChurchPoint;
    


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<FightPoint>(out FightPoint fightpoint))
        {
            _onFigntPoint = true;
        }
        if (collider.gameObject.TryGetComponent<Gates>(out Gates gates))
        {
            _onGates = true;
        }
        if (collider.gameObject.TryGetComponent<ChurchPoint>(out ChurchPoint point))
        {
            _onChurchPoint = true;
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<FightPoint>(out FightPoint fightpoint))
        {
            _onFigntPoint = false;
        }
        if (collider.gameObject.TryGetComponent<Gates>(out Gates gates))
        {
            _onGates = false;
        }
        if (collider.gameObject.TryGetComponent<ChurchPoint>(out ChurchPoint point))
        {
            _onChurchPoint = false;
        }
    }


    public void MoveToPoint(Vector3 targetPoint)
    {
        gameObject.transform.parent.DOMove(targetPoint, Vector3.Distance(gameObject.transform.position, targetPoint)/_walkSpeed);
    }
}
