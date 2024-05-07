using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Company_Logic : MonoBehaviour
{
    [SerializeField]
    private Company companyData;

    private EnergySource currentEnergySource;

    [SerializeField]
    private ValueMultipliers equationMultipliers;

    [SerializeField]
    private float unlockCost;

    [SerializeField]
    private GameObject unlocked, locked;

    public void Start()
    {
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    public void UnlockCompany()
    {
        float balanceLeft = GameManager.Instance.GetBalance() - unlockCost;
        if (balanceLeft >= 0)
        {
            companyData.unlocked = true;
            locked.SetActive(false);
            unlocked.SetActive(true);
            IncreaseProduction();
            IncreaseEfficencyLvl();
            IncreasePrice();
        }
        else
        {

        }
    }

    public void IncreaseProduction()
    {
        companyData.currentBuyLvl++;
        CurrentProductionCalculation();
    }

    public void IncreaseEfficencyLvl()
    {
        companyData.currentEfficencyLvl++;
        CurrentProductionCalculation();
    }

    public void IncreasePrice()
    {
        companyData.currentPriceLvl++;
        CurrentPriceCalculation();
    }

    public void CurrentProductionCalculation()
    {
        companyData.currentProductionValue = companyData.initProduction * ((companyData.currentBuyLvl * equationMultipliers.increaseBuyValue) + (companyData.currentEfficencyLvl * equationMultipliers.increaseEffValue) / currentEnergySource.enrgySourceEff);
        CurrentPollutionCalculation();
    }

    public void CurrentPriceCalculation()
    {
        companyData.currentPriceValue = companyData.initPrice * Mathf.Pow(equationMultipliers.increasePriceValue, companyData.currentPriceLvl);
    }

    public void CurrentPollutionCalculation()
    {
        companyData.currentPollutionValue = companyData.initPollution * (Mathf.Pow(companyData.currentBuyLvl, equationMultipliers.increaseBuyValue) - Mathf.Pow(companyData.currentEfficencyLvl, equationMultipliers.increaseEffValue));
    }

    public void NewEnergySource(EnergySource newEnergySource)
    {
        currentEnergySource = newEnergySource;
    }
}
