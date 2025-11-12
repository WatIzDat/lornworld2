using MemoryPack;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class BinaryFileDataHandler
{
    private readonly string dataDirPath;
    //private readonly string dataFileName;

    private readonly BlockingCollection<(string fullPath, byte[] bytes)> writeQueue = new();
    private readonly Task writeTask;

    public BinaryFileDataHandler(string dataDirPath)
    {
        this.dataDirPath = dataDirPath;
        //this.dataFileName = dataFileName;

        writeTask = Task.Factory.StartNew(ConsumeWriteQueue, TaskCreationOptions.LongRunning);
    }

    public T Load<T>(string dataFileName) where T : IGameData
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        T loadedData = default;

        if (File.Exists(fullPath))
        {
            try
            {
                byte[] dataToLoad = File.ReadAllBytes(fullPath);

                Debug.Log(dataFileName + dataToLoad.Length);

                //loadedData = MemoryPackSerializer.Deserialize<T>(dataToLoad);

                if (MemoryPackSerializer.Deserialize<IGameData>(dataToLoad) is T convertedData)
                {
                    loadedData = convertedData;
                }
                else
                {
                    Debug.LogError($"Data from file {fullPath} could not be converted to target type.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occurred when trying to load data from file: {fullPath}\n{e}");
            }
        }

        return loadedData;
    }

    public void Save(IGameData data, string dataFileName)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            byte[] dataToStore = MemoryPackSerializer.Serialize(data);

            //File.WriteAllBytes(fullPath, dataToStore);

            writeQueue.Add((fullPath, dataToStore));

            //using FileStream stream = new(fullPath, FileMode.Create);
            //using StreamWriter writer = new(stream);

            //writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occurred when trying to save data to file: {fullPath}\n{e}");
        }
    }

    public void Terminate()
    {
        writeQueue.CompleteAdding();

        writeTask.Wait();
    }

    private void ConsumeWriteQueue()
    {
        foreach ((string fullPath, byte[] bytes) in writeQueue.GetConsumingEnumerable())
        {
            File.WriteAllBytes(fullPath, bytes);
        }
    }
}
