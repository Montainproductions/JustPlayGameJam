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

    [SerializeField]
    private TextMeshProUGUI[] costText;

    private float constantValues;

    public void Start()
    {
        unlockCostText.text = unlockCost.ToString();
        locked.SetActive(true);
        unlocked.SetActive(false);

        constantValues = 1.3249f;
    }

    public void UnlockCompany()
    {
        float balanceLeft = GameManager.Instance.GetBalance() - unlockCost;
        if (balanceLeft >= 0)
        {
            GameManager.Instance.AffectPlayersBalance(-unlockCost);
            companyData.unlocked = true;
            locked.SetActive(false);
            unlocked.SetActive(true);
            UnlockCompanyFirst();
            UpdateAllButtons();

            if(companyData.unlocksEnrgySource != 0)
            {
                GameManager.Instance.EnergySourceDiscovered(companyData.unlocksEnrgySource);
            }
        }
        else
        {

        }
    }

    public void UnlockCompanyFirst()
    {
        companyData.currentBuyLvl++;
        companyData.currentEfficencyLvl++;
        companyData.currentPriceLvl++;
        CurrentProductionCalculation();
        CurrentPriceCalculation();
        CurrentPollutionCalculation();
    }

    public void IncreaseProduction()
    {
        float remainingUpgradeBalance = GameManager.Instance.GetBalance() - companyData.currentProductionCost;
        if (remainingUpgradeBalance >= 0)
        {
            GameManager.Instance.AffectPlayersBalance(-companyData.currentProductionCost);
            companyData.currentBuyLvl++;
            ProductionButtonCost();
            CurrentProductionCalculation();
        }
    }

    public void IncreaseEfficencyLvl()
    {
        float remainingUpgradeBalance = GameManager.Instance.GetBalance() - companyData.currentEfficencyCost;
        if (remainingUpgradeBalance >= 0)
        {
            GameManager.Instance.AffectPlayersBalance(-companyData.currentEfficencyCost);
            companyData.currentEfficencyLvl++;
            EfficencyButtonCost();
            CurrentProductionCalculation();
        }
    }

    public void IncreasePrice()
    {
        float remainingUpgradeBalance = GameManager.Instance.GetBalance() - companyData.currentPriceCost;
        if (remainingUpgradeBalance >= 0)
        {
            GameManager.Instance.AffectPlayersBalance(-companyData.currentPriceCost);
            companyData.currentPriceLvl++;
            PriceButtonCost();
            CurrentPriceCalculation();
        }
    }

    public void CurrentProductionCalculation()
    {
        companyData.currentProductionValue = companyData.initProduction * ((companyData.currentBuyLvl * equationMultipliers.increaseBuyValue) + (companyData.currentEfficencyLvl * equationMultipliers.increaseEffValue) / GameManager.Instance.GetEnergySource().enrgySourceEff);
        //Debug.Log(companyData.currentProductionValue);
        CurrentPollutionCalculation();
    }

    public void CurrentPriceCalculation()
    {
        companyData.currentPriceValue = companyData.initPrice * Mathf.Pow(equationMultipliers.increasePriceValue, companyData.currentPriceLvl);
    }

    public void CurrentPollutionCalculation()
    {
        float expontialVal = 44.5311f * companyData.initPollution;
        companyData.currentPollutionValue = 1.70721f*Mathf.Pow(companyData.currentBuyLvl, expontialVal) - (constantValues / companyData.currentEfficencyLvl);
    }

    public void UpdateAllButtons()
    {
        PriceButtonCost();
        ProductionButtonCost();
        EfficencyButtonCost();
    }

    public void ProductionButtonCost()
    {
        companyData.currentProductionCost = (companyData.initBuyCost * companyData.currentBuyLvl) * constantValues;
        //Debug.Log(companyData.currentProductionCost);
        costText[0].text = companyData.currentProductionCost.ToString();
    }

    public void PriceButtonCost()
    {
        companyData.currentPriceCost = (companyData.initPriceCost * companyData.currentPriceLvl) * constantValues;
        //Debug.Log(companyData.currentPriceCost);
        costText[1].text = companyData.currentPriceCost.ToString();
    }

    public void EfficencyButtonCost()
    {
        companyData.currentEfficencyCost = (companyData.initEfficencyCost * companyData.currentEfficencyLvl) * constantValues;
        costText[2].text = companyData.currentEfficencyCost.ToString();
    }
}
