using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Company companyData;

    [SerializeField]
    private EnergySource currentEnergySource;

    [SerializeField]
    private float increaseProductionValue, increasePriceValue, increaseEffValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UnlockCompany()
    {
        companyData.unlocked = true;
    }

    public void IncreaseProduction()
    {
        companyData.currentProductionLvl++;
        CurrentProductionCalculation();
    }

    public void CurrentProductionCalculation()
    {
        companyData.currentProductionValue = companyData.initProduction * ((companyData.currentProductionLvl * increaseProductionValue) + (companyData.currentEfficencyLvl * increaseEffValue)/currentEnergySource.enrgySourceEff);
    }

    public void IncreasePrice()
    {
        companyData.currentPriceLvl++;
    }

    public void CurrentPriceCalculation()
    {
        companyData.currentPriceValue = companyData.initPrice * Mathf.Pow(increasePriceValue, companyData.currentPriceLvl);
    }

    public void IncreasePollutionLvl()
    {
        companyData.currentPollutionLvl++;
    }

    public void CurrentPollutionCalculation()
    {
        companyData.currentPollutionValue = companyData.initPollution * (Mathf.Pow(companyData.currentProductionLvl, 1.15f) - Mathf.Pow(companyData.currentEfficencyLvl, 1.045f));
    }

    public void NewEnergySource(EnergySource newEnergySource)
    {
        currentEnergySource = newEnergySource;
    }
}
