using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private float playerBankBalance;

    [SerializeField]
    private float currentPollutionLevels;

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
        StartCoroutine(MonthlyTick());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MonthlyTick()
    {
        yield return new WaitForSeconds(currentPollutionLevels);

        StartCoroutine(MonthlyTick());
        yield return null;
    }

    public float GetBalance()
    {
        return playerBankBalance;
    }

    public float GetPollutionLevel()
    {
        return currentPollutionLevels;
    }
}
