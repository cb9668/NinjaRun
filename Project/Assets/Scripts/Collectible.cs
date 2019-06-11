using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

    private Transform spawnLocation;
    private Camera main;
    public Transform spawnEndLocation;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.tag == "Player")
        {
            transform.position = new Vector3(spawnLocation.position.x, Random.Range((float)spawnLocation.position.y - 1.0f, (float)spawnLocation.position.y + 1.0f), spawnLocation.position.z);
        }
    }

    private void Start()
    {
        main = Camera.main;
        spawnLocation = GameObject.Find("CollectibleSpawnLocation").transform;
        spawnEndLocation = GameObject.Find("SpawnEndPoint").transform;
    }

    private void Update()
    {
        if (!RendererExtensions.IsVisibleFrom(gameObject.GetComponentInChildren<Renderer>(), main) && transform.position.x < spawnEndLocation.position.x)
        {
            transform.position = new Vector3 (spawnLocation.position.x, Random.Range((float)spawnLocation.position.y - 1.0f, (float)spawnLocation.position.y + 1.0f),spawnLocation.position.z);
        }
    }
}
