using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializator : MonoBehaviour
{
    private Timer _timer;
    void Awake()
    {
        StaticObjects.GetEntities();
        _timer = StaticObjects.Timer;
        _timer.StartTimer();
    }
}
