using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Car : MonoBehaviour
{

    private Rigidbody rb;
    private int minDistance;

    private Road road;

    int maxRoadSpeed; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        this.road = GameObject.FindGameObjectWithTag("road").GetComponent<Road>();
        maxRoadSpeed = this.road.maxSpeed;
        double initialSpeed = SetInitialSpeed(maxRoadSpeed);
        Vector3 force = new Vector3((float) -initialSpeed, 0, 0);
        
        this.rb.velocity = force;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int carCount = GameObject.FindGameObjectsWithTag("car").Length;

        if (carCount > 1) {
            float brakingForce = CheckDistanceSpeed(getClosestCar());
            Vector3 velo = this.rb.velocity;

            if (brakingForce >= 0) {
                this.rb.velocity = new Vector3(velo.x + brakingForce, 0, velo.z); //(velo.x + brakingForce, 0, velo.z);
            }else if (brakingForce < 0)
            {
                this.rb.velocity = new Vector3(velo.x + brakingForce, 0, velo.z);
            }
        }
    }

    float getClosestCar() {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        List<float> distances = new List<float>();
        float minDistance = 500F; 
        foreach (GameObject car in cars)
        {
            Vector3 otherCar = car.transform.position;
            Vector3 thisCar = this.rb.transform.position;

            float angle = Vector3.Angle(thisCar, otherCar);

            if (angle < 5) {
                if (thisCar.x > otherCar.x) {
                    float distance = System.Math.Abs(otherCar.x - thisCar.x);
                    
                    if (distance < minDistance) {
                        minDistance = distance;
                    }
                }
            }
        }
        
        //Debug.Log(minDistance);
        return minDistance;
    }

    

    double SetInitialSpeed(int max)
    {
        int std = this.road.std; // 10km/h was randomly chosen because we do not know how big the differences in speed are between drivers exactly
        int averageSpeed = max - std; // On average not everyone will be exactly at the speed limit. People prefer to stay a little below it
        int minSpeed = max - (std * 2); // Australia does not allow one to drive more than 20km/h below the speed limit

        System.Random rand = new System.Random();

        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) *
                        System.Math.Sin(2.0 * System.Math.PI * u2);
        double randNormal = averageSpeed + std * randStdNormal;

        if (randNormal > max) {
            return max;
        } else if (randNormal < minSpeed) {
            return minSpeed;
        } else {
            return randNormal; 
        }
    }

    private float CheckDistanceSpeed(float distance)
    {
        print(distance);

        if (distance != -1 && distance < 15 && this.rb.velocity.x < -maxRoadSpeed+90) {
            Debug.DrawLine(this.rb.position, new Vector3(this.rb.position.x,10,this.rb.position.z), Color.red);
            return this.rb.velocity.sqrMagnitude / 250;
        }else if (distance != -1 && distance > 40 && distance != 500 &&  this.rb.velocity.x > -maxRoadSpeed)
        {
            return -(this.rb.velocity.sqrMagnitude / 800);
        }

        return 0F;
    }
}
