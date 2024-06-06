using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

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

    //List of all the unlocked companies
    List<Company> unlockedCompanies = new List<Company>();


    [SerializeField]
    private TextMeshProUGUI balanceText, pollutionText;

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
            for (int i = 0; i < unlockedCompanies.Count; i++)
            {
                playerBankBalance += unlockedCompanies[i].currentProductionValue * unlockedCompanies[i].currentPriceValue;
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
                unlockedCompanies[i].currentPriceLvl = 0;
            }
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

    //Returns a float value updates to the nearest hundth value
    public float TwoDecimalPoint(float newVal)
    {
        float simplifiedVal = Mathf.Round(newVal * 100) * 0.01f;
        return simplifiedVal;
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

    //Updates the players balance.
    public void AffectPlayersBalance(float cost)
    {
        playerBankBalance += cost;
        UpdateValues();
    }

    //Updates current text total values for the players balance and pollutions.
    public void UpdateValues()
    {
        balanceText.text = TwoDecimalPoint(playerBankBalance).ToString();
        pollutionText.text = TwoDecimalPoint(currentPollutionLevels).ToString();
    }
}
