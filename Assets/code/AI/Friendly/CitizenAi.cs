using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenAi : MonoBehaviour
{
    public float _MoveSpeed;

    public Animator _Animator;
    public Rigidbody2D _RigidBody;
    public MoveType _MoveType; //move type
    private MoveType _InitalType;
    private float _WaitTime = 2; //time to wait at each checkpoint
    public Vector2 _WanderRange; //the max x and y wander values
    public Transform[] _MoveSpots; 
    private Vector2 _RandomWanderSpot;
    private int _CurrentSpot = 0; 
    private int _NumOfSpots;
    private Vector2 _InitalLocation;
    private float _MoveDirection = 0;


    /*
     * float "MoveDirection" - Direction of Movement (-1 for left, 1 for right, 0 for not moving)
     * bool "Idle" - Is in Idle state
     * bool "Wander" - IS in the wander state
     * bool "WaypointsInOrder" - Is in the WaypointInOrder state
     * bool "WaypointsRandom" - Is in the WaypointsRandom state
     * 
     */

    // Start is called before the first frame update
    void Start()
    {
        _RandomWanderSpot = new Vector2(Random.Range(-_WanderRange.x, _WanderRange.x) + _InitalLocation.x, Random.Range(-_WanderRange.y, _InitalLocation.y) + _InitalLocation.y);
        ComponentDoubleCheck();
        if (_MoveType == MoveType.WaypointsInOrder || _MoveType == MoveType.WaypointsRandom)
        {
            if (_MoveSpots != null) { _NumOfSpots = _MoveSpots.Length; }
            if (_NumOfSpots <= 0) { _MoveType = MoveType.Idle; }
        }
        _InitalType = _MoveType;
    }

    // Update is called once per frame
    void Update()
    {
        if (_RigidBody.velocity.sqrMagnitude > 0)
        {
            _RigidBody.velocity = new Vector3(0, 0, 0);
        }
        _Animator.SetFloat("MoveDirection", _MoveDirection);
        switch (_MoveType)
        {
            case MoveType.Idle:
                {
                    _MoveDirection = 0;
                    break;
                }
            case MoveType.Wander:
                {
                    transform.position = Vector2.MoveTowards(transform.position, _RandomWanderSpot, _MoveSpeed * Time.deltaTime);
                    _MoveDirection = (transform.position.x < _RandomWanderSpot.x) ? 1 : -1; //new move direction code
                    if (Vector2.Distance(transform.position, _RandomWanderSpot) < .2f)
                    {
                        _RandomWanderSpot = new Vector2(Random.Range(-_WanderRange.x, _WanderRange.x) + _InitalLocation.x, Random.Range(-_WanderRange.y, _InitalLocation.y) + _InitalLocation.y);
                        WaitAtCheckPoint();
                    }
                    break;
                }
            case MoveType.WaypointsInOrder:
                {
                    transform.position = Vector2.MoveTowards(transform.position, _MoveSpots[_CurrentSpot].position, _MoveSpeed * Time.deltaTime);
                    _MoveDirection = (transform.position.x < _MoveSpots[_CurrentSpot].position.x) ? 1 : -1; //new move direction code
                    if (Vector2.Distance(transform.position, _MoveSpots[_CurrentSpot].position) < .2f)
                    {
                        _CurrentSpot = (_CurrentSpot >= _MoveSpots.Length - 1 ? 0 : _CurrentSpot + 1);
                        WaitAtCheckPoint();
                    }
                    break;
                }
            case MoveType.WaypointsRandom:
                {
                    transform.position = Vector2.MoveTowards(transform.position, _MoveSpots[_CurrentSpot].position, _MoveSpeed * Time.deltaTime);
                    _MoveDirection = (transform.position.x < _MoveSpots[_CurrentSpot].position.x) ? 1 : -1; //new move direction code
                    if (Vector2.Distance(transform.position, _MoveSpots[_CurrentSpot].position) < .2f)
                    {
                        _CurrentSpot = Random.Range(0, _NumOfSpots);
                        WaitAtCheckPoint();
                    }
                    break;
                }
        }
        
    }
    private void WaitAtCheckPoint() //makes the Ai wait at a check for the given time
    {
        ChangeAiState(MoveType.Idle);
        StartCoroutine(Wait(_WaitTime));
    }

    public void SwitchToBaseState() //switches AI to its default patrol state
    {
        switch (_InitalType)
        {
            case MoveType.Idle:
                {
                    ChangeAiState(MoveType.Idle);
                    break;
                }
            case MoveType.Wander:
                {
                    ChangeAiState(MoveType.Wander);
                    break;
                }
            case MoveType.WaypointsInOrder:
                {
                    ChangeAiState(MoveType.WaypointsInOrder);
                    break;
                }
            case MoveType.WaypointsRandom:
                {
                    ChangeAiState(MoveType.WaypointsRandom);
                    break;
                }
            default:
                {
                    Debug.Log("<color = cyan>ERROR: No base patrol state set for MeleeEnemy</color>");
                    break;
                }
        }
    }

    public void ChangeAiState(MoveType _MoveType) //changes the State of the Ai
    {
        _Animator.SetBool(this._MoveType.ToString(), false);
        _Animator.SetBool(_MoveType.ToString(), true);
        this._MoveType = _MoveType;
    }

    void ComponentDoubleCheck() //assigns components if they were not added
    {
        _Animator = (_Animator == null) ? gameObject.GetComponent<Animator>() : _Animator;
        _RigidBody = (_RigidBody == null) ? gameObject.GetComponent<Rigidbody2D>() : _RigidBody;
    }

    IEnumerator Wait(float _Time)
    {
        yield return new WaitForSeconds(_Time);
        SwitchToBaseState();
    }
}

public enum MoveType
{
    Idle,
    Wander,
    WaypointsRandom,
    WaypointsInOrder
}