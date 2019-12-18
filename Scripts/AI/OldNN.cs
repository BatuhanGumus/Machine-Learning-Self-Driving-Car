/*

using System.Collections.Generic;
using System;

public class OldNN
{
    private int[] layer_sizes; //layer_sizess
    private float[][] neurons; //neuron matix
    public float[][][] weights; //weight matrix
    private float fitness = 0; //fitness of the network


    public OldNN(int[] layers) /// Initilizes and neural network with random weights
    {
        //deep copy of layer_sizes of this network 
        this.layer_sizes = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layer_sizes[i] = layers[i];
        }


        //generate matrix
        InitNeurons();
        InitWeights();
    }


    public void CopyNN(OldNN copyNetwork) /// copy info from another NN
    {
        CopyWeights(copyNetwork.weights);
    }

    private void CopyWeights(float[][][] copyWeights) /// copy paste weights
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }


    private void InitNeurons()/// Create neuron matrix
    {
        //Neuron Initilization
        List<float[]> neuronsList = new List<float[]>();

        for (int i = 0; i < layer_sizes.Length; i++) //run through all layer_sizes
        {
            neuronsList.Add(new float[layer_sizes[i]]); //add layer to neuron list
        }

        neurons = neuronsList.ToArray(); //convert list to array
    }


    private void InitWeights()/// Create weights matrix.
    {

        List<float[][]> weightsList = new List<float[][]>(); //weights list which will later will converted into a weights 3D array

        //itterate over all neurons that have a weight connection
        for (int i = 1; i < layer_sizes.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>(); //layer weight list for this current layer (will be converted to 2D array)

            int neuronsInPreviousLayer = layer_sizes[i - 1];

            //itterate over all neurons in this current layer
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //neruons weights

                //itterate over all neurons in the previous layer and set the weights randomly between 0.5f and -0.5
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //give random weights to neuron weights
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }

                layerWeightsList.Add(neuronWeights); //add neuron weights of this current layer to layer weights
            }

            weightsList.Add(layerWeightsList.ToArray()); //add this layer_sizes weights converted into 2D array into weights list
        }

        weights = weightsList.ToArray(); //convert to 3D array
    }

    public float[] 
    (float[] inputs) /// Feed forward this neural network with a given input array
    {
        //Add inputs to the neuron matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        //itterate over all neurons and compute feedforward values 
        for (int i = 1; i < layer_sizes.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;

                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k]; //sum off all weights connections of this neuron weight their values in previous layer
                }

                neurons[i][j] = (float)Math.Tanh(value); //Hyperbolic tangent activation
            }
        }

        return neurons[neurons.Length - 1]; //return output layer
    }

    public void Mutate()/// Mutate neural network weights
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

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
*/




/*
 * using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public  enum ActivationType
{
    hyperbolicTangent
}

public class Neuron
{
    public ActivationType activationType = ActivationType.hyperbolicTangent;
    public Neuron[] inputNeurons;
    public double[] weights;
    public double bias;
    public double value = 0;

    public Neuron()
    {
        bias = 0;
    }

    public Neuron(Neuron[] input_neurons)
    {
        this.inputNeurons = input_neurons;

        InitializeWeights();
    }

    private void InitializeWeights()
    {
        weights = new double[inputNeurons.Length];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        bias = UnityEngine.Random.Range(-1f, 1f);
    }

    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = Randomize(weights[i]);
        }

        bias = Randomize(bias);
    }

    private double Randomize(double inp)
    {
        double randomized = inp;

        //mutate weight value 
        float randomNumber = UnityEngine.Random.Range(0f, 100f);

        if (randomNumber <= 2f)
        { //if 1
            //flip sign of weight
            randomized *= -1f;
        }
        else if (randomNumber <= 4f)
        { //if 2
            //pick random weight between -1 and 1
            randomized = UnityEngine.Random.Range(-0.5f, 0.5f);
        }
        else if (randomNumber <= 6f)
        { //if 3
            //randomly increase by 0% to 100%
            float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
            randomized *= factor;
        }
        else if (randomNumber <= 8f)
        { //if 4
            //randomly decrease by 0% to 100%
            float factor = UnityEngine.Random.Range(0f, 1f);
            randomized *= factor;
        }

        return randomized;
    }

    public void CopyWeight(double[] toCopy)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = toCopy[i];
        }
    }

    public void CalculateValue()
    {
        value = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            value += inputNeurons[i].value * weights[i];
        }

        value += bias;

        value = ActivationFunction(value);
    }

    private double ActivationFunction(double inp)
    {
        return Math.Tanh(inp);
    }
}

public class NeuralNetwork
{
    private int[] layerSizes; //layer sizes
    private Neuron[][] neurons; //neuron matix
    private float fitness = 0; //fitness of the network

    public NeuralNetwork(int[] layer_sizes)
    {
        this.layerSizes = layer_sizes;
        InitializeNeurons();
    }

    private void InitializeNeurons()
    {
        neurons = new Neuron[layerSizes.Length][];

        for (int i = 0; i < layerSizes.Length; i++)
        {
            neurons[i] = new Neuron[layerSizes[i]];

            for (int j = 0; j < layerSizes[i]; j++)
            {
                if (i == 0)
                {
                    neurons[0][j] = new Neuron();
                }
                else
                {
                    neurons[i][j] = new Neuron(neurons[i - 1]);
                }
            }
        }

    }

    public void Mutate()
    {
        for (int i = 1; i < layerSizes.Length; i++)
        {
            for (int j = 0; j < layerSizes[i]; j++)
            {
                neurons[i][j].Mutate();
            }
        }
    }

    public void CopyNN(NeuralNetwork nn)
    {
        CopyNeurons(nn.neurons);
    }

    private void CopyNeurons(Neuron[][] toCopy)
    {
        if (this.neurons.Length != toCopy.Length)
        {
            Debug.Log("Can't copy different weight sizes");
            return;
        }

        for (int i = 1; i < layerSizes.Length; i++)
        {
            for (int j = 0; j < layerSizes[i]; j++)
            {
                this.neurons[i][j].CopyWeight(toCopy[i][j].weights);
            }
        }
    }

    public float[] FeedForward(float[] inp)
    {
        for (int i = 0; i < layerSizes.Length; i++)
        {
            for (int j = 0; j < layerSizes[i]; j++)
            {
                if (i == 0)
                {
                    neurons[0][j].value = inp[j];
                }
                else
                {
                    neurons[i][j].CalculateValue();
                }
            }
        }

        float[] output = new float[layerSizes[layerSizes.Length - 1]];
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = (float) neurons[layerSizes.Length - 1][i].value;
        }

        return output;
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
*/