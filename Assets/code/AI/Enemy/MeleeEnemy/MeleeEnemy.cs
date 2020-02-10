using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    //Ai mode
    //Idle - Stays put, Wandering - Patrols a set radius, Patroling - Moves inbetween set waypoints, Aggro - Aggressive state and chases target
    public AiState _State;
    public PatrolType _PatrolType;

    //Stats
    private float _MoveSpeed = 2f;
    private float _CurrentHealth = 100f;
    private float _MaxHealth = 100f;
    private float _AggroRange = 3f;
    private float _AttackFrequency = 1f;
    private float _AttackRange = 1.2f;
    private float _AttackDamage = 20f;
    private float _AttackCounter = 0; //Counts time between attacks

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

    /*
     * Animator values
     * trigger "Attacked" - When the AI Attacks
     * bool "Idle" - Is in the Idle state
     * bool "Patroling" - Is in the patroling state
     * bool "Wandering" - Is in the Wandering State
     * bool "Aggressive" - Is in the aggressive state
     * trigger "Healed" - When the AI is healed
     * trigger "Damaged" - when the AI is damaged 
     * bool "IsImmune" - Is immune to damage
     * bool "IsStunned" - If the AI is stunned
     * float "MoveDirection" - Direction of Movement (-1 for left, 1 for right, 0 for not moving)
     */


    // Start is called before the first frame update
    void Start()
    {
        ComponentDoubleCheck();
        _InitalLocation = transform.position;
        _RandomWanderSpot = new Vector2(Random.Range(-_WanderRange.x, _WanderRange.x) + _InitalLocation.x, Random.Range(-_WanderRange.y, _InitalLocation.y) + _InitalLocation.y);
        if(_State == AiState.Patroling)
        {
            if (_MoveSpots != null) { _NumOfSpots = _MoveSpots.Length; }
            if (_NumOfSpots <= 0) { _PatrolType = PatrolType.Idle; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_RigidBody.velocity.sqrMagnitude > 0)
        {
            _RigidBody.velocity = new Vector3(0, 0, 0);
        }
        _Animator.SetFloat("MoveDirection", _MoveDirection);
        if (_MoveDirection != 0) { _Animator.SetBool("Moving", true); }
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
                                    if(Vector2.Distance(transform.position, _MoveSpots[_CurrentSpot].position) < .2f)
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
                        if(Vector2.Distance(transform.position, _RandomWanderSpot) < .2f)
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
                            if(_AttackCounter <= 0)
                            {
                                if (Attack())
                                {
                                    _AttackCounter = _AttackFrequency;
                                }
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

    private bool Attack()
    {
        if(_Target == null) { return false; }
        _Target.BroadcastMessage("ApplyDamage", _AttackDamage);
        _Animator.SetTrigger("Attacked");
        return true;
    }

    public void ChangeAiState(AiState _State) //changes the State of the Ai
    {
        _Animator.SetBool(this._State.ToString(), false);
        _Animator.SetBool(_State.ToString(), true);
        this._State = _State;
    }

    private bool CheckIfTargetLost() //Resets the AI to base state if target is lost
    {
        if(_Target == null)
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
        if(_State == AiState.Aggressive) { return; }
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
            _StunCounter = _Duration;
        }
    }

    public void StartInvincibility() //makes Ai immune to damage for default time
    {
        if(_InvincibilityDuration <= 0) { return; }
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
        if(_CurrentHealth <= 0)
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

public enum AiState
{
    Idle,
    Wandering,
    Patroling,
    Aggressive
}

public enum PatrolType
{
    Idle,
    Wander,
    RandomPatrol,
    InOderPatrol
}

