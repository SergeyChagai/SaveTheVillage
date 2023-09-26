using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : MonoBehaviour, IDamagable, IComparable
{
    public float TotalHealth;
    public int Row;

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
    public bool HasEnemy { get => _enemy != null; }
    public event Action Death;

    private UnitState _peasantState;
    public UnitState PeasantState
    {
        get => _peasantState;
        set
        {
            _peasantState = value;
            _animator?.SetInteger("State", (int)value);
        }
    }


    private const string PeasantTag = "Peasant";
    private GameObject _enemy;
    private Animator _animator;
    private BoxCollider2D _bodyCollider;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        PeasantState = _peasantState;
        _actualHealth = TotalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject != null && collision.gameObject.tag != PeasantTag) {
        
        }
    }

    public void GetDamage(float rangeOfDamage)
    {
        ActualHealth -= rangeOfDamage;
    }

    public void Die()
    {
        _bodyCollider.enabled = false;
        PeasantState = UnitState.Die;
        Death?.Invoke();
        transform.gameObject.SetActive(false);
    }

    public void RestartAfterDie()
    {
        _bodyCollider.enabled = true;
        PeasantState = UnitState.Idle;
        transform.gameObject.SetActive(true);
    }

    public void SetAttackState()
    {
        PeasantState = UnitState.Hurt;
    }

    public int CompareTo(object obj)
    {
        if (obj == null || !(obj is Peasant)) return -1;
        return Row.CompareTo(((Peasant)obj).Row);
    }
}
