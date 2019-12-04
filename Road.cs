using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{   
    public GameObject carPrefab;
    public int lanes = 1;
    public int carCount = 10;
    private List<Vector3> spawnLocations = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        setSpawnLocations();
        spawnCars();
    }

    void spawnCars() {
        for (int i = 0; i < carCount; i++)
        {
            System.Random rnd = new System.Random();
            int r = rnd.Next(this.spawnLocations.Count);
            Vector3 spawnLocation = this.spawnLocations[r];
            Instantiate(carPrefab, spawnLocation, Quaternion.identity);
        }
            
    }

    void setSpawnLocations() {
        float groundLevel = 0F;
        float lanePadding = 6.7F;
        float startOfLevel = 50F;

        Vector3 midLane = new Vector3(0, groundLevel, startOfLevel);
        Vector3 rightLane = new Vector3(-lanePadding, groundLevel, startOfLevel);
        Vector3 leftLane = new Vector3(lanePadding, groundLevel, startOfLevel);

        if (this.lanes == 1)
        {
            this.spawnLocations.Add(midLane);
        }

        if (this.lanes == 2)
        {
            this.spawnLocations.Add(leftLane);
            this.spawnLocations.Add(midLane);
        }

        if (this.lanes == 3)
        {
            this.spawnLocations.Add(leftLane);
            this.spawnLocations.Add(midLane);
            this.spawnLocations.Add(rightLane);
        }

        this.spawnLocations.ForEach(lane => Debug.Log(lane));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
