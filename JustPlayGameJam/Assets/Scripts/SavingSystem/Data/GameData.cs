using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private float playerBankBalance, currentPollutionLevels;

    //List of all the unlocked companies
    public List<Company> unlockedCompanies = new List<Company>();

    public GameData() 
    {
        this.playerBankBalance = 100;
        this.currentPollutionLevels = 0;
    }

    public void SetPlayerBalance(float balance)
    {
        playerBankBalance = balance;
    }

    public float ReturnPlayerBalance()
    {
        return playerBankBalance;
    }

    public void SetPollution(float pollution)
    {
        currentPollutionLevels = pollution;
    }

    public float ReturnPollution()
    {
        return currentPollutionLevels;
    }
}
