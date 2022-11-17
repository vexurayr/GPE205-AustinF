using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Reference to itself to reach objects anywhere in the heiarchy
    public static GameManager instance;

    [Range(1, 100)] public int maxAISpawned;
    [Range(1, 100)] public int maxPickupsSpawned;

    // Things the game manager will need to reference
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;

    /// <summary>
    /// Leave Empty
    /// </summary>
    public List<PlayerController> players;
    private GameObject newPawnObject;

    // AI tank prefabs
    public GameObject sittingDuckPrefab;
    public GameObject noobGamerPrefab;
    public GameObject guardPrefab;
    public GameObject sharpEarsPrefab;

    // AI tank behaviors/controllers
    public GameObject sittingDuckBehavior;
    public GameObject noobGamerBehavior;
    public GameObject guardBehavior;
    public GameObject sharpEarsBehavior;

    /// <summary>
    /// Leave Empty
    /// </summary>
    public List<AIController> aIPlayers;
    private GameObject newAIPawnObject;

    /// <summary>
    /// Leave Empty
    /// </summary>
    public List<GameObject> playerSpawnPoints;

    /// <summary>
    /// Leave Empty
    /// </summary>
    public List<GameObject> aISpawnPoints;
    private List<GameObject> remainingAISpawnPoints;

    // Contains a list of waypoint objects, which contain a list of the individual waypoint game objects
    public List<Waypoint> aIWaypoints;
    private Waypoint waypointPattern;

    // Used for placing spawner prefabs when the map generates
    public List<GameObject> pickupSpawners;

    /// <summary>
    /// Leave Empty
    /// </summary>
    public List<GameObject> pickupSpawnerSpawnPoints;
    private List<GameObject> remainingPickupSpawnerSpawnPoints;

    // Designers leave blank, used for AI to see nearby pickups
    /// <summary>
    /// Leave Empty
    /// </summary>
    public List<GameObject> pickups;

    // Runs as soon as this object is enabled, one frame before Start()
    private void Awake()
    {
        players = new List<PlayerController>();
        aIPlayers = new List<AIController>();
        playerSpawnPoints = new List<GameObject>();
        aISpawnPoints = new List<GameObject>();
        remainingAISpawnPoints = new List<GameObject>();
        remainingPickupSpawnerSpawnPoints = new List<GameObject>();
        pickups = new List<GameObject>();

        // Only allows for one game manager, one singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < aISpawnPoints.Count; i++)
        {
            remainingAISpawnPoints.Add(aISpawnPoints[i]);
        }

        for (int i = 0; i < pickupSpawnerSpawnPoints.Count; i++)
        {
            remainingPickupSpawnerSpawnPoints.Add(pickupSpawnerSpawnPoints[i]);
        }

        SpawnPlayer();
        SpawnAI();
        SpawnPickupSpawners();
    }

    public void SpawnPlayer()
    {
        // Spawns player controller into the scene
        GameObject newPlayerController = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);

        int randNum = GetRandomNumberInRange(0, playerSpawnPoints.Count);

        newPawnObject = Instantiate(tankPawnPrefab,
            playerSpawnPoints[randNum].transform.position,
            playerSpawnPoints[randNum].transform.rotation);

        // Grabs the components from the newly instantiated game objects to set the controller to the tank
        PlayerController newController = newPlayerController.GetComponent<PlayerController>();
        PlayerTankPawn newPawn = newPawnObject.GetComponent<PlayerTankPawn>();

        // Tank head wouldn't always face the same direction as the tank when it's spawned
        newPawn.tankHead.transform.forward = newPawn.transform.forward;

        // Sets the controller's pawn variable to the player
        newController.pawn = newPawn;
    }

    public void SpawnAI()
    {
        for (int i = 0; i < maxAISpawned; i++)
        {
            int randNum = GetRandomNumberInRange(0, 3);

            switch (randNum)
            {
                case 0:
                    SpawnSittingDuck();
                    break;
                case 1:
                    SpawnNoobGamer();
                    break;
                case 2:
                    SpawnGuard();
                    break;
                case 3:
                    SpawnSharpEars();
                    break;
                default:
                    Debug.Log("Something went wrong with the SpawnAI function.");
                    break;
            }
        }
    }

    public void SpawnSittingDuck()
    {
        // Can't spawn in a new tank if there are no spawn points left
        if (remainingAISpawnPoints.Count != 0)
        {
            GameObject newAIPlayerController = Instantiate(sittingDuckBehavior, Vector3.zero, Quaternion.identity);

            int randNum = GetRandomNumberInRange(0, remainingAISpawnPoints.Count);

            newAIPawnObject = Instantiate(sittingDuckPrefab,
                remainingAISpawnPoints[randNum].transform.position,
                remainingAISpawnPoints[randNum].transform.rotation);

            // Removes the spawn point that was just used so AI don't spawn on top of one another
            remainingAISpawnPoints.Remove(remainingAISpawnPoints[randNum]);

            AIController newAIController = newAIPlayerController.GetComponent<AIController>();
            AITankPawn newAIPawn = newAIPawnObject.GetComponent<AITankPawn>();

            newAIController.pawn = newAIPawn;

            newAIController.GetComponent<AIController>().target = newPawnObject;
        }
    }

    public void SpawnNoobGamer()
    {
        if (remainingAISpawnPoints.Count != 0)
        {
            GameObject newAIPlayerController = Instantiate(noobGamerBehavior, Vector3.zero, Quaternion.identity);

            int randNum = GetRandomNumberInRange(0, remainingAISpawnPoints.Count);

            newAIPawnObject = Instantiate(noobGamerPrefab,
            remainingAISpawnPoints[randNum].transform.position,
            remainingAISpawnPoints[randNum].transform.rotation);

            // Removes the spawn point that was just used so AI don't spawn on top of one another
            remainingAISpawnPoints.Remove(remainingAISpawnPoints[randNum]);

            AIController newAIController = newAIPlayerController.GetComponent<AIController>();
            AITankPawn newAIPawn = newAIPawnObject.GetComponent<AITankPawn>();

            newAIController.pawn = newAIPawn;

            newAIController.GetComponent<AIController>().target = newPawnObject;
        }
    }

    public void SpawnGuard()
    {
        if (remainingAISpawnPoints.Count != 0)
        {
            GameObject newAIPlayerController = Instantiate(guardBehavior, Vector3.zero, Quaternion.identity);

            int randNum = GetRandomNumberInRange(0, remainingAISpawnPoints.Count);

            newAIPawnObject = Instantiate(guardPrefab,
                remainingAISpawnPoints[randNum].transform.position,
                remainingAISpawnPoints[randNum].transform.rotation);

            // Removes the spawn point that was just used so AI don't spawn on top of one another
            remainingAISpawnPoints.Remove(remainingAISpawnPoints[randNum]);

            AIController newAIController = newAIPlayerController.GetComponent<AIController>();
            AITankPawn newAIPawn = newAIPawnObject.GetComponent<AITankPawn>();

            newAIController.pawn = newAIPawn;

            newAIController.GetComponent<AIController>().target = newPawnObject;

            // Will loop the waypoints or travel back and forth at random
            randNum = GetRandomNumberInRange(0, 1);

            if (randNum == 0)
            {
                newAIController.GetComponent<AIController>().isWaypointPathLooping = false;
            }
            else
            {
                newAIController.GetComponent<AIController>().isWaypointPathLooping = true;
            }

            // Gives the Guard a randomly selected pattern of waypoints
            int randWaypoints = GetRandomNumberInRange(0, aIWaypoints.Count);

            // Pulls the waypoint object out of the list of waypoints
            waypointPattern = aIWaypoints[randWaypoints];
            //Debug.Log("aIWaypoints[randWaypoints]: " + aIWaypoints[randWaypoints]);

            for (int i = 0; i < waypointPattern.waypoints.Count; i++)
            {
                //Debug.Log("WaypointPattern.waypoints[i]: " + waypointPattern.waypoints[i]);
                newAIController.GetComponent<AIController>().waypoints.Add(waypointPattern.waypoints[i]);
            }
        }
    }

    public void SpawnSharpEars()
    {
        if (remainingAISpawnPoints.Count != 0)
        {
            GameObject newAIPlayerController = Instantiate(sharpEarsBehavior, Vector3.zero, Quaternion.identity);

            int randNum = GetRandomNumberInRange(0, remainingAISpawnPoints.Count);

            newAIPawnObject = Instantiate(sharpEarsPrefab,
                remainingAISpawnPoints[randNum].transform.position,
                remainingAISpawnPoints[randNum].transform.rotation);

            // Removes the spawn point that was just used so AI don't spawn on top of one another
            remainingAISpawnPoints.Remove(remainingAISpawnPoints[randNum]);

            AIController newAIController = newAIPlayerController.GetComponent<AIController>();
            AITankPawn newAIPawn = newAIPawnObject.GetComponent<AITankPawn>();

            newAIController.pawn = newAIPawn;

            newAIController.GetComponent<AIController>().target = newPawnObject;
        }
    }

    public void SpawnPickupSpawners()
    {
        for (int i = 0; i < maxPickupsSpawned; i++)
        {
            // Can't create new pickup spawners if there are no spawn points left
            if (remainingPickupSpawnerSpawnPoints.Count != 0)
            {
                int randNum = GetRandomNumberInRange(0, remainingPickupSpawnerSpawnPoints.Count);

                int randPickup = GetRandomNumberInRange(0, pickupSpawners.Count);

                // Creates a random pickup spawner in a random pickup spawner spawn point
                GameObject newPickupSpawnerObject = Instantiate(pickupSpawners[randPickup],
                    remainingPickupSpawnerSpawnPoints[randNum].transform.position,
                    remainingPickupSpawnerSpawnPoints[randNum].transform.rotation);

                remainingPickupSpawnerSpawnPoints.Remove(remainingPickupSpawnerSpawnPoints[randNum]);
            }
        }
    }

    public void ResetAvailableAISpawnPoints()
    {
        remainingAISpawnPoints.Clear();

        for (int i = 0; i < aISpawnPoints.Count; i++)
        {
            remainingAISpawnPoints.Add(aISpawnPoints[i]);
        }
    }

    public void ResetAvailablePickupSpawnerSpawnPoints()
    {
        remainingPickupSpawnerSpawnPoints.Clear();

        for (int i = 0; i < pickupSpawnerSpawnPoints.Count; i++)
        {
            remainingPickupSpawnerSpawnPoints.Add(pickupSpawnerSpawnPoints[i]);
        }
    }

    public void RemovePlayerFromPlayers(PlayerController controller)
    {
        players.Remove(controller);
    }

    public void RemoveAIPlayerFromAIPlayers(AIController controller)
    {
        aIPlayers.Remove(controller);
    }

    public int GetRandomNumberInRange(int low, int high)
    {
        int randNum = Random.Range(low, high);

        return randNum;
    }

    public void SetAITargetToPlayerOne()
    {
        foreach(AIController controller in aIPlayers)
        {
            controller.target = players[0].pawn.gameObject;
        }
    }

    public void RespawnPlayer()
    {
        //Debug.Log("Waiting to respawn player.");
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(4f);

        SpawnPlayer();

        yield return new WaitForEndOfFrame();
        SetAITargetToPlayerOne();
    }
}