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

    [SerializeField]
    private GameObject energySource;

    //Texts showing costs for upgrades
    [SerializeField]
    private TextMeshProUGUI[] costText;

    //Texts to show current production.
    [SerializeField]
    private TextMeshProUGUI[] perMonthVals;

    private float perMonthProdVal;

    [SerializeField]
    private CustomButton[] buyButtons;

    private Color[] correctAmount, incorrectAmount;

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
        if (GameManager.Instance.ChecksPurcheseAbility(companyData.initUnlockCost))
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
            if(companyData.unlocksEnrgySource)
            {
                GameManager.Instance.EnergySourceDiscovered(energySource);
            }
        }
        else
        {

        }
    }

    //Updates all the values for when the player initially unlocks the company for the first time.
    public void UnlockCompanyFirst()
    {
        NewsCycle.Instance.IncreaseNewsList(companyData);

        companyData.currentBuyLvl++;
        companyData.currentEfficencyLvl++;

        CurrentProductionCalculation();
        CurrentPollutionCalculation();

        ProductionButtonCost();
        EfficencyButtonCost();
    }

    public void IncreaseProductionLvl()
    {
        //Checks if the player has enough money to buy the upgrade
        if (GameManager.Instance.ChecksPurcheseAbility(companyData.currentProductionCost))
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
        if (GameManager.Instance.ChecksPurcheseAbility(companyData.currentEfficencyCost))
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
        companyData.currentProductionValue = companyData.initProduction * (((companyData.currentBuyLvl * equationMultipliers.increaseBuyValue) + (companyData.currentEfficencyLvl * equationMultipliers.increaseEffValue)) / GameManager.Instance.GetEnergySourceEff());
        //Debug.Log(companyData.currentProductionValue);

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
        perMonthProdVal = GameManager.Instance.ReworkedDecimalPoint(companyData.initPrice * companyData.currentProductionValue, 0.01f, 100);
        perMonthProdVal.ToString();
        perMonthVals[1].text = GameManager.Instance.ReworkedDecimalPoint(companyData.currentPollutionValue, 0.01f, 100).ToString();
    }

    public void ButtonColor()
    {
        if (companyData.unlocked)
        {
            CheckPrice(1, companyData.currentProductionCost);
            CheckPrice(2, companyData.currentEfficencyCost);
        }
        else
        {
            CheckPrice(0, companyData.initUnlockCost);
        }
    }

    public void CheckPrice(int button, float costVal)
    {
        //Debug.Log("Which Button: " + button);
        float leftOver = GameManager.Instance.GetBalance() - costVal;
        //Debug.Log(leftOver);
        //Debug.Log(" ");
        if (leftOver < 0)
        {
            /*Debug.Log(" ");
            Debug.Log("Which Button: " + button);
            Debug.Log("Absolitly Available");
            Debug.Log(" ");*/
            //buyButtons[button].UpdateColors(correctAmount[0], correctAmount[1], correctAmount[2]);
            buyButtons[button].UpdateColors(incorrectAmount[0], incorrectAmount[1], incorrectAmount[2]);
        }
        else
        {
            /*Debug.Log(" ");
            Debug.Log("Which Button: " + button);
            Debug.Log("Not Available");
            Debug.Log(" ");*/
            buyButtons[button].UpdateColors(correctAmount[0], correctAmount[1], correctAmount[2]);
            //buyButtons[button].UpdateColors(incorrectAmount[0], incorrectAmount[1], incorrectAmount[2]);
        }
    }

    public void ReciveColors(Color[] correctAmount, Color[] incorrectAmount)
    {
        this.correctAmount = correctAmount;
        this.incorrectAmount = incorrectAmount;
    }

    public float UnlockCostReturn()
    {
        return companyData.initUnlockCost;
    }

    public float ProductionCostReturn()
    {
        return companyData.currentProductionCost;
    }

    public float EfficencyCostReturn() 
    {
        return companyData.currentEfficencyCost;
    }

    public float ProdValReturn()
    {
        return perMonthProdVal;
    }
}
