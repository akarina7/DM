using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private enum Type
    {
        Damage,
        Stun
    }
    [SerializeField]
    private Type _Type;
    private float _ProjectileSpeed = 15f;
    private float _Damage;
    private float _StunDuration;
    private Vector2 _MousePos;

    private Weapon _Weapon;


    private void OnCollisionEnter2D(Collision2D _Collision)
    {
        switch(_Type)
        {
            case Type.Damage:
            {
                if(!_Collision.gameObject.CompareTag("Player"))
                {
                    _Collision.transform.SendMessage("ApplyDamage", _Damage);
                }
                else if (_Collision.gameObject.CompareTag("Player"))
                {
                    return;
                }
                Die();
                break;
            }
            case Type.Stun:
            {
                if (!_Collision.gameObject.CompareTag("Player"))
                {
                    _Collision.transform.SendMessage("ApplyStun", _StunDuration);
                }
                else if (_Collision.gameObject.CompareTag("Player"))
                { 
                    return;
                }
                Die();
                break;
            }
        }
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _MousePos, _ProjectileSpeed * Time.deltaTime);
        if(Vector2.Distance(transform.position, _MousePos) <= .1)
        {
            Die();
        }
    }

    private void Start()
    {
        _Weapon = gameObject.GetComponentInParent<Weapon>();
        if (_Type == Type.Damage)
        {
            switch(_Weapon._DamageProjectileLvl)
            {
                case WeaponLevel.Lv1:
                    {
                        _Damage = 20f;
                        break;
                    }
                case WeaponLevel.Lv2:
                    {
                        _Damage = 30f;
                        break;
                    }
                case WeaponLevel.Lv3:
                    {
                        _Damage = 40f;
                        break;
                    }
                default:
                    {
                        goto case WeaponLevel.Lv1;
                    }
            }
        }
        else
        {
            switch(_Weapon._StunProjectileLvl)
            {
                case WeaponLevel.Lv1:
                    {
                        _StunDuration = 3f;
                        break;
                    }
                case WeaponLevel.Lv2:
                    {
                        _StunDuration = 4f;
                        break;
                    }
                case WeaponLevel.Lv3:
                    {
                        _StunDuration = 5f;
                        break;
                    }
                default:
                    {
                        goto case WeaponLevel.Lv1;
                    }
            }
        }
        _MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}

