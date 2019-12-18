using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Reproduction
{
    public static void Asexual(NeuralNetwork[] currentGen)
    {
        int killOff = (int)(currentGen.Length * 0.5f);
        int[] ordered = OrderAgentsFitness(currentGen);


        for (int i = 0; i < killOff; i++)
        {
            currentGen[ordered[i]].CopyNeuralNetwork(currentGen[ordered[i + killOff]]);
            currentGen[ordered[i]].Mutate();
        }

        for (int i = 0; i < currentGen.Length; i++)
        {
            currentGen[i].SetFitness(0);
        }
    }

    public static int[] OrderAgentsFitness(NeuralNetwork[] ags)
    {
        int[] ordered = new int[ags.Length];
        bool[] added = new bool[ags.Length];


        for (int j = 0; j < ags.Length; j++)
        {
            float lowestFit = float.MaxValue;
            int minID = 0;

            for (int i = 0; i < ags.Length; i++)
            {
                if (added[i] == false && ags[i].GetFitness() < lowestFit)
                {
                    lowestFit = ags[i].GetFitness();
                    minID = i;
                }
            }

            added[minID] = true;
            ordered[j] = minID;
        }

        return ordered;
    }
}
