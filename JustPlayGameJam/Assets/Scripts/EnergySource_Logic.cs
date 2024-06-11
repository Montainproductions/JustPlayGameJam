using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnergySource_Logic : MonoBehaviour
{
    [SerializeField]
    private GameObject availabilityBox;

    //Current Energy Source
    [SerializeField]
    private EnergySource energySource;

    //Text showing unlock clost
    [SerializeField]
    private TextMeshProUGUI unlockCostText;

    //Unlocked and locked UI
    [SerializeField]
    private GameObject unlocked, locked;

    // Start is called before the first frame update
    void Start()
    {
        availabilityBox.SetActive(true);
        energySource.availableSource = false;

        unlockCostText.text = energySource.unlockCost.ToString();
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    //Unlock Energy Source
    public void UnlockEnergySource()
    {
        if (energySource.availableSource)
        {
            //Checks if the player has enough money
            float balanceLeft = GameManager.Instance.GetBalance() - energySource.unlockCost;
            if (balanceLeft >= 0)
            {
                //Updates players balance
                GameManager.Instance.AffectPlayersBalance(-energySource.unlockCost);
                
                //Updates UI
                locked.SetActive(false);
                unlocked.SetActive(true);

                //Update current energy source
                //GameManager.Instance.EnergySourceUnlocked(energySource);

                IncreaseEfficencyLvl();
            }
            else
            {

            }
        }
    }

    //Increases efficiency of the energy source
    public void IncreaseEfficencyLvl()
    {
        if (energySource.enrgySourceEff < energySource.maxEff)
        {
            energySource.enrgySourceEff += 0.01f;
        }
    }

    //
    public void EnergySourceAvailability(){
        availabilityBox.SetActive(false);
        energySource.availableSource = true;
    }
}
