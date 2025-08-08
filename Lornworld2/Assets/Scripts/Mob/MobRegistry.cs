using System;
using UnityEngine;

public class MobRegistry : Registry<MobScriptableObject, MobIdentifier>
{
    public static MobRegistry Instance { get; private set; }

    protected override Type IdsType { get; } = typeof(MobIds);

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another MobRegistry instance");
        }
        else
        {
            Instance = this;
        }

        base.Awake();
    }

    protected override MobIdentifier CreateId(string id)
    {
        return new MobIdentifier(id);
    }
}
