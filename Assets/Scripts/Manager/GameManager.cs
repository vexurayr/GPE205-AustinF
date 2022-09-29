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

    // Runs as soon as this object is enabled, one frame before Start()
    private void Awake()
    {
        players = new List<PlayerController>();

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
    }

    private void SpawnPlayer()
    {
        // Spawns player controller into the scene
        GameObject newPlayerObject = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject newPawnObject = Instantiate(tankPawnPrefab, playerSpawnTransform.position, playerSpawnTransform.rotation);

        Controller newController = newPlayerObject.GetComponent<Controller>();
        Pawn newPawn = newPawnObject.GetComponent<Pawn>();

        newController.pawn = newPawn;
    }
}