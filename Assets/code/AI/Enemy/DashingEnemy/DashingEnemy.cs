using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : MonoBehaviour
{
    public AiState _State;
    public PatrolType _PatrolType;
    public GameObject WinMessage;

    //Stats
    private float _MoveSpeed = 2f;
    private float _DashSpeed = 7f;
    private float _CurrentHealth = 600f;
    private float _MaxHealth = 600f;
    private float _AggroRange = 15f;
    private float _AttackFrequency = 1f;
    [HideInInspector]
    public float _CurrentAttackFrequency;
    private float _AttackRange = 20f;
    private float _AttackDamage = 35f;
    private float _AttackCounter = 0f; //Counts time between attacks
    private float _StunResistance = 0.5f;

    //Wander & Patrol
    private Vector2 _InitalLocation;
    private float _WaitTime = 2; //time that the AI waits when reaching a spot

    //Wander
    public Vector2 _WanderRange;
    private Vector2 _RandomWanderSpot;

    //Patrol waypoints
    public Transform[] _MoveSpots;
    private int _CurrentSpot = 0; //variable to store the random spot to patrol
    private int _NumOfSpots;

    //Components
    public Animator _Animator;
    public Rigidbody2D _RigidBody;
    public GameObject _SpawnOnDeath;

    //Utility
    private readonly float _InvincibilityDuration = 0.25f; //immune duration
    private bool _IsImmune = false;
    private GameObject _Target;
    private float _StunCounter = 0;

    //movement
    private float _MoveDirection = 0f;

    //dashing
    private bool _IsDashing = false;
    private Vector2 _TargetLocation;

    void Start()
    {
        _CurrentAttackFrequency = _AttackFrequency;
        ComponentDoubleCheck();
        _InitalLocation = transform.position;
        _RandomWanderSpot = new Vector2(Random.Range(-_WanderRange.x, _WanderRange.x) + _InitalLocation.x, Random.Range(-_WanderRange.y, _InitalLocation.y) + _InitalLocation.y);
        if (_State == AiState.Patroling)
        {
            if (_MoveSpots != null) { _NumOfSpots = _MoveSpots.Length; }
            if (_NumOfSpots <= 0) { _PatrolType = PatrolType.Idle; }
        }
    }

    void Update()
    {
        if (_RigidBody.velocity.sqrMagnitude > 0)
        {
            _RigidBody.velocity = new Vector3(0, 0, 0);
        }
        if (_CurrentHealth / _MaxHealth < .25f)
        {
            _CurrentAttackFrequency = 0.25f * _AttackFrequency;
        }
        else if (_CurrentHealth / _MaxHealth < .5f)
        {
            _CurrentAttackFrequency = 0.5f * _AttackFrequency;
        }
        else if (_CurrentHealth / _MaxHealth < .75f)
        {
            _CurrentAttackFrequency = 0.75f * _AttackFrequency;
        }

        _Animator.SetFloat("MoveDirection", _MoveDirection);
        if (_StunCounter <= 0)
        {
            _Animator.SetBool("IsStunned", false);
            switch (_State)
            {
                case AiState.Idle:
                    {
                        if (CheckAggro()) { break; }
                        _MoveDirection = 0;
                        break;
                    }
                case AiState.Patroling:
                    {
                        if (CheckAggro()) { break; }
                        switch (_PatrolType)
                        {
                            case PatrolType.InOderPatrol:
                                {
                                    transform.position = Vector2.MoveTowards(transform.position, _MoveSpots[_CurrentSpot].position, _MoveSpeed * Time.deltaTime);
                                    _MoveDirection = (transform.position.x < _MoveSpots[_CurrentSpot].position.x) ? 1 : -1;
                                    if (Vector2.Distance(transform.position, _MoveSpots[_CurrentSpot].position) < .2f)
                                    {
                                        _CurrentSpot = (_CurrentSpot >= _MoveSpots.Length - 1 ? 0 : _CurrentSpot + 1);
                                        WaitAtCheckPoint();
                                    }
                                    break;
                                }
                            case PatrolType.RandomPatrol:
                                {
                                    transform.position = Vector2.MoveTowards(transform.position, _MoveSpots[_CurrentSpot].position, _MoveSpeed * Time.deltaTime);
                                    _MoveDirection = (transform.position.x < _MoveSpots[_CurrentSpot].position.x) ? 1 : -1;
                                    if (Vector2.Distance(transform.position, _MoveSpots[_CurrentSpot].position) < .2f)
                                    {
                                        _CurrentSpot = Random.Range(0, _NumOfSpots);
                                        WaitAtCheckPoint();
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                case AiState.Wandering:
                    {
                        if (CheckAggro()) { break; }
                        transform.position = Vector2.MoveTowards(transform.position, _RandomWanderSpot, _MoveSpeed * Time.deltaTime);
                        _MoveDirection = (transform.position.x < _RandomWanderSpot.x) ? 1 : -1;
                        if (Vector2.Distance(transform.position, _RandomWanderSpot) < .2f)
                        {
                            _RandomWanderSpot = new Vector2(Random.Range(-_WanderRange.x, _WanderRange.x) + _InitalLocation.x, Random.Range(-_WanderRange.y, _InitalLocation.y) + _InitalLocation.y);
                            WaitAtCheckPoint();
                        }
                        break;
                    }
                case AiState.Aggressive:
                    {
                        if (CheckIfTargetLost()) { break; }
                        transform.position = Vector2.MoveTowards(transform.position, _Target.transform.position, _MoveSpeed * Time.deltaTime);
                        _MoveDirection = (transform.position.x < _Target.transform.position.x) ? 1 : -1;
                        if (Vector2.Distance(transform.position, _Target.transform.position) < _AttackRange)
                        {
                            if(_IsDashing)
                            {
                                transform.position = Vector2.MoveTowards(transform.position, _TargetLocation, _DashSpeed * Time.deltaTime);
                                if(Vector2.Distance(transform.position, _TargetLocation) < .1f)
                                {
                                    _AttackCounter = _CurrentAttackFrequency;
                                    _IsDashing = false;
                                    _Animator.SetBool("IsDashing", false);
                                }
                            }
                            else if (_AttackCounter <= 0)
                            {
                                _Animator.SetBool("IsDashing", true);
                                _TargetLocation = _Target.transform.position;
                                _IsDashing = true;
                            }
                            else
                            {
                                _AttackCounter -= Time.deltaTime;
                            }

                        }
                        break;
                    }
                default:
                    {
                        SwitchToBaseState();
                        break;
                    }
            }
        }
        else
        {
            _StunCounter -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D _Collision)
    {
        if(!_Collision.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log(_Collision.contactCount);
            Collider2D[] _Results = new Collider2D[_Collision.contactCount];
            Physics2D.OverlapCollider(gameObject.GetComponent<Collider2D>(), new ContactFilter2D(), _Results);
            foreach (Collider2D _Collisions in _Results)
            {
                try
                {
                    //Debug.Log(_Collisions.gameObject.name);
                    if (!_Collisions.CompareTag("Projectile"))
                    {
                        _Collisions.gameObject.SendMessage("ApplyDamage", _AttackDamage);
                    }
                }
                catch
                {
                    //heck
                }
            }
            //_Collision.gameObject.SendMessage("ApplyDamage", _AttackDamage);
        }
    }

    private bool Attack()
    {
        if (_Target == null) { return false; }
        _Animator.SetBool("IsDashing", true);
        _TargetLocation = _Target.transform.position;
        while(Vector2.Distance(transform.position, _TargetLocation) > .1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _TargetLocation, _DashSpeed * Time.deltaTime);
        }
        _Animator.SetBool("IsDashing", false);
        return true;
    }

    private void UpdateDashFreq() //makes the boss dash for frequently the less health he has
    {
        if(_CurrentHealth / _MaxHealth < .25f)
        {
            _CurrentAttackFrequency = 0.25f * _AttackFrequency;
        }
        else if (_CurrentHealth / _MaxHealth < .5f)
        {
            _CurrentAttackFrequency = 0.5f * _AttackFrequency;
        }
        else if (_CurrentHealth / _MaxHealth < .75f)
        {
            _CurrentAttackFrequency = 0.75f * _AttackFrequency;
        }
    }

    public void ChangeAiState(AiState _State) //changes the State of the Ai
    {
        _Animator.SetBool(this._State.ToString(), false);
        _Animator.SetBool(_State.ToString(), true);
        this._State = _State;
    }

    private bool CheckIfTargetLost() //Resets the AI to base state if target is lost
    {
        if (_Target == null)
        {
            SwitchToBaseState();
            return true;
        }
        return false;
    }

    public void SwitchToBaseState() //switches AI to its default patrol state
    {
        switch (_PatrolType)
        {
            case PatrolType.Idle:
                {
                    ChangeAiState(AiState.Idle);
                    break;
                }
            case PatrolType.Wander:
                {
                    ChangeAiState(AiState.Wandering);
                    break;
                }
            case PatrolType.InOderPatrol:
                {
                    ChangeAiState(AiState.Patroling);
                    break;
                }
            case PatrolType.RandomPatrol:
                {
                    ChangeAiState(AiState.Patroling);
                    break;
                }
            default:
                {
                    Debug.Log("<color = cyan>ERROR: No base patrol state set for MeleeEnemy</color>");
                    break;
                }
        }
    }

    private bool CheckAggro() //checks if the player is within aggro range, if the player is, switches AI to aggressive state
    {
        _Target = (_Target == null) ? GameObject.FindGameObjectWithTag("Player") : _Target;
        if (_Target != null && Vector2.Distance(transform.position, _Target.transform.position) <= _AggroRange)
        {
            ChangeAiState(AiState.Aggressive);
            return true;
        }
        return false;
    }

    private void WaitAtCheckPoint() //makes the Ai wait at a check for the given time
    {
        if (_State == AiState.Aggressive) { return; }
        ChangeAiState(AiState.Idle);
        StartCoroutine(Wait(_WaitTime));
    }

    public void Heal(float _Health) //Heals the Ai a set ammount, will not heal above max health
    {
        if (_Health > 0 && _CurrentHealth != _MaxHealth)
        {
            _CurrentHealth += _Health;
            if (_CurrentHealth > _MaxHealth)
            {
                _CurrentHealth = _MaxHealth;
            }
            _Animator.SetTrigger("Healed");
        }
    }

    public void ApplyDamage(float _Damage) //Applies damage to Ai
    {
        if (_Damage > 0 && !_IsImmune)
        {
            UpdateDashFreq();
            _CurrentHealth -= _Damage;
            if (_CurrentHealth <= 0)
            {
                Die();
            }
            _Animator.SetTrigger("Damaged");
            StartInvincibility();
        }
    }
    
    

    public void ApplyDamage(float _Damage, bool _BypassImmune) //if _BypassImmune is true, will apply damage to Ai even if they are immune
    {
        if (_BypassImmune)
        {
            if (_Damage > 0)
            {
                _CurrentHealth -= _Damage;
                UpdateDashFreq();
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

    public void ApplyStun(float _Duration) //stuns the AI for a set duration
    {
        if (_Duration >= 0)
        {
            _Animator.SetBool("IsStunned", true);
            _StunCounter = _Duration * (_IsDashing ? 0 : _StunResistance);
        }
    }

    public void StartInvincibility() //makes Ai immune to damage for default time
    {
        if (_InvincibilityDuration <= 0) { return; }
        _IsImmune = true;
        _Animator.SetBool("IsImmune", true);
        float _Timer = _InvincibilityDuration;
        while (_Timer > 0)
        {
            _Timer -= Time.deltaTime;
        }
        _IsImmune = false;
        _Animator.SetBool("IsImmune", false);
    }

    public void StartInvincibility(float _Duration) //makes Ai immune to damage for given duration
    {
        if (_Duration <= 0 || _InvincibilityDuration <= 0) { return; }
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

    public void MakeImmune(bool _IsImmune) //sets the immunity status of Ai
    {
        this._IsImmune = _IsImmune;
        _Animator.SetBool("IsImmune", _IsImmune);
    }

    public void Teleport(Vector2 _Location) //moves Ai to given location
    {
        _RigidBody.MovePosition(_Location);
    }

    public void Teleport(Vector3 _Location) //moves Ai to given location
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

    public void CheckIfDead() //Function that checks if the health is below zero
    {
        if (_CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die() //destroys the object
    {
        if (_SpawnOnDeath != null)
        {
            Instantiate(_SpawnOnDeath, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        WinMessage.SetActive(true);
    }
    
    void ComponentDoubleCheck() //assigns components if they were not added
    {
        _Animator = (_Animator == null) ? gameObject.GetComponent<Animator>() : _Animator;
        _RigidBody = (_RigidBody == null) ? gameObject.GetComponent<Rigidbody2D>() : _RigidBody;
        _Target = (_Target == null) ? GameObject.FindGameObjectWithTag("Player") : _Target;
    }

    IEnumerator Wait(float _Time)
    {
        yield return new WaitForSeconds(_Time);
        SwitchToBaseState();
    }
}

