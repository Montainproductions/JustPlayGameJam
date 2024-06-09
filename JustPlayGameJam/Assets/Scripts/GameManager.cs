using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //Amount of time per month
    [SerializeField]
    private float tickTimer;

    //Code Base for energy sources
    [SerializeField]
    private EnergySource_Logic[] codeEnergySource;

    //All energy sources
    [SerializeField]
    private EnergySource[] allEnergySources;

    private List<EnergySource> unlockedEnergySources;

    private EnergySource currentEnergySource;

    //Players current balance and pollution level
    [SerializeField]
    private float playerBankBalance, currentPollutionLevels;

    private string displayedMoney, fullTextMoney;
    private float moneylength, reducedBalance;

    //List of all the unlocked companies
    List<Company> unlockedCompanies = new List<Company>();

    //UI Text for total balance and total pollution
    [SerializeField]
    private TextMeshProUGUI perMonthText, balanceText, pollutionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentEnergySource = allEnergySources[0];
        UpdateValues();
        StartCoroutine(MonthlyTick());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        RestartCompanies();
    }

    //Monthly tick to update all the values.
    IEnumerator MonthlyTick()
    {
        yield return new WaitForSeconds(tickTimer);
        Debug.Log("Update");

        if (CheckCompaniesUnlocked())
        {
            playerBankBalance += MonthlyProfit();
            for (int i = 0; i < unlockedCompanies.Count; i++)
            {
                currentPollutionLevels += unlockedCompanies[i].currentPollutionValue;
            }
        }
        //Debug.Log("Money Earned: " + moneyIn);
        //Debug.Log("Players Bank: " + playerBankBalance);

        //Update values
        UpdateValues();

        StartCoroutine(MonthlyTick());
        yield return null;
    }

    //Player buys a new company and is added to the current list
    public void CompanyUnlocked(Company newCompany)
    {
        unlockedCompanies.Add(newCompany);
    }

    //When the player buys a new energy source and it updates the current one in use.
    public void BoughtEnergySource(EnergySource newEnergySrouce)
    {
        currentEnergySource = newEnergySrouce;
    }

    //Unlocks new energy source
    public void EnergySourceDiscovered(int newEnergySource)
    {
        codeEnergySource[newEnergySource].EnergySourceAvailability();
        unlockedEnergySources.Add(allEnergySources[newEnergySource]);
    }

    //Will restart the buy levels to 0 when the game is restarted.
    public void RestartCompanies()
    {
        if (CheckCompaniesUnlocked())
        {
            for (int i = 0; i < unlockedCompanies.Count; i++)
            {
                unlockedCompanies[i].currentBuyLvl = 0;
                unlockedCompanies[i].currentEfficencyLvl = 0;
            }
        }
    }

    //Updates the players balance.
    public void AffectPlayersBalance(float cost)
    {
        playerBankBalance += cost;
        UpdateValues();
    }

    public float MonthlyProfit()
    {
        float montlyProfits = 0;
        for (int i = 0; i < unlockedCompanies.Count; i++)
        {
            montlyProfits += unlockedCompanies[i].currentProductionValue * unlockedCompanies[i].initPrice;
        }
        return montlyProfits;
    }

    //Updates current text total values for the players balance and pollutions.
    public void UpdateValues()
    {
        if(1000000 > playerBankBalance){
            displayedMoney = $"{ReworkedDecimalPoint(playerBankBalance, 0.01f, 100)}";
            balanceText.text = displayedMoney;
        }else if(1000000000 <= playerBankBalance){
            moneylength = Mathf.Log10(playerBankBalance);

            fullTextMoney = playerBankBalance.ToString("F0");

            for (int i = 0; i < 4; i++)
            {
                displayedMoney += fullTextMoney[i];
            }

            CorrectPrefex();

            balanceText.text = displayedMoney;
        }
        displayedMoney = " ";
        perMonthText.text = ReworkedDecimalPoint(MonthlyProfit(), 0.01f, 100).ToString();
        //balanceText.text = TwoDecimalPoint(playerBankBalance).ToString();
        pollutionText.text = ReworkedDecimalPoint(currentPollutionLevels, 0.01f, 100).ToString();
    }

    public string CorrectPrefex()
    {
        //1000000
        switch (moneylength)
        {
            case 6: //Million
            case 7: //Million
            case 8: //Million
                return "Million";
            case 9: //Billion
            case 10: //Billion
            case 11: //Billion
                return "Billion";
            case 12:
            case 13:
            case 14:
                return "Trillion";
            default:
                return "";
        }
    }

    //Checks if the company is currently unlocked
    public bool CheckCompaniesUnlocked()
    {
        if(unlockedCompanies.Count > 0)
        {
            return true;
        }
        return false;
    }

    //Returns a float value updates to the nearest hundth
    public float ReworkedDecimalPoint(float value, float decimalPoint, float roundedNumb = 1f)
    {
        //float reducedValue = value

        float newVal = Mathf.Round(value * roundedNumb) * decimalPoint;
        return newVal;
    }

    public int NumbLength()
    {
        string val = $"{ReworkedDecimalPoint(playerBankBalance, 1, 1)}";
        return val.Length;
    }

    //Returns the current active energy source
    public EnergySource GetEnergySource()
    {
        return currentEnergySource;
    }

    //Returns the current player balance
    public float GetBalance()
    {
        return playerBankBalance;
    }

    //Returns the current pollution level.
    public float GetPollutionLevel()
    {
        return currentPollutionLevels;
    }
}
