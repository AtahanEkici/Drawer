using UnityEngine;
using UnityEngine.UIElements;

public class Draw : MonoBehaviour
{
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
        if(Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            Debug.Log("Mouse Down");
            InitialMousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButtonUp((int)MouseButton.LeftMouse))
        {
            Debug.Log("Mouse Up");
            LastMousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButton((int)MouseButton.LeftMouse))
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
