using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIManager uiManager;
    
    [Header("Points Spawners")]
    [SerializeField] private StartPointsSpawner startPointsSpawner;
    [SerializeField] private DefendPointsSpawner defendPointsSpawner;
    [SerializeField] private FightPointsSpawner fightPointsSpawner;
    
    [Header("Objects Spawners")]
    [SerializeField] private RewardSpawner rewardSpawner;
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    [SerializeField] private VikingsSpawner vikingsSpawner;
    
    [Header("Prefabs")]
    [SerializeField] private Ship ship;
    [SerializeField] private Door door;

    [Header("Configs")]
    [SerializeField] private int vikingsCount;
    
    private bool _isStarted;
    private Player _player;
    
    private async void Start()
    {
        /* 0 - Initialize - All points spawning */
        defendPointsSpawner.StartSpawn(vikingsCount);
        startPointsSpawner.StartSpawn(vikingsCount);
        fightPointsSpawner.StartSpawn(vikingsCount);
        
        /* 0 - Initialize - Enemies and vikings spawners setting to start points */
        enemiesSpawner.SetSpawnPoints(defendPointsSpawner);
        vikingsSpawner.SetSpawnPoints(startPointsSpawner);
        
        /* 0 - Initialize - Enemies, vikings and rewards prefabs spawning at start points */
        enemiesSpawner.StartSpawn(vikingsCount);
        vikingsSpawner.StartSpawn(vikingsCount);
        rewardSpawner.StartSpawn(vikingsCount + 1);
        
        /* 0 - Initialize - Door rewards list setting when all rewards have been instantiated */
        door.SetRewardsList(vikingsCount);
        
        /* 0 - Initialize - Player instantiate */
        _player = vikingsSpawner.PlayerInstantiate();
        
        /* 1 - PreStart - Wait start button clicked */
        uiManager.ShowStartPanel();
        await UniTask.WaitWhile(() => _isStarted == false);
        
        /* 1 - PreStart - Ship moving to start position */
        uiManager.DisableCurrentPanel();
        await ship.StartMove();
        
        /* 1 - PreStart - Vikings moving to start position */
        await vikingsSpawner.MoveToPoints(fightPointsSpawner);
        
        /* 1 - PreStart - Wait until player moved */
        uiManager.ShowGamePanel();
        _player.ActivateMoving();
        await UniTask.WaitWhile(() => _player.IsMoving == false);
        
        /* 2 - Main logic - Vikings moving to church, rewards and back to ship */
        await vikingsSpawner.MoveToPoints(defendPointsSpawner);
        await vikingsSpawner.MoveToChurch();
        await vikingsSpawner.MoveToRewards();
        await vikingsSpawner.MoveToShip(startPointsSpawner);
        
        /* 3 - Ending - Wait until player returned to ship */
        await UniTask.WaitWhile(() => _player.IsFinished == false);
        _player.DeactivateMoving();
        uiManager.DisableCurrentPanel();
        await ship.EndMove();
        
        /* 4 - Ending - Score */
        uiManager.ShowWinPanel();
    }

    public void StartGame()
    {
        _isStarted = true;
    }
}
