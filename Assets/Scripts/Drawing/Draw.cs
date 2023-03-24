using System.Collections.Generic;
using UnityEngine;
public class Draw : MonoBehaviour
{
    private const string LineMaterialResourcePath = "Materials/LineMaterial/LineMaterial";

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer Line_Renderer;
    [SerializeField] private float LineRendererWidth = 0.1f;

    [Header("Mouse Options")]
    [SerializeField] private Vector3 mousePos;

    [Header("New Object")]
    [SerializeField] private GameObject NewDrawing;

    [Header("Camera Info")]
    [SerializeField] private Camera MainCamera;

    [Header("Drawings")]
    [SerializeField] private List<GameObject> Drawings = new();

    [Header("Simplification Coefficient")]
    [SerializeField] private float SimplificationCoefficient = 0.01f;

    [Header("Line Material")]
    [SerializeField] private Material LineMaterial;
    private void Awake()
    {
        LineMaterial = GetLineMaterial();
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
        mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);

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
    private void StartDrawing()
    {
        NewDrawing = new("Drawing" + Drawings.Count.ToString());
        Line_Renderer = NewDrawing.AddComponent<LineRenderer>();
        Line_Renderer.material = LineMaterial;
        Line_Renderer.startWidth = LineRendererWidth;
        Line_Renderer.endWidth = LineRendererWidth;
        Line_Renderer.positionCount = 1;
        Line_Renderer.SetPosition(Line_Renderer.positionCount - 1, mousePos);
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

        Destroy(Line_Renderer);

        Rigidbody2D rb = NewDrawing.AddComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.useAutoMass = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        AttachCapsuleCollidersToPoints(Line_Renderer, NewDrawing);

        Drawings.Add(NewDrawing);

        //RemoveOverlappingColliders(NewDrawing); // Does not work //
    }
    private void AttachCapsuleCollidersToPoints(LineRenderer lr, GameObject go)
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
                GameObject capsule = new GameObject("Capsule Collider " + i);
                CapsuleCollider2D collider = capsule.AddComponent<CapsuleCollider2D>();
                collider.transform.position = (positions[i] + positions[i + 1]) / 2f;

                float distance = Vector2.Distance(positions[i], positions[i + 1]);
                collider.size = new Vector2(distance, LineRendererWidth);
                collider.direction = CapsuleDirection2D.Horizontal;
                Vector2 direction = positions[i + 1] - positions[i];
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                collider.transform.rotation = Quaternion.Euler(0, 0, angle);

                capsule.transform.parent = go.transform;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void RemoveOverlappingColliders(GameObject go)
    {
        CapsuleCollider2D[] colliders = go.GetComponentsInChildren<CapsuleCollider2D>();

        foreach (CapsuleCollider2D collider in colliders)
        {
            Collider2D[] overlappingColliders = Physics2D.OverlapCapsuleAll(
                collider.transform.position,
                collider.size,
                collider.direction,
                collider.transform.rotation.eulerAngles.z,
                collider.gameObject.layer
            );

            foreach (Collider2D overlappingCollider in overlappingColliders)
            {
                if (overlappingCollider != collider)
                {
                    Destroy(collider.gameObject);
                    Debug.Log("Destroyed");
                    break;
                }
            }
        }
    }
    private static void AssignPolygonCollider(LineRenderer lr, GameObject go)
    {
        PolygonCollider2D polCol = go.AddComponent<PolygonCollider2D>();

        int posCount = lr.positionCount;

        Vector2[] points = new Vector2[posCount];

        for (int i = 0; i < posCount; i++)
        {
            points[i] = (Vector2)lr.GetPosition(i);
        }

        polCol.points = points;

        Collider2DOptimization.PolygonColliderOptimizer.OptimizePoligonCollider(polCol);
    }
    private static void AssignEdgeCollider(LineRenderer lr, GameObject go)
    {
        int maxPos = lr.positionCount;

        EdgeCollider2D EdgeCollider = go.AddComponent<EdgeCollider2D>();

        List<Vector2> Points = new();

        for (int i = 0; i < maxPos; i++)
        {
            Points.Add((Vector2)lr.GetPosition(i));
        }
        EdgeCollider.SetPoints(Points);
    }
    private static Material GetLineMaterial()
    {
        return Resources.Load(LineMaterialResourcePath) as Material;
    }
}