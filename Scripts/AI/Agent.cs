using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent
{
    public NeuralNetwork Brain;
    private double fitness = 0; //fitness of the agent
    private static double globalFitness = 0;

    public double ReproductionChance
    {
        get { return fitness / globalFitness; }
    }

    public Agent(int[] _layerSizes)
    {
        Brain = new NeuralNetwork(_layerSizes);
    }

    public void AddFitness(double fit)
    {
        fitness += fit;
        globalFitness += fit;
    }

    public void SetFitness(double fit)
    {
        globalFitness -= fitness;
        fitness = fit;
        globalFitness += fit;
    }

    public double GetFitness()
    {
        return fitness;
    }
}
