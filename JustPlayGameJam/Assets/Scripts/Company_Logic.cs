using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Company_Logic : MonoBehaviour
{
    [SerializeField]
    private Company companyData;

    [SerializeField]
    private ValueMultipliers equationMultipliers;

    [SerializeField]
    private float unlockCost;

    [SerializeField]
    private TextMeshProUGUI unlockCostText;

    [SerializeField]
    private GameObject unlocked, locked;

    public void Start()
    {
        unlockCostText.text = unlockCost.ToString();
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    public void UnlockCompany()
    {
        float balanceLeft = GameManager.Instance.GetBalance() - unlockCost;
        if (balanceLeft >= 0)
        {
            GameManager.Instance.SetPlayersBalance(-1000);
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
        Debug.Log(companyData.currentBuyLvl);
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
        companyData.currentProductionValue = companyData.initProduction * ((companyData.currentBuyLvl * equationMultipliers.increaseBuyValue) + (companyData.currentEfficencyLvl * equationMultipliers.increaseEffValue) / GameManager.Instance.GetEnergySource().enrgySourceEff);
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
}
