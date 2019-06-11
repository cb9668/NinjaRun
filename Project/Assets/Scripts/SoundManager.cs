using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    static SoundManager _instance = null;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public float musicVolume;
    public float sfxVolume;
    public int musicToggle;
    public int sfxToggle;

    // Use this for initialization
    void Start()
    {

        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }


        musicVolume = GameManager.instance.saveData.PDmusicVolume;
        sfxVolume = GameManager.instance.saveData.PDsfxVolume;
        musicToggle = GameManager.instance.saveData.PDmusicToggle;
        sfxToggle= GameManager.instance.saveData.PDsfxToggle;

        Debug.Log("musicVol " + musicVolume);
        Debug.Log("sfxVOl " + sfxVolume);
    }

    
    public void PlaySingleSound(AudioClip clip, float volume)
    {
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.Play();
    }

    public static SoundManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
}
