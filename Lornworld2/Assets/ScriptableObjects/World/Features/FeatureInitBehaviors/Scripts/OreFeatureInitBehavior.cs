using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "OreFeatureInitBehavior", menuName = "Scriptable Objects/World/Features/InitBehaviors/Ore")]
public class OreFeatureInitBehavior : FeatureInitBehaviorScriptableObject<OreFeatureData>
{
    [SerializeField]
    private OreTypeToSpriteMap[] oreTypeToSpriteMap;

    [System.Serializable]
    private class OreTypeToSpriteMap
    {
        public OreType oreType;
        public Sprite sprite;
    }

    public override Feature Init(Feature feature, FeatureData data)
    {
        feature = base.Init(feature, data);

        OreFeatureData oreFeatureData = (OreFeatureData)data;

        feature.GetComponent<SpriteRenderer>().sprite =
            oreTypeToSpriteMap.First(map => map.oreType == oreFeatureData.oreType).sprite;

        return feature;
    }
}
