using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySource : ScriptableObject
{
    [System.Serializable]
    public enum TypesOfEnergySource
    {
        none = 0,
        coal = 1,
        gas = 2,
        oil = 3,
        wind = 4,
        solar = 5,
        nuclear = 6,
    }

    public TypesOfEnergySource currentEnergySource;

    public float enrgySourceEff, enrgySourcePollution;
}
