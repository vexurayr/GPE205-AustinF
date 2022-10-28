using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    // Used for spawning correct pickup at correct time
    public GameObject pickupPrefab;
    public float spawnDelay;
    private float nextSpawnTime;

    // Used to make sure a pickup is not spawned when there is one still there
    private GameObject spawnedPickup;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnPickup();
    }

    public void SpawnPickup()
    {
        // If there is no pickup already spawned
        if (spawnedPickup == null)
        {
            // If enough time has passed to spawn a new pickup
            if (Time.time > nextSpawnTime)
            {
                // Spawn that pickup and set next time it can spawn
                spawnedPickup = Instantiate(pickupPrefab, transform.position, pickupPrefab.transform.rotation);
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else
        {
            // Object still exists, so reset timer for next spawn attempt
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}