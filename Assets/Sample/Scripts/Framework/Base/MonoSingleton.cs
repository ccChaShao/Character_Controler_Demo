using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindAnyObjectByType<T>();
                if (!_instance)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<T>();
                }
            }

            return _instance;
        }
    }
}
