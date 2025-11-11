public interface IDataPersistence
{
    bool LoadData(GameData data);
    void SaveData(ref GameData data);
}
