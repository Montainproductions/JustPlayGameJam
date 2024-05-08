using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergySource_Logic : MonoBehaviour
{
    [SerializeField]
    private EnergySource energySource;

    [SerializeField]
    private float unlockCost;

    [SerializeField]
    private TextMeshProUGUI unlockCostText;

    [SerializeField]
    private GameObject unlocked, locked;

    // Start is called before the first frame update
    void Start()
    {
        unlockCostText.text = unlockCost.ToString();
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    public void UnlockCompany()
    {
        float balanceLeft = GameManager.Instance.GetBalance() - unlockCost;
        if (balanceLeft >= 0)
        {
            GameManager.Instance.SetPlayersBalance(-1000);
            energySource.unlocked = true;
            locked.SetActive(false);
            unlocked.SetActive(true);

            IncreaseEfficencyLvl();
        }
        else
        {

        }
    }

    public void IncreaseEfficencyLvl()
    {
        if ()
        {
            energySource.enrgySourceEff += 0.01f;
        }
    }
}
