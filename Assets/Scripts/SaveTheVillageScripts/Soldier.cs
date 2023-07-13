using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Soldier : MonoBehaviour, IDamagable
{
    public float Speed;
    public float Strenght;
    public float TotalHealth;
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
        Role = Role.Soldier;
        var weapon = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<BoxCollider2D>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        _colliders = GetComponentsInChildren<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        SoldierState = _soldierState;
        _actualHealth = TotalHealth;
        _startPosition = transform.position;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (SoldierState == UnitState.Walk)
        {
            var direction = new Vector2(Speed * Time.deltaTime * (Role == Role.Soldier ? 1 : -1), 0);
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
            DoUnderAttack();
            collision.gameObject.GetComponent<Soldier>()?.SetEnemy(collision.gameObject);
            collision.gameObject.GetComponent<IDamagable>().DoUnderAttack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject != null)
        {
            ;
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
        Death?.Invoke();
        //transform.gameObject.SetActive(false);
    }

    public void GetDamage(float rangeOfDamage)
    {
        ActualHealth -= rangeOfDamage;
    }

    public void ReloadStrength()
    {
        Strenght = 1;
    }

    private void SetEnemy(GameObject enemy)
    {
        if (_enemy != enemy)
        {
            _enemy = enemy;
            _enemy.GetComponent<IDamagable>().Death += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        if (_enemy.GetComponent<IDamagable>() is Soldier && ActualHealth > 0)
        {
            Die();
            return;
        }
        _enemy = null;
        SoldierState = UnitState.Walk;
    }

    public void Restart()
    {
        _bodyCollider.enabled = true;
        foreach (var coll in _colliders)
        {
            coll.enabled = true;
        }
        SoldierState = UnitState.Idle;
        transform.position = _startPosition;
        transform.gameObject.SetActive(true);
    }

    public void DoUnderAttack()
    {
        SoldierState = UnitState.Attack;
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
