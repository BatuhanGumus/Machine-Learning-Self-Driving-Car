using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarManager : MonoBehaviour
{
    public UIManager uim;

    public TrackBuilder tb;
    public GameObject carPrefab;

    private int spawnAmount = 30;

    private Car[] cars;
    private Agent[] agents;
    private int gen = 1;

    //========= for cars ===========
    public LayerMask trackMask;
    public Color crashCol;
    public Color stopCol;
    public Color originalCol;
    public Color lapDoneColor;
    public int[] sensorDegrees = { 0, 45, 90, 135, -45, -90, -135, 180 };
    public float acceleration = 20;
    public float steering = 8f;
    public float maxSpeed = 10;
    [Space]
    public float hitWallPoints = -8;
    public float rewardGatPoints = 10;
    public float stopPoints = -2;
    public float finishReward = 16;
    //==============================

    public void SpawnCars()
    {
        cars = new Car[spawnAmount];
        agents = new Agent[spawnAmount];

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject hold = Instantiate(carPrefab, tb.spawnPos, Quaternion.identity);

            cars[i] = hold.GetComponent<Car>();
            cars[i].agent = new Agent(new int[] {10, 20, 5, 2});
            cars[i].ID = i;

            agents[i] = cars[i].agent;
            hold.GetComponent<Car>().carManager = this;
        }
    }

    public void GenComplete()
    {
        Reproduction.Asexual(agents);

        for (int i = 0; i < spawnAmount; i++)
        {
            cars[i].ResetCar();
            cars[i].transform.position = tb.spawnPos;
            cars[i].transform.rotation = Quaternion.identity;
            
        }
        uim.UpdateGenCounter(++gen);
    }

    public void CheckGenComplete()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].active == true)
            {
                return;
            }
        }

        GenComplete();
    }

    
}
