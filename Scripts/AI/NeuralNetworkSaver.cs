using System;
using System.IO;
using UnityEngine;

public static class NeuralNetworkSaver
{
    public static void Save(string fileName, NeuralNetwork NN)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName + ".txt");
        string seperator = ", ";

        using (StreamWriter sw = File.CreateText(filePath))
        {
            // =========== LAYER SIZES =============
            string wright = "Layer Sizes: ";

            for (int i = 0; i < NN.layerSizes.Length; i++)
            {
                wright += NN.layerSizes[i] + seperator;
            }

            wright += "\n";
            // ======================================

            // ================== WIGHTS ==============
            wright += "Weights: ";

            for (int i = 0; i < NN.weights.Length; i++)
            {
                for (int j = 0; j < NN.weights[i].Length; j++)
                {
                    for (int k = 0; k < NN.weights[i][j].Length; k++)
                    {
                        wright += NN.weights[i][j][k] + seperator;
                    }
                }
            }

            wright += "\n";
            // =================================

            wright += "Biases: ";
            // ================= BIASES ==================
            for (int i = 0; i < NN.biases.Length; i++)
            {
                for (int j = 0; j < NN.biases[i].Length; j++)
                {
                    wright += NN.biases[i][j] + seperator;
                }
            }
            // ========================================


            sw.WriteLine(wright);
        }

    }

    public static NeuralNetwork Load(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName + ".txt");

        if (File.Exists(filePath) == true)
        {
            using (StreamReader sr = File.OpenText(filePath))
            {
                NeuralNetwork retNN = null;
                int[] layer_sizes;
                string[] spearator = { ", ", ": "};

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                   // Debug.Log(line);
                    string[] bits = line.Split(spearator, line.Length, StringSplitOptions.RemoveEmptyEntries);

                   /*
                    for (int i = 0; i < bits.Length; i++)
                    {
                        Debug.Log(bits[i]);
                    }
                    */
                    if (bits[0] == "Layer Sizes")
                    {
                        layer_sizes = new int[bits.Length - 1];

                        for (int i = 1; i < bits.Length; i++)
                        {
                            layer_sizes[i - 1] = int.Parse(bits[i]);
                        }

                        retNN = new NeuralNetwork(layer_sizes);
                    }
                    else if (bits[0] == "Weights" && retNN != null)
                    {
                        int count = 1;
                        for (int i = 0; i < retNN.weights.Length; i++)
                        {
                            for (int j = 0; j < retNN.weights[i].Length; j++)
                            {
                                for (int k = 0; k < retNN.weights[i][j].Length; k++)
                                {
                                    retNN.weights[i][j][k] = double.Parse(bits[count]);
                                    count++;
                                }
                            }
                        }
                    }
                    else if (bits[0] == "Biases" && retNN != null)
                    {
                        int count = 1;
                        for (int i = 0; i < retNN.biases.Length; i++)
                        {
                            for (int j = 0; j < retNN.biases[i].Length; j++)
                            {
                                retNN.biases[i][j] = double.Parse(bits[count]);
                                count++;
                            }
                        }
                    }
                }

                return retNN;

            }

        }
        else
        {
            Debug.Log(filePath + " does not exist...");
            return null;
        }

        return null;
    }
}
