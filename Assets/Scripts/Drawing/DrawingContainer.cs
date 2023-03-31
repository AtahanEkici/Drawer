using UnityEngine;
[DefaultExecutionOrder(-900)]
public class DrawingContainer : MonoBehaviour
{
    public static DrawingContainer Instance { get; private set; }
    private DrawingContainer() { }

    [Header("Warning Label")]
    [SerializeField] private bool isDisplaying = false;
    [SerializeField] private string Gui;
    [SerializeField] private string CannotSpawnWarning = "You can not spawn a colliding drawn object";
    [SerializeField] private float DisplayCounter = 2f;
    private float initialCounter = 0f;

    [Header("Child Operations")]
    [SerializeField] private int ChildCount = 0;
    private void Awake()
    {
        CheckInstance();
        initialCounter = DisplayCounter;
    }
    private void Update()
    {
        ChildListener();
        DisplayTimer();
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
    private void DisplayTimer()
    {
        if (!isDisplaying) { return; }
        else
        {
            DisplayCounter -= Time.deltaTime;

            if(DisplayCounter <= 0f)
            {
                isDisplaying = false;
                DisplayCounter = initialCounter;
            }
        }  
    }
    private void DisplayError()
    {
        if (isDisplaying) { DisplayCounter = initialCounter; }
        else { isDisplaying = true; }
    }
    private void ChildListener()
    {
        int currentChildCount = transform.childCount;

        if (ChildCount == currentChildCount) { return; }

        if (transform.childCount > ChildCount)
        {
            //Debug.Log("Child Added");
            ChildCount++;
        }
        else // if(transform.childCount < ChildCount)
        {
            //Debug.Log("Child removed");
            ChildCount--;
            DisplayError();
        }
    }
    private void OnGUI()
    {
        if(isDisplaying)
        {
            Gui = GUI.TextField(new Rect(Screen.width / 2 - 80, Screen.height / 2, Screen.height/20, Screen.height / 20), CannotSpawnWarning, 100, GUIStyle.none);
        }
        
    }
}
