using UnityEngine;
public class Draw : MonoBehaviour
{
    private const string LineMaterialResourcePath = "Materials/LineMaterial/LineMaterial";
    private const string DrawPhysicsMaterialResourcePath = "Materials/LineMaterial/DrawMaterial";

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer Line_Renderer;
    [SerializeField] private float LineRendererWidth = 0.1f;

    [Header("Mouse Options")]
    [SerializeField] private Vector2 mousePos;

    [Header("New Object")]
    [SerializeField] private GameObject Drawings;
    [SerializeField] private GameObject NewDrawing;
    [SerializeField] private int TotalCount = 1;

    [Header("Camera Info")]
    [SerializeField] private Camera MainCamera;

    [Header("Simplification Coefficient")]
    [SerializeField] private float SimplificationCoefficient = 0.05f;

    [Header("Line Material")]
    [SerializeField] private Material LineMaterial;
    [SerializeField] private PhysicsMaterial2D physicMaterial2D;
    private void Awake()
    {
        StartUp();
    }
    private void OnEnable()
    {
        MainCamera = Camera.main;
    }
    private void Update()
    {
        GetMousePosition();
        GetMouseInputs();
    }
    private void StartUp()
    {
        GetResources();
        Drawings = new("Drawings");
        Drawings.AddComponent<DrawingContainer>();
    }
    private void GetMouseInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }
        else if (Input.GetMouseButton(0))
        {
            WhileDrawing();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndDrawing();
        }
    }
    private void GetMousePosition()
    {
        mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    private void DisposeLineRenderer()
    {
        if(Line_Renderer != null)
        {
            Destroy(Line_Renderer);
        }
    }
    private void StartDrawing()
    {
        NewDrawing = new("Drawing"+ TotalCount.ToString());

        Line_Renderer = NewDrawing.AddComponent<LineRenderer>();
        Line_Renderer.material = LineMaterial;
        Line_Renderer.startWidth = LineRendererWidth;
        Line_Renderer.endWidth = LineRendererWidth;
        Line_Renderer.positionCount = 1;
        Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);
        NewDrawing.transform.SetParent(Drawings.transform);
        TotalCount++;
    }
    private void WhileDrawing()
    {
        Line_Renderer.positionCount++;
        Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);
    }
    private void EndDrawing()
    {
        Line_Renderer.Simplify(SimplificationCoefficient);

        Mesh LineMesh = new();
        Line_Renderer.BakeMesh(LineMesh, MainCamera, true);

        MeshFilter NewMeshFilter = NewDrawing.AddComponent<MeshFilter>();
        NewMeshFilter.mesh = LineMesh;

        MeshRenderer NewMeshRenderer = NewDrawing.AddComponent<MeshRenderer>();
        NewMeshRenderer.material = LineMaterial;

        DisposeLineRenderer();

        Rigidbody2D rb = NewDrawing.AddComponent<Rigidbody2D>();
        rb.sharedMaterial = physicMaterial2D;
        //rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.useAutoMass = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        AttachCapsuleCollidersToPoints(Line_Renderer, NewDrawing, true);
    }
    private void AttachCapsuleCollidersToPoints(LineRenderer lr, GameObject go, bool isBox)
    {
        try
        {
            int posCount = lr.positionCount;

            Vector2[] positions = new Vector2[posCount];

            for (int i = 0; i < posCount; i++)
            {
                positions[i] = (Vector2)lr.GetPosition(i);
            }

            for (int i = 0; i < positions.Length - 1; i++)
            {
                if (isBox)
                {
                    GameObject box = new ("Box Collider " + i);
                    BoxCollider2D collider = box.AddComponent<BoxCollider2D>();
                    collider.isTrigger = true;
                    collider.sharedMaterial = physicMaterial2D;
                    box.AddComponent<CollisionChecker>().col = collider;
                    collider.transform.position = (positions[i] + positions[i + 1]) / 2f;
                    float distance = Vector2.Distance(positions[i], positions[i + 1]);
                    collider.size = new Vector2(distance, LineRendererWidth);
                    Vector2 direction = positions[i + 1] - positions[i];
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    collider.transform.rotation = Quaternion.Euler(0, 0, angle);
                    box.transform.SetParent(go.transform);
                }
                else
                {
                    GameObject capsule = new GameObject("Capsule Collider " + i);
                    CapsuleCollider2D collider = capsule.AddComponent<CapsuleCollider2D>();
                    collider.isTrigger = true;
                    capsule.AddComponent<CollisionChecker>().col = collider;
                    collider.transform.position = (positions[i] + positions[i + 1]) / 2f;
                    float distance = Vector2.Distance(positions[i], positions[i + 1]);
                    collider.size = new Vector2(distance, LineRendererWidth);
                    collider.direction = CapsuleDirection2D.Horizontal;
                    Vector2 direction = positions[i + 1] - positions[i];
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    collider.transform.rotation = Quaternion.Euler(0, 0, angle);
                    capsule.transform.SetParent(go.transform);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void GetResources()
    {
        LineMaterial =  Resources.Load(LineMaterialResourcePath) as Material;
        physicMaterial2D = Resources.Load(LineMaterialResourcePath) as PhysicsMaterial2D;
    }
}