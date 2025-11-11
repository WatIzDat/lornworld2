using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    private GameData gameData = new(Vector2.zero);

    //private List<IDataPersistence> dataPersistenceObjects;

    private BinaryFileDataHandler dataHandler;

    //public delegate T LoadCallback<T>(string dataFileName) where T : IGameData;
    //public static event Func<LoadCallback, bool> LoadTriggered;

    public delegate void SaveAction(Action<IGameData, string> saveCallback);
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

        dataHandler = new(Application.persistentDataPath);
    }

    private void Start()
    {
        //dataHandler = new BinaryFileDataHandler(Application.persistentDataPath);
        //dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData(Vector2.zero);
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load<GameData>("data");

        if (gameData == null)
        {
            Debug.Log("new game");

            NewGame();
        }

        //foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //{
        //    dataPersistenceObj.LoadData(gameData);
        //}

        //T load<T>(string dataFileName) where T : IGameData => dataHandler.Load<T>(dataFileName);

        //LoadTriggered?.Invoke(load);

        Debug.Log("loaded pos: " + gameData.playerPosition);
    }

    public void SaveGame()
    {
        //foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //{
        //    dataPersistenceObj.SaveData(ref gameData);
        //}

        SaveTriggered?.Invoke((data, fileName) =>
        {
            dataHandler.Save(data, fileName);
        });

        Debug.Log("saved pos: " + gameData.playerPosition);

        //dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public bool LoadObject<T>(Func<T, bool> func, string dataFileName) where T : IGameData
    {
        return func(dataHandler.Load<T>(dataFileName));
    }

    public void SaveObject(SaveAction action)
    {
        action((data, fileName) =>
        {
            dataHandler.Save(data, fileName);
        });
    }

    //private List<IDataPersistence> FindAllDataPersistenceObjects()
    //{
    //    IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
    //        .OfType<IDataPersistence>();

    //    return new List<IDataPersistence>(dataPersistenceObjects);
    //}
}
