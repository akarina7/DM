using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    private GameObject _Player;
    private float _MoveSpeed = 6f;
    private float _Damage = 20;
    public GameObject _SpawnOnDeath;
    public CircleCollider2D _Collider;
    private Vector2 _Target;
    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        if(_Player == null)
        {
            Destroy(gameObject);
        }
        _Target = _Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _Target, _MoveSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, _Target) <= .1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D _Collision)
    {
        if(!_Collision.gameObject.CompareTag("Enemy"))
        {
            _Collision.gameObject.SendMessage("ApplyDamage", _Damage);
        }
        else if (_Collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }
        Die();
    }

    private void Die()
    {
        if(_SpawnOnDeath != null)
        {
            Instantiate(_SpawnOnDeath, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
