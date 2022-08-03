using UnityEngine;
using DG.Tweening;


public class Viking : MonoBehaviour
{
    public string vikingName;
    private int _walkSpeed = 20;     //readonly))
    private bool _onFigntPoint;
    public bool OnFightPoint => _onFigntPoint;
    
    private bool _onPoint = false;
    public bool OnPoint => _onPoint;
    
    private bool _onGates;
    public bool OnGates => _onGates;
    
    private bool _onChurchPoint;
    public bool OnChurchPoint => _onChurchPoint;
    
    

    private void OnTriggerEnter(Collider CollisionCollider)
    {
        if (CollisionCollider.gameObject.TryGetComponent<FightPoint>(out FightPoint fightpoint))
        {
            _onFigntPoint = true;
        }
        if (CollisionCollider.gameObject.TryGetComponent<Gates>(out Gates gates))
        {
            _onGates = true;
        }
        if (CollisionCollider.gameObject.TryGetComponent<ChurchPoint>(out ChurchPoint point))
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
