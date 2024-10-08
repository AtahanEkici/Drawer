using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-1000)]
public class Draw : MonoBehaviour
{
    public static Draw instance = null;
    private Draw() { }

    public const string DrawingTag = "Drawing";

    private const string LineMaterialResourcePath = "Materials/LineMaterial/LineMaterial";
    private const string DrawPhysicsMaterialResourcePath = "Materials/LineMaterial/DrawMaterial";

    [Header("Is Drawing Disabled")]
    [SerializeField] private bool isDrawingDisabled = false;

    [Header("Is On UI Element")]
    [SerializeField] private bool IsOnUI = false;

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer Line_Renderer;
    [SerializeField] private float LineRendererWidth = 0.1f;

    [Header("Mouse Options")]
    [SerializeField] private Vector2 mousePos;

    [Header("New Drawings")]
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

    [Header("Minimum Distance")]
    [SerializeField] private float totalDistance = 0f;

    [Header("Restrictions")]
    [SerializeField] private int MaxDrawingCount = 0;
    [SerializeField] private float MaxDrawingLenght = 0f;
    [SerializeField] private float MinDrawingLenght = 0f;
    private void Awake()
    {
        CheckInstance();
        GetResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        StartUp(scene);
        GetRestrictions();
    }
    private void Start()
    {
        MainCamera = Camera.main;
    }
    private void Update()
    {
        GetMousePosition();
        IsPointerOverUIObject();
        GetMouseInputs();
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetRestrictions()
    {
        object[] restrictions = RestrictionSystem.instance.GetRestrictions();

        MaxDrawingLenght = (float)restrictions[0];
        MaxDrawingCount  = (int)restrictions[1];
        MinDrawingLenght = (int)restrictions[2];
    }
    private void StartUp(Scene scene)
    {
        if(scene.name == GameManager.StartMenuScene) { /* DisableDrawing(); */ }
        else { EnableDrawing(); }

        TotalCount = 0;

        if (Drawings == null)
        {
            Drawings = new("DrawingContainer");
            Drawings.AddComponent<DrawingContainer>();
        }
    }
    private void GetMouseInputs()
    {
        if (isDrawingDisabled) { return; }

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
        //if(SceneManager.GetActiveScene().name == GameManager.StartMenuScene) { /*Debug.Log("Can not draw on start menu");*/ return; }
        if (IsOnUI) { /*Debug.Log("Mouse Over UI Object");*/ return; }
        else if (GameManager.IsGamePaused() && IsLineDynamic()) { ErrorSystem.instance.SetErrorMessage(ErrorSystem.DynamicLineWhileGamePaused); return; }
        else if(DrawingContainer.instance.GetTotalDrawingCount() >= MaxDrawingCount) { ErrorSystem.instance.SetErrorMessage(ErrorSystem.ReachedMaxDrawingCount); return; }

        NewDrawing = new("Drawing_" + TotalCount.ToString()){tag = DrawingTag};

        Line_Renderer = NewDrawing.AddComponent<LineRenderer>();
        Line_Renderer.material = LineMaterial;
        Line_Renderer.startWidth = LineRendererWidth;
        Line_Renderer.endWidth = LineRendererWidth;
        Line_Renderer.positionCount = 1;
        Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);

        TotalCount++;
    }
    public void DisableDrawing()
    {
        //Debug.Log("Drawing Disabled");
        isDrawingDisabled = true;
    }
    public void EnableDrawing()
    {
        //Debug.Log("Drawing Enabled");
        isDrawingDisabled = false;
    }
    private void WhileDrawing()
    {
        if(Line_Renderer == null) { return; }

        Line_Renderer.positionCount++;
        Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);
    }
    private void EndDrawing()
    {
        if (Line_Renderer == null) { return; }

        Line_Renderer.Simplify(SimplificationCoefficient);

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

        if(NewDrawing != null)
        {
            NewDrawing.transform.SetParent(Drawings.transform); 
        }

        NewDrawing.AddComponent<Drawing>();
    }
    private float AttachCapsuleCollidersToPoints(LineRenderer lr, GameObject go) // returns total density for configuring drawing mass //
    {
        float totaldensity = 1f;
        totalDistance = 0f;
        try
        {
            int posCount = lr.positionCount;
            Vector2[] positions = new Vector2[posCount];

            for (int i = 0; i < posCount; i++)
            {
                positions[i] = (Vector2)(lr.GetPosition(i));

                if(i>0)
                {
                    totalDistance += Vector2.Distance(positions[i], positions[i - 1]); 
                }
            }

            //Debug.Log("Total Distance of the Drawing: " + totalDistance);

            if (totalDistance < MinDrawingLenght)
            {
                Destroy(go);
                TotalCount--;
                ErrorSystem.instance.SetErrorMessage(ErrorSystem.DrawingTooSmall);
                return 0f;
            }
            else if(totalDistance >= MaxDrawingLenght)
            {
                Destroy(go);
                TotalCount--;
                ErrorSystem.instance.SetErrorMessage(ErrorSystem.DrawingTooLarge);
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
    private bool IsLineDynamic()
    {
        int type = PlayerPrefs.GetInt(DrawingSpriteController.LinephysicsType, 1);

        if(type == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void IsPointerOverUIObject()
    {
#if UNITY_ANDROID
        if (Input.touchCount < 0) { return; }
#endif
        PointerEventData eventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        IsOnUI =  results.Count > 0;
    }
}
