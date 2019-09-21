using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{

    public GameObject houseSmallPrefab;
    public GameObject houseLargePrefab;
    public GameObject treePrefab;
    public GameObject enemyPrefab;
    public GameObject collectiblePrefab;

    public int platformPoolSize = 15;
    public int enemyPoolSize = 5;
    public int collectiblePoolSize = 5;

    public float spawnRate =200f;
    public float increaseRate=2000f;
    public float MinY = -1f;                                   
    public float MaxY = 3.5f;
    public float speed = 2f;
    public float enemySpeed = 5f;
    public float collectibleSpeed = 8f;

    private GameObject[] pool;                                   //Collection of pooled objects.
    private GameObject[] enemyPool; 
    private GameObject[] collectiblePool;

    private int currentObj = 0;                                  //Index of the current object in the collection.
    private int currentEnemy = 0;
    private int currentCollectible = 0;


    private Vector3 smallHousePoolPosition;                         //A holding position for our unused objects offscreen.
    private Vector3 largeHousePoolPosition;
    private Vector3 treePoolPosition;
    private Vector3 enemyPoolPosition;
    private Vector3 collectiblePoolPosition;

    private float timeSinceLastSpawned;

    private Vector3 spawnEndPoint;

    void Start()
    {
        
        spawnEndPoint = GameObject.Find("SpawnEndPoint").transform.position;

        smallHousePoolPosition = GameObject.Find("PlatformSpawnPoint").transform.position;
        largeHousePoolPosition = GameObject.Find("PlatformLargeSpawnPoint").transform.position;
        treePoolPosition = GameObject.Find("PlatformTallSpawnPoint").transform.position;

        enemyPoolPosition = GameObject.Find("EnemySpawnPoint").transform.position;

        collectiblePoolPosition= GameObject.Find("CollectibleSpawnLocation").transform.position;

        timeSinceLastSpawned = 0f;

        //Initialize the columns collection.
        pool = new GameObject[platformPoolSize];
        enemyPool = new GameObject[enemyPoolSize];
        collectiblePool = new GameObject[collectiblePoolSize];


        //Loop through the collection and instantiate pools
        for (int i = 0; i < platformPoolSize; i++)
        {
            //...and create the individual columns.
            int temp = Random.Range(0, 4);
            if (temp <=1)
            {
                pool[i] = (GameObject)Instantiate(houseSmallPrefab, smallHousePoolPosition, Quaternion.identity);
            }
            else if (temp <=2)
            {
                pool[i] = (GameObject)Instantiate(houseLargePrefab, largeHousePoolPosition, Quaternion.identity);
            }
            else if (temp<=3)
            {
                pool[i] = (GameObject)Instantiate(treePrefab, treePoolPosition, Quaternion.identity);
            }
        }


        for (int i=0; i <enemyPoolSize;i++)
        {        
            enemyPool[i] = Instantiate(enemyPrefab, new Vector3(enemyPoolPosition.x, Random.Range((float)enemyPoolPosition.y - 1.0f, (float)enemyPoolPosition.y + 1.0f), enemyPoolPosition.z), Quaternion.identity);
        }
        
        for (int i=0; i<collectiblePoolSize;i++)
        {
            collectiblePool[i] = Instantiate(collectiblePrefab, new Vector3(collectiblePoolPosition.x, Random.Range((float)collectiblePoolPosition.y - 1.0f, (float)collectiblePoolPosition.y + 1.0f), collectiblePoolPosition.z), Quaternion.identity);
        }
           
    }


    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
        //GameControl.instance.gameOver == false
        if (timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0f;


            StartCoroutine(MoveLinear(pool[currentObj], speed));

            if (pool[currentObj].name.StartsWith("Tree"))
            {
                pool[currentObj].transform.position = smallHousePoolPosition;
            }
            else if (pool[currentObj].name.StartsWith("BigHouse"))
            {
                pool[currentObj].transform.position = largeHousePoolPosition;
            }
            else
            {
                pool[currentObj].transform.position = treePoolPosition;
            }

                //Increase the value of currentObj. If the new size is too big, set it back to zero
                currentObj++;

            if (currentObj >= platformPoolSize)
            {
                currentObj = 0;
            }

            enemyPool[currentEnemy].GetComponent<Rigidbody2D>().AddForce(new Vector2(-100f, 0), ForceMode2D.Force);  //fly across screen

            currentEnemy++;

            if (currentEnemy >= enemyPoolSize)
            {
                currentEnemy = 0;
            }



            //move collectibles across screen
            collectiblePool[currentCollectible].GetComponent<Rigidbody2D>().AddForce(new Vector2(-100f, 0),ForceMode2D.Force); 

           // StartCoroutine(MoveLinear(collectiblePool[currentCollectible], collectibleSpeed));
            currentCollectible++;

            if (currentCollectible >= collectiblePoolSize)
            {
                currentCollectible = 0;
            }

        }

        if (Time.time >= increaseRate)
        {
            spawnRate += 10f;
            speed += 1f;
        }

     }


    // move platform from across screen
    IEnumerator MoveLinear(GameObject obj, float speed)
    {
        Vector3 destination = new Vector3(spawnEndPoint.x, obj.transform.position.y, obj.transform.position.z);

        bool moving = true;
        while (moving)
        {
            float moveDistance = Vector3.Distance(obj.transform.position, spawnEndPoint);

            if (moveDistance > 1f)
            {
                obj.transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
            }
            else
            {
                moving = false;
                obj.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
 

            yield return null;

        }

    }
}
