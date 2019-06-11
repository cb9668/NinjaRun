using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour {
    static DataManager _instance = null;

    private float highscore;
    private float musicVol;
    private float sfxVol;
    private int musicToggle;
    private int sfxToggle;

    // Use this for initialization
    void Start () {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static DataManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    public PlayerData LoadPlayerData()
    {

        if (PlayerPrefs.HasKey("highestScore"))
            highscore = PlayerPrefs.GetFloat("highscore");
        else
            highscore = 0;

        if (PlayerPrefs.HasKey("musicVolume"))
            musicVol = PlayerPrefs.GetFloat("musicVolume");
        else
            musicVol = 1;

        if (PlayerPrefs.HasKey("sfxVolume"))
            sfxVol= PlayerPrefs.GetFloat("sfxVolume");
        else
            sfxVol= 1;

        if (PlayerPrefs.HasKey("musicToggle"))
            musicToggle = PlayerPrefs.GetInt("musicToggle");
        else
           musicToggle = 1;

        if (PlayerPrefs.HasKey("sfxToggle"))
            sfxToggle = PlayerPrefs.GetInt("sfxToggle");
        else
            sfxToggle = 1;

        PlayerData playerData = new PlayerData()
        {
            PDhighscore = highscore,
            PDmusicVolume = musicVol,
            PDsfxVolume = sfxVol,
            PDmusicToggle = musicToggle,
            PDsfxToggle = sfxToggle,
        };

        return playerData;
    }

    private void SavePlayerData()
    {
        // Save highscore and pref

        SaveHighScore();
        SavePlayerPref();

    }

    //private int SetBool(bool pref)
    //{
    //    if (pref == true)
    //        return 1;
    //    else
    //        return 0;
    //}

    //private bool SetInt(int pref)
    //{
    //    if (pref == 1)
    //        return true;
    //    else
    //        return false;
    //}

    public void SaveHighScore()
    {
        PlayerPrefs.SetFloat("highscore", GameManager.instance.highscore);
    }

    public void SavePlayerPref()
    {
        PlayerPrefs.SetInt("musicToggle", SoundManager.instance.musicToggle);
        PlayerPrefs.SetInt("sfxToggle", SoundManager.instance.sfxToggle);

        PlayerPrefs.SetFloat("musicVolume", SoundManager.instance.musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", SoundManager.instance.sfxVolume);
    }
}
