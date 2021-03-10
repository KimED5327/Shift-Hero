using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string sound;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] Sound[] _sfx = null;
    [SerializeField] Sound[] _bgm = null;

    [SerializeField] AudioSource _sfxPlayer = null;
    [SerializeField] AudioSource _bgmPlayer = null;


    private void Awake()
    {
        instance = this;
    }

    public void SetMuteSFXPlayer(bool flag)
    {
        _sfxPlayer.mute = flag;
    }

    public void SetMuteBGMPlayer(bool flag)
    {
        _bgmPlayer.mute = flag;
    }

    public void PlaySFX(string soundName)
    {
        for (int i = 0; i < _sfx.Length; i++)
        {
            if (string.Equals(_sfx[i].sound, soundName))
            {
                _sfxPlayer.PlayOneShot(_sfx[i].clip);
            }
        }
    }

    public void PlayBGM(string soundName)
    {
        for (int i = 0; i < _bgm.Length; i++)
        {
            if (string.Equals(_bgm[i].sound, soundName))
            {
                _bgmPlayer.clip = _bgm[i].clip;
                _bgmPlayer.Play();
            }
        }
    }

}
