using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private Image _backgroundImage;
    private Image _foregroundImage;
    private Timer _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = InitialFactory.Timer;
        var components = Component.FindObjectsOfType<Image>().ToList();
        _backgroundImage = components.Find(o => o.name == "BackgroundImage");
        _foregroundImage = components.Find(o => o.name == "ForegroundImage");
    }

    private void Update()
    {
        var tmp = _timer.TimerTime;
        _foregroundImage.fillAmount = tmp / _timer.TimeRange;
    }

    public void StartClock()
    {
        _foregroundImage.fillAmount = _timer.TimerTime % _timer.TimeRange;
    }
}
