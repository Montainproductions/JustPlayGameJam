using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnergySource")]
public class EnergySource : ScriptableObject
{
    [System.Serializable]
    public enum TypesEnergySource
    {
        none = 0,
        coal = 1,
        gas = 2,
        oil = 3,
        wind = 4,
        solar = 5,
        nuclear = 6,
    }

    public TypesEnergySource energySource;

    public bool availableSource, unlocked;

    public float unlockCost;

    public float initEffCost;

    public float currentEffCost;

    public float currentEnrgySourceEff, currentEnrgySourceEffScaled, startingEff, maxEff, currentEffLvl;

    public float enrgySourcePollution;

    public float energySourceScaler;
}
