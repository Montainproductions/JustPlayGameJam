using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CompanyData")]
public class Company : ScriptableObject
{
    [System.Serializable]
    public enum CompanyName
    {
        none = 0,
        matches = 1,
        corrugatedGalvanisedIron = 2,
        portlandCement = 3,
        electricMotor = 4,
        steamHammer = 5,
        leadAcidBattery = 6,
        lawnMower = 7,
        gasStoves = 8,
        stainlessSteel = 9,
        streetLanterns = 10,
        treeFarm = 11,
        dieselEngine = 12,
        recharchableBatteries = 13,
        syntheticPlastic = 14,
        windTubrines = 15,
        nuclearPowerPlant = 19,
    }

    [System.Serializable]
    public enum EnergySourceUnlock
    {
        none = 0,
        coal = 1,
        gas = 2,
        oil = 3,
        wind = 4,
        solar = 5,
        nuclear = 6,
    }

    public bool unlocked;

    public CompanyName businessName;

    public EnergySourceUnlock unlocksEnrgySource;

    //Initial values
    public float initProduction, initPrice, initPollution;

    //Current level
    public float currentBuyLvl, currentEfficencyLvl, currentPriceLvl;

    //Current values
    public float currentProductionValue, currentPriceValue, currentPollutionValue, reductionPollution;
}
