using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{

    public GameObject _Player;
    private PlayerScript _PlayerScript;
    public Color _DefaultColor;

    public GameObject[] _AmmoSegments;
    public GameObject[] _EmptyAmmoSegments;
    
    private float _AmmoPerSeg;
    private float _NumOfSeg;


    // Start is called before the first frame update
    void Start()
    {
        _PlayerScript = _Player.GetComponent<PlayerScript>();
        _NumOfSeg = _AmmoSegments.Length;
        _AmmoPerSeg = _PlayerScript._AmmoPerShot;

        if(_EmptyAmmoSegments.Length != _NumOfSeg)
        {
            Debug.LogError("The ammount of Segments in ammo bar dont equal eachother");
        }

        for (int i = 0; i < _NumOfSeg; i++)
        {
            _AmmoSegments[i].GetComponent<Image>().DOColor(_DefaultColor, 0f);
            _EmptyAmmoSegments[i].GetComponent<Image>().DOColor(_DefaultColor, 0f);
        }

    }

    private void FixedUpdate()
    {
        UpdateAmmo();
    }

    void UpdateAmmo()
    {
        for (int i = 0; i < _NumOfSeg; i++)
        {
            _AmmoSegments[i].GetComponent<Image>().DOFade(PercentFull(i), .5f);
            _EmptyAmmoSegments[i].GetComponent<Image>().DOFade(1f - PercentFull(i), .5f);
        }
    }

    float PercentFull(int index)
    {
        float lowerBound = index * _AmmoPerSeg;
        float upperBound = (index + 1) * _AmmoPerSeg;

        if (_PlayerScript._AmmoCount >= upperBound)
        {
            return 1;
        }
        else if (_PlayerScript._AmmoCount <= lowerBound)
        {
            return 0;
        }
        else
        {
            return (_PlayerScript._AmmoCount % _AmmoPerSeg) / _AmmoPerSeg;
        }

    }
}
