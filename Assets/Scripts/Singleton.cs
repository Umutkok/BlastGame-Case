using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(T)) as T;
            
            return instance;
        }
    }
    protected virtual void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            Debug.LogError("umut");
        }
    }
    
}
