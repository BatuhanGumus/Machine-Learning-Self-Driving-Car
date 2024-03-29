﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public Agent agent;
    public int ID;

    public Rigidbody2D rb;
    public Renderer ren;
    [HideInInspector] public CarManager carManager;

    public bool active = true;

    private float stableTime = 0;

    public float currentSpeed;

    void FixedUpdate()
    {
        if (active == false)
        {
            return;
        }

        
        double[] outs = agent.Brain.FeedForward(GetSensorData());
        DriveCar((float)-outs[0], (float)outs[1]);
        
        /*
        GetSensorData();
        float xInp = -Input.GetAxis("Horizontal");
        float yInp = Input.GetAxis("Vertical");
        DriveCar(xInp, yInp);
        */

        agent.AddFitness(-Time.fixedDeltaTime);

        if (currentSpeed < 0.2f)
        {
            stableTime += Time.fixedDeltaTime;
        }
        else
        {
            stableTime = 0;
        }

        if (stableTime > 5f || agent.GetFitness() < -20)
        {
            agent.AddFitness(carManager.stopPoints);
            ren.material.color = carManager.stopCol;
            active = false;
            carManager.CheckGenComplete();
        }
    }

    private void DriveCar(float x, float y)
    {
        // Calculate speed from input and acceleration (transform.up is forward)
         Vector2 speed = transform.up * (y * carManager.acceleration);
         rb.AddForce(speed);
 
         // Create car rotation
         float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
         if (direction >= 0.0f)
         {
             rb.rotation += x * carManager.steering * (rb.velocity.magnitude / carManager.maxSpeed);
         }
         else
         {
             rb.rotation -= x * carManager.steering * (rb.velocity.magnitude / carManager.maxSpeed);
         }
 
         // Change velocity based on rotation
         float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.left)) * 2f;
         Vector2 relativeForce = Vector2.right * driftForce;
         rb.AddForce(rb.GetRelativeVector(relativeForce));
 
         // Force max speed limit
         if (rb.velocity.magnitude > carManager.maxSpeed)
         {
             rb.velocity = rb.velocity.normalized * carManager.maxSpeed;
         }
         currentSpeed = rb.velocity.magnitude;
    }


    private float[] GetSensorData()
    {
        float[] data = new float[carManager.sensorDegrees.Length + 1];

        for (int i = 0; i < carManager.sensorDegrees.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, carManager.sensorDegrees[i]) * transform.up, Mathf.Infinity, carManager.trackMask);
            if (hit.collider != null)
            {
               // Debug.DrawLine(transform.position, hit.point);
                data[i] = hit.distance / 10.0f;
            }
        }
        
        data[data.Length - 1] = rb.velocity.magnitude / carManager.maxSpeed;
        return data;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Track"))
        {
            rb.velocity = Vector2.zero;
            agent.AddFitness(carManager.hitWallPoints);
            ren.material.color = carManager.crashCol;
            active = false;
            carManager.CheckGenComplete();
        }
    }

    private List<Transform> prevRewardGates = new List<Transform>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("RewardGate"))
        {
            bool newGate = true;
            for (int i = 0; i < prevRewardGates.Count; i++)
            {
                if (prevRewardGates[i] == other.transform)
                {
                    newGate = false;
                    break;
                }
            }

            if (newGate == true)
            {
                agent.AddFitness(carManager.rewardGatPoints);
                prevRewardGates.Add(other.transform);
            }
            
        }
        
        if (other.transform.CompareTag("SpawnLane") && prevRewardGates.Count > 10)
        {
            agent.AddFitness(carManager.finishReward);
            ren.material.color = carManager.lapDoneColor;
            active = false;
            carManager.CheckGenComplete();
        }
        
    }

    public void ResetCar()
    {
        active = true;
        ren.material.color = carManager.originalCol;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        currentSpeed = 0;
        prevRewardGates.Clear();
        stableTime = 0;
    }
}
