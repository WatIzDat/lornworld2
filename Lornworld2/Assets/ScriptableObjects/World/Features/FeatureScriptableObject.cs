using UnityEngine;

//[CreateAssetMenu(fileName = "FeatureScriptableObject", menuName = "Scriptable Objects/World/Features/Feature")]
//public class FeatureScriptableObject : FeatureScriptableObject<EmptyFeatureData>
//{
//}

[CreateAssetMenu(fileName = "FeatureScriptableObject", menuName = "Scriptable Objects/World/Features/Feature")]
public class FeatureScriptableObject : ScriptableObject
{
    public GameObject prefab;
    public float maxHealth;
    public InventoryItem[] itemDrops;
    public BaseFeatureInitBehaviorScriptableObject featureInitBehavior;
    public BaseFeatureHitBehaviorScriptableObject featureHitBehavior;
}
