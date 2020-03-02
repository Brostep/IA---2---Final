using UnityEngine;

/// <summary>
/// Inherit from this class in order to create a singleton.
/// </summary>
public abstract class Singleton<T> : Singleton where T : MonoBehaviour {
    // [CanBeNull]
    private static T _instance;

    // [NotNull]
    private static readonly object _lock = new object();

    /// <summary>
    /// Activate if singleton is desired to persist across scenes.
    /// </summary>
    [SerializeField]
    private static bool _persistent = true;
    
    public static T Instance {
        get {
            // Prevent access to the singleton instance when the application quits to avoid problems.
            if (IsQuitting) {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                return null;
            }

            lock (_lock) {
                if (_instance != null)
                    return _instance;

                // Search for existing instances.
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;

                if (count > 0) {
                    if (count == 1)
                        return instances[0];

                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found."
                        + " The first instance found will be used, and all others will be destroyed.");

                    for (int i = 1; i < count; i++)
                        Destroy(instances[i]);

                    return _instance = instances[0];
                }

                // Create a new instance if one doesn't already exist.
                Debug.Log($"[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}").AddComponent<T>();
            }
        }
    }
    
    private void Awake() {
        if (_persistent)
            DontDestroyOnLoad(gameObject);

        OnAwake();
    }

    protected virtual void OnAwake() { }
}

public abstract class Singleton : MonoBehaviour {
    public static bool IsQuitting { get; private set; }
    
    private void OnApplicationQuit() {
        IsQuitting = true;
    }
}
