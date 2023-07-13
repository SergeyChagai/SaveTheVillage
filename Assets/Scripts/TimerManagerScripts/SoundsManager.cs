using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioSource EagleAudioSource;

    private System.Random _random;

    private Timer _timer;

    void Start()
    {
        _random = new System.Random();
        _timer = StaticObjects.Timer;
        _timer.EagleMustCryNow += OnEagleMustCryNow;
    }


    private void OnEagleMustCryNow()
    {
        var loudRange = _random.Next(20, 100);
        EagleAudioSource.volume = (float) loudRange / 100;
        EagleAudioSource.Play();
    }

    void Update()
    {
        
    }
}
