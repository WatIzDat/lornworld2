using MemoryPack;
using System;

public interface IDataPersistence<T> where T : IGameData
{
    bool LoadData(T data);
    void SaveData(Action<IGameData, string> saveCallback);
}
