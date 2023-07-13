using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    public float TimeRange;
    public int CyclesForWave;
    public event Action IteractionChanged;
    public event Action EagleMustCryNow;
    public event Action WaveMustCome;

    private UnityEngine.UI.Image _timerImage;
    private const int EagleFrequencyTimeMinValue = 15;
    private const int EagleFrequencyTimeMaxValue = 20;
    private readonly System.Random _random = new System.Random();
    private int _cyclesCounter;

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
            _timerTime = value;
            _timerImage.fillAmount = value / TimeRange;
            if (_timerTime >= TimeRange)
            {
                IteractionChanged?.Invoke();
                _cyclesCounter++;
                if (_cyclesCounter >= CyclesForWave)
                {
                    _cyclesCounter = 0;
                    WaveMustCome?.Invoke();
                }
                ResetTimer();
            }
        }
    }
    #endregion

    private float _startTime;
    [SerializeField]
    private float _passedTime;
    private TimerState _state;

    private void Start()
    {
        _timerImage = GetComponent<UnityEngine.UI.Image>();
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

    public void ResetTimer()
    {
        _startTime = GetTime();
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
