using System;

public class NeuralNetwork
{
    public int[] layerSizes; //layer sizes
    private double[][] neurons; //neuron matrix
    public double[][][] weights; //weight matrix
    public double[][] biases; //weight matrix

    private static Random rand = new Random();
    private double RandDouble(double min, double max)
    {
        return rand.NextDouble() * (max - min) + min;
    }

    public NeuralNetwork(int[] _layerSizes)
    {
        this.layerSizes = _layerSizes;

        InitNeurons();
        InitWeights();
    }

    public NeuralNetwork(NeuralNetwork copyNN)
    {
        this.layerSizes = copyNN.layerSizes;

        InitNeurons();
        InitWeights();

        CopyNeuralNetwork(copyNN);
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
                    weights[i - 1][j][k] = RandDouble(-0.5, 0.5);
                }
                biases[i - 1][j] = RandDouble(-0.5, 0.5);
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
       double x = RandDouble(-10.0, 10.0) / absMax;
       return Math.Exp(-Math.Pow(x, 2.0)) * x * 2.331 * absMax;
    }

    public double RandomizeWeight(double inp)
    {
        double retVal = inp;

        double chance = RandDouble(0, 100.0);

        if (chance <= 2f)
        { //if 1
            //flip sign of weight
            retVal *= -1f;
        }
        else if (chance <= 4f)
        { //if 2
            //pick random weight between -1 and 1
            retVal = RandDouble(-0.5, 0.5);
        }
        else if (chance <= 6f)
        { //if 3
            //randomly increase by 0% to 100%
            double factor = RandDouble(1.0, 2.0);
            retVal *= factor;
        }
        else if (chance <= 8f)
        { //if 4
            //randomly decrease by 0% to 100%
            double factor = RandDouble(0f, 1f);
            retVal *= factor;
        }
        
        return retVal;
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