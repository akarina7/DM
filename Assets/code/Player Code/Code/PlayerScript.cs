using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private float _MoveSpeed = 5f;
    private Vector2 _Movement;

    public float _CurrentHealth = 100f;
    public float _MaxHealth = 100f;

    public Animator _Animator;
    public Rigidbody2D _RigidBody;
    public Weapon _Weapon;
    
    private float _InvincibilityDuration = 0.25f; //immune duration
    private bool _IsImmune = false;
    private bool _IsInputDisabled = false;

    public float _AmmoCount = 4.5f;
    [HideInInspector]
    public  float _MaxAmmoCount;
    public float _AmmoPerShot = 1.5f;

 /*
 * Animator values
 * float "input_x" - Horizontal movement speed
 * float "input_y" - Vertical movement speed
 * float "AimingAngle" - The angle of of where the player is aiming
 * float "Speed" - Speed magnitude 
 * trigger "Healed" - When the AI is healed
 * trigger "Damaged" - when the AI is damaged 
 * bool "IsImmune" - Is immune to damage
 * bool "IsStunned" - If the AI is stunned
 * trigger "FiredPrimary" - When the primary weapon is fired
 * trigger "MeleeAttack" - When the player does a melee attack
 */


    void Start()
    {
        ComponentDoubleCheck();
        _MaxAmmoCount = _AmmoCount;
    }

    void Update()
    {
        if(_AmmoCount < _MaxAmmoCount)
        {
            _AmmoCount += Time.deltaTime;
        }
        if(true)
        {
            _Movement.x = Input.GetAxisRaw("Horizontal");
            _Movement.y = Input.GetAxisRaw("Vertical");
            if(Input.GetButtonDown("Fire1"))
            {
                if(_AmmoCount >= _AmmoPerShot)
                {
                    _Weapon.PrimaryFire();
                    _AmmoCount -= _AmmoPerShot;
                }
            }
            if(Input.GetButtonDown("Fire2"))
            {
                _Weapon.MeleeAttack();
            }
        }
        else
        {
            _Movement.x = 0f;
            _Movement.y = 0f;
        }
        _Animator.SetFloat("Horizontal", _Movement.x);
        _Animator.SetFloat("Vertical", _Movement.y);
        //_Animator.SetFloat("Speed", _Movement.sqrMagnitude);
        if(_Movement.sqrMagnitude != 0)
        {
            _Animator.SetBool("isWalking", true);
        }
        else
        {
            _Animator.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        _RigidBody.MovePosition(_RigidBody.position + _Movement * _MoveSpeed * Time.deltaTime);
    }

    public void Heal(float _Health) //Heals the player a set ammount, will not heal above max health
    {
        if(_Health > 0 && _CurrentHealth != _MaxHealth)
        {
            _CurrentHealth += _Health;
            if(_CurrentHealth > _MaxHealth)
            {
                _CurrentHealth = _MaxHealth;
            }
            _Animator.SetTrigger("Healed");
        }
    }

    public void ApplyDamage(float _Damage) //Applies damage to character
    {
        if(_Damage > 0 && !_IsImmune) 
        {
            _CurrentHealth -= _Damage;
            if (_CurrentHealth <= 0)
            {
                Die();
            }
            _Animator.SetTrigger("Damaged");
            StartInvincibility();
        }
    }

    public void ApplyDamage(float _Damage, bool _BypassImmune) //if _BypassImmune is true, will apply damage to character even if they are immune
    {
        if(_BypassImmune)
        {
            if (_Damage > 0)
            {
                _CurrentHealth -= _Damage;
                if (_CurrentHealth <= 0)
                {
                    Die();
                }
                _Animator.SetTrigger("Damaged");
            }
        }
        else
        {
            ApplyDamage(_Damage);
        }
    }

    public void StartInvincibility() //makes character immune to damage for default time
    {
        if(_InvincibilityDuration <= 0)
        {
            return;
        }
        _IsImmune = true;
        _Animator.SetBool("IsImmune", true);
        float _Timer = _InvincibilityDuration;
        while(_Timer > 0)
        {
            _Timer -= Time.deltaTime;
        }
        _IsImmune = false;
        _Animator.SetBool("IsImmune", false);
    }

    public void StartInvincibility(float _Duration) //makes character immune to damage for given duration
    {
        if(_Duration <= 0 || _InvincibilityDuration <= 0) { return; }
        _IsImmune = true;
        _Animator.SetBool("IsImmune", true);
        float _Timer = _Duration;
        while (_Timer > 0)
        {
            _Timer -= Time.deltaTime;
        }
        _IsImmune = false;
        _Animator.SetBool("IsImmune", false);
    }

    public void MakeImmune(bool _IsImmune) //sets the immunity status of main character
    {
        this._IsImmune = _IsImmune;
        _Animator.SetBool("IsImmune", _IsImmune);
    }

    public void Teleport(Vector2 _Location) //moves character to given location
    {
        _RigidBody.MovePosition(_Location);
    }

    public void Teleport(Vector3 _Location) //moves character to given location
    {
        _RigidBody.MovePosition(_Location);
    }

    public float GetCurrentHealth() //returns current health
    {
        return _CurrentHealth;
    }

    public float GetMaxHealth() //returns max health
    {
        return _MaxHealth;
    }

    public void SetCurrentHealth(float _Ammount) //modify current health
    {

        _CurrentHealth = _Ammount;
    }

    public void SetMaxHealth(float _Ammount) //modify max health
    {
        _CurrentHealth = _Ammount;
    }

    public void DisableInput(bool _Input) //enables or disables input for character
    {
        _IsInputDisabled = _Input;
        _Animator.SetBool("IsInputDisabled", _Input);
    }

    public void CheckIfDead() //Function that checks if the health is below zero
    {
        if (_CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die() //sudo death script
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    void ComponentDoubleCheck() //assigns components if they were not added
    {
        _Animator = (_Animator == null) ? gameObject.GetComponent<Animator>() : _Animator;
        _RigidBody = (_RigidBody == null) ? gameObject.GetComponent<Rigidbody2D>() : _RigidBody;
        _Weapon = (_Weapon == null) ? gameObject.GetComponent<Weapon>() : _Weapon;
    }

}
