using System.Collections.Generic;
using System;
using System.Diagnostics;

public class NeuralNetwork
{
    private int[] layerSizes; //layer sizes
    private double[][] neurons; //neuron matrix
    private double[][][] weights; //weight matrix
    private float fitness = 0; //fitness of the network

    public NeuralNetwork(int[] _layerSizes)
    {
        this.layerSizes = _layerSizes;

        InitNeurons();
        InitWeights();
    }

    void InitNeurons()
    {
        neurons = new double[layerSizes.Length][];
        for (int i = 0; i < layerSizes.Length; i++)
        {
            neurons[i] = new double[layerSizes[i]];
            for (int j = 0; j < layerSizes[i]; j++)
            {
                neurons[i][j] = new double();
            }
        }
    }

    void InitWeights()
    {
        weights = new double[layerSizes.Length - 1][][];
        for (int i = 1; i < layerSizes.Length; i++)
        {
            weights[i - 1] = new double[layerSizes[i]][];
            for (int j = 0; j < layerSizes[i]; j++)
            {
                weights[i - 1][j] = new double[layerSizes[i - 1] + 1];
                for (int k = 0; k < layerSizes[i - 1] + 1; k++)
                {
                    weights[i - 1][j][k] = (double)UnityEngine.Random.Range(-0.5f, 0.5f);
                }
            }
        }
    }

    public double[] FeedForward(float[] input)
    {
        if (input.Length != neurons[0].Length)
        {
            UnityEngine.Debug.Log("Inputs don't match neuron setup");
            return null;
        }

        for (int i = 0; i < layerSizes[0]; i++)
        {
            neurons[0][i] = input[i];
        }

        for (int i = 1; i < layerSizes.Length; i++)
        {
            for (int j = 0; j < layerSizes[i]; j++)
            {
                double value = 0;

                for (int k = 0; k < layerSizes[i - 1]; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }

                value += weights[i - 1][j][layerSizes[i - 1]];
                neurons[i][j] = (float)Math.Tanh(value); //Hyperbolic tangent activation
            }
        }

        return neurons[neurons.Length - 1];
    }

    public void Mutate()/// Mutate neural network weights
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    double weight = weights[i][j][k];

                    //mutate weight value 
                    float randomNumber = UnityEngine.Random.Range(0f, 100f);

                    if (randomNumber <= 2f)
                    { //if 1
                        //flip sign of weight
                        weight *= -1f;
                    }
                    else if (randomNumber <= 4f)
                    { //if 2
                        //pick random weight between -1 and 1
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    { //if 3
                        //randomly increase by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    { //if 4
                        //randomly decrease by 0% to 100%
                        float factor = UnityEngine.Random.Range(0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public void CopyNeuralNetwork(NeuralNetwork copyNN)
    {
        if (weights.Length != copyNN.weights.Length)
        {
            UnityEngine.Debug.Log("Weights don't match can't copy");
            return;
        }

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyNN.weights[i][j][k];
                }
            }
        }
    }

    public void AddFitness(float fit)
    {
        fitness += fit;
    }

    public void SetFitness(float fit)
    {
        fitness = fit;
    }

    public float GetFitness()
    {
        return fitness;
    }
}