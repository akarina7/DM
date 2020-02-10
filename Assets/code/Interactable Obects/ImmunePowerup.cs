using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunePowerup : MonoBehaviour
{
    public float _ImmuneDuration;
    public BoxCollider2D _BoxCollider2D;

    void Start()
    {
        _BoxCollider2D = (_BoxCollider2D == null ? gameObject.GetComponent<BoxCollider2D>() : _BoxCollider2D);
    }
    private void OnTriggerEnter2D(Collider2D _Collision)
    {
        if (_Collision.CompareTag("Player") && _ImmuneDuration > 0)
        {
            _Collision.BroadcastMessage("StartInvincibility", _ImmuneDuration);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
