using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    public float TimeRange;

    // Time booster for tests
    #region Multiplier
#if DEBUG
    [SerializeField]
    private int _multiplier;
#else
    private int _multiplier = 1;
#endif
    #endregion

    // Time for display
    #region TimerTime
    private float _timerTime;
    public float TimerTime
    {
        get => _timerTime;
        set
        {
            _timerTime = value % TimeRange;
        }
    }
    #endregion

    private float _startTime;
    private float _passedTime;
    private TimerState _state;


    private void Start()
    {

    }

    void Update()
    {
        if (_state == TimerState.Run)
        {
            _passedTime = GetTime() - _startTime;
            TimerTime = _passedTime;
        }
    }


    public void StartTimer()
    {
        _startTime = GetTime();
        _state = TimerState.Run;
    }

    public void StopTimer()
    {
        _state = TimerState.Stop;
        ClearFields();
    }


    private float GetTime() => Time.time * _multiplier;

    private void ClearFields()
    {
        _startTime = 0;
    }
}
public enum TimerState : short
{
    Stop,
    Run
}
