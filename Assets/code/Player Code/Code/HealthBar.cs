using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject _Player;
    public Color _DefaultColor;
    public GameObject[] _HealthSegments;
    private Vector3[] _HealthSegmentsOriginalPos;

    private float _HealthPerSeg;
    private float _NumOfSeg;
    private PlayerScript _PlayerScript;

    void Start()
    {
        _PlayerScript = _Player.GetComponent<PlayerScript>();
        _NumOfSeg = _HealthSegments.Length;
        _HealthPerSeg = _PlayerScript._MaxHealth / _NumOfSeg;
        for (int i = 0; i < _NumOfSeg; i++)
        {
            _HealthSegments[i].GetComponent<Image>().DOColor(_DefaultColor, 0f);
        }
        
        for(int i = 0; i < _NumOfSeg; i++)
        {
            //_HealthSegmentsOriginalPos[i] = _HealthSegments[i].transform.position;    //code to move back to original pos
        }
    }

    private void FixedUpdate()
    {
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        for (int i = 0; i < _NumOfSeg; i++)
        {
            //_HealthSegments[i].transform.position = _HealthSegmentsOriginalPos[i]; //code to move back to original pos
        }
        for (int i = 0; i < _NumOfSeg; i++)
        {
            if (new Color(_DefaultColor.r, _DefaultColor.g, _DefaultColor.b, PercentFull(i)) != _HealthSegments[i].GetComponent<Image>().color)
            {
                //_HealthSegments[i].GetComponent<Transform>().DOShakePosition(.5f, 15, 10);
            }
            //healthSegments[i].GetComponent<Image>().color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, PercentFull(i));
            _HealthSegments[i].GetComponent<Image>().DOFade(PercentFull(i), .5f);
        }
    }
    float PercentFull(int index)
    {
        float lowerBound = index * _HealthPerSeg;
        float upperBound = (index + 1) * _HealthPerSeg;


        if (_PlayerScript._CurrentHealth >= upperBound)
        {
            return 1;
        }
        else if (_PlayerScript._CurrentHealth <= lowerBound)
        {
            return 0;
        }
        else
        {
            return (_PlayerScript._CurrentHealth % _HealthPerSeg) / _HealthPerSeg;
        }

    }
}
