using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    private GameData gameData;

    //private List<IDataPersistence> dataPersistenceObjects;

    private BinaryFileDataHandler dataHandler;

    public static event Func<GameData, bool> LoadTriggered;

    public delegate void SaveAction(ref GameData data);
    public static event SaveAction SaveTriggered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another DataPersistenceManager instance");
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        dataHandler = new BinaryFileDataHandler(Application.persistentDataPath, fileName);
        //dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("new game");

            NewGame();
        }

        //foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //{
        //    dataPersistenceObj.LoadData(gameData);
        //}

        LoadTriggered?.Invoke(gameData);

        Debug.Log("loaded pos: " + gameData.playerPosition);
    }

    public void SaveGame()
    {
        //foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //{
        //    dataPersistenceObj.SaveData(ref gameData);
        //}

        SaveTriggered?.Invoke(ref gameData);

        Debug.Log("saved pos: " + gameData.playerPosition);

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public bool LoadObject(Func<GameData, bool> func)
    {
        return func(gameData);
    }

    public void SaveObject(SaveAction action)
    {
        action(ref gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
