using FMODUnity;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneEntranceFeatureInitBehavior", menuName = "Scriptable Objects/World/Features/InitBehaviors/SceneEntrance")]
public class SceneEntranceFeatureInitBehavior : FeatureInitBehaviorScriptableObject<SceneEntranceFeatureData>
{
    [SerializeField]
    private BuildIndexToSoundEmitterSettingsMap[] buildIndexToEmitterSettingsMap;

    [System.Serializable]
    private class BuildIndexToSoundEmitterSettingsMap
    {
        public int buildIndex;
        public EventReference eventReference;
        public float attenuationMinDistance;
        public float attenuationMaxDistance;
    }

    public override Feature Init(Feature feature, FeatureData data)
    {
        feature = base.Init(feature, data);

        SceneEntranceFeatureData sceneEntranceFeatureData = (SceneEntranceFeatureData)data;

        BuildIndexToSoundEmitterSettingsMap emitterSettings = buildIndexToEmitterSettingsMap
            .FirstOrDefault(map => map.buildIndex == sceneEntranceFeatureData.sceneBuildIndex);

        if (emitterSettings == null)
        {
            return feature;
        }

        StudioEventEmitter emitter = feature.gameObject.AddComponent<StudioEventEmitter>();

        emitter.EventReference = emitterSettings.eventReference;

        emitter.OverrideAttenuation = true;
        emitter.OverrideMinDistance = emitterSettings.attenuationMinDistance;
        emitter.OverrideMaxDistance = emitterSettings.attenuationMaxDistance;

        emitter.EventStopTrigger = EmitterGameEvent.ObjectDestroy;

        AudioManager.Instance.AddEventEmitter(emitter);

        emitter.Play();

        return feature;
    }
}
