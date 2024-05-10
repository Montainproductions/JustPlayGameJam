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

    public bool unlocked;

    public CompanyName businessName;

    public int unlocksEnrgySource;

    [Header("Initial values")]
    public float initProduction;
    public float initPrice, initPollution;

    [Header("Initial costs for upgrades")]
    public float initBuyCost;
    public float initPriceCost, initEfficencyCost;

    [Header("Current levels bought")]
    public float currentBuyLvl;
    public float currentPriceLvl, currentEfficencyLvl;

    [Header("Current calculated values")]
    public float currentProductionValue;
    public float currentPriceValue, currentPollutionValue, reductionPollution;

    [Header("CurrentButtonCosts")]
    public float currentProductionCost;
    public float currentPriceCost, currentEfficencyCost;

    [Header("Arbitrary company value")]
    public float companyValAffecter;
}
