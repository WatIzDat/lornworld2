using UnityEngine;

[CreateAssetMenu(fileName = "FeatureScriptableObject", menuName = "Scriptable Objects/World/Features/Feature")]
public class FeatureScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public float maxHealth;
    public InventoryItem[] itemDrops;
}
