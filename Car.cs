using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public double speed;

    private GameObject road = GameObject.FindGameObjectWithTag("road");
    // Start is called before the first frame update
 
    void Start()
    {
        int maxRoadSpeed = road.GetComponent<Road>().maxSpeed;
        SetInitialSpeed(maxRoadSpeed); 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInitialSpeed(int max)
    {
        int std = 10; // 10km/h was randomly chosen because we do not know how big the differences in speed are between drivers exactly
        int averageSpeed = max - std; // On average not everyone will be exactly at the speed limit. People prefer to stay a little below it
        int minSpeed = max - 20; // Australia does not allow one to drive more than 20km/h below the speed limit

        System.Random rand = new System.Random();

        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) *
                         System.Math.Sin(2.0 * System.Math.PI * u2);
        double randNormal = averageSpeed + std * randStdNormal;

        if (randNormal > max)
        {
            this.speed = max;
        }else if (randNormal < minSpeed)
        {
            this.speed = minSpeed;
        }
        else
        {
            this.speed = randNormal; 
        }

   
        print(randNormal);
    }
}
