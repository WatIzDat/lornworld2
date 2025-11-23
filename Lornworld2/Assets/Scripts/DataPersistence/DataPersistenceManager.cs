using MemoryPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance { get; private set; }

    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    private GameData gameData = new(Vector2.zero);

    //private List<IDataPersistence> dataPersistenceObjects;

    private BinaryFileDataHandler dataHandler;

    private readonly Queue<(Action<byte[]> action, byte[] data)> readCallbacks = new();
    private readonly Queue<Action> writeCallbacks = new();

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

        dataHandler = new(Path.Combine(Application.persistentDataPath, "data"), readCallbacks, writeCallbacks);
    }

    //private void OnEnable()
    //{
    //    SceneManager.activeSceneChanged += OnActiveSceneChanged;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    //}

    public void OnSceneChanged()
    {
        //if (isFirstSceneChange)
        //{
        //    Debug.Log("didn't work");

        //    isFirstSceneChange = false;

        //    return;
        //}

        SaveGame();

        Debug.Log("game saved");
    }

    //private void Start()
    //{
    //dataHandler = new BinaryFileDataHandler(Application.persistentDataPath);
    //dataPersistenceObjects = FindAllDataPersistenceObjects();

    //LoadGame();
    //}

    private void Update()
    {
        while (readCallbacks.Count > 0)
        {
            (Action<byte[]> action, byte[] data) = readCallbacks.Dequeue();

            action(data);
        }

        while (writeCallbacks.Count > 0)
        {
            Action action = writeCallbacks.Dequeue();

            action();
        }
    }

    public void NewGame()
    {
        gameData = new GameData(Vector2.zero);
    }

    //public void LoadGame()
    //{
        //gameData = dataHandler.Load<GameData>("data");

        //if (gameData == null)
        //{
        //    Debug.Log("new game");

        //    NewGame();
        //}

        //foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //{
        //    dataPersistenceObj.LoadData(gameData);
        //}

        //T load<T>(string dataFileName) where T : IGameData => dataHandler.Load<T>(dataFileName);

        //LoadTriggered?.Invoke(load);

    //    Debug.Log("loaded pos: " + gameData.playerPosition);
    //}

    public void SaveGame()
    {
        //foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        //{
        //    dataPersistenceObj.SaveData(ref gameData);
        //}

        SaveTriggered?.Invoke((data, fileName) =>
        {
            dataHandler.Save(data, fileName, () => { });
        });

        Debug.Log("saved pos: " + gameData.playerPosition);

        //dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();

        dataHandler.Terminate();
    }

    public void LoadObject<T>(Action<T> successCallback, Action failCallback, string dataFileName) where T : IGameData
    {
        //return func(dataHandler.Load<T>(dataFileName));
        dataHandler.Load(dataFileName, successCallback, failCallback);
    }

    public void SaveObject(SaveAction action, Action callback)
    {
        action((data, fileName) =>
        {
            dataHandler.Save(data, fileName, callback);
        });
    }

    //private List<IDataPersistence> FindAllDataPersistenceObjects()
    //{
    //    IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
    //        .OfType<IDataPersistence>();

    //    return new List<IDataPersistence>(dataPersistenceObjects);
    //}
}
