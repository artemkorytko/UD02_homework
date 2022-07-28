using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading;
using System.Linq;
using Cysharp.Threading.Tasks;


public class Level : MonoBehaviour
{
    [SerializeField] List<GameObject> _vikings;
    [SerializeField] List<FightPoint> _fightPoints;
    [SerializeField] List<GameObject> _churchPoints;
    [SerializeField] GameObject _vikingSpawn;
    [SerializeField] GameObject _gates;
    private List<GameObject> _currentVikings = new List<GameObject>();
    private Viking _currentViking;
    private Queue<GameObject> _currentVikingsQueue = new Queue<GameObject>();
    private Stack<Viking> _currentVikingsStack = new Stack<Viking>();
    private Dictionary<String, string> _rewardedVikings = new Dictionary<String, string>();
    private FightPoint _currentFightPoint;
    private Coroutine _corut;
    private bool _corutGatesStarted = true;
    private bool _corutFightPointsStarted = true;
    private bool _corutChurchStarted = true;
    private bool _corutSpawnStarted = true;
    private int _waitTime = 2000;
    private int GatesMethodCounter = 0;



    private void Awake()
    {
        SpawnOneVikingAndMatchPoint();
        _currentViking.MoveToPoint(_currentFightPoint.gameObject.transform.position);
    }


    private void Update()
    {
        if (_currentViking.OnFightPoint)
        {
            if (_fightPoints.Count > 0)
            {
                SpawnOneVikingAndMatchPoint();
                _currentViking.MoveToPoint(_currentFightPoint.gameObject.transform.position);
            }
            else if (_corutGatesStarted == true)  //чтобы запустить только один раз
            {
                MoveVikingsToGates();
                _corutGatesStarted = false;
            }
        }
        else if ((_currentVikings.Count == 5) && CheckVikingsOnGatesLocation() && _corutChurchStarted == true)
        {
            MoveVikingsToChurchPoints();
            _corutChurchStarted = false;
        }
        if (CheckVikingsReward() && _corutSpawnStarted == true)
        {
            MoveVikingsToBoat();            
            _corutSpawnStarted=false;
            ShowAllInfo();
        }
        if (GatesMethodCounter == 2)
        {
            MoveVikingsToSpawn();
        }
    }


    private bool CheckVikingsOnGatesLocation()
    {
        bool indicator = true;
        Viking checkCurrentViking;
        foreach (var viking in _currentVikings)
        {
            checkCurrentViking = viking.GetComponent<Viking>();
            if (checkCurrentViking.OnGates == false)
            {
                indicator = false;
            }
        }
        return indicator;
    }


    private bool CheckVikingsReward()
    {
        bool indicator = true;
        Viking checkCurrentViking;
        foreach (var viking in _currentVikings)
        {
            checkCurrentViking = viking.GetComponent<Viking>();
            if (checkCurrentViking.OnChurchPoint == false)
            {
                indicator = false;
            }
        }
        return indicator;
    }

    int vikingTitleIndex = 0;
    public void SpawnOneVikingAndMatchPoint()
    {
        int _vikingIndex = UnityEngine.Random.Range(0, _vikings.Count);
        _currentViking = Instantiate(_vikings[_vikingIndex]).GetComponentInChildren<Viking>();
        switch (_vikingIndex)
        {
            case 0:
                {
                    _currentViking.Name = "Brown viking";
                    break;
                }
            case 1:
                {
                    _currentViking.Name = "Black viking";
                    break ;
                }
            case 2:
                {
                    _currentViking.Name = "Green viking";
                    break;
                }
        }
        _currentVikings.Add(_currentViking.gameObject);
        _currentViking.transform.parent.position = _vikingSpawn.transform.position;
        int _fightPointIndex = UnityEngine.Random.Range(0, _fightPoints.Count);
        _currentFightPoint = _fightPoints[_fightPointIndex];
        _fightPoints.Remove(_currentFightPoint);
    }


    private async void  MoveVikingsToChurchPoints()
    {
        _churchPoints = (from churchPoint in _churchPoints orderby Vector3.Distance(churchPoint.transform.position, _gates.transform.position) descending select churchPoint).ToList();
        for (int i = 0; i < _currentVikings.Count; i++)
        {
            switch (i)
            {
                case 0:
                    {
                        _churchPoints[i].GetComponent<ChurchPoint>().Name = "Blue treasure";
                        break;
                    }
                case 1:
                    {
                        _churchPoints[i].GetComponent<ChurchPoint>().Name = "Green treasure";
                        break;
                    }
                case 2:
                    {
                        _churchPoints[i].GetComponent<ChurchPoint>().Name = "Yellow treasure";
                        break;
                    }
                case 3:
                    {
                        _churchPoints[i].GetComponent<ChurchPoint>().Name = "Orange treasure";
                        break;
                    }
                case 4:
                    {
                        _churchPoints[i].GetComponent<ChurchPoint>().Name = "Red treasure";
                        break;
                    }
            }
            _currentViking = _currentVikings[i].GetComponent<Viking>();
            _currentViking.MoveToPoint(_churchPoints[i].transform.position);
            _currentVikingsStack.Push(_currentViking);
            if (_rewardedVikings.ContainsKey(_currentViking.Name) == false)
            {
                _rewardedVikings.Add(_currentViking.Name, _churchPoints[i].GetComponent<ChurchPoint>().Name);
            }
            else
            {
                _rewardedVikings.Add(_currentViking.Name + i.ToString(), _churchPoints[i].GetComponent<ChurchPoint>().Name);
            }
            await UniTask.Delay(_waitTime);
        }
       
    }


    private async void MoveVikingsToBoat()
    {
        MoveVikingsToGates();
        await UniTask.Delay(_waitTime);
    }


    private async void MoveVikingsToGates()
    {

        _currentVikings = (from viking in _currentVikings orderby (Vector3.Distance(viking.transform.position, _gates.transform.position)) select viking).ToList();

        var sequence = DOTween.Sequence();
        for (int i = 0; i < _currentVikings.Count; i++)
        {
            _currentViking = _currentVikings[i].GetComponent<Viking>();
            _currentViking.MoveToPoint(_gates.transform.position);
            await UniTask.Delay(_waitTime);
        }
        GatesMethodCounter++;
    }


    private async void MoveVikingsToSpawn()
    {
        Viking _currentVikingStack;
        for (int i = 0; i < _currentVikingsStack.Count; i++)
        {
            _currentVikingStack = _currentVikingsStack.Pop();
            _currentVikingStack.MoveToPoint(_vikingSpawn.transform.position);
            await UniTask.Delay(_waitTime);
            
        }
    }


    private async void ShowAllInfo()
    {
        foreach (var item in _rewardedVikings)
        {
            print(item.Key + " - " + item.Value);
            await UniTask.Delay(_waitTime);
        }
    }
}



//await 1
//    2