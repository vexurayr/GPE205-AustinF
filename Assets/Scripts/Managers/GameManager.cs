using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Reference to itself to reach objects anywhere in the heiarchy
    public static GameManager instance;

    // Things the game manager will need to reference
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;
    public Transform playerSpawnTransform;
    public List<PlayerController> players;
    private GameObject newPawnObject;

    public GameObject aITankPawnPrefab;
    public Transform aIPlayerSpawnTransform;
    public GameObject sittingDuckBehavior;
    public GameObject noobGamerBehavior;
    public GameObject guardBehavior;
    public GameObject sharpEarsBehavior;
    public List<AIController> aIPlayers;

    public List<GameObject> pickups;

    // Runs as soon as this object is enabled, one frame before Start()
    private void Awake()
    {
        players = new List<PlayerController>();
        aIPlayers = new List<AIController>();
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
        SpawnPlayer();
        //SpawnAI();
        SetAITargeting();
    }

    private void SpawnPlayer()
    {
        // Spawns player controller into the scene
        GameObject newPlayerObject = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
        newPawnObject = Instantiate(tankPawnPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);

        Controller newController = newPlayerObject.GetComponent<Controller>();
        Pawn newPawn = newPawnObject.GetComponent<Pawn>();

        newController.pawn = newPawn;
    }

    private void SpawnAI(GameObject behavior)
    {
        // Spawn AI player into scene
        GameObject newAIPlayerObject = Instantiate(behavior, Vector3.zero, Quaternion.identity);
        GameObject newAIPawnObject = Instantiate(aITankPawnPrefab, aIPlayerSpawnTransform.position, aIPlayerSpawnTransform.rotation);

        Controller newAIController = newAIPlayerObject.GetComponent<Controller>();
        Pawn newAIPawn = newAIPawnObject.GetComponent<Pawn>();

        newAIController.pawn = newAIPawn;

        // Set AI to target player in scene
        newAIController.GetComponent<AIController>().target = newPawnObject;
    }

    public void SetAITargeting()
    {
        for (int i = 0; i < aIPlayers.Count; i++)
        {
            aIPlayers[i].target = newPawnObject;
        }
    }
}