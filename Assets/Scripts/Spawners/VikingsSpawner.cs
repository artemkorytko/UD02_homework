using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class VikingsSpawner : WarriorsSpawner
{
    [SerializeField] private Player player;
    
    private const int START_MOVE_DELAY = 500;
    private const int WAIT_VIKINGS_MOVING_DELAY = 500;
    
    private Dictionary<Viking, RewardComponent> _rewards = new Dictionary<Viking, RewardComponent>();
    private readonly List<Viking> _vikings = new List<Viking>();
    
    private Player _player;
    private Door _door;
    private Vector3 _doorPosition = Vector3.zero; 
    
    public Dictionary<Viking, RewardComponent> Rewards => _rewards;

    protected override void CreatingObjects(int objectsCount)
    {
        base.CreatingObjects(objectsCount);
        _vikings.AddRange(GetComponentsInChildren<Viking>());

        foreach (var viking in _vikings)
        {
            viking.OnRewarded += UpdateDictionary;
        }
    }

    public Player PlayerInstantiate()
    {
        _player = Instantiate(player.gameObject, transform).GetComponent<Player>();
        _player.gameObject.transform.position = SpawnPoint.position;
        return _player;
    }

    private void UpdateDictionary(Viking viking, RewardComponent reward)
    {
        _rewards.Add(viking, reward);
        Debug.Log(viking.name + " " + reward.name);
    }

    public async UniTask MoveToPoints(IPoints points)
    {
        var movingCount = 0;
        
        foreach (var viking in _vikings)
        {
            _ = viking.StartMove(points.GetPoint());
            await UniTask.Yield();
        }

        while (movingCount != _vikings.Count)
        {
            await UniTask.Delay(WAIT_VIKINGS_MOVING_DELAY);
        
            foreach (var viking in _vikings)
            {
                var movable = viking.GetComponent<Movable>();
                if (movable.IsMoving == false) movingCount++;
            }

            if (movingCount < _vikings.Count) movingCount = 0;
        }
    }

    public async UniTask MoveToChurch()
    {
        _door = FindObjectOfType<Door>();
        _doorPosition = _door.gameObject.transform.position;
        var vikingsCount = _objectsParent.gameObject.transform.childCount;

        for (var i = 0; i < vikingsCount - 1; i++)
        {
            for (var j = i + 1; j < vikingsCount; j++)
            {
                if ((_vikings[i].gameObject.transform.position - _doorPosition).sqrMagnitude > 
                    (_vikings[j].gameObject.transform.position - _doorPosition).sqrMagnitude)
                {
                    (_vikings[i], _vikings[j]) = (_vikings[j], _vikings[i]);
                }
            }
        }

        foreach (var viking in _vikings)
        {
            var movable = viking.GetComponent<Movable>();
            await UniTask.WaitUntil(() => movable.IsMoving == false);
            _ = viking.StartMove(_doorPosition);
            await UniTask.Delay(START_MOVE_DELAY);
        }
    }

    public async UniTask MoveToRewards()
    {
        foreach (var viking in _vikings)
        {
            var movable = viking.GetComponent<Movable>();
            await UniTask.WaitUntil(() => movable.IsMoving == false);
            await viking.StartMove(_door.GetPoint());
        }
    }

    public async UniTask MoveToShip(IPoints points)
    {
        var viking = _door.GetStack();
        var movable = viking.GetComponent<Movable>();
        
        while (viking)
        {
            await UniTask.WaitUntil(() => movable.IsMoving == false);
            
            await viking.StartMove(_doorPosition);
            await UniTask.WaitUntil(() => movable.IsMoving == false);
            
            await viking.StartMove(points.GetPoint());
            await UniTask.WaitUntil(() => movable.IsMoving == false);
            
            viking = _door.GetStack();
        }
    }
}
