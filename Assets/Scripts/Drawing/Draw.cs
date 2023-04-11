using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    [SerializeField] private float SimplificationCoefficient = 0.025f;

    [Header("Line Material")]
    [SerializeField] private Material LineMaterial;
    [SerializeField] private PhysicsMaterial2D physicMaterial2D;

    [Header("Minimum Distance")]
    [SerializeField] private float MinDistance = 10f;

    [Header("Is On UI Element")]
    [SerializeField] private bool IsOnUI = false;
    private void Awake()
    {
        CheckInstance();
        GetResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        StartUp(scene);
    }
    private void Start()
    {
        MainCamera = Camera.main;
    }
    private void Update()
    {
        GetMousePosition();
        MouseOverUI();
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
    private void StartUp(Scene scene)
    {
        if(scene.name == GameManager.StartMenuScene) { DisableDrawing(); }
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
        if(SceneManager.GetActiveScene().name == GameManager.StartMenuScene) { Debug.Log("Can not draw on start menu"); return; }
        else if (IsOnUI) { Debug.Log("Mouse Over UI Object"); return; }
        else if (isDrawingDisabled) { Debug.Log("Drawing Disabled"); return; }
        else if (GameManager.IsGamePaused() && IsLineDynamic()) { Debug.Log("Can not Place Dynamic Line while Game is Paused"); ErrorSystem.instance.SetErrorMessage(ErrorSystem.DynamicLineWhileGamePaused); return; }

        NewDrawing = new("Drawing_" + TotalCount.ToString())
        {
            tag = DrawingTag
        };

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
        Debug.Log("Drawing Disabled");
        isDrawingDisabled = true;
    }
    public void EnableDrawing()
    {
        Debug.Log("Drawing Enabled");
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

        if (Line_Renderer.positionCount <= 1)
        {
            Destroy(NewDrawing);
            TotalCount--;
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

        if(NewDrawing != null)
        {
            NewDrawing.transform.SetParent(Drawings.transform);
        } 
    }
    public void RaycastUIElement()
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = mousePos
        };

        GraphicRaycaster[] raycasters = FindObjectsOfType<GraphicRaycaster>();

        for (int i = 0; i < raycasters.Length; i++)
        {
            List<RaycastResult> results = new();
            raycasters[i].Raycast(eventData, results);
            if (results.Count > 0)
            {
                Debug.Log("UI element detected: " + results[0].gameObject.name);
                IsOnUI = true;
            }
        }
        IsOnUI =  false;
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
    private void MouseOverUI()
    {
        IsOnUI = EventSystem.current.IsPointerOverGameObject();
    }
}