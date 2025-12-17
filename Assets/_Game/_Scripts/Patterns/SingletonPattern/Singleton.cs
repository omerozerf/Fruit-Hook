using UnityEngine;

namespace _Game._Scripts.Patterns.SingletonPattern
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T ms_Instance;
        private static readonly object LOCK = new();
        private static bool ms_IsShuttingDown;

        public static T Instance
        {
            get
            {
                if (ms_IsShuttingDown)
                    return null;

                lock (LOCK)
                {
                    if (!ms_Instance)
                    {
                        ms_Instance = FindObjectOfType<T>();

                        if (!ms_Instance)
                        {
                            var go = new GameObject(typeof(T).Name);
                            ms_Instance = go.AddComponent<T>();
                        }
                    }

                    return ms_Instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (!ms_Instance)
            {
                ms_Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (ms_Instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnApplicationQuit()
        {
            ms_IsShuttingDown = true;
        }
    }
}