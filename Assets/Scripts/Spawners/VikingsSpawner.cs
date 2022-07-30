using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class VikingsSpawner : WarriorsSpawner
{
    private List<Viking> _vikings = new List<Viking>();
    private Door _door = null;
    private Vector3 _doorPosition = Vector3.zero; 
    private Dictionary<Viking, Reward> _rewards = new Dictionary<Viking, Reward>();

    public async UniTask MoveToChurch()
    {
        _door = FindObjectOfType<Door>();
        _doorPosition = _door.gameObject.transform.position;
        var vikingsCount = _objectsParent.gameObject.transform.childCount;
        _vikings.AddRange(GetComponentsInChildren<Viking>());

        for (int i = 0; i < vikingsCount - 1; i++)
        {
            for (int j = i + 1; j < vikingsCount; j++)
            {
                if ((_vikings[i].gameObject.transform.position - _doorPosition).sqrMagnitude > 
                    (_vikings[j].gameObject.transform.position - _doorPosition).sqrMagnitude)
                {
                    var viking = _vikings[i];
                    _vikings[i] = _vikings[j];
                    _vikings[j] = viking;
                }
            }
        }

        foreach (var viking in _vikings)
        {
            _ = viking.StartMove(_doorPosition);
            await UniTask.Delay(1000);
        }
    }

    public async UniTask MoveToRewards()
    {
        foreach (var viking in _vikings)
        {
            await UniTask.WaitUntil(() => viking.GetComponent<Movable>().IsMoving == false);
            _ = viking.StartMove(_door.GetPoint());
            viking.OnRewarded += UpdateDictionary;
        }
    }

    public void UpdateDictionary(Viking viking, Reward reward)
    {
        _rewards.Add(viking, reward);
        print(viking.name + " " + reward.name);
    }

    public async UniTask MoveToShip()
    {
        var viking = _door.GetStack();
        var shipPosition = FindObjectOfType<Ship>().transform.position;

        while (viking)
        {
            await UniTask.WaitUntil(() => viking.GetComponent<Movable>().IsMoving == false);
            _ = viking.StartMove(_doorPosition);
            await UniTask.WaitUntil(() => viking.GetComponent<Movable>().IsMoving == false);
            _ = viking.StartMove(shipPosition);
            await UniTask.WaitUntil(() => viking.GetComponent<Movable>().IsMoving == false);
            viking.ReturnToPool();
            viking = _door.GetStack();
        }
    }
}
