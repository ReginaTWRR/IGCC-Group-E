using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError($"Singleton: {typeof(T)} instance is null!");
            }

            return instance;
        }
    }
}
