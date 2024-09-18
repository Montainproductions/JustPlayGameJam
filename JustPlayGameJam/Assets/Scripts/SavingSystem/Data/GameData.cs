using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private float perMonthProfit, playerBankBalance, perMonthPollution, currentPollutionLevels;


    public GameData() 
    {
        this.playerBankBalance = 100;
        this.currentPollutionLevels = 0;
        this.perMonthProfit = 0;
        this.perMonthPollution = 0;
    }

    public void SetPlayerBalance(float balance)
    {
        this.playerBankBalance = balance;
    }

    public float ReturnPlayerBalance()
    {
        return playerBankBalance;
    }

    public void SetProfitRate(float perMonthProfit)
    {
        this.perMonthProfit = perMonthProfit;
    }

    public float ReturnProfitRate()
    {
        return perMonthProfit;
    }

    public void SetPollution(float pollution)
    {
        this.currentPollutionLevels = pollution;
    }

    public float ReturnPollution()
    {
        return currentPollutionLevels;
    }

    public void SetPollutionRate(float perMonthPollution)
    {
        this.perMonthPollution = perMonthPollution;
    }

    public float ReturnPollutionRate()
    {
        return perMonthPollution;
    }
}
