using MemoryPack;
using System;
using System.IO;
using UnityEngine;

public class BinaryFileDataHandler
{
    private readonly string dataDirPath;
    private readonly string dataFileName;

    public BinaryFileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                byte[] dataToLoad = File.ReadAllBytes(fullPath);

                loadedData = MemoryPackSerializer.Deserialize<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occurred when trying to load data from file: {fullPath}\n{e}");
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
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
