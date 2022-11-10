using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public int mapSeed;
    public bool isUsingDailySeed;

    // Limited to 10 for now, any more than that and performance drops substantially
    [Range(1, 10)] public int mapRows;
    [Range(1, 10)] public int mapColumns;

    // Tiles should always be square, tells how far apart to spawn new tiles
    public int tileSize;

    public List<GameObject> mapTiles;
    public GameObject wallPrefab;

    private int currentRow = 0;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    // Start is called before the first frame update
    void Awake()
    {
        // Don't generate new List<GameObject>() if designer feeds it GameObjects

        GenerateMap();
    }

    public void GenerateMap()
    {
        if (isUsingDailySeed)
        {
            GetDailySeed();
        }
        // If no seed is set, it will be set to the moment in time the game runs
        else if (mapSeed == 0)
        {
            string seed = DateTime.Now.Millisecond.ToString() + DateTime.Now.Second.ToString() +
                DateTime.Now.Minute.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Day.ToString() +
                DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            mapSeed = (int)Convert.ToInt64(seed);
        }

        Random.InitState(mapSeed);

        GenerateRow();
    }

    // Starts generating tiles at (0,0,0) and moves right and down in intervals of tileSize
    public void GenerateRow()
    {
        for(int i = 0; i < mapColumns; i++)
        {
            // Find the right location to spawn the next tile/wall at
            spawnPosition = new Vector3(currentRow * tileSize, 0, i * tileSize);

            // Spawn the wall on the far left side of the row
            if (i == 0)
            {
                Vector3 position = spawnPosition;
                position.z -= 20;
                Quaternion rotation = Quaternion.Euler(0, 0, 0);

                Instantiate(wallPrefab, position, rotation);
            }

            // Spawn the wall on the far right side of the row
            if (i == mapColumns - 1)
            {
                Vector3 position = spawnPosition;
                position.z += 20;
                Quaternion rotation = Quaternion.Euler(0, 0, 0);

                Instantiate(wallPrefab, position, rotation);
            }

            // Spawn the wall on the top of the first row
            if (currentRow == 0)
            {
                Vector3 position = spawnPosition;
                position.x -= 20;
                Quaternion rotation = Quaternion.Euler(0, 90, 0);

                Instantiate(wallPrefab, position, rotation);
            }

            // Spawn the wall on the bottom of the last row
            if (currentRow == mapRows - 1)
            {
                Vector3 position = spawnPosition;
                position.x += 20;
                Quaternion rotation = Quaternion.Euler(0, 90, 0);

                Instantiate(wallPrefab, position, rotation);
            }

            // Random ground tile rotation
            int randRotation = GetRandomRotation();
            spawnRotation = Quaternion.Euler(0, randRotation, 0);

            // Spawn random ground tile at given position with given rotation
            Instantiate(mapTiles[GetRandomNumberInRange(0, mapTiles.Count)], spawnPosition, spawnRotation);
        }

        MoveUpRow();
    }

    // Increments currentRow so GenerateRow will place tiles an additional tileSize higher than before
    public void MoveUpRow()
    {
        currentRow++;

        if (currentRow < mapRows)
        {
            GenerateRow();
        }
    }

    public void GetDailySeed()
    {
        // Will get 1152022 instead of 2038
        string day = DateTime.Today.Day.ToString();
        string month = DateTime.Today.Month.ToString();
        string year = DateTime.Today.Year.ToString();

        int dailySeed = Convert.ToInt32(day + month + year);

        mapSeed = dailySeed;
    }

    public int GetRandomNumberInRange(int low, int high)
    {
        int randomNum = Random.Range(low, high);

        return randomNum;
    }

    // Get random 90 degree rotation, 0, 90, 180, or 270
    public int GetRandomRotation()
    {
        int randNumb = GetRandomNumberInRange(0, 3);
        int randRotation = 0;

        if (randNumb == 0)
        {
            randRotation = 0;
        }
        else if (randNumb == 1)
        {
            randRotation = 90;
        }
        else if (randNumb == 2)
        {
            randRotation = 180;
        }
        else if (randNumb == 3)
        {
            randRotation = 270;
        }

        return randRotation;
    }
}