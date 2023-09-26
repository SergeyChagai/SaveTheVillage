using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Soldier : MonoBehaviour, IDamagable, IComparable
{
    public float Speed;
    public float Strenght;
    public float TotalHealth;
    public int Row;
    public bool HasEnemy
    {
        get => _enemy != null;
    }

    private float _actualHealth;
    public float ActualHealth
    {
        get => _actualHealth;
        set
        {
            _actualHealth = value;
            if (value <= 0)
            {
                Die();
            }
        }
    }

    public Role Role;

    private UnitState _soldierState;
    public UnitState SoldierState
    {
        get => _soldierState;
        set
        {
            _soldierState = value;
            _animator?.SetInteger("State", (int)value);
        }
    }

    public event Action Death;

    private GameObject _enemy;
    private BoxCollider2D _bodyCollider;
    private BoxCollider2D[] _colliders;
    private Animator _animator;
    private Vector3 _startPosition;

    private void Awake()
    {
        //Role = Role.Soldier;
        //var weapon = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<BoxCollider2D>();
        SoldierState = _soldierState;
        _actualHealth = TotalHealth;
        _startPosition = transform.position;
        _animator = GetComponent<Animator>();
        _colliders = GetComponentsInChildren<BoxCollider2D>();
        _bodyCollider = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        if (SoldierState == UnitState.Walk)
        {
            var direction = new Vector2(Speed * Time.deltaTime, 0);
            transform.Translate(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject != null
            && collision.transform.name != "Head" && collision.transform.name != "Weapon"
            && collision.gameObject.tag != tag && SoldierState != UnitState.Attack
            && !HasEnemy && !collision.gameObject.GetComponent<IDamagable>().HasEnemy)
        {
            SetEnemy(collision.gameObject);
            collision.gameObject.GetComponent<Soldier>()?.SetEnemy(transform.gameObject);
        }
    }

    public void Die()
    {
        _bodyCollider.enabled = false;
        foreach (var coll in _colliders)
        {
            coll.enabled = false;
        }
        SoldierState = UnitState.Die;
        UnsetEnemy();
    }

    public void GetDamage(float rangeOfDamage)
    {
        ActualHealth -= rangeOfDamage;
    }

    // Activated as animation event on Attack
    public void ReloadStrength()
    {
        Strenght = 1;
    }

    // Activated as animation event on Die
    public void RestartAfterDie()
    {
        SoldierState = UnitState.Idle;
        Restart();
        transform.gameObject.SetActive(false);
        Death?.Invoke();
    }

    public void Restart()
    {
        if (!isActiveAndEnabled) return;

        _bodyCollider.enabled = true;
        foreach (var coll in _colliders)
        {
            coll.enabled = true;
        }
        SoldierState = UnitState.Idle;
        Strenght = 0;
        transform.position = _startPosition;
    }

    public bool CheckTheEnemy(GameObject potentialEnemy)
    {
        return _enemy == potentialEnemy;
    }

    private void SetEnemy(GameObject enemy)
    {
        if (_enemy != enemy)
        {
            _enemy = enemy;
            _enemy.GetComponent<IDamagable>().Death += OnEnemyDeath;
            SoldierState = UnitState.Attack;
        }
    }

    private void UnsetEnemy()
    {
        _enemy.GetComponent<IDamagable>().Death -= OnEnemyDeath;
        _enemy = null;
    }

    private void OnEnemyDeath()
    {
        if (_enemy.GetComponent<IDamagable>() is Soldier && ActualHealth > 0)
        {
            _enemy.GetComponent<IDamagable>().Death -= OnEnemyDeath;
            Die();
            return;
        }
        _enemy = null;
        SoldierState = UnitState.Walk;
    }

    public int CompareTo(object obj)
    {
        if (obj == null || !(obj is Soldier)) return -1;
        return Row.CompareTo(((Soldier)obj).Row);
    }
}

public enum UnitState
{
    Idle,
    Walk,
    Casting,
    Attack,
    Hurt,
    Die,
    Victory
}
