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
    private TextMeshProUGUI unlockCostText, upgradeCostText;

    //Unlocked and locked UI
    [SerializeField]
    private GameObject unlocked, locked;

    [SerializeField]
    private CustomButton[] buyButtons;

    //Texts to show current production.
    [SerializeField]
    private TextMeshProUGUI[] perMonthVals;

    private Color[] correctAmount, incorrectAmount;

    // Start is called before the first frame update
    void Start()
    {
        availabilityBox.SetActive(true);
        energySource.availableSource = false;

        unlockCostText.text = energySource.unlockCost.ToString();
        locked.SetActive(true);
        unlocked.SetActive(false);

        perMonthVals[1].text = energySource.enrgySourcePollution.ToString();
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
                energySource.unlocked = true;
                GameManager.Instance.EnergySourceUnlocked(energySource);

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
        if (energySource.currentEnrgySourceEff > energySource.maxEff)
        {
            GameManager.Instance.RecalculateProduction();
            energySource.currentEffLvl++;
            energySource.currentEnrgySourceEff -= 0.02f;
            energySource.currentEnrgySourceEffScaled = 1 + (1 - energySource.currentEnrgySourceEff);
            energySource.currentEffCost = energySource.initEffCost * Mathf.Pow(energySource.energySourceScaler, energySource.currentEffLvl);
            perMonthVals[0].text = energySource.currentEnrgySourceEffScaled.ToString();
        }
    }

    //
    public void EnergySourceAvailability(){
        availabilityBox.SetActive(false);
        energySource.availableSource = true;
        energySource.currentEnrgySourceEff = energySource.startingEff;
    }

    #region ColorUpdating
    public void ButtonColor()
    {
        if (energySource.unlocked)
        {
            CheckPrice(1, energySource.currentEffCost);
        }
        else
        {
            CheckPrice(0, energySource.unlockCost);
        }
    }

    public void CheckPrice(int button, float costVal)
    {
        float leftOver = GameManager.Instance.GetBalance() - costVal;
        if (leftOver < 0)
        {
            buyButtons[button].UpdateColors(incorrectAmount[0], incorrectAmount[1], incorrectAmount[2]);
        }
        else
        {
            buyButtons[button].UpdateColors(correctAmount[0], correctAmount[1], correctAmount[2]);
        }
    }

    public void ReciveColors(Color[] correctAmount, Color[] incorrectAmount)
    {
        this.correctAmount = correctAmount;
        this.incorrectAmount = incorrectAmount;
    }

    #endregion

    public float UnlockCostReturn()
    {
        return energySource.unlockCost;
    }

    public float UpgradeCostReturn()
    {
        return energySource.currentEffCost;
    }
}
