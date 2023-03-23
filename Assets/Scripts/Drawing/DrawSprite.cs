using UnityEngine;
public class DrawSprite : MonoBehaviour
{
    [Header("Line Resource")]
    [SerializeField]private GameObject linePrefab;

    [Header("Main Camera")]
    [SerializeField] private Camera MainCamera;

    [Header("Object Count")]
    [SerializeField] private int ObjectCount = 0;

    private float prefabScale = 0f;

    private GameObject currentLine; 

    private Vector2 mousePosition;
    private Vector2 LastPosition;
    private void Start()
    {
        GetForeignReferences();
    }
    private void Update()
    {
        GetMousePosition();
        DrawObjects();
    }
    private void GetForeignReferences()
    {
        if(MainCamera == null)
        {
            MainCamera = Camera.main;
        }
        if(prefabScale <= 0f)
        {
            prefabScale = linePrefab.transform.localScale.x / 1.5f;
        }
    }
    private void GetMousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void DrawObjects()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentLine = Instantiate(linePrefab, mousePosition, Quaternion.identity);
            currentLine.name = "ParentObject";
            ObjectCount++;
        }
        else if (Input.GetMouseButton(0) && Vector2.Distance(LastPosition, mousePosition) >= prefabScale)
        {
            GameObject newLine = Instantiate(linePrefab, mousePosition, Quaternion.identity);
            newLine.transform.SetParent(currentLine.transform);
            LastPosition = newLine.transform.position;
            ObjectCount++;
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            CombineMeshes(currentLine);
            Debug.Log(ObjectCount);
            ObjectCount = 0;
        }
    }
    private void CombineMeshes(GameObject go)
    {
        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i=0;i<meshFilters.Length;i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        GameObject Combined = new("CombinedObject");
        MeshFilter mesh_Filter = Combined.AddComponent<MeshFilter>();
        mesh_Filter.mesh.CombineMeshes(combine);
        Combined.AddComponent<MeshRenderer>();
        Destroy(go.gameObject);

        Rigidbody2D rb = Combined.AddComponent<Rigidbody2D>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        PolygonCollider2D PolCol = Combined.AddComponent<PolygonCollider2D>();

        Vector3[] vertices = mesh_Filter.mesh.vertices;
        Vector2[] vertices2D = new Vector2[vertices.Length];

        for(int i=0;i<vertices.Length;i++)
        {
            vertices2D[i] = (Vector2) vertices[i];
        }

        PolCol.points = vertices2D;

        Collider2DOptimization.PolygonColliderOptimizer.OptimizePoligonCollider(PolCol);
    }
}
