using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour, IDataPersistance
{
    public static GameManager Instance { get; private set; }

    //Amount of time per month
    [SerializeField]
    private float tickTimer;

    //Players current balance and pollution level
    [SerializeField]
    private float playerBankBalance, currentPollutionLevels;

    private string displayedMoney, fullTextMoney;
    private float moneylength, reducedValue;

    [SerializeField]
    private Color[] correctAmount, incorrectAmount;

    //List of all the unlocked companies
    List<Company> unlockedCompanies = new List<Company>();

    private bool companyUnlocked;

    [SerializeField]
    private Company_Logic[] company_Logic;

    //All energy sources
    [SerializeField]
    private EnergySource currentEnergySources;

    private List<EnergySource> unlockedEnergySources = new List<EnergySource>();

    private bool energySourcesUnlocked;

    [SerializeField]
    private EnergySource_Logic[] energySource_Logic;

    //UI Text for total balance and total pollution
    [SerializeField]
    private TextMeshProUGUI perMonthProfitText, perMonthPollutionText, balanceText, pollutionText;

    [SerializeField]
    private TextMeshProUGUI[] textUICompanyButtons, textUIenergySourceButtons, textProdPerMonth;

    //Audio
    [SerializeField]
    private AudioClip[] musicClips;

    [SerializeField]
    private AudioSource audioSource;

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
        companyUnlocked = false;
        energySourcesUnlocked = false;

        StartCoroutine(PlayMusic());

        UpdateColorValues();

        StartCoroutine(UIUpdateTimer());

        StartCoroutine(MonthlyTick());
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(PlayMusic());
    }

    private void OnDestroy()
    {
        RestartCompanies();
    }

    #region AffectingMoneyValues
    //Monthly tick to update all the values.
    IEnumerator MonthlyTick()
    {
        //Debug.Log("Update");

        if (companyUnlocked)
        {
            playerBankBalance += MonthlyProfit();

            for (int i = 0; i < unlockedCompanies.Count; i++)
            {
                currentPollutionLevels += unlockedCompanies[i].currentPollutionValue;
            }
        }
        //Debug.Log("Money Earned: " + moneyIn);
        //Debug.Log("Players Bank: " + playerBankBalance);

        yield return new WaitForSeconds(tickTimer);

        StartCoroutine(MonthlyTick());
        yield return null;
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

    public float MonthlyPollution()
    {
        float montlyPollution = 0;
        for (int i = 0; i < unlockedCompanies.Count; i++)
        {
            montlyPollution += unlockedCompanies[i].currentPollutionValue;
        }
        return montlyPollution;
    }

    //Updates the players balance.
    public void AffectPlayersBalance(float cost)
    {
        playerBankBalance += cost;
    }
#endregion

    #region CompanyInfo
    //Player buys a new company and is added to the current list
    public void CompanyUnlocked(Company newCompany)
    {
        unlockedCompanies.Add(newCompany);
        companyUnlocked = true;
    }

    //Unlocks new energy source
    public void EnergySourceUnlocked(EnergySource newEnergySource)
    {
        currentEnergySources = newEnergySource;
        unlockedEnergySources.Add(newEnergySource);
        energySourcesUnlocked = true;
    }

    public void EnergySourceDiscovered(GameObject newEnergySource)
    {
        newEnergySource.GetComponent<EnergySource_Logic>().EnergySourceAvailability();
    }

    //Will restart the buy levels to 0 when the game is restarted.
    public void RestartCompanies()
    {
        if (companyUnlocked)
        {
            for (int i = 0; i < unlockedCompanies.Count; i++)
            {
                unlockedCompanies[i].unlocked = false;
                unlockedCompanies[i].currentBuyLvl = 0;
                unlockedCompanies[i].currentEfficencyLvl = 0;
            }
        }
        if (energySourcesUnlocked)
        {
            for (int i = 0; i < unlockedEnergySources.Count; i++)
            {
                unlockedEnergySources[i].availableSource = false;
                unlockedEnergySources[i].unlocked = false;
                unlockedEnergySources[i].currentEffLvl = 0;
            }
        }
    }
    #endregion CompanyInfo

    #region UpdatingTextUI
    IEnumerator UIUpdateTimer()
    {
        UpdateValues(balanceText, playerBankBalance);
        UpdateValues(perMonthProfitText, MonthlyProfit());
        UpdateValues(pollutionText, currentPollutionLevels);
        UpdateValues(perMonthPollutionText, MonthlyPollution());
        for (int i = 0; i < textUICompanyButtons.Length; i++)
        {
            float remainder = Remainder(i,3);
            int arrayPos = (int)Mathf.Floor(i/3);
            //Debug.Log("Current text to update: " + i);
            //Debug.Log("Remainder: " + remainder + " Text Array Pos: " + i + " Company Array Pos: " + arrayPos);

            //Debug.Log(" ");

            if (textUICompanyButtons[i].IsActive() && remainder <= 0.3f) 
            {
                //Debug.Log("Current text to update: " + i);
                //Debug.Log("Remainder: " + remainder);
                //Debug.Log(i);
                UpdateValues(textUICompanyButtons[i], company_Logic[arrayPos].UnlockCostReturn());
                //Debug.Log(company_Logic[arrayPosition].UnlockCostReturn());
            }
            else if(textUICompanyButtons[i].IsActive() && remainder >= 0.4f)
            {
                //Debug.Log("Efficiency");
                UpdateValues(textUICompanyButtons[i], company_Logic[arrayPos].EfficencyCostReturn());
            }
            else if (textUICompanyButtons[i].IsActive())
            {
                //Debug.Log("Production");
                UpdateValues(textUICompanyButtons[i], company_Logic[arrayPos].ProductionCostReturn());
            }
        }

        for (int i = 0; i < textUIenergySourceButtons.Length; i++)
        {
            float remainder = Remainder(i, 2);
            int arrayPos = (int)Mathf.Floor(i / 2);
            if (textUIenergySourceButtons[i].IsActive() && remainder == 0)
            {
                UpdateValues(textUIenergySourceButtons[i], energySource_Logic[arrayPos].UnlockCostReturn());
            }else if (textUIenergySourceButtons[i].IsActive())
            {
                UpdateValues(textUIenergySourceButtons[i], energySource_Logic[arrayPos].UpgradeCostReturn());
            }
        }

        for (int i = 0; i < textProdPerMonth.Length; i++)
        {
            int arrayPos = (int)Mathf.Floor(i / 3);
            if (textProdPerMonth[i].IsActive())
            {
                UpdateValues(textProdPerMonth[i], company_Logic[i].ProdValReturn()); ;
            }
        }

        UpdateButtonColor();

        yield return new WaitForSeconds(0.075f);
        StartCoroutine(UIUpdateTimer());
        yield return null;
    }

    //Updates current text total values for the players balance and pollutions.
    public void UpdateValues(TextMeshProUGUI textPresentation, float cost)
    {
        //10100000
        if (1000000 > cost)
        {
            displayedMoney = $"{ReworkedDecimalPoint(cost, 0.01f, 100)}";
            textPresentation.text = displayedMoney;
        } else if (1000000 <= cost)
        {
            moneylength = Mathf.Floor(Mathf.Log10(cost));
            //Debug.Log("Length of log: " + moneylength + " ");

            //Debug.Log("Entire Value In Text: " + fullTextMoney);

            CorrectValueSize(cost);

            string prefex = CorrectPrefex();

            textPresentation.text = $"{ReworkedDecimalPoint(reducedValue, 0.001f)}" + prefex;
        }

        displayedMoney = " ";

        //perMonthText.text = ReworkedDecimalPoint(MonthlyProfit(), 0.01f, 100).ToString();
        //balanceText.text = TwoDecimalPoint(playerBankBalance).ToString();
        //pollutionText.text = ReworkedDecimalPoint(currentPollutionLevels, 0.01f, 100).ToString();
    }

    public void CorrectValueSize(float cost)
    {
        //Debug.Log(moneylength/3);
        moneylength = moneylength / 3;
        float remainder = moneylength;
        remainder = remainder - Mathf.Floor(moneylength);
        //Debug.Log("Remainder: " + remainder);

        if (remainder <= 0)
        {
            BalanceSize(0, cost);
        } else if (0.3f < remainder && remainder < 0.4f)
        {
            //Debug.Log("Hello");
            BalanceSize(1, cost);
        }
        else
        {
            BalanceSize(2, cost);
        }
    }

    public void BalanceSize(int caseSize, float cost)
    {
        fullTextMoney = cost.ToString("F0");
        for (int i = 0; i < 4 + caseSize; i++)
        {
            displayedMoney += fullTextMoney[i];
        }
        reducedValue = Int32.Parse(displayedMoney);
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

    public void UpdateColorValues()
    {
        for (int i = 0; i < company_Logic.Length; i++)
        {
            //Debug.Log("Which Comanpy: " + i);
            company_Logic[i].ReciveColors(correctAmount, incorrectAmount);
        }

        for (int i = 0; i < energySource_Logic.Length; i++)
        {
            energySource_Logic[i].ReciveColors(correctAmount, incorrectAmount);
        }
    }

    public void UpdateButtonColor()
    {
        for (int i = 0; i < company_Logic.Length; i++)
        {
            //Debug.Log("Which Comanpy: " + i);
            company_Logic[i].ButtonColor();
        }

        for (int i = 0; i < energySource_Logic.Length; i++)
        {
            energySource_Logic[i].ButtonColor();
        }
    }
    #endregion UpdatingTextUI


    public bool ChecksPurcheseAbility(float costOfItem)
    {
        float moneyLeft = playerBankBalance - costOfItem;
        if (moneyLeft < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //Returns a float value updates to the nearest hundth
    public float ReworkedDecimalPoint(float value, float decimalPoint, float roundedNumb = 1f)
    {
        //float reducedValue = value

        float newVal = Mathf.Round(value * roundedNumb) * decimalPoint;
        return newVal;
    }

    public float Remainder(float mainVal, int baseValue)
    {
        mainVal = mainVal / baseValue;
        float remainder = mainVal;
        remainder = remainder - Mathf.Floor(mainVal);
        return remainder;
    }

    public void RecalculateProduction()
    {
        for (int i = 0; i < company_Logic.Length; i++)
        {
            company_Logic[i].CurrentProductionCalculation();
        }
    }

    IEnumerator PlayMusic()
    {
        if (!audioSource.isPlaying) 
        {
            audioSource.clip = musicClips[UnityEngine.Random.Range(0, musicClips.Length)];
            audioSource.Play();
        }

        yield return null;
    }

    //Returns the current player balance
    public float GetBalance()
    {
        return playerBankBalance;
    }

    public float GetEnergySourceEff()
    {
        if (currentEnergySources)
        {
            return currentEnergySources.currentEnrgySourceEff;
        }
        return 1;
    }

    public void LoadData(GameData data)
    {
        this.playerBankBalance = data.playerBankBalance;
        this.currentPollutionLevels = data.currentPollutionLevels;
    }

    public void SaveData(ref GameData data)
    {
        data.currentPollutionLevels = this.currentPollutionLevels;
        data.playerBankBalance = this.playerBankBalance;
    }
}
