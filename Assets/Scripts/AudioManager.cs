using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicPrefab;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Toggle _audioToogle;

    private static AudioSource _music;

    void Start()
    {
        if(_music is null)
        {
            _music = Instantiate(_musicPrefab).GetComponent<AudioSource>();
            _music.transform.parent = null;
        }

        DontDestroyOnLoad(_music.gameObject);
        _music.volume = MusicVolume;

        _musicSlider.value = MusicVolume;
        _soundSlider.value = SoundVolume;
        _audioToogle.isOn = Mute;

        _musicSlider.onValueChanged.AddListener(MusicVolumeChange);
        _soundSlider.onValueChanged.AddListener(SoundVolumeChange);
        _audioToogle.onValueChanged.AddListener(MuteAudio);
    }

    private void MuteAudio(bool value)
    {
        Mute = value;
    }
    private void MusicVolumeChange(float value)
    {
        MusicVolume = value;
    }
    private void SoundVolumeChange(float value)
    {
        SoundVolume = value;
    }

    public bool Mute
    {
        get
        {
            return PlayerPrefs.GetInt("Mute",  0) == 1 ? true : false;
        }
        set
        {
            _music.mute = !value;
            PlayerPrefs.SetInt("Mute", value ? 1 : 0);
        }
    }

    public float SoundVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("Sound", 0.6f);
        }
        set
        {
            PlayerPrefs.SetFloat("Sound", (float)value);
        }
    }

    public float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat("Music", 0.6f);
        }
        set
        {
            PlayerPrefs.SetFloat("Music", (float)value);
            _music.volume = PlayerPrefs.GetFloat("Music", 0.6f);
        }
    }
}
