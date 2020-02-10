using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float _HealingAmmount;
    public BoxCollider2D _BoxCollider2D;

    void Start()
    {
        _BoxCollider2D = (_BoxCollider2D == null ? gameObject.GetComponent<BoxCollider2D>() : _BoxCollider2D);
    }

    private void OnTriggerEnter2D(Collider2D _Collision)
    {
        if(_Collision.CompareTag("Player") && _HealingAmmount > 0)
        {
            _Collision.BroadcastMessage("Heal", _HealingAmmount);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

