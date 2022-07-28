using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Level _level;
    private void Awake()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        _level = Instantiate(_level.gameObject).GetComponent<Level>();
    }

}
