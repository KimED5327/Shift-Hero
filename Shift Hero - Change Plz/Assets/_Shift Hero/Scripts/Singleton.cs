using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)FindObjectOfType(typeof(T));
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}