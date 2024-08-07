using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Company;

public class NewsCycle : MonoBehaviour
{
    public static NewsCycle Instance { get; private set; }

    [SerializeField]
    private List<string> newsTextList;

    [SerializeField]
    private List<string> baseNewsText, corigatedIronNewsText, matchesNewsText, portlandCementNewsText, electricMotorNewsText, steamHammerNewsText, leadAcidBatteryNewsText, lawnMowerNewsText, gasStoveNewsText;

    [SerializeField]
    private TextMeshProUGUI newsText;

    private int randomStory;

    [SerializeField]
    private float minTimePerStory, maxTimePerStory;

    private float timePerStory;

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
        IncreaseNewsList();

        StartCoroutine(NewsFeed());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator NewsFeed()
    {
        randomStory = Random.Range(0, newsTextList.Count);
        newsText.text = newsTextList[randomStory];

        timePerStory = Random.Range(minTimePerStory, maxTimePerStory);

        yield return new WaitForSeconds(timePerStory);

        StartCoroutine(NewsFeed());
    }

    public void IncreaseNewsList(Company company = null)
    {
        if(company.businessName == CompanyName.none)
        {
            newsTextList.AddRange(baseNewsText);
        }
        else if (company.businessName == CompanyName.corrugatedGalvanisedIron)
        {
            newsTextList.AddRange(corigatedIronNewsText);
        }
        else if (company.businessName == CompanyName.matches)
        {
            newsTextList.AddRange(matchesNewsText);
        }
        else if (company.businessName == CompanyName.portlandCement)
        {
            newsTextList.AddRange(portlandCementNewsText);
        }
        else if (company.businessName == CompanyName.electricMotor)
        {
            newsTextList.AddRange(electricMotorNewsText);
        }
        else if (company.businessName == CompanyName.steamHammer)
        {
            newsTextList.AddRange(steamHammerNewsText);
        }
        else if (company.businessName == CompanyName.leadAcidBattery)
        {
            newsTextList.AddRange(leadAcidBatteryNewsText);
        }
        else if (company.businessName == CompanyName.lawnMower)
        {
            newsTextList.AddRange(lawnMowerNewsText);
        }
        else if (company.businessName == CompanyName.gasStoves)
        {
            newsTextList.AddRange(gasStoveNewsText);
        }
    }
}
