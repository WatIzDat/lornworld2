using UnityEngine;

public abstract class RegistryEntry : ScriptableObject
{
    public string EntryName { get; private set; }

    private void Awake()
    {
        EntryName = name;
    }

    public static T CreateUnique<T>(T original) where T : RegistryEntry
    {
        string entryName = original.EntryName;

        T newObject = Instantiate(original);

        newObject.EntryName = entryName;

        return newObject;
    }
}
