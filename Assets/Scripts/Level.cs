using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using System.Threading;
using System.Linq;
using Cysharp.Threading.Tasks;


[RequireComponent(typeof(FightPoint))]
public class Level : MonoBehaviour
{
    [SerializeField] List<GameObject> vikings;
    [SerializeField] List<FightPoint> fightPoints;
    [SerializeField] List<GameObject> churchPoints;
    [SerializeField] GameObject vikingSpawn;
    [SerializeField] GameObject gates;
    private List<GameObject> _currentVikings = new List<GameObject>();
    private Viking _currentViking;
    private Queue<GameObject> _currentVikingsQueue = new Queue<GameObject>();
    private readonly Stack<Viking> _currentVikingsStack = new Stack<Viking>();
    private readonly Dictionary<String, string> _rewardedVikings = new Dictionary<String, string>();
    private FightPoint _currentFightPoint;
    private Coroutine _corut;
    private bool _corutGatesStarted = true;
    private bool _corutFightPointsStarted = true;
    private bool _corutChurchStarted = true;
    private bool _corutSpawnStarted = true;
    private int _waitTime = 2000;
    private int GatesMethodCounter = 0;



    private void Start()
    {
        InitializeViking();
        _currentViking.MoveToPoint(_currentFightPoint.gameObject.transform.position);
    }


    private void Update()
    {
        if (_currentViking.OnFightPoint)
        {
            if (fightPoints.Count > 0)
            {
                InitializeViking();
                _currentViking.MoveToPoint(_currentFightPoint.gameObject.transform.position);
            }
            else if (_corutGatesStarted == true)  //����� ��������� ������ ���� ���
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

    public void InitializeViking()
    {
        int vikingIndex = UnityEngine.Random.Range(0, vikings.Count);
        _currentViking = Instantiate(vikings[vikingIndex]).GetComponentInChildren<Viking>();
        _currentVikings.Add(_currentViking.gameObject);
        _currentViking.transform.parent.position = vikingSpawn.transform.position;
        int fightPointIndex = UnityEngine.Random.Range(0, fightPoints.Count);
        _currentFightPoint = fightPoints[fightPointIndex];
        fightPoints.Remove(_currentFightPoint);
    }


    private async void  MoveVikingsToChurchPoints()
    {
        churchPoints = (from churchPoint in churchPoints orderby Vector3.Distance(churchPoint.transform.position, gates.transform.position) descending select churchPoint).ToList();
        for (int i = 0; i < _currentVikings.Count; i++)
        {
            _currentViking = _currentVikings[i].GetComponent<Viking>();
            _currentViking.MoveToPoint(churchPoints[i].transform.position);
            _currentVikingsStack.Push(_currentViking);
            if (_rewardedVikings.ContainsKey(_currentViking.name))
            {
                _rewardedVikings.Add(_currentViking.name+i, churchPoints[i].GetComponent<ChurchPoint>().Name);
            }
            else
            {
                _rewardedVikings.Add(_currentViking.name, churchPoints[i].GetComponent<ChurchPoint>().Name);
            }
            await UniTask.Delay(_waitTime);
            Destroy(churchPoints[i]);
            Instantiate(churchPoints[i], _currentViking.transform);
            churchPoints[i].transform.localPosition = new Vector3(0, 5, 0);
        }
    }


    private async void MoveVikingsToBoat()
    {
        MoveVikingsToGates();
        await UniTask.Delay(_waitTime);
    }


    private async void MoveVikingsToGates()
    {
        _currentVikings = (from viking in _currentVikings orderby (Vector3.Distance(viking.transform.position, gates.transform.position)) select viking).ToList();

        var sequence = DOTween.Sequence();
        for (int i = 0; i < _currentVikings.Count; i++)
        {
            _currentViking = _currentVikings[i].GetComponent<Viking>();
            _currentViking.MoveToPoint(gates.transform.position);
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
            _currentVikingStack.MoveToPoint(vikingSpawn.transform.position);
            await UniTask.Delay(_waitTime);
            print("���� �������");
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