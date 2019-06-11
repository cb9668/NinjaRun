using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainCanvas : MonoBehaviour {

    public Button pauseButton;
    public GameObject pauseMenu;
    public Toggle musicToggle;
    public Slider musicSlider;
    public Toggle sfxToggle;
    public Slider sfxSlider;
    public AudioSource mainBGM;

    private float musicVolume;
    private float sfxVolume;

	// Use this for initialization
	void Start () {
        GameManager.instance.spawnPlayer("PlayerSpawnLocation");


        if (SoundManager.instance.musicToggle == 1)
            musicToggle.isOn = true;
        else
            musicToggle.isOn = false;
        if (SoundManager.instance.sfxToggle == 1)
            sfxToggle.isOn = true;
        else
            sfxToggle.isOn = false;


        musicSlider.value = SoundManager.instance.musicVolume;
        sfxSlider.value = SoundManager.instance.sfxVolume;

        pauseMenu.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PauseGame()
    {
        Time.timeScale = 0;
        mainBGM.Pause();
        pauseMenu.SetActive(true);


    }

    public void ContinueGame()
    {
        SetVolumne();
        
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        
        mainBGM.UnPause();

        //Debug.Log("musicVol " + musicVolume);
        Debug.Log("sfxVOl " + SoundManager.instance.sfxVolume);

        //Debug.Log("music toggle" + musicSlider.value);
        Debug.Log("sfxslider" + sfxSlider.value);

       // Debug.Log("musictoggle " + musicToggle);
        Debug.Log("sfxVOl " + sfxToggle);
    }

    public void ExitGame()
    {
        SetVolumne();
        GameManager.instance.LoadStart();
    }

    void SetVolumne()
    {

        SoundManager.instance.musicVolume = musicSlider.value;

        if (musicToggle.isOn)
        {
            SoundManager.instance.musicToggle = 1;
            mainBGM.volume = SoundManager.instance.musicVolume;
        }
        else
        {
            SoundManager.instance.musicToggle = 0;
            mainBGM.volume = 0;         //if toggle is off, mute music ignoring slider
        }

        SoundManager.instance.sfxVolume = sfxSlider.value;

        if (sfxToggle.isOn)
        {           
            SoundManager.instance.sfxToggle = 1;
        }
        else
        {
            SoundManager.instance.sfxToggle = 0;
        }

        DataManager.instance.SavePlayerPref();

    }


}
