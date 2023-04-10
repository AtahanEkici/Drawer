using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-500)]
public class DrawingContainer : MonoBehaviour
{
    public static DrawingContainer instance = null;
    private DrawingContainer() { }
    private void Awake()
    {
        CheckInstance();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnSceneUnloaded(Scene scene)
    {
        DeleteAllChildren(); // When scene unloads delete all the drawings //
    }
    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            try
            {
                Destroy(gameObject);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                Debug.Log("Could not destroy " + gameObject.name + "");
                Destroy(this);
            }
        }
    }
    private void DeleteAllChildren()
    {
        if(this == null) { return; }

        for(int i=0;i<transform.childCount;i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    public GameObject[] GetAllDrawings()
    {
        return GetComponentsInChildren<GameObject>();
    }
    public void DeleteAllGlowing()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            if(transform.GetChild(i).gameObject.GetComponent<Glow>() != null)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
