using UnityEngine;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-200)]
public class WallGenerate : MonoBehaviour
{
    public static WallGenerate Instance { get; private set; }
    private WallGenerate() { }

    [Header("Draw Debugging Rays")]
    [SerializeField] private bool DebugMode = false;

    [Header("Is Bottom Wall Death Switch")]
    [SerializeField] private bool isDeathSwitch = true;

    [Header("Foreign References")]
    [SerializeField] private Camera cam;

    [Header("Camera Bounds")]
    [SerializeField] public Vector2 left = Vector2.zero;
    [SerializeField] public Vector2 right = Vector2.zero;
    [SerializeField] public Vector2 top = Vector2.zero;
    [SerializeField] private Vector2 bottom = Vector2.zero;

    [Header("Walls Info")]
    [SerializeField] private GameObject[] Walls = new GameObject[4];
    [SerializeField] public GameObject WallObject = null;
    [SerializeField] private float LeftDistance = 0f;
    [SerializeField] private float TopDistance = 0f;

    [Header("Scale")]
    [SerializeField] private float Scale = 0.1f;
    private void Awake()
    {
        CheckInstance();
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetLocalReferences();
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        
    }
    private void OnEnable()
    {
        GetForeignReferences(); // Get Foreign References in Start //
        GetOrthographicBounds(); // Get Camera Boundaries //
        CalculateBounds();
        GenerateWalls();
        DeathSwitch();
    }
    private void DeathSwitch()
    {
        try
        {
            if (!isDeathSwitch && TryGetComponent(out DeathWall dw) && TryGetComponent(out Glow gl))
            {
                Destroy(dw);
                Destroy(gl);
            }    
            else
            {
                if(!Walls[3].TryGetComponent(out DeathWall deatwall_ref))
                {
                    Walls[3].AddComponent<DeathWall>();
                }

                if (!Walls[3].TryGetComponent(out Glow glow_ref))
                {
                    Walls[3].AddComponent<Glow>();
                }
            }
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetForeignReferences()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }
    private void GetLocalReferences()
    {
        if (WallObject == null)
        {
            WallObject = transform.GetChild(0).gameObject;
        }
    }
    private void CalculateBounds() // Draw Debugging Rays //
    {
        Vector2 topleft = top + left;

        TopDistance = Vector2.Distance(topleft, top) * 1.1f;
        LeftDistance = Vector2.Distance(topleft, left) * 1.1f;

        if (DebugMode)
        {
            Vector2 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
            Vector2 topright = top + right;
            Vector2 bottomleft = bottom + left;
            Vector2 bottomright = bottom + right;

            Ray TopRay = new(topleft, top - topleft);
            Ray LeftRay = new(topleft, left - topleft);

            Debug.DrawRay(center, top, Color.blue);
            Debug.DrawRay(center, bottom, Color.red);
            Debug.DrawRay(center, left, Color.yellow);
            Debug.DrawRay(center, right, Color.magenta);

            Debug.DrawRay(center, topleft, Color.cyan);
            Debug.DrawRay(center, topright, Color.white);
            Debug.DrawRay(center, bottomleft, Color.green);
            Debug.DrawRay(center, bottomright, Color.black);

            Debug.DrawRay(TopRay.origin, TopRay.direction * (TopDistance * 2f), Color.blue, Mathf.Infinity); // TOP viewport ray //
            Debug.DrawRay(LeftRay.origin, LeftRay.direction * (LeftDistance * 2f), Color.yellow, Mathf.Infinity); // LEFT viewport ray //
        }
    }
    private void GenerateWalls()
    {
        GameObject LeftWall = WallObject;
        LeftWall.transform.position = left;
        LeftWall.transform.localScale = new Vector3(Scale, LeftDistance, LeftWall.transform.localScale.z);
        LeftWall.name = "Left";
        Walls[0] = LeftWall;

        GameObject RightWal = Instantiate(WallObject, right, Quaternion.Euler(Vector3.zero));
        RightWal.transform.localScale = new Vector3(Scale, LeftDistance, RightWal.transform.localScale.z);
        RightWal.transform.SetParent(transform);
        RightWal.name = "Right";
        Walls[1] = RightWal;

        GameObject TopWall = Instantiate(WallObject, top, Quaternion.Euler(0, 0, 90));
        TopWall.transform.localScale = new Vector3(Scale, TopDistance, TopWall.transform.localScale.z);
        TopWall.transform.SetParent(transform);
        TopWall.name = "Top";
        Walls[2] = TopWall;

        GameObject BottomWall = Instantiate(WallObject, bottom, Quaternion.Euler(0, 0, 90));
        BottomWall.transform.localScale = new Vector3(Scale, TopDistance, BottomWall.transform.localScale.z);
        BottomWall.transform.SetParent(transform);
        BottomWall.name = "Bottom";
        Walls[3] = BottomWall;
    }
    private void GetOrthographicBounds() // Get Camera Bounds //
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = cam.orthographicSize * 2;
        Bounds bounds = new(cam.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

        left = bounds.center - new Vector3(bounds.extents.x, 0, 0);
        right = bounds.center + new Vector3(bounds.extents.x, 0, 0);
        top = bounds.center + new Vector3(0, bounds.extents.y, 0);
        bottom = bounds.center - new Vector3(0, bounds.extents.y, 0);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}