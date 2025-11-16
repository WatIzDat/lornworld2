using MemoryPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class BinaryFileDataHandler
{
    private readonly string dataDirPath;
    private readonly Queue<(Action<byte[]>, byte[])> readCallbacks;

    private readonly BlockingCollection<(string fullPath, byte[] bytes, Action<byte[]> readCallback)> fileOperationsQueue = new();
    private readonly Task fileOperationsTask;

    public BinaryFileDataHandler(string dataDirPath, Queue<(Action<byte[]>, byte[])> readCallbacks)
    {
        this.dataDirPath = dataDirPath;
        this.readCallbacks = readCallbacks;

        fileOperationsTask = Task.Factory.StartNew(ConsumeFileOperationsQueue, TaskCreationOptions.LongRunning);
    }

    public void Load<T>(string dataFileName, Action<T> successCallback, Action failCallback) where T : IGameData
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        //T loadedData = default;

        if (File.Exists(fullPath))
        {
            try
            {
                fileOperationsQueue.Add((fullPath, null, dataToLoad =>
                {
                    if (MemoryPackSerializer.Deserialize<IGameData>(dataToLoad) is T convertedData)
                    {
                        //loadedData = convertedData;
                        successCallback(convertedData);
                    }
                    else
                    {
                        Debug.LogError($"Data from file {fullPath} could not be converted to target type.");
                    }
                }
                ));

                //fileOperationsTask.Wait();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occurred when trying to load data from file: {fullPath}\n{e}");
            }
        }
        else
        {
            failCallback();
        }

        Debug.Log("hello");

        //return loadedData;
    }

    public void Save(IGameData data, string dataFileName)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            byte[] dataToStore = MemoryPackSerializer.Serialize(data);

            fileOperationsQueue.Add((fullPath, dataToStore, null));
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occurred when trying to save data to file: {fullPath}\n{e}");
        }
    }

    public void Terminate()
    {
        fileOperationsQueue.CompleteAdding();

        fileOperationsTask.Wait();
    }

    private void ConsumeFileOperationsQueue()
    {
        foreach ((string fullPath, byte[] bytes, Action<byte[]> readCallback) in fileOperationsQueue.GetConsumingEnumerable())
        {
            if (readCallback != null)
            {
                //readCallback(File.ReadAllBytes(fullPath));
                readCallbacks.Enqueue((readCallback, File.ReadAllBytes(fullPath)));
            }
            else
            {
                File.WriteAllBytes(fullPath, bytes);
            }
        }
    }
}
