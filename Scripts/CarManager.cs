using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public UIManager uim;

    public TrackBuilder tb;
    public GameObject carPrefab;

    private int spawnAmount = 20;

    private Car[] agents;
    private NeuralNetwork[] nns;
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
        agents = new Car[spawnAmount];
        nns = new NeuralNetwork[spawnAmount];

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject hold = Instantiate(carPrefab, tb.spawnPos, Quaternion.identity);

            agents[i] = hold.GetComponent<Car>();
            agents[i].neuralNetwork = new NeuralNetwork(new int[] {10, 100, 30, 2});
            agents[i].ID = i;

            nns[i] = agents[i].neuralNetwork;
            hold.GetComponent<Car>().carManager = this;
        }
    }

    public void GenComplete()
    {
        Reproduction.Asexual(nns);

        for (int i = 0; i < spawnAmount; i++)
        {
            agents[i].ResetAgent();
            agents[i].transform.position = tb.spawnPos;
            agents[i].transform.rotation = Quaternion.identity;
            
        }
        uim.UpdateGenCounter(++gen);
    }

    public void CheckGenComplete()
    {
        for (int i = 0; i < agents.Length; i++)
        {
            if (agents[i].active == true)
            {
                return;
            }
        }

        GenComplete();
    }
}
