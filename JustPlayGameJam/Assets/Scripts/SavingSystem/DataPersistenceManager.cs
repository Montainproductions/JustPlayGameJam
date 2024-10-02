using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] 
    private string fileName;

    [SerializeField]
    private bool newGameBool;

    private GameData gameData;
    private List<IDataPersistance> dataPersistancesObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }

    public void Start()
    {
        GameManager.Instance.RestartCompanies();
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistancesObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if(this.gameData == null || newGameBool)
        {
            NewGame();
        }

        foreach(IDataPersistance dataPersistancesObj in dataPersistancesObjects) 
        {
            dataPersistancesObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistance dataPersistancesObj in dataPersistancesObjects)
        {
            dataPersistancesObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);

        GameManager.Instance.RestartCompanies();
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
