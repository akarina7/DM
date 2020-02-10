using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] _Sounds;

    void Awake()
    {
        foreach (Sound _S in _Sounds)
        {
            _S.source = gameObject.AddComponent<AudioSource>();
            _S.source.clip = _S._Clip;
            _S.source.volume = _S._Volume;
            _S.source.pitch = _S._Pitch;
            _S.source.loop = _S._Loop;
        }
    }

    void Start()
    {
        Play("Theme");
    }

    public void Play(string _Name)
    {
        Sound _S = Array.Find(_Sounds, _Sound => _Sound._Name == _Name);
        if(_S == null)
        {
            return;
        }
        _S.source.Play();  
    }
}
