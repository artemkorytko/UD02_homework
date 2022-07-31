using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private DefendPointsSpawner defendPointsSpawner;
    [SerializeField] private RewardSpawner rewardSpawner;
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    [SerializeField] private VikingsSpawner vikingsSpawner;
    [SerializeField] private Ship ship;
    [SerializeField] private Door door;
    [SerializeField] private int vikingsCount;
    [SerializeField] private Player player;
    
    private async void Start()
    {
        await defendPointsSpawner.StartSpawn(vikingsCount);
        await rewardSpawner.StartSpawn(vikingsCount);
        door.Initialize(vikingsCount);
        enemiesSpawner.Initialize(defendPointsSpawner);
        await enemiesSpawner.StartSpawn(vikingsCount);
        await ship.StartMove();
        vikingsSpawner.Initialize(defendPointsSpawner);
        await vikingsSpawner.StartSpawn(vikingsCount);
        await vikingsSpawner.MoveToChurch();
        await vikingsSpawner.MoveToRewards();
        await vikingsSpawner.MoveToShip();
        await ship.EndMove();
    }
}
