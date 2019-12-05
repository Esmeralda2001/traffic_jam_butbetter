using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public double speed;

    private Rigidbody rb;
    private Vector3 frontalrange;
    private int minDistance;

    private SphereCollider coll;
    private Road road;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        this.coll = rb.GetComponent<SphereCollider>();
        
        this.road = GameObject.FindGameObjectWithTag("road").GetComponent<Road>();
        int maxRoadSpeed = this.road.maxSpeed;
        double initialSpeed = SetInitialSpeed(maxRoadSpeed);
        Vector3 force = new Vector3((float)-initialSpeed, 0, 0);
        this.rb.AddForce(force);
        // this.rb.velocity = new Vector3((float) -initialSpeed, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // // frontalrange = transform.TransformDirection(Vector3.left);
        // // float temp = (float)this.speed;
        // // rb.velocity += new Vector3(temp,0,0);
        // // Physics.Raycast(transform.position, frontalrange, minDistance);

        // Debug.Log(getClosestCar());
        int carCount = GameObject.FindGameObjectsWithTag("car").Length;

        if (carCount > 1) {
            double speed = CheckDistanceSpeed(getClosestCar());
            // Debug.Log(speed);

            if (speed > 0) {
                // this.rb.velocity = new Vector3((float) speed, 0,0);
                this.rb.AddForce(new Vector3((float) -speed, 0, 0));
            }
        }
    }

    float getClosestCar() {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("car");
        float closestCar = 0F;

        foreach (GameObject car in cars)
        {
            float currentX = this.rb.transform.position.x;
            float otherX = car.transform.position.x;
            float distance = otherX - currentX;
            
            if (distance > closestCar) {
                closestCar = distance;
            }
        }

        return closestCar;
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

    // // double CheckDistanceSpeed(double distance)
    // // {
    // //     double speedMeterSec = this.rb.velocity.x / 3.6;
    // //     double minDistance = speedMeterSec * 2; 
    // //
    // //     if (minDistance > distance) {
    // //         return ((distance / 2) * 3.6);// - this.road.std; 
    // //     } else {
    // //         return this.rb.velocity.x;
    // //     }
    // // }

    private float CheckDistanceSpeed(float targetPos)
    {
        float remainingDistance = (targetPos - this.rb.position.x);
        float brake = 0.5f * this.rb.velocity.sqrMagnitude / remainingDistance;
        return brake;
    }
}
