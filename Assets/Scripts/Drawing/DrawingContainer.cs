using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-500)]
public class DrawingContainer : MonoBehaviour
{
    public static DrawingContainer instance = null;
    private DrawingContainer() { }

    [Header("Children Info")]
    [SerializeField] private int ChildAmount = 0;
    private void Awake()
    {
        CheckInstance();
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnSceneUnloaded(Scene scene)
    {
        DeleteAllChildren(); // When scene unloads delete all the drawings //
    }
    private void Update()
    {
        //ListenChildren();
    }
    private void ListenChildren() // ? Do i need this idk  //
    {
        int currentchildcount = transform.childCount;

        if (currentchildcount == ChildAmount) { return; } // No changes made //

        if(currentchildcount > ChildAmount)
        {
            Debug.Log("New Child Added");
            ChildAmount++;
        }
        else // currentchildcount < ChildAmount
        {
            Debug.Log("Child Subtracted");
            ChildAmount--;
        }
        
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

        ChildAmount = 0;
    }
    public int GetTotalDrawingCount()
    {
        return transform.childCount;
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
                GameObject go = transform.GetChild(i).gameObject;
                ParticleManager.SpawnDestroyedParticle(go);
                Destroy(go); 
            }
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
