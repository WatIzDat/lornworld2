using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEntrance : MonoBehaviour
{
    [SerializeField]
    private int sceneBuildIndex;

    public void EnterScene()
    {
        SceneManager.LoadSceneAsync(sceneBuildIndex);
    }
}
