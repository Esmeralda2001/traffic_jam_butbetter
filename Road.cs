using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{   
    public GameObject carPrefab;
    public int lanes = 1;
    public int carCount = 10;
    private int currentCarCount = 0;

    public int maxSpeed = 5;
    public int std = 1;

    private System.Random rnd = new System.Random();
    
    private List<Vector3> spawnLocations = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        setSpawnLocations();
        
        // !NOTE: Update currentCarCount so we can update it without updating the unity options UI
        this.currentCarCount = this.carCount;

        StartCoroutine(spawnCars());
    }

    IEnumerator spawnCars()
    {
        int r = rnd.Next(this.spawnLocations.Count);
        Vector3 spawnLocation = this.spawnLocations[r];

        for (int i = 0; i < this.currentCarCount; i++)
        {
            if (Physics.OverlapSphere(spawnLocation, 0.5F).Length <= 1) {
                Instantiate(carPrefab, spawnLocation, Quaternion.identity);
            } else {
                yield return new WaitForSeconds(1);
                this.currentCarCount += 1;
            }
        }
    }

    void setSpawnLocations()
    {
        float groundLevel = 0.5F;
        float lanePadding = 6.7F;
        float startOfLevel = 49F;

        Vector3 midLane = new Vector3(startOfLevel, groundLevel, 0);
        Vector3 rightLane = new Vector3(startOfLevel, groundLevel, lanePadding);
        Vector3 leftLane = new Vector3(startOfLevel, groundLevel, -lanePadding);

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
}
