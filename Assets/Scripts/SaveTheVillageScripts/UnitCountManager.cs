using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UnitCountManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text WheatInfoField;
    [SerializeField]
    private TMP_Text PeasantsInfoField;
    [SerializeField]
    private TMP_Text SoldiersInfoField;
    [SerializeField]
    private TMP_Text EnemiesCountInfoField;
    [SerializeField]
    private TMP_Text CyclesForWaveInfoField;
    [field: SerializeField, ReadOnlyField]
    public int WavesCount { get; set; }
    

    private MyGameProperties _properties;
    private int _wheatPerCycleForOnePeasant;
    private int _wheatPerCycleForOneSoldier;
    private int _wheatCount;
    private int _wheatCountForUnit;
    private int _peasantCount;
    private int _soldiersCount;
    private int _enemiesCount;
    private int[] _enemiesWaves;
    private Peasant[] _peasants;
    private Soldier[] _soldiers;
    private Soldier[] _enemies;
    private StringBuilder _infoText;
    private Timer _timer;
    private CombatManager _combatManager;

    public event Action WheatNotEnoughAction;
    public event Action LooseAction;


    // Start is called before the first frame update
    void Start()
    {
        _peasants = (Peasant[])FindObjectsByType(typeof(Peasant), FindObjectsInactive.Include, FindObjectsSortMode.None);
        var allWariors = (Soldier[])FindObjectsByType(typeof(Soldier), FindObjectsInactive.Include, FindObjectsSortMode.None);
        _soldiers = allWariors.Where(x => x.Role == Role.Soldier).ToArray();
        _enemies = allWariors.Where(x => x.Role == Role.Enemy).ToArray();
        Array.Sort(_peasants);
        Array.Sort(_enemies);
        Array.Sort(_soldiers);
        Array.Reverse(_peasants);
        Array.Reverse(_enemies);
        Array.Reverse(_soldiers);
        _infoText = new StringBuilder();

        _properties = GameObject.Find(nameof(MyGameProperties)).GetComponent<MyGameProperties>();
        _combatManager = GameObject.Find(nameof(CombatManager)).GetComponent<CombatManager>();
        var wavesQueue = (WaveDTO)JsonConvert.DeserializeObject(Resources.Load(_properties.WavesDTOFile).ToString(), typeof(WaveDTO));
        _enemiesWaves = wavesQueue.Waves;

        _wheatPerCycleForOnePeasant = _properties.WheatPerCycleForOnePeasant;
        _wheatPerCycleForOneSoldier = _properties.WheatPerCycleForOneSoldier;
        _wheatCount = _properties.StartWheatCount;
        _wheatCountForUnit = _properties.WheatForUnit;
        _timer = (Timer)FindAnyObjectByType(typeof(Timer));
        _timer.StartTimer();
        _timer.IteractionChanged += OnIteractionChanged;
        _timer.WaveMustCome += EnableEnemiesForWave;
        EnablePlayerUnit<Peasant>(_properties.StartPeasantsCount);
        UpdateInfo();
    }

    private void EnableEnemiesForWave()
    {
        AddEnemy(_enemiesWaves[WavesCount++]);
    }

    public void AddPeasant(int count = 1)
    {
        if (_wheatCount - _wheatCountForUnit * count < 0)
        {
            WheatNotEnoughAction?.Invoke();
            return;
        }
        _wheatCount -= _wheatCountForUnit * count;
        EnablePlayerUnit<Peasant>(count);
        UpdateInfo();
    }

    public void AddSoldier(int count = 1)
    {
        if (_wheatCount - _wheatCountForUnit * count < 0)
        {
            WheatNotEnoughAction?.Invoke();
            return;
        }
        _wheatCount -= _wheatCountForUnit * count;
        EnablePlayerUnit<Soldier>(count);
        UpdateInfo();
    }

    public void AddEnemy(int count = 1)
    {
        EnableEnemies(count);
        UpdateInfo();
    }

    private void DisableEnemy()
    {
        _enemiesCount--;
        if (_enemiesCount <= 0)
        {
            _timer.StartTimer();
            _combatManager.BattleFinished();
            foreach (var soldier in _soldiers)
            {
                if (soldier.isActiveAndEnabled == true)
                {
                    soldier.Restart();
                }
            }
        }
        UpdateInfo();

    }
    private void DisablePlayerUnit<T>()
    {
        if (typeof(T) == typeof(Peasant)) _peasantCount--;
        else if (typeof(T) == typeof(Soldier)) _soldiersCount--;
        else throw new ArgumentException();
        if (_soldiersCount <= 0) LooseAction?.Invoke();
        UpdateInfo();

    }

    private void OnIteractionChanged()
    {
        CalculateWheat();
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        WheatInfoField.text = _wheatCount.ToString();
        PeasantsInfoField.text = _peasantCount.ToString();
        SoldiersInfoField.text = _soldiersCount.ToString();
        EnemiesCountInfoField.text = _enemiesWaves[WavesCount].ToString();
        CyclesForWaveInfoField.text = _timer.CyclesForWave.ToString();
    }

    private void EnablePlayerUnit<T>(int count)
    {
        var array = ChooseCollection<T>();

        if (typeof(T).IsAssignableFrom(typeof(Peasant)))
            _peasantCount += count;
        else if (typeof(T).IsAssignableFrom(typeof(Soldier)))
            _soldiersCount += count;
        else
            throw new ArgumentException();
        int firstIndex = 0;

        // Find first index of inactive unit

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null && array[i].gameObject.activeInHierarchy != true)
            {
                firstIndex = i;
                break;
            }
        }

        // Activate the gameObject & subscribe UnitCountManager to the Death event

        for (int j = 0; j + firstIndex < array.Length && j <= count - 1; j++)
        {
            array[j + firstIndex].gameObject.SetActive(true);
            array[j + firstIndex].GetComponent<IDamagable>().Death += DisablePlayerUnit<T>;
        }
    }

    private MonoBehaviour[] ChooseCollection<T>()
    {
        if (typeof(T).IsAssignableFrom(typeof(Peasant)))
            return _peasants;
        else if (typeof(T).IsAssignableFrom(typeof(Soldier)))
            return _soldiers;
        else
            throw new ArgumentException();
    }

    private void EnableEnemies(int count)
    {
        _enemiesCount += count;
        int firstIndex = 0;
        for (int i = 0; i < _enemies.Length; i++)
        {
            if (_enemies[i] != null && _enemies[i].gameObject.activeInHierarchy != true)
            {
                firstIndex = i;
                break;
            }
        }
        for (int j = 0; j + firstIndex < _enemies.Length && j <= count - 1; j++)
        {
            _enemies[j + firstIndex].gameObject.SetActive(true);
            _enemies[j + firstIndex].GetComponent<Soldier>().Death += DisableEnemy;
        }
    }

    private void CalculateWheat()
    {
        _wheatCount = _wheatCount + _peasantCount * _wheatPerCycleForOnePeasant - _soldiersCount * _wheatPerCycleForOneSoldier;
        if (_wheatCount < 0)
        {
            _soldiers.FirstOrDefault(i => i.enabled).enabled = false;
            DisablePlayerUnit<Soldier>();
        }
    }
}

internal class WaveDTO
{
    public int[] Waves { get; set; }
}
