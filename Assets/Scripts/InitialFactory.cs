using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitialFactory
{
    public static Timer Timer { get; private set; }

    public static void GetEntities()
    {
        Timer = GameObject.FindObjectOfType<Timer>();
    }
}
