using System.Collections.Generic;
using UnityEngine;

public class DrawSprite : MonoBehaviour
{
    public GameObject linePrefab;

    private GameObject currentLine; 
    private Vector2 mousePosition;

    private void Awake()
    {
        
    }
    private void Start()
    {
        
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
            currentLine = Instantiate(linePrefab, mousePosition, Quaternion.identity); // Change the rotation //
        }
        else if (Input.GetMouseButton(0))
        {
                GameObject newLine = Instantiate(linePrefab, mousePosition, Quaternion.identity);
                newLine.transform.SetParent(currentLine.transform);
        }
        else if (Input.GetMouseButtonUp(0)) // Check if the left mouse button is released
        {
            currentLine.AddComponent<Rigidbody2D>();
            PolygonCollider2D PC2D = currentLine.AddComponent<PolygonCollider2D>();
            //SetColliderBounds(currentLine, PC2D);
            Debug.Log(currentLine.transform.childCount);
        }
    }
    /*
    private void SetColliderBounds(GameObject go, PolygonCollider2D pc2d)
    {
        SpriteRenderer[] childSprites = go.GetComponentsInChildren<SpriteRenderer>();

        List<Vector2> vertices = new List<Vector2>();

        foreach (SpriteRenderer sprite in childSprites)
        {
            Vector3[] corners = new Vector3[4];
            sprite.transform.localToWorldMatrix.MultiplyPoint3x4(sprite.sprite.bounds.min);
            sprite.transform.localToWorldMatrix.MultiplyPoint3x4(sprite.sprite.bounds.max);

            corners[0] = sprite.transform.InverseTransformPoint(corners[0]);
            corners[1] = sprite.transform.InverseTransformPoint(corners[1]);
            corners[2] = sprite.transform.InverseTransformPoint(corners[2]);
            corners[3] = sprite.transform.InverseTransformPoint(corners[3]);

            vertices.Add(corners[0]);
            vertices.Add(corners[1]);
            vertices.Add(corners[2]);
            vertices.Add(corners[3]);
        }

        // Set the collider vertices to the vertex list
        pc2d.points = vertices.ToArray();
    }
    */
}
