using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsCycle : MonoBehaviour
{
    [SerializeField]
    private GameObject[] newsList;

    [SerializeField]
    private TextMeshProUGUI newsText;

    private int currentNewsIndex;

    [SerializeField]
    private float timePerStory;

    // Start is called before the first frame update
    void Start()
    {
        currentNewsIndex = 0;
        StartCoroutine(NewsFeed());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator NewsFeed()
    {
        yield return new WaitForSeconds(timePerStory);
        newsList[currentNewsIndex].SetActive(false);

        if(currentNewsIndex >= newsList.Length)
        {
            currentNewsIndex = 0;
        }
        else
        {
            currentNewsIndex++;
        }

        newsList[currentNewsIndex].SetActive(true);
        StartCoroutine(NewsFeed());
    }
}
