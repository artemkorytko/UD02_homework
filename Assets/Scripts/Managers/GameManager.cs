using System;
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
    [SerializeField] private RewardsSpawner rewardsSpawner;
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    [SerializeField] private VikingsSpawner vikingsSpawner;
    
    [Header("Prefabs")]
    [SerializeField] private Ship ship;
    [SerializeField] private Door door;
    [SerializeField] private Player player;

    [Header("Configs")]
    [SerializeField] private int vikingsCount;
    [SerializeField] private CamerasController camerasController;
    
    private bool _isStarted;

    private void Awake()
    {
        player.OnFinish += OnPlayerFinish;
    }

    private async void Start()
    {
        /* Initialize - All points spawning */
        defendPointsSpawner.StartSpawn(vikingsCount);
        startPointsSpawner.StartSpawn(vikingsCount);
        fightPointsSpawner.StartSpawn(vikingsCount);
        
        /* Initialize - Enemies and vikings spawners setting to start points */
        enemiesSpawner.SetSpawnPoints(defendPointsSpawner);
        vikingsSpawner.SetSpawnPoints(startPointsSpawner);
        
        /* Initialize - Enemies, vikings and rewards prefabs spawning at start points */
        enemiesSpawner.StartSpawn(vikingsCount);
        vikingsSpawner.StartSpawn(vikingsCount);
        rewardsSpawner.StartSpawn(vikingsCount + 1);
        
        /* Initialize - Door rewards list setting when all rewards have been instantiated */
        door.SetRewardsList(vikingsCount);
        
        /* PreStart - Wait start button clicked */
        camerasController.SetStartCamera();
        await uiManager.ShowStartPanel();
        await UniTask.WaitWhile(() => _isStarted == false);
        
        /* PreStart - Ship moving to start position */
        camerasController.SetShipCamera();
        await uiManager.DisableCurrentPanel();
        await ship.StartMove();

        /* PreStart - Vikings moving to start position */
        await vikingsSpawner.MoveToPoints(fightPointsSpawner);
        
        /* PreStart - Wait until player start moving */
        camerasController.SetPlayerCamera();
        await uiManager.ShowGamePanel();
        player.ActivateMoving();
        await UniTask.WaitWhile(() => player.IsMoving == false);
        
        /* Main logic - Vikings moving to church, rewards and back to ship */
        await vikingsSpawner.MoveToPoints(defendPointsSpawner);
        await vikingsSpawner.MoveToChurch();
        await vikingsSpawner.MoveToRewards();
        await vikingsSpawner.MoveToShip(startPointsSpawner);
        
        /* Ending - Wait until player returned to ship */
        await UniTask.WaitWhile(() => player.IsFinished == false);
        await ship.EndMove();
        
        /* Ending - Score */
        await uiManager.ShowWinPanel(vikingsSpawner.Rewards, player.Reward);
    }
    
    private void OnDestroy()
    {
        player.OnFinish -= OnPlayerFinish;
    }

    public void StartGame()
    {
        _isStarted = true;
    }

    private async void OnPlayerFinish()
    {
        player.DeactivateMoving();
        camerasController.SetEndCamera();
        await uiManager.DisableCurrentPanel();
    }
}
