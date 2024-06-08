using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Company_Logic : MonoBehaviour
{
    //Company data
    [SerializeField]
    private Company companyData;

    [SerializeField]
    private ValueMultipliers equationMultipliers;

    //Unlock cost
    [SerializeField]
    private TextMeshProUGUI unlockCostText;

    //Unlocked and locked UI options
    [SerializeField]
    private GameObject unlocked, locked;

    //Texts showing costs for upgrades
    [SerializeField]
    private TextMeshProUGUI[] costText;

    //Texts to show current production.
    [SerializeField]
    private TextMeshProUGUI[] perMonthVals;

    public void Start()
    {
        unlockCostText.text = companyData.initUnlockCost.ToString();
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    //Player unlocks/buys a company.
    public void UnlockCompany()
    {
        //Checks if the player has enough money to buy it.
        float balanceLeft = GameManager.Instance.GetBalance() - companyData.initUnlockCost;
        if (balanceLeft >= 0)
        {
            //Updates players balance
            GameManager.Instance.AffectPlayersBalance(-companyData.initUnlockCost);
            
            //Adds to list of companies that have been unlocked.
            GameManager.Instance.CompanyUnlocked(companyData);

            companyData.unlocked = true;
            
            //Updates the UI that appears for the company.
            locked.SetActive(false);
            unlocked.SetActive(true);

            //Sets correct values for the company when first unlocked
            UnlockCompanyFirst();

            //Checks if the company also unlocks a power source
            if(companyData.unlocksEnrgySource != 0)
            {
                GameManager.Instance.EnergySourceDiscovered(companyData.unlocksEnrgySource);
            }
        }
        else
        {

        }
    }

    //Updates all the values for when the player initially unlocks the company for the first time.
    public void UnlockCompanyFirst()
    {
        companyData.currentBuyLvl++;
        companyData.currentEfficencyLvl++;

        CurrentProductionCalculation();
        CurrentPollutionCalculation();

        ProductionButtonCost();
        EfficencyButtonCost();
    }

    public void IncreaseProduction()
    {
        //Checks if the player has enough money to buy the upgrade
        float remainingUpgradeBalance = GameManager.Instance.GetBalance() - companyData.currentProductionCost;
        if (remainingUpgradeBalance >= 0)
        {
            //Update player balance
            GameManager.Instance.AffectPlayersBalance(-companyData.currentProductionCost);
            companyData.currentBuyLvl++;

            //Update the cost of the next upgrade
            ProductionButtonCost();

            //Updates current production per month
            CurrentProductionCalculation();
        }
    }

    //Increases the efficency of the company. Helps improve how much is produced per month.
    public void IncreaseEfficencyLvl()
    {
        //Checks if the player has enough money to buy the upgrade
        float remainingUpgradeBalance = GameManager.Instance.GetBalance() - companyData.currentEfficencyCost;
        if (remainingUpgradeBalance >= 0)
        {
            //Updates players balance
            GameManager.Instance.AffectPlayersBalance(-companyData.currentEfficencyCost);
            companyData.currentEfficencyLvl++;
            
            //Update the cost of the next upgrade
            EfficencyButtonCost();

            //Updates current production per month
            CurrentProductionCalculation();
        }
    }

    //Calculated the amount produced per month of the item.
    public void CurrentProductionCalculation()
    {
        companyData.currentProductionValue = companyData.initProduction * ((companyData.currentBuyLvl * equationMultipliers.increaseBuyValue) + (companyData.currentEfficencyLvl * equationMultipliers.increaseEffValue) / GameManager.Instance.GetEnergySource().enrgySourceEff);
        //Debug.Log(companyData.currentProductionValue);

        //Updates text values
        PerMonthValues();

        //Updated pollution produced per month.
        CurrentPollutionCalculation();
    }

    //Calculates how much pollution is currently being made per month.
    public void CurrentPollutionCalculation()
    {
        float expontialVal = 44.5311f * companyData.initPollution;
        companyData.currentPollutionValue = 1.70721f*Mathf.Pow(companyData.currentBuyLvl, expontialVal) - (companyData.currentEfficencyLvl / 1.3249f);
        
        //Updates text values
        PerMonthValues();
    }

    /// <summary>
    /// All button costs and respective text areas are updated.
    /// Cost values are based on some initial cost of the item when at level one (1) times whatever the current level is and all multiplied by a constant value. 
    /// The constant value (1.3249) is a float value I found during testing would help return updated values that where closer to what seemed to be properly balanced values.
    /// I also make sure  to round the values to the closest second decimal point as to reduce the amount of text thats shown on the screen. Also most people wont be reading past the first two decimal points so it dosent matter as much.
    /// </summary>

    public void ProductionButtonCost()
    {
        companyData.currentProductionCost = companyData.initBuyCost * Mathf.Pow(companyData.companyScaler, companyData.currentBuyLvl);
        //Debug.Log(companyData.currentProductionCost);
        costText[0].text = GameManager.Instance.ReworkedDecimalPoint(companyData.currentProductionCost, 0.01f, 100).ToString();
    }

    public void EfficencyButtonCost()
    {
        companyData.currentEfficencyCost = companyData.initEfficencyCost * Mathf.Pow(companyData.companyScaler, companyData.currentEfficencyLvl);
        costText[1].text = GameManager.Instance.ReworkedDecimalPoint(companyData.currentEfficencyCost, 0.01f, 100).ToString();
    }

    //Updates text on how much is produced per month
    public void PerMonthValues()
    {
        GameManager.Instance.UpdateValues();
        perMonthVals[0].text = GameManager.Instance.ReworkedDecimalPoint(companyData.initPrice * companyData.currentProductionValue, 0.01f, 100).ToString();
        perMonthVals[1].text = GameManager.Instance.ReworkedDecimalPoint(companyData.currentPollutionValue, 0.01f, 100).ToString();
    }
}
