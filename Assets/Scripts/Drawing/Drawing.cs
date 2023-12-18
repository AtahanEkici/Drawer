using UnityEngine;
public class Drawing : MonoBehaviour
{
    public const string DrawingTag = "Drawing";

    [Header("Local Components")]
    [SerializeField] private MeshRenderer render;
    [SerializeField] private MeshFilter mesh_filter;

    [Header("Debugging")]
    [SerializeField] private bool DebugMode = false;
    [SerializeField] private Vector3 CenterOfObject = Vector3.zero;

    [Header("Initial Color")]
    [SerializeField] private Color InitialColor;

    [Header("Position Threshold")]
    [SerializeField] private float MaxDistance = -50f;

    private void Awake()
    {
        GetComponents();
    }
    private void Start()
    {
        StartUpTasks();
    }
    private void Update()
    {
        Debugging();
        DestroyOnCertainPoint();
    }
    private void StartUpTasks()
    {
        InitialColor = GetComponent<Renderer>().material.color;
    }
    private void GetComponents()
    {
        render = GetComponent<MeshRenderer>();
        mesh_filter = GetComponent<MeshFilter>();
    }
    public Vector3 GetWorldPosition()
    {
        Vector3 local_Position = transform.localPosition;
        return transform.parent.TransformPoint(local_Position);
    }
    public Vector3 GetCenter()
    {
        return render.bounds.center;
    }
    public Vector3 GetMeshCenter()
    {
        return mesh_filter.mesh.bounds.center;
    }
    private void Debugging()
    {
        if (!DebugMode) { return; }
       Debug.DrawLine(transform.position,CenterOfObject,Color.magenta,Time.deltaTime);
       Debug.DrawLine(transform.position, transform.localPosition, Color.yellow, Time.deltaTime);
    }
    public Color GetInitialColor()
    {
        return InitialColor;
    }
    private void DestroyOnCertainPoint()
    {
        if(transform.position.y < MaxDistance)
        {
            Destroy(gameObject);
            ParticleManager.SpawnDestroyedParticle(this.gameObject);
        }
    }
}
