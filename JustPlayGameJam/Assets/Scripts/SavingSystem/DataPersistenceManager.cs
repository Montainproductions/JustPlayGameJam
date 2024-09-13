using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    private List<IDataPersistance> dataPersistancesObjects;

    public static DataPersistenceManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }

    public void Start()
    {
        this.dataPersistancesObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if(this.gameData == null)
        {
            NewGame();
        }

        foreach(IDataPersistance dataPersistancesObj in dataPersistancesObjects) 
        {
            dataPersistancesObj.LoadData(gameData);
        }

        Debug.Log("Game Loaded");
    }

    public void SaveGame()
    {
        foreach (IDataPersistance dataPersistancesObj in dataPersistancesObjects)
        {
            dataPersistancesObj.SaveData(ref gameData);
        }

        Debug.Log("Game Saved");
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }

    public void OnApplicationQuit()
    {
        SaveGame();
    }
}
