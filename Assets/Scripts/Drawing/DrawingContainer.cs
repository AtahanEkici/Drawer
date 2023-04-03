using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-900)]
public class DrawingContainer : MonoBehaviour
{
    public static DrawingContainer Instance { get; private set; }
    private DrawingContainer() { }

    [Header("Child Operations")]
    [SerializeField] private int ChildCount = 0;
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
        ChildListener();
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
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
        ChildCount = 0;
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
            ChildCount--;        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
