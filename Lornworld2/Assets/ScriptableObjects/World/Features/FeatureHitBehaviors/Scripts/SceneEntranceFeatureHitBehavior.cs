using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneEntranceFeatureHitBehavior", menuName = "Scriptable Objects/World/Features/HitBehaviors/SceneEntrance")]
public class SceneEntranceFeatureHitBehavior : FeatureHitBehaviorScriptableObject<SceneEntranceFeatureData>
{
    public override void Hit(FeatureData data, Vector2 pos)
    {
        base.Hit(data, pos);

        SceneEntranceFeatureData sceneEntranceData = (SceneEntranceFeatureData)data;

        if (sceneEntranceData == null)
        {
            return;
        }

        Debug.Log(sceneEntranceData.sceneBuildIndex);

        DataPersistenceManager.Instance.OnSceneChanged();

        ScenePersistentInfo.SceneId = sceneEntranceData.sceneId;

        SceneManager.LoadScene(sceneEntranceData.sceneBuildIndex);
    }
}
