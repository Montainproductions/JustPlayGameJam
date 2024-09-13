using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float playerBankBalance, currentPollutionLevels;

    public Company[] unlockedCompanies;

    public GameData() 
    {
        this.playerBankBalance = 0;
        this.currentPollutionLevels = 0;

        this.unlockedCompanies = null;
    }
}
