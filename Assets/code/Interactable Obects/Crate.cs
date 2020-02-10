using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public float _CurrentHealth;
    public float _MaxHealth;

    public GameObject _SpawnOnDeath;
    public Rigidbody2D _RigidBody2D;

    public void ApplyDamage(float _Ammount) //Apply damage to crate
    {
        if(_Ammount > 0)
        {
            _CurrentHealth -= _Ammount;
            if(_CurrentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        if(_SpawnOnDeath = null)
        {
            Instantiate(_SpawnOnDeath, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
