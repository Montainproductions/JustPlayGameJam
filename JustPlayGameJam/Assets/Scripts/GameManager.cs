using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private float tickTimer;

    [SerializeField]
    private EnergySource_Logic[] codeEnergySource;

    [SerializeField]
    private EnergySource[] allEnergySources;

    private List<EnergySource> unlockedEnergySources;

    private EnergySource currentEnergySource;
    private int currentEnergySourceIndex;

    [SerializeField]
    private float playerBankBalance, currentPollutionLevels;

    private float moneyIn, pollutionIn;

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

    IEnumerator MonthlyTick()
    {
        yield return new WaitForSeconds(tickTimer);
        Debug.Log("Update");

        if (CheckCompaniesUnlocked())
        {
            for (int i = 0; i < unlockedCompanies.Count; i++)
            {
                moneyIn = unlockedCompanies[i].currentProductionValue * unlockedCompanies[i].currentPriceValue;
                pollutionIn = unlockedCompanies[i].currentPollutionValue;
            }
        }

        playerBankBalance += moneyIn;
        currentPollutionLevels += pollutionIn;

        //Debug.Log("Money Earned: " + moneyIn);
        //Debug.Log("Players Bank: " + playerBankBalance);

        UpdateValues();

        StartCoroutine(MonthlyTick());
        yield return null;
    }

    public void CompanyUnlocked(Company newCompany)
    {
        unlockedCompanies.Add(newCompany);
    }

    public void BoughtEnergySource(EnergySource newEnergySrouce)
    {
        currentEnergySource = newEnergySrouce;
    }

    public void EnergySourceDiscovered(int newEnergySource)
    {
        allEnergySources[newEnergySource].availableSource = true;
        codeEnergySource[newEnergySource].EnergySourceAvailability();
        unlockedEnergySources.Add(allEnergySources[newEnergySource]);
    }

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

    public bool CheckCompaniesUnlocked()
    {
        if(unlockedCompanies.Count > 0)
        {
            return true;
        }
        return false;
    }

    public EnergySource GetEnergySource()
    {
        return currentEnergySource;
    }

    public float GetBalance()
    {
        return playerBankBalance;
    }

    public float GetPollutionLevel()
    {
        return currentPollutionLevels;
    }

    public void AffectPlayersBalance(float cost)
    {
        playerBankBalance += cost;
        UpdateValues();
    }

    public void UpdateValues()
    {
        balanceText.text = playerBankBalance.ToString();
        pollutionText.text = currentPollutionLevels.ToString();
    }
}
