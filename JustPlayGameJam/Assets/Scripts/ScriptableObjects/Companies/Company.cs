using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnergySource;

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

    public bool unlocksEnrgySource;

    [Header("Unlock Price")]
    public float initUnlockCost;

    [Header("Initial values")]
    public float initProduction;
    public float initPollution, initPrice;

    [Header("Initial costs for upgrades")]
    public float initBuyCost;
    public float initEfficencyCost;

    [Header("Current levels bought")]
    public float currentBuyLvl;
    public float currentEfficencyLvl;

    [Header("Current calculated values")]
    public float currentProductionValue;
    public float currentPollutionValue, reductionPollution;

    [Header("CurrentButtonCosts")]
    public float currentProductionCost;
    public float currentEfficencyCost;

    [Header("Arbitrary company value")]
    public float companyScaler;

    [Header("Id")]
    [SerializeField]
    public string id;

    [ContextMenu("Generate id GUID")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
}
