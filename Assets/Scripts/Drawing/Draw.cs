using UnityEngine;
using UnityEngine.SceneManagement;
public class Draw : MonoBehaviour
{
    public static Draw Instance { get; private set; }
    private Draw() { }

    public const string DrawingTag = "Drawing";

    private const string LineMaterialResourcePath = "Materials/LineMaterial/LineMaterial";
    private const string DrawPhysicsMaterialResourcePath = "Materials/LineMaterial/DrawMaterial";

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer Line_Renderer;
    [SerializeField] private float LineRendererWidth = 0.1f;
    //[SerializeField] private float LineLenght = 0f;

    [Header("Mouse Options")]
    [SerializeField] private Vector2 mousePos;

    [Header("New Object")]
    [SerializeField] private GameObject Drawings;
    [SerializeField] private GameObject NewDrawing;
    [SerializeField] private int TotalCount = 1;

    [Header("Camera Info")]
    [SerializeField] private Camera MainCamera;

    [Header("Simplification Coefficient")]
    [SerializeField] private float SimplificationCoefficient = 0.025f;

    [Header("Line Material")]
    [SerializeField] private Material LineMaterial;
    [SerializeField] private PhysicsMaterial2D physicMaterial2D;

    [Header("minimum Distance")]
    [SerializeField] private float MinDistance = 1f;
    private void Awake()
    {
        CheckInstance();
        StartUp();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        TotalCount = 0;

        if (Drawings == null)
        {
            Drawings = new("Drawings");
            Drawings.AddComponent<DrawingContainer>();
        }
    }
    private void Start()
    {
        MainCamera = Camera.main;
    }
    private void Update()
    {
        GetMousePosition();
        GetMouseInputs();
    }
    private void CheckInstance()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void StartUp()
    {
        GetResources();
        
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
        if (GameManager.IsGamePaused() && IsLineDynamic()) { return; }

        NewDrawing = new("Drawing" + TotalCount.ToString())
        {
            tag = DrawingTag
        };

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
        if(Line_Renderer == null) { return; }

        Line_Renderer.positionCount++;
        Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);

        //LineLenght = CalculateLineRendererTotalLenght();
    }
    private void EndDrawing()
    {
        //LineLenght = 0;

        if (Line_Renderer == null) { return; }

        //Debug.Log("Before End-Start Simplify: " + CalculateLineRendererEndTOStart());
        //Debug.Log("Before Total Simplify: " + CalculateTotalLenghtOfLineRenderer());
        Line_Renderer.Simplify(SimplificationCoefficient);
        //Debug.Log("After  End-Start Simplify: " + CalculateLineRendererEndTOStart());
        //Debug.Log("After  Total Simplify: " + CalculateTotalLenghtOfLineRenderer());

        if (Line_Renderer.positionCount <= 1)
        {
            Destroy(NewDrawing);
            return;
        }

        Mesh LineMesh = new();
        Line_Renderer.BakeMesh(LineMesh, MainCamera, true);

        MeshFilter NewMeshFilter = NewDrawing.AddComponent<MeshFilter>();
        NewMeshFilter.mesh = LineMesh;

        MeshRenderer NewMeshRenderer = NewDrawing.AddComponent<MeshRenderer>();
        NewMeshRenderer.material = LineMaterial;

        DisposeLineRenderer();

        Rigidbody2D rb = NewDrawing.AddComponent<Rigidbody2D>();

        if(IsLineDynamic())
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
        }

        rb.sharedMaterial = physicMaterial2D;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.mass = AttachCapsuleCollidersToPoints(Line_Renderer, NewDrawing);
    }
    private float CalculateLineRendererEndTOStart()
    {
        return Vector2.Distance(Line_Renderer.GetPosition(0), Line_Renderer.GetPosition(Line_Renderer.positionCount - 1));
    }
    private float CalculateTotalLenghtOfLineRenderer()
    {
        float totalLength = 0f;

        for (int i = 0; i < Line_Renderer.positionCount - 1; i++)
        {
            totalLength += Vector2.Distance(Line_Renderer.GetPosition(i), Line_Renderer.GetPosition(i + 1));
        }

        return totalLength;
    }
    private float AttachCapsuleCollidersToPoints(LineRenderer lr, GameObject go) // returns total density for configuring drawing mass //
    {
        float totaldensity = 1f;
        float totalDistance = 0f;
        try
        {
            int posCount = lr.positionCount;
            Vector2[] positions = new Vector2[posCount];

            for (int i = 0; i < posCount; i++)
            {
                positions[i] = (Vector2)(lr.GetPosition(i));
            }

            totalDistance = GetLineRendererLengthRelativeToCamera();

            //Debug.Log("Total Distance: " + totalDistance);

            if (totalDistance < MinDistance)
            {
                Destroy(go);
                return 0f;
            }

            for (int i = 0; i < positions.Length - 1; i++)
            {
                    GameObject box = new("Box Collider_" + i);
                    BoxCollider2D collider = box.AddComponent<BoxCollider2D>();

                if(IsLineDynamic())
                {
                    collider.isTrigger = true;
                    box.AddComponent<CollisionChecker>().col = collider;
                }
                    collider.sharedMaterial = physicMaterial2D;
                    collider.transform.position = (positions[i] + positions[i + 1]) / 2f;

                    float distance = Vector2.Distance(positions[i], positions[i + 1]);
                    collider.size = new Vector2(distance, LineRendererWidth);
                    Vector2 direction = positions[i + 1] - positions[i];
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    collider.transform.rotation = Quaternion.Euler(0, 0, angle);
                    box.transform.SetParent(go.transform);
                    totaldensity += collider.density;
               
            } 
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        return totaldensity;
    }
    private void GetResources()
    {
        LineMaterial =  Resources.Load(LineMaterialResourcePath) as Material;
        physicMaterial2D = Resources.Load(DrawPhysicsMaterialResourcePath) as PhysicsMaterial2D;
    }
    private float GetLineRendererLengthRelativeToCamera()
    {
        Vector3[] positions = new Vector3[Line_Renderer.positionCount];
        Line_Renderer.GetPositions(positions);

        float length = 0f;

        for (int i = 1; i < positions.Length; i++)
        {
            Vector3 viewportPos1 = MainCamera.WorldToViewportPoint(positions[i - 1]);
            Vector3 viewportPos2 = MainCamera.WorldToViewportPoint(positions[i]);
            length += Vector3.Distance(viewportPos1, viewportPos2);
        }

        float screenWidth = MainCamera.pixelWidth;
        float screenHeight = MainCamera.pixelHeight;
        float screenDiagonal = Mathf.Sqrt(screenWidth * screenWidth + screenHeight * screenHeight);

        return length * screenDiagonal;
    }
    private bool IsLineDynamic()
    {
        int type = PlayerPrefs.GetInt(UI_Controller.LinephysicsType, 1);

        if(type == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}