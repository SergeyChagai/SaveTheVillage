using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public float TotalHealth { get; }
    public float ActualHealth { get; set; }
    [SerializeField]
    public Role Role { get; set; }
}

public interface ICombatable : IUnit
{
    public float Strenght { get; }
}

public enum Role
{
    Peasant,
    Soldier,
    Enemy
}
