using UnityEngine;

public class Draw : MonoBehaviour
{
    [Header("Line Options")]
    [SerializeField] private GameObject linePrefab; // The prefab for the line sprite

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer Line_Renderer;
    [SerializeField] private LineRenderer ResetState;
    [SerializeField] private float LineRendererWidth = 0.2f;

    [Header("Mouse Options")]
    [SerializeField] private Vector3 InitialMousePosition = Vector3.zero;
    [SerializeField] private Vector3 CurrentMousePosition = Vector3.zero;
    [SerializeField] private Vector3 LastMousePosition = Vector3.zero;

    [Header("Camera Info")]
    [SerializeField] private Camera MainCamera;

    private void Awake()
    {
        Line_Renderer = GetComponent<LineRenderer>();
        Line_Renderer.startWidth = LineRendererWidth;
        Line_Renderer.endWidth = LineRendererWidth;
        ResetState = Line_Renderer;

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
        Vector3 mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        

        if (Input.GetMouseButtonDown(0))
        {
            InitialMousePosition = mousePos;
            Line_Renderer.positionCount = 1;
            Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);
        }
        else if(Input.GetMouseButton(0))
        {
            CurrentMousePosition = mousePos;
            Line_Renderer.positionCount++;
            Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            LastMousePosition = mousePos;
            GameObject NewDrawing = new("Drawing");
            LineRenderer lr = NewDrawing.AddComponent<LineRenderer>();
            lr = Line_Renderer;
            Line_Renderer = ResetState;
            NewDrawing.AddComponent<Rigidbody2D>();
            NewDrawing.AddComponent<EdgeCollider2D>();
        }
    }
    
}
