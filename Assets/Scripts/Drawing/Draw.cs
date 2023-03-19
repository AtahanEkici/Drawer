using UnityEngine;

public class Draw : MonoBehaviour
{
    [Header("Line Options")]
    [SerializeField] private GameObject linePrefab; // The prefab for the line sprite

    [Header("Mouse Options")]
    [SerializeField] private Vector3 InitialMousePosition = Vector3.zero;
    [SerializeField] private Vector3 CurrentMousePosition = Vector3.zero;
    [SerializeField] private Vector3 LastMousePosition = Vector3.zero;

    [Header("Camera Info")]
    [SerializeField] private Camera MainCamera;

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        MainCamera = Camera.main;
    }
    private void Update()
    {
        GetMouseInputs();
    }
    private void GetMouseInputs()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Down");
            InitialMousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Up");
            LastMousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0))
        {
            Debug.Log("Mouse Held Down");
            CurrentMousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
           // Debug.Log("No Mouse Interaction");
        }
    }
    
}
