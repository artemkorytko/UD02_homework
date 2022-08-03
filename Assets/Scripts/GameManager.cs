using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Level level;
    private void Start()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        level = Instantiate(level.gameObject).GetComponent<Level>();
    }

}
