using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    private LevelManager() { }
    private void Awake()
    {
        CheckInstance();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode SceneLoadMode)
    {
        
    }
    private void Start()
    {
        
    }
    private void CheckInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            try 
            {
                Destroy(gameObject);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Could not destroy "+gameObject.name+"");
                Destroy(this);
            }
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
