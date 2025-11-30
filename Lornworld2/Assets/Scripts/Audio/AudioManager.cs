using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private readonly List<EventInstance> eventInstances = new();
    private List<StudioEventEmitter> eventEmitters = new();

    private EventInstance ambienceEventInstance;

    [SerializeField]
    private EventReference musicEventReference;

    private EventInstance musicEventInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another AudioManager instance");
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;

        ChunkManager.LoadedChunksShifted += OnLoadedChunksShifted;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;

        ChunkManager.LoadedChunksShifted -= OnLoadedChunksShifted;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        WorldManager worldManager = FindFirstObjectByType<WorldManager>();

        if (!worldManager.hasAmbience)
        {
            return;
        }

        ambienceEventInstance = InitializePersistentEventInstance(worldManager.ambienceAudio);
    }

    private void Start()
    {
        musicEventInstance = InitializePersistentEventInstance(musicEventReference);
    }

    private void OnActiveSceneChanged(Scene fromScene, Scene toScene)
    {
        CleanUp();
    }

    private void OnLoadedChunksShifted(Vector2Int direction)
    {
        RemoveUnusedEventEmitters();
    }

    public void PlayOneShot(EventReference sound, Vector2 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);

        eventInstances.Add(eventInstance);

        return eventInstance;
    }

    private EventInstance InitializePersistentEventInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        
        eventInstance.start();

        return eventInstance;
    }

    public void AddEventEmitter(StudioEventEmitter emitter)
    {
        eventEmitters.Add(emitter);
    }

    public void RemoveUnusedEventEmitters()
    {
        Debug.Log("Before: " + eventEmitters.Count);

        eventEmitters = eventEmitters.Where(emitter => emitter != null).ToList();

        Debug.Log("After: " + eventEmitters.Count);
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        eventInstances.Clear();

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }

        eventEmitters.Clear();
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
