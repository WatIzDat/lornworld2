using UnityEngine;

public class ScenePersistent : MonoBehaviour
{
    private static ScenePersistent instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
}
