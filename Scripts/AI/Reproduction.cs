using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Reproduction
{
    public static void Asexual(Agent[] currentGen)
    {
        int killOff = (int)(currentGen.Length * 0.5f);
        int[] ordered = OrderAgentsOnRepoFitness(currentGen);

        NeuralNetworkSaver.Save("NN1", currentGen[0].Brain);
        NeuralNetworkSaver.Load("NN1");
        /*
        Debug.ClearDeveloperConsole();
        double total = 0;
        for (int i = 0; i < currentGen.Length; i++)
        {
            total += currentGen[ordered[i]].ReproductionChance;
            Debug.Log("Chance: " + currentGen[ordered[i]].ReproductionChance);
        }
        Debug.Log("total: " + total);
        */

        for (int i = 0; i < killOff; i++)
        {
            currentGen[ordered[i]].Brain.CopyNeuralNetwork(currentGen[ordered[i + killOff]].Brain);
            currentGen[ordered[i]].Brain.Mutate();
        }

        for (int i = 0; i < currentGen.Length; i++)
        {
            currentGen[i].SetFitness(10);
        }
    }

    public static int[] OrderAgentsOnRepoFitness(Agent[] ags)
    {
        int[] ordered = new int[ags.Length];
        bool[] added = new bool[ags.Length];


        for (int j = 0; j < ags.Length; j++)
        {
            double lowestFitness = double.MaxValue;
            int minID = 0;

            for (int i = 0; i < ags.Length; i++)
            {
                if (added[i] == false && ags[i].GetFitness() < lowestFitness)
                {
                    lowestFitness = ags[i].GetFitness();
                    minID = i;
                }
            }

            added[minID] = true;
            ordered[j] = minID;
        }

        return ordered;
    }
}
