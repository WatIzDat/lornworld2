using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Registry<T, U> : MonoBehaviour where T : Object where U : Identifier
{
    public T[] Entries { get; private set; }

    private readonly Dictionary<U, T> entriesMap = new();
    private readonly Dictionary<T, U> idsMap = new();

    [SerializeField]
    private string entriesPath;

    protected abstract System.Type IdsType { get; }

    protected virtual void Awake()
    {
        Entries = Resources.LoadAll<T>(entriesPath);

        Assembly assembly = IdsType.Assembly;

        System.Type idsType = assembly.GetType(IdsType.Name);

        foreach (T entry in Entries)
        {
            U id = CreateId(entry.name);

            idsType.GetProperty(entry.name).SetValueOptimized(null, id);

            entriesMap.Add(id, entry);
            idsMap.Add(entry, id);
        }
    }

    public T GetEntry(U id)
    {
        return entriesMap[id];
    }

    public U GetId(T entry)
    {
        return idsMap[entry];
    }

    protected abstract U CreateId(string id);
}
