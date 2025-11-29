using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayOneShotFeatureHitBehavior", menuName = "Scriptable Objects/World/Features/HitBehaviors/PlayOneShot")]
public class PlayOneShotFeatureHitBehavior : FeatureHitBehaviorScriptableObject<FeatureData>
{
    [SerializeField]
    private EventReference oneShotSound;

    public override void Hit(FeatureData data, Vector2 pos)
    {
        AudioManager.Instance.PlayOneShot(oneShotSound, pos);
    }
}
