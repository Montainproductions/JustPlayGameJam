using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergySource_Logic : MonoBehaviour
{
    [SerializeField]
    private GameObject availabilityBox;

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
        availabilityBox.SetActive(true);
        energySource.availableSource = false;

        unlockCostText.text = unlockCost.ToString();
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    public void UnlockCompany()
    {
        if (energySource.availableSource)
        {
            float balanceLeft = GameManager.Instance.GetBalance() - unlockCost;
            if (balanceLeft >= 0)
            {
                GameManager.Instance.AffectPlayersBalance(-unlockCost);
                locked.SetActive(false);
                unlocked.SetActive(true);

                GameManager.Instance.BoughtEnergySource(energySource);

                IncreaseEfficencyLvl();
            }
            else
            {

            }
        }
    }

    public void IncreaseEfficencyLvl()
    {
        if (energySource.enrgySourceEff < energySource.maxEff)
        {
            energySource.enrgySourceEff += 0.01f;
        }
    }

    public void EnergySourceAvailability(){
        availabilityBox.SetActive(false);
        energySource.availableSource = true;
    }
}
