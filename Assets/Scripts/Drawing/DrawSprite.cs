using System.Collections.Generic;
using UnityEngine;

public class DrawSprite : MonoBehaviour
{
    public GameObject linePrefab;

    private float LineScale_X;
    private GameObject currentLine; 
    private Vector2 mousePosition;
    private Vector2 LastPosition;

    private void Awake()
    {
        
    }
    private void Start()
    {
        LineScale_X = linePrefab.transform.localScale.x;
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
        else if (Input.GetMouseButton(0) && Vector2.Distance(LastPosition,mousePosition) <= LineScale_X)
        {
                GameObject newLine = Instantiate(linePrefab, mousePosition, Quaternion.identity);
                newLine.transform.SetParent(currentLine.transform);
                newLine.AddComponent<BoxCollider2D>().autoTiling = true;
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            currentLine.AddComponent<Rigidbody>();
            //MergeColliders(currentLine);
            //PolygonCollider2D PC2D = currentLine.AddComponent<PolygonCollider2D>();
            //PC2D.autoTiling = true;
            //SetColliderBounds(currentLine, PC2D);
            //Debug.Log(currentLine.transform.childCount);
        }
        LastPosition = mousePosition;
    }
    private void MergeColliders(GameObject go)
    {
        Collider2D[] colliders = go.GetComponentsInChildren<Collider2D>();
        CompositeCollider2D comp = go.AddComponent<CompositeCollider2D>();

        comp.geometryType = CompositeCollider2D.GeometryType.Polygons;
        comp.generationType = CompositeCollider2D.GenerationType.Synchronous;
        comp.vertexDistance = 0.01f;
        comp.edgeRadius = 0.01f;

        /*
        for(int i=0;i< colliders.Length;i++)
        {
            Vector2[] points = new Vector2[4];
            points[0] = new Vector2(colliders[i].offset.x - colliders[i].bounds.size.x / 2, colliders[i].offset.y - colliders[i].bounds.size.y / 2);
            points[1] = new Vector2(colliders[i].offset.x + colliders[i].bounds.size.x / 2, colliders[i].offset.y - colliders[i].bounds.size.y / 2);
            points[2] = new Vector2(colliders[i].offset.x + colliders[i].bounds.size.x / 2, colliders[i].offset.y + colliders[i].bounds.size.y / 2);
            points[3] = new Vector2(colliders[i].offset.x - colliders[i].bounds.size.x / 2, colliders[i].offset.y + colliders[i].bounds.size.y / 2);
            comp.bounds = points;
            Destroy(colliders[i]);
        }
        */

    }
    private void SetColliderBounds(GameObject go, PolygonCollider2D pc2d)
    {
        SpriteRenderer[] childSpriteRenderers = go.GetComponentsInChildren<SpriteRenderer>();
        Debug.Log(childSpriteRenderers.Length);

        Bounds bounds = new(childSpriteRenderers[0].bounds.center, Vector3.zero);

        for (int i = 1; i < childSpriteRenderers.Length; i++)
        {
            bounds.Encapsulate(childSpriteRenderers[i].bounds);
        }

        Vector2[] points = new Vector2[4];

        points[0] = new Vector2(bounds.min.x, bounds.min.y);
        points[1] = new Vector2(bounds.max.x, bounds.min.y);
        points[2] = new Vector2(bounds.max.x, bounds.max.y);
        points[3] = new Vector2(bounds.min.x, bounds.max.y);

        pc2d.points = points;
    }
}
