using System;
using UnityEngine;

public class FeatureRegistry : Registry<FeatureScriptableObject, FeatureIdentifier>
{
    public static FeatureRegistry Instance { get; private set; }

    protected override Type IdsType { get; } = typeof(FeatureIds);

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another FeatureRegistry instance");
        }
        else
        {
            Instance = this;
        }

        base.Awake();
    }

    protected override FeatureIdentifier CreateId(string id)
    {
        return new FeatureIdentifier(id);
    }
}
