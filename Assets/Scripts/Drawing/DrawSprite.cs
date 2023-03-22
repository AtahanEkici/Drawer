using System.Collections.Generic;
using UnityEngine;
public class DrawSprite : MonoBehaviour
{
    public GameObject linePrefab;

    private float prefabScale = 0f;
    private GameObject currentLine; 
    private Vector2 mousePosition;
    private Vector2 LastPosition;
    private void Start()
    {
        prefabScale = linePrefab.transform.localScale.x / 2;
    }
    private void Update()
    {
        DrawObjects();
    }
    private void DrawObjects()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            currentLine = Instantiate(linePrefab, mousePosition, Quaternion.identity);
            currentLine.name = "ParentObject";
        }
        else if (Input.GetMouseButton(0) && Vector2.Distance(LastPosition, mousePosition) >= prefabScale)
        {
                GameObject newLine = Instantiate(linePrefab, mousePosition, Quaternion.identity);
                newLine.transform.SetParent(currentLine.transform);
                LastPosition = newLine.transform.position;
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            CombineMeshes(currentLine);
        }
    }
    private void CombineMeshes(GameObject go)
    {
        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        BoxCollider2D[] boxColliders = go.GetComponentsInChildren<BoxCollider2D>();

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

            List<Vector2> colliderPoints = new List<Vector2>();

            foreach (BoxCollider2D boxCollider in boxColliders)
            {
                Vector2[] points = GetBoxColliderPoints(boxCollider);
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = boxCollider.transform.TransformPoint(points[i]);
                }
                colliderPoints.AddRange(points);
                Destroy(boxCollider);
            }
        
        PolygonCollider2D polygonCollider = Combined.AddComponent<PolygonCollider2D>();
        polygonCollider.points = colliderPoints.ToArray();
        Collider2DOptimization.PolygonColliderOptimizer.OptimizePoligonCollider(polygonCollider);

    /*
    PolygonCollider2D PolCol = Combined.AddComponent<PolygonCollider2D>();

    Vector3[] vertices = mesh_Filter.mesh.vertices;

    int[] triangles = mesh_Filter.mesh.triangles;

    Vector2[] edges = new Vector2[triangles.Length];

    int edgeIndex = 0;

    for (int i = 0; i < triangles.Length; i += 3)
    {
        edges[edgeIndex++] = new Vector2(vertices[triangles[i]].x, vertices[triangles[i]].y);
        edges[edgeIndex++] = new Vector2(vertices[triangles[i + 1]].x, vertices[triangles[i + 1]].y);
        edges[edgeIndex++] = new Vector2(vertices[triangles[i + 2]].x, vertices[triangles[i + 2]].y);
    }
    PolCol.points = edges;
    */
}
    private Vector2[] GetBoxColliderPoints(BoxCollider2D boxCollider)
    {
        Vector2 size = boxCollider.size;
        Vector2 center = boxCollider.offset;

        Vector2 topLeft = new Vector2(center.x - size.x / 2f, center.y + size.y / 2f);
        Vector2 topRight = new Vector2(center.x + size.x / 2f, center.y + size.y / 2f);
        Vector2 bottomLeft = new Vector2(center.x - size.x / 2f, center.y - size.y / 2f);
        Vector2 bottomRight = new Vector2(center.x + size.x / 2f, center.y - size.y / 2f);

        return new Vector2[] { topLeft, topRight, bottomRight, bottomLeft };
    }
}
