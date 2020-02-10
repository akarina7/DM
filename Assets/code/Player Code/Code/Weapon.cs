using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{

    public CurrentPrimary _CurrentPrimary;
    public CurrentSpecial _CurrentSpecial;
    public CurrentMelee _CurrentMelee;
    //public DebugMode _DebugMode;

    private Animator _Animator;

    private Vector3 _MousePos; //position of mouse relative to world cordinates 
    public GameObject _CenterPointOfPlayer;
    private float _SafeZoneRadius = 1;

    public GameObject _StunProjectile;
    public GameObject _DamageProjectile;

    private Vector3 _FirePoint;

    /**********************************Weapon Levels *******************************/
    public WeaponLevel _StunRayLvl = WeaponLevel.Lv1;
    public WeaponLevel _DamageRayLvl = WeaponLevel.Lv1;
    public WeaponLevel _StunProjectileLvl = WeaponLevel.Lv1;
    public WeaponLevel _DamageProjectileLvl = WeaponLevel.Lv1;
    public WeaponLevel _MeleeLvl = WeaponLevel.Lv1;

    //See PlayerScript.cs for animator---------------------------------------------------------------------------------------------------

    void Start()
    {
        _Animator = gameObject.GetComponent<Animator>();
    }

    
    void Update()
    {
        _MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _MousePos.x -= _CenterPointOfPlayer.transform.position.x;
        _MousePos.y -= _CenterPointOfPlayer.transform.position.y;
        _Animator.SetFloat("AimingAngle", GetAngle());
        /*
        switch (_DebugMode)
        {
            case DebugMode.Origin:
                {
                    Debug.DrawRay(_CenterPointOfPlayer.transform.position, _MousePos, Color.blue);
                    //Debug.Log(Mathf.Atan(_MousePos.y / _MousePos.x) * 180 / Mathf.PI);
                    break; 
                }
            case DebugMode.Ranged:
                {
                    float _Angle = Mathf.Atan(_MousePos.y / _MousePos.x);
                    Debug.Log("X:" + _MousePos.x + " Y:" + _MousePos.y);
                    if (_MousePos.x > _CenterPointOfPlayer.transform.position.x)
                    {
                        _FirePoint.x = Mathf.Cos(_Angle) * _SafeZoneRadius + _CenterPointOfPlayer.transform.position.x;
                        _FirePoint.y = Mathf.Sin(_Angle) * _SafeZoneRadius + _CenterPointOfPlayer.transform.position.x;

                    }
                    else if(_MousePos.x < _CenterPointOfPlayer.transform.position.x)
                    {
                        _FirePoint.x = Mathf.Cos(_Angle) * -_SafeZoneRadius + _CenterPointOfPlayer.transform.position.x;
                        _FirePoint.y = Mathf.Sin(_Angle) * -_SafeZoneRadius + _CenterPointOfPlayer.transform.position.x;
                    }
                    else
                    {
                        _FirePoint.x = _CenterPointOfPlayer.transform.position.x;
                        _FirePoint.y = (_MousePos.y > _CenterPointOfPlayer.transform.position.y) ? (_SafeZoneRadius) : (-1 * _SafeZoneRadius) + _CenterPointOfPlayer.transform.position.y;
                    }
                    Debug.DrawRay(_FirePoint, _MousePos, Color.blue);
                    break;
                }
            case DebugMode.Off:
                break;
        }
        */
    }
    
    private float GetAngle2() //gets the angle that the character is aiming
    {
        if (_MousePos.y > _CenterPointOfPlayer.transform.position.y)
        {
            //return (Mathf.Atan(_MousePos.y - _CenterPointOfPlayer.transform.position.y  / _MousePos.x - _CenterPointOfPlayer.transform.position.x) * 180 / Mathf.PI) + ((_MousePos.x > _CenterPointOfPlayer.transform.position.x) ? 0f : 0f);
            if (_MousePos.x > _CenterPointOfPlayer.transform.position.x)
            {
                return (Mathf.Atan(_MousePos.y - _CenterPointOfPlayer.transform.position.y / _MousePos.x - _CenterPointOfPlayer.transform.position.x) * 180 / Mathf.PI);
            }
            else
            {
                return 180 - (Mathf.Atan(_MousePos.y - _CenterPointOfPlayer.transform.position.y / _MousePos.x - _CenterPointOfPlayer.transform.position.x) * 180 / Mathf.PI);
            }
        }
        else
        {
            //return (Mathf.Atan(_MousePos.y - _CenterPointOfPlayer.transform.position.x / _MousePos.x - _CenterPointOfPlayer.transform.position.x) * 180 / Mathf.PI) + ((_MousePos.x > _CenterPointOfPlayer.transform.position.x) ? 360f : 270f);
            if (_MousePos.x > _CenterPointOfPlayer.transform.position.x)
            {
                return 360 + (Mathf.Atan(_MousePos.y - _CenterPointOfPlayer.transform.position.x / _MousePos.x - _CenterPointOfPlayer.transform.position.x) * 180 / Mathf.PI);
            }
            else
            {
                return 180 - (Mathf.Atan(_MousePos.y - _CenterPointOfPlayer.transform.position.x / _MousePos.x - _CenterPointOfPlayer.transform.position.x) * 180 / Mathf.PI);
            }
        }
    }

    private float GetAngle()
    {
        return Vector2.SignedAngle(Vector2.right, Camera.main.ScreenToWorldPoint(Input.mousePosition) - _CenterPointOfPlayer.transform.position);
    }

    public void PrimaryFire() //Primary weapon slot
    {
        _Animator.SetTrigger("FiredPrimary");
        switch (_CurrentPrimary)
        {
            case CurrentPrimary.Unarmed: //no weapon
                {
                    break;
                }
            case CurrentPrimary.StunGunRay: //shotos a stun ray
                {
                    ShootStunGunRay();
                    break;
                }
            case CurrentPrimary.StunGunProjectile: //shots a stun projectile
                {
                    ShootStunProjectile();
                    break;
                }
            case CurrentPrimary.DamageRay:
                {
                    ShootDamageRay();
                    break;
                }
            case CurrentPrimary.DamageProjectile:
                {
                    ShootDamageProjectile();
                    break;
                }
        }
    }

    public void SpecialFire()
    {
        switch (_CurrentSpecial)
        {
            case CurrentSpecial.Unarmed:
                {
                    break;
                }
        }
    }

    public void MeleeAttack()
    {
        _Animator.SetTrigger("MeleeAttack");
        switch (_CurrentMelee)
        {
            case CurrentMelee.Damage:
                {
                    ShootDamageProjectile();
                    break;
                }
        }
    }

    /************************************************************** Primary Ray Attacks *****************************************************************/
    private void ShootStunGunRay()
    {
        float _StunDuration;
        switch (_StunRayLvl)
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
        RaycastHit2D[] _Results = new RaycastHit2D[2];
        Physics2D.Raycast(_CenterPointOfPlayer.transform.position, _MousePos, new ContactFilter2D(), _Results, Mathf.Infinity);
        if(!_Results[0].transform.CompareTag("Player"))
        {
            _Results[0].transform.BroadcastMessage("ApplyStun", _StunDuration);
        }
        else if (_Results[1])
        {
            _Results[1].transform.BroadcastMessage("ApplyStun", _StunDuration);
        }
    }

    private void ShootDamageRay()
    {
        float _Damage;
        switch (_StunRayLvl)
        {
            case WeaponLevel.Lv1:
                {
                    _Damage = 20f;
                    break;
                }
            case WeaponLevel.Lv2:
                {
                    _Damage = 35f;
                    break;
                }
            case WeaponLevel.Lv3:
                {
                    _Damage = 50f;
                    break;
                }
            default:
                {
                    goto case WeaponLevel.Lv1;
                }
        }
        RaycastHit2D[] _Results = new RaycastHit2D[2];
        Physics2D.Raycast(_CenterPointOfPlayer.transform.position, _MousePos, new ContactFilter2D(), _Results, Mathf.Infinity);
        if (!_Results[0].transform.CompareTag("Player"))
        {
            _Results[0].transform.BroadcastMessage("ApplyDamage", _Damage);
        }
        else if (_Results[1])
        {
            _Results[1].transform.BroadcastMessage("ApplyDamage", _Damage);
        }
    }
    /************************************************************** Primary Projectile Attacks **********************************************************/
    private void ShootStunProjectile()
    {
        if (_StunProjectile != null)
        {
            Instantiate(_StunProjectile, _CenterPointOfPlayer.transform.position, Quaternion.Euler(0, 0, GetAngle()), transform);
        }
    }

    private void ShootDamageProjectile()
    {
        if(_DamageProjectile != null)
        {
            Instantiate(_DamageProjectile, _CenterPointOfPlayer.transform.position, Quaternion.Euler(0, 0, GetAngle()), transform);
        }
    }
    /************************************************************** Melee Attack ************************************************************************/
    private void PreformDamageMelee()
    {
        float _Damage;
        float _MeleeRange;
        switch (_MeleeLvl)
        {
            case WeaponLevel.Lv1:
                {
                    _Damage = 40f;
                    _MeleeRange = 1.0f;
                    break;
                }
            case WeaponLevel.Lv2:
                {
                    _Damage = 55f;
                    _MeleeRange = 1.15f;
                    break;
                }
            case WeaponLevel.Lv3:
                {
                    _Damage = 70f;
                    _MeleeRange = 1.3f;
                    break;
                }
            default:
                {
                    goto case WeaponLevel.Lv1;
                }
        }
        RaycastHit2D[] _Results = new RaycastHit2D[2];
        Physics2D.Raycast(_CenterPointOfPlayer.transform.position, _MousePos, new ContactFilter2D(), _Results, _MeleeRange);
        if (!_Results[0].transform.CompareTag("Player"))
        {
            _Results[0].transform.BroadcastMessage("ApplyDamage", _Damage);
        }
        else if (_Results[1])
        {
            _Results[1].transform.BroadcastMessage("ApplyDamage", _Damage);
        }
    }
    /************************************************************** Special *****************************************************************************/
}

public enum CurrentPrimary
{
    Unarmed,
    StunGunRay,
    StunGunProjectile,
    DamageRay,
    DamageProjectile
}

public enum CurrentSpecial
{
    Unarmed
}

public enum CurrentMelee
{
    Unarmed,
    Damage
}
/*
public enum DebugMode
{
    Origin,
    Ranged,
    Off
}
*/
public enum WeaponLevel
{
    Lv1,
    Lv2,
    Lv3
}


