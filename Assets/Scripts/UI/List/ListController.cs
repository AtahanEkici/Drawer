using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-400)]
public class ListController : MonoBehaviour
{
    [Header("Scroll Area")]
    [SerializeField] private GameObject ScrollArea;

    [Header("Scroll")]
    [SerializeField] private GameObject Scroll;

    [Header("Container")]
    [SerializeField] private GameObject Container;

    [Header("Drawing Container")]
    [SerializeField] private DrawingContainer drawings;

    [Header("List Objects")]
    [SerializeField] private GameObject[] ListObjects = new GameObject[1];
    private void Awake()
    {
        GetLocalReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        GetForeignReferences();
    }
    private void Start()
    {
        GetForeignReferences();
    }
    private void OnEnable()
    {
        // Get All Drawing References //
    }
    private void OnDisable()
    {
        // Delete All List Object References //
    }
    private void GetLocalReferences()
    {
        if(ScrollArea == null)
        {
            ScrollArea = transform.GetChild(0).gameObject;
        }

        if(Scroll == null)
        {
            Scroll = ScrollArea.transform.GetChild(0).gameObject;
        }

        if(Container == null)
        {
            Container = Scroll.transform.GetChild(0).gameObject;
        }
    }
    private void GetForeignReferences()
    {
        if(drawings == null)
        {
            drawings = DrawingContainer.instance;
        }
    }
}
