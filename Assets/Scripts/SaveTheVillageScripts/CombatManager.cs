using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private Timer _timer;
    private Army _army;
    private Enemies _enemies;
    private bool _isBattle;
    // Start is called before the first frame update
    void Start()
    {
        _timer = FindAnyObjectByType<Timer>();
        _timer.WaveMustCome += StartWave;
        _army = FindAnyObjectByType<Army>(FindObjectsInactive.Include);
        _enemies = FindAnyObjectByType<Enemies>(FindObjectsInactive.Include);
    }

    private void StartWave()
    {
        if (!_isBattle)
        {
            _isBattle = true;
            _army.SoldiersState = UnitState.Walk;
            _army.Attack();
            _enemies.SoldiersState = UnitState.Walk;
            _enemies.Attack();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
