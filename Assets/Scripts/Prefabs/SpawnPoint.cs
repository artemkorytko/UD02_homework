using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    public Transform GetPoint()
    {
        return this.gameObject.transform;
    }
}
