using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Sound 
{
    public string _Name;
    public AudioClip _Clip;
    [Range(0f,1f)]
    public float _Volume;
    [Range(.1f, 3f)]
    public float _Pitch;
    public bool _Loop;
    [HideInInspector]
    public AudioSource source;
}
