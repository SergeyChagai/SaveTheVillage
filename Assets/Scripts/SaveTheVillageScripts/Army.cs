using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Army : MonoBehaviour
{
    public float Speed;

    private Soldier[] _units;
    private UnitState _soldiersState;
    private Animator _animator;
    private bool _isBattle;
    private Vector2 _startPosition;
    public UnitState SoldiersState
    {
        get => _soldiersState;
        set
        {
            _soldiersState = value;
            foreach (Soldier soldier in _units)
            {
                soldier.SoldierState = value;
            }
        }
    }
    void Start()
    {
        var tmp = (Soldier[])FindObjectsByType(typeof(Soldier), FindObjectsInactive.Include, FindObjectsSortMode.None);
        _units = tmp.Where(i => i.Role == Role.Soldier).ToArray();
        _animator = GetComponent<Animator>();
        _startPosition = transform.position;
    }

    public void Attack() 
    {
        _isBattle = true;
    }

    public void Restart()
    {
        foreach (Soldier soldier in _units)
        {
            soldier.Restart();
        }
    }

    private void Update()
    {
       
    }
}
