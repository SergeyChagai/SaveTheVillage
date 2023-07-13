using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    GameObject _soldier;
    
    void Start()
    {
        _soldier = transform.parent.gameObject      //front arm
            .transform.parent.gameObject            //body
            .transform.parent.gameObject;           //soldier
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_soldier.tag != collision.transform.parent?.transform.parent?.tag && collision.transform.name == "Head")
        {
            var enemy = collision.transform.parent.gameObject.transform.parent.GetComponent<IDamagable>();
            var soldier = _soldier.GetComponent<Soldier>();
            if (enemy.ActualHealth <= 0)
            {
                soldier.SoldierState = UnitState.Walk;
            }
            enemy.GetDamage(_soldier.GetComponent<Soldier>().Strenght);
            soldier.Strenght = 0;
        }
    }
}
