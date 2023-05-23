using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSetter : MonoBehaviour
{
    [SerializeField] private float _soundDistance = 10;

    private float _maxVolume;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _maxVolume = _audioSource.volume * PlayerPrefs.GetFloat("Sound", 0.6f);
        _audioSource.volume = _maxVolume;
    }

    private void FixedUpdate()
    {
        float newVolume = 1 - Vector2.Distance(CharacterMovement.Position, transform.position) / _soundDistance;
        if (newVolume < 0) newVolume = 0;
        _audioSource.volume = newVolume * _maxVolume;
    }
}
