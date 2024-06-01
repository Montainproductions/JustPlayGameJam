using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersStanding : MonoBehaviour
{
    [SerializeField]
    private int playersPublicStanding;

    List<ActionInfo> previousActions = new List<ActionInfo>();

    private int fame = 0;

    public struct ActionInfo
    {
        public string name;
        public int fameEffect;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseFame(ActionInfo action)
    {
        PlayersAction(action);
        fame = + action.fameEffect;
    }

    public int GetFame() 
    { 
        return fame;
    }

    public void PlayersAction(ActionInfo action)
    {
        for (int i = 0; i < previousActions.Count; i++)
        {
            Debug.Log(previousActions[i].name);
        }

        if (previousActions.Count < 12)
        {
            previousActions.Add(action);
        }
        else
        {
            //previousActions.Remove();
        }
    }
}
