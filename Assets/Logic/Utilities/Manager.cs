using UnityEngine;

/// <summary>
/// This is an approach to implement Managers as Singletons.
///
/// <para>Note that Managers may be destroyed before other game objects on scene
/// change / shutdown.Incorporate this fact into your OnDisable / OnDestory
/// handlers by checking HasInstance.</para>
/// </summary>
///
/// <typeparam name="T">The Manager which also inherits from this class.</typeparam>
[DisallowMultipleComponent]
public abstract class Manager<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Instance {
        get {
            if (!_instance)
                SetInstance();

            return _instance;
        }
    }

    public static bool HasInstance => _instance;

    static T _instance;

    //////////////////////////////////////////////////////////////////////////

    protected virtual void Awake()
    {
        if (Instance != this)
            DestroyImmediate(this);
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    //////////////////////////////////////////////////////////////////////////

    const string ManagersGameObjectName = "Managers";

    static void SetInstance()
    {
        _instance = FindObjectOfType<T>();

        if (!_instance)
        {
            var gameObject = GameObject.Find(ManagersGameObjectName);
            if (!gameObject)
                gameObject = new GameObject(ManagersGameObjectName);

            _instance = gameObject.AddComponent<T>();
        }
    }
}
