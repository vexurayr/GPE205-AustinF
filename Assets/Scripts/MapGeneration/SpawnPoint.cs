using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Class simply to hold spawn points for game manager to pull from
    public List<GameObject> playerSpawnPoints;
    public List<GameObject> aISpawnPoints;
    public List<GameObject> pickupSpawnPoints;
    public List<Waypoint> waypointSpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        // Adds each object within the list to the corresponding list in the game manager
        if (GameManager.instance != null && GameManager.instance.playerSpawnPoints != null)
        {
            foreach(GameObject spawnPoint in playerSpawnPoints)
            {
                GameManager.instance.playerSpawnPoints.Add(spawnPoint);
            }
        }
        
        if (GameManager.instance != null && GameManager.instance.aISpawnPoints != null)
        {
            foreach(GameObject spawnPoint in aISpawnPoints)
            {
                GameManager.instance.aISpawnPoints.Add(spawnPoint);
            }
        }

        if (GameManager.instance != null && GameManager.instance.pickupSpawnerSpawnPoints != null)
        {
            foreach(GameObject spawnPoint in pickupSpawnPoints)
            {
                GameManager.instance.pickupSpawnerSpawnPoints.Add(spawnPoint);
            }
        }

        if (GameManager.instance != null && GameManager.instance.aIWaypoints != null)
        {
            foreach(Waypoint waypoint in waypointSpawnPoints)
            {
                //Debug.Log("Adding " + waypoint + " to game manager.");
                GameManager.instance.aIWaypoints.Add(waypoint);
            }
        }
    }
}