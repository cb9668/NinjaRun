using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    static GameManager _instance = null;
    public GameObject playerPrefab;
    public Text mainText;

    GameObject _player;

    Player cc;

    public float highscore;

    public PlayerData saveData;

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

        saveData = DataManager.instance.LoadPlayerData();

        highscore = saveData.PDhighscore;

        Debug.Log(highscore);
    }


    // Update is called once per frame
    void Update () {
		
	}

    public void spawnPlayer(int spawnLocation)
    {
        string spawnPointName = SceneManager.GetActiveScene().name + "_" + spawnLocation;  

        Transform spawnPointTransform = GameObject.Find(spawnPointName).GetComponent<Transform>();

        if (playerPrefab && spawnPointTransform)
        {
            player = Instantiate(playerPrefab, spawnPointTransform.position, spawnPointTransform.rotation);
        }
        else
            Debug.Log("Missing prefab or could not find transform.");

    }

    public static GameManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public GameObject player
    {
        get { return _player; }
        set
        {
            _player = value;
            cc = player.GetComponent<Player>();

        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("main");

    }


    public void LoadStart()
    {
        SceneManager.LoadScene("Start");
    }

    public void spawnPlayer(string spawnLocation)
    {

        Transform spawnPointTransform = GameObject.Find(spawnLocation).GetComponent<Transform>();

        if (playerPrefab && spawnPointTransform)
        {
            player = Instantiate(playerPrefab, spawnPointTransform.position, spawnPointTransform.rotation);
        }
        else
            Debug.Log("Missing prefab or could not find transform.");

    }

    public void Load(string name)
    {
        SceneManager.LoadScene(name);
    }
}
