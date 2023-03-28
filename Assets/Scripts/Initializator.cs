using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializator : MonoBehaviour
{
    private Timer _timer;
    void Start()
    {
        InitialFactory.GetEntities();
        _timer = InitialFactory.Timer;
        _timer.StartTimer();
    }
}
