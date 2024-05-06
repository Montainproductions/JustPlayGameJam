using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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
    public enum EnergySource
    {
        none = 0,
        coal = 1,
        gas = 2,
        oil = 3,
        wind = 4,
        solar = 5,
        nuclear = 6,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
