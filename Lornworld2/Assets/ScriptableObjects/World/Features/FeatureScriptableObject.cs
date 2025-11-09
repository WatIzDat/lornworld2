using UnityEngine;

[CreateAssetMenu(fileName = "FeatureScriptableObject", menuName = "Scriptable Objects/World/Features/Feature")]
public class FeatureScriptableObject : ScriptableObject
{
    public float maxHealth;
    public InventoryItem[] itemDrops;
}
