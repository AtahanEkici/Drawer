using UnityEngine;
public class ZigZagController : MonoBehaviour
{
    [Header("Local Components")]
    [SerializeField] private LineRenderer lr;

    [Header("Children")]
    [SerializeField] private Transform Center;
    [SerializeField] private Transform Rotater;

    [Header("Rotation Speed")]
    [SerializeField] private float RotationSpeed = 60f;

    [Header("Is Rotating ?")]
    [SerializeField] private bool IsRotating = true;
    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        if(Center == null)
        {
            Center = transform.GetChild(0).transform;
        }
        if (Rotater == null)
        {
            Rotater = transform.GetChild(1).transform;
        }
    }
    private void Update()
    {
        RenderLineRenderer();
        RotateRotator();
    }
    private void Initialize()
    {
        if(lr == null)
        {
            lr = GetComponent<LineRenderer>();
        }
        lr.positionCount = 2;
    } 
    private void RotateRotator()
    {
        if (!IsRotating) { return; }

        Rotater.RotateAround(Center.position, Vector3.back, RotationSpeed * Time.smoothDeltaTime);
    }
    private void RenderLineRenderer()
    {
        lr.SetPosition(0, Center.position);
        lr.SetPosition(1, Rotater.position);
    }
}
