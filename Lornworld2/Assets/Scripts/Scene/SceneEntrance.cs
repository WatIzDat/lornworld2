using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEntrance : Feature
{
    [SerializeField]
    private int sceneBuildIndex;

    public void EnterScene()
    {
        SceneManager.LoadSceneAsync(sceneBuildIndex);
    }
}
