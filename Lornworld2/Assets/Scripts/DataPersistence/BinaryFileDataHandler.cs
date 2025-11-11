using MemoryPack;
using System;
using System.IO;
using UnityEngine;

public class BinaryFileDataHandler
{
    private readonly string dataDirPath;
    //private readonly string dataFileName;

    public BinaryFileDataHandler(string dataDirPath)
    {
        this.dataDirPath = dataDirPath;
        //this.dataFileName = dataFileName;
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

            File.WriteAllBytes(fullPath, dataToStore);

            //using FileStream stream = new(fullPath, FileMode.Create);
            //using StreamWriter writer = new(stream);

            //writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occurred when trying to save data to file: {fullPath}\n{e}");
        }
    }
}
