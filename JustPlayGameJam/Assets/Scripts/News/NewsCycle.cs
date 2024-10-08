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
    private float minTimePerStory, maxTimePerStory;

    [SerializeField]
    private List<string> newsTextList;

    [SerializeField]
    private List<string> baseNewsText, goodNewsTextPart1, badNewsTextPart1, goodNewsTextPart2, badNewsTextPart2;

    [SerializeField]
    private TextMeshProUGUI newsText;

    private int randomStory;

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
        if(company == null)
        {
            newsTextList.AddRange(baseNewsText);
        }
        else if (company.businessName == CompanyName.matches)
        {
            newsTextList.AddRange(goodNewsTextPart1);
        }
        else if (company.businessName == CompanyName.electricMotor)
        {
            newsTextList.AddRange(badNewsTextPart1);
        }
        else if (company.businessName == CompanyName.leadAcidBattery)
        {
            newsTextList.AddRange(goodNewsTextPart2);
        }
        else if (company.businessName == CompanyName.gasStoves)
        {
            newsTextList.AddRange(badNewsTextPart2);
        }
    }

    public void AllowBuying()
    {

    }
}
