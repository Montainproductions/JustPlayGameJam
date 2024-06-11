using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;
using static EnergySource;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //Amount of time per month
    [SerializeField]
    private float tickTimer;

    //All energy sources
    [SerializeField]
    private GameObject energySources;

    //Players current balance and pollution level
    [SerializeField]
    private float playerBankBalance, currentPollutionLevels;

    private string displayedMoney, fullTextMoney;
    private float moneylength, reducedBalance;

    //List of all the unlocked companies
    List<Company> unlockedCompanies = new List<Company>();

    [SerializeField]
    private Company_Logic[] company_Logic;

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

    //Unlocks new energy source
    public void EnergySourceUnlocked(GameObject newEnergySource)
    {
        energySources = newEnergySource;
    }

    public void EnergySourceDiscovered(GameObject newEnergySource)
    {
        newEnergySource.GetComponent<EnergySource_Logic>().EnergySourceAvailability();
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
        UpdateButtonColor();

        //10100000
        if (1000000 > playerBankBalance)
        {
            displayedMoney = $"{ReworkedDecimalPoint(playerBankBalance, 0.01f, 100)}";
            balanceText.text = displayedMoney;
        } else if (1000000 <= playerBankBalance)
        {

            moneylength = Mathf.Floor(Mathf.Log10(playerBankBalance));
            //Debug.Log("Length of log: " + moneylength);
            moneylength = moneylength / 3;

            fullTextMoney = playerBankBalance.ToString("F0");
            //Debug.Log("Entire Value In Text: " + fullTextMoney);

            CorrectValueSize();

            string prefex = CorrectPrefex();

            balanceText.text = $"{ReworkedDecimalPoint(reducedBalance, 0.001f)}" + prefex;
        }

        displayedMoney = " ";

        perMonthText.text = ReworkedDecimalPoint(MonthlyProfit(), 0.01f, 100).ToString();
        //balanceText.text = TwoDecimalPoint(playerBankBalance).ToString();
        pollutionText.text = ReworkedDecimalPoint(currentPollutionLevels, 0.01f, 100).ToString();
    }

    public void CorrectValueSize()
    {
        //Debug.Log(moneylength%3);
        float intValue = Mathf.Floor(moneylength);
        float remainder = moneylength % 3;
        remainder = remainder - intValue;
        //Debug.Log("Remainder: " + remainder);

        if (remainder <= 0)
        {
            BalanceSize(0);
        } else if (0.4f > remainder && remainder > 0.3f)
        {
            BalanceSize(1);
        }
        else
        {
            BalanceSize(2);
        }
    }

    public void BalanceSize(int caseSize)
    {
        for (int i = 0; i < 4 + caseSize; i++)
        {
            displayedMoney += fullTextMoney[i];
        }
        reducedBalance = Int32.Parse(displayedMoney);
        //Debug.Log("String: " + displayedMoney);
        //Debug.Log("Float: " + reducedBalance);

        //reducedBalance = ReworkedDecimalPoint(reducedBalance, 0.001f);

        //Debug.Log("Updated Reduced Bal: " + reducedBalance);
    }

    public string CorrectPrefex()
    {
        //1000000
        moneylength = Mathf.Floor(moneylength);
        //Debug.Log("After minimizing: " + moneylength);

        switch (moneylength)
        {
            case 2: //Million
                return " Million";

            case 3: //Billion
                return " Billion";

            case 4: //Trillion
                return " Tillion";

            case 5:
                return " Quattuorion";

            default:
                return "";
        }
    }

    //Updates the players balance.
    public void AffectPlayersBalance(float cost)
    {
        playerBankBalance += cost;
        //Debug.Log("Players Balanace: " + playerBankBalance);
        UpdateValues();
    }

    public bool ChecksPurcheseAbility(float costOfItem)
    {
        float moneyLeft = playerBankBalance - costOfItem;
        if (moneyLeft < 0)
        {
            return false;
        }
        return true;
    }

    //Checks if the company is currently unlocked
    public bool CheckCompaniesUnlocked()
    {
        if (unlockedCompanies.Count > 0)
        {
            return true;
        }
        return false;
    }

    public void UpdateButtonColor()
    {
        for (int i = 0; i < company_Logic.Length; i++)
        {
            company_Logic[i].ButtonColor();
        }
    }

    //Returns a float value updates to the nearest hundth
    public float ReworkedDecimalPoint(float value, float decimalPoint, float roundedNumb = 1f)
    {
        //float reducedValue = value

        float newVal = Mathf.Round(value * roundedNumb) * decimalPoint;
        return newVal;
    }

    public void ButtonColorUpdate()
    {

    }

    //Returns the current player balance
    public float GetBalance()
    {
        return playerBankBalance;
    }

    public float GetEnergySource()
    {
        return 1;
    }
}
