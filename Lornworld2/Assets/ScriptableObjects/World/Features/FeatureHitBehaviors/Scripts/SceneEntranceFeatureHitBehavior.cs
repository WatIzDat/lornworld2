using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneEntranceFeatureHitBehavior", menuName = "Scriptable Objects/World/Features/HitBehaviors/SceneEntrance")]
public class SceneEntranceFeatureHitBehavior : FeatureHitBehaviorScriptableObject<SceneEntranceFeatureData>
{
    public override void Hit(FeatureData data)
    {
        base.Hit(data);

        SceneEntranceFeatureData sceneEntranceData = (SceneEntranceFeatureData)data;

        if (sceneEntranceData == null)
        {
            return;
        }

        Debug.Log(sceneEntranceData.sceneBuildIndex);

        SceneManager.LoadSceneAsync(sceneEntranceData.sceneBuildIndex);
    }
}
