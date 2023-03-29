using UnityEngine;
public class DrawingContainer : MonoBehaviour
{
    public static DrawingContainer Instance { get; private set; }
    private DrawingContainer() { }

    [Header("Child Operations")]
    [SerializeField] private int ChildCount = 0;
    private void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        ChildListener();  
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
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
    private void ChildListener()
    {
        int currentChildCount = transform.childCount;

        if (ChildCount == currentChildCount) { return; }

        if (transform.childCount > ChildCount)
        {
            Debug.Log("Child Added");
            ChildCount++;
        }
        else // if(transform.childCount < ChildCount)
        {
            Debug.Log("Child removed");
            ChildCount--;
        }
    }
}
