using System.Collections.Generic;
using System;
using System.Diagnostics;
using Random = UnityEngine.Random;

public class NeuralNetwork
{
    private int[] layerSizes; //layer sizes
    private double[][] neurons; //neuron matrix
    private double[][][] weights; //weight matrix
    private double[][] biases; //weight matrix
    

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
        biases = new double[layerSizes.Length - 1][];
        for (int i = 1; i < layerSizes.Length; i++)
        {
            weights[i - 1] = new double[layerSizes[i]][];
            biases[i - 1] = new double[layerSizes[i]];
            for (int j = 0; j < layerSizes[i]; j++)
            {
                weights[i - 1][j] = new double[layerSizes[i - 1]];
                for (int k = 0; k < layerSizes[i - 1]; k++)
                {
                    weights[i - 1][j][k] = (double)UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                biases[i - 1][j] = (double)UnityEngine.Random.Range(-0.5f, 0.5f);
            }
        }
    }

    private double Activation(double inp)
    {
        return Math.Tanh(inp); //Hyperbolic tangent activation
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

                value += biases[i - 1][j];
                neurons[i][j] = Activation(value);
            }
        }

        return neurons[neurons.Length - 1];
    }

    private double CustomRandValue(double absMax, double variance)
    {
        double x = Random.Range(-10f, 10f) / absMax;
        return Math.Exp(-Math.Pow(x, 2.0)) * x * 2.331 * absMax;
    }

    public double RandomizeWeight(double inp)
    {
        double retVal = inp;

        if (Random.value < 0.5f)
        {
            retVal += CustomRandValue(0.5, 0.5);
        }
        return retVal;

        /*
        if (randomNumber <= 2f)
        { //if 1
            //flip sign of weight
            retVal *= -1f;
        }
        else if (randomNumber <= 4f)
        { //if 2
            //pick random weight between -1 and 1
            retVal = UnityEngine.Random.Range(-0.5f, 0.5f);
        }
        else if (randomNumber <= 6f)
        { //if 3
            //randomly increase by 0% to 100%
            float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
            retVal *= factor;
        }
        else if (randomNumber <= 8f)
        { //if 4
            //randomly decrease by 0% to 100%
            float factor = UnityEngine.Random.Range(0f, 1f);
            retVal *= factor;
        }
        */
    }

    public void Mutate()/// Mutate neural network weights
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = RandomizeWeight(weights[i][j][k]);
                }
            }
        }

        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                biases[i][j] = RandomizeWeight(biases[i][j]);
            }
        }
    }

    public void CopyNeuralNetwork(NeuralNetwork copyNN)
    {
        if (weights.Length != copyNN.weights.Length || biases.Length != copyNN.biases.Length)
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

        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                biases[i][j] = copyNN.biases[i][j];
            }
        }
    }

    
}