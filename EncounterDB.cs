using System.Collections.Generic;
using UnityEngine;

public class EncounterDB : MonoBehaviour
{
    public static List<EncounterType> encounterTypes = new List<EncounterType>()
    {
        EncounterType.NormalFight,
        EncounterType.HardFight,
        EncounterType.Lottery,
        EncounterType.Random,
    };

    public static List<float> encounterWeights = new List<float>()
    {
        0.7f,
        0.1f,
        0.1f,
        0.1f,
    };

    public static EncounterType GetRandomEncounterWeighted()
    {
        List<int> indices = new List<int>();

        for (int i = 0; i < encounterTypes.Count; i++)
        {
            for (int j = 0; j < encounterWeights[i] * 10; j++)
            {
                indices.Add(i);
            }
        }

        int randomIndex = Random.Range(0, indices.Count);

        return encounterTypes[indices[randomIndex]];
    }

    public static EncounterType GetRandomEncounterUnweighted()
    {
        int randomIndex = Random.Range(0, encounterTypes.Count);

        return encounterTypes[randomIndex];
    }
}
