using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    private Vector3 _StartingPosition;
    public Transform _FollowTarget;
    private Vector3 _TargetPos;
    public float _MoveSpeed;
    public bool _AddMouse = false;
    public Vector2 _ScreenMaxValues;
    public float _NormalizationMin;
    public float _NormalizationMax;

    void Start()
    {
        _StartingPosition = transform.position;
    }

    void Update()
    {
        if (_FollowTarget != null)
        {
            if(!_AddMouse)
            {
                _TargetPos = new Vector3(_FollowTarget.position.x, _FollowTarget.position.y, transform.position.z);
                Vector3 velocity = (_TargetPos - transform.position) * _MoveSpeed;
                transform.position = Vector3.SmoothDamp(transform.position, _TargetPos, ref velocity, 1.0f, Time.deltaTime);
            }
            else
            {
                _TargetPos = new Vector3(_FollowTarget.position.x, _FollowTarget.position.y, transform.position.z);
                //_TargetPos.x +=(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _TargetPos.x);
                //_TargetPos.y +=(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - _TargetPos.y);
                //_NormalizationMin + ((z - _ScreenMaxValues.x) * (_NormalizationMax - _NormalizationMin))/(ScreenMaxValues.x + ScreenMaxValues.x)
                _TargetPos.x += _NormalizationMin + (((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - _TargetPos.x) + _ScreenMaxValues.x) * (_NormalizationMax - _NormalizationMin)) / (_ScreenMaxValues.x + _ScreenMaxValues.x);
                _TargetPos.y += _NormalizationMin + (((Camera.main.ScreenToWorldPoint(Input.mousePosition).y - _TargetPos.y) + _ScreenMaxValues.y) * (_NormalizationMax - _NormalizationMin)) / (_ScreenMaxValues.y + _ScreenMaxValues.y); 
                _TargetPos.z = transform.position.z;
                Vector3 velocity = (_TargetPos - transform.position) * _MoveSpeed;
                transform.position = Vector3.SmoothDamp(transform.position, _TargetPos, ref velocity, 1.0f, Time.deltaTime);
            }
        }
    }

    void TrackPlayer()
    {
        _FollowTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void TrackTarget(Transform _Target)
    {
        _FollowTarget = (_Target ? _FollowTarget : _Target);
    }
}
