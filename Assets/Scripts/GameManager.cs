using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DefendPointsSpawner _defendPointsSpawner;
    [SerializeField] private RewardSpawner _rewardSpawner;
    [SerializeField] private EnemiesSpawner _enemiesSpawner;
    [SerializeField] private VikingsSpawner _vikingsSpawner;
    [SerializeField] private Ship _ship;
    [SerializeField] private Door _door;
    [SerializeField] private int _vikingsCount;

    private async void Start()
    {
        await _defendPointsSpawner.StartSpawn(_vikingsCount);
        await _rewardSpawner.StartSpawn(_vikingsCount);
        _door.Initialize(_vikingsCount);
        _enemiesSpawner.Initialize(_defendPointsSpawner);
        await _enemiesSpawner.StartSpawn(_vikingsCount);
        await _ship.StartMove();
        _vikingsSpawner.Initialize(_defendPointsSpawner);
        await _vikingsSpawner.StartSpawn(_vikingsCount);
        await _vikingsSpawner.MoveToChurch();
        await _vikingsSpawner.MoveToRewards();
        await _vikingsSpawner.MoveToShip();
        await _ship.EndMove();
    }
}
