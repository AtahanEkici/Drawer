using System.Collections.Generic;
using UnityEngine;
public class SlingLineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [Header("Lists")]
    [SerializeField] private List<GameObject> colliderObjects;
    [SerializeField] private List<Vector3> ropePositions;
    [SerializeField] private List<Rigidbody2D> chainsRigidbodies;

    private GameObject ropeCollidersCollection;

    [Header("Chain Layer")]
    [SerializeField] private int chainColliderLayer;
    [SerializeField] private int RopeColliderLayer;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        ropeCollidersCollection = new("Rope Colliders Collection_" + GetInstanceID())
        {
            tag = "Sling"
        };
        ropeCollidersCollection.transform.localScale = Vector3.one;

        chainColliderLayer = LayerMask.NameToLayer("Chain");
        RopeColliderLayer = LayerMask.NameToLayer("Rope");

        //Physics2D.IgnoreLayerCollision(chainColliderLayer, RopeColliderLayer);
    }

    private void Update()
    {
        GetChainLocations();
        UpdateColliders();
    }
    private void Initialize()
    {
        lineRenderer = GetComponent<LineRenderer>();

        colliderObjects = new();
        ropePositions = new();
        chainsRigidbodies = new();
    }
    private void GetChainLocations()
    {
        ropePositions.Clear();
        ropePositions.Add(transform.position);

        foreach (Transform child in transform)
        {
            ropePositions.Add(child.position);
            chainsRigidbodies.Add(child.GetComponent<Rigidbody2D>());
        }

        lineRenderer.positionCount = ropePositions.Count;
        lineRenderer.SetPositions(ropePositions.ToArray());
    }

    private void UpdateColliders()
    {
        if (Time.timeScale <= 0f) return; // Do not update colliders if the game is paused

        // Clear existing collider GameObjects
        foreach (var colliderObject in colliderObjects)
        {
            Destroy(colliderObject);
        }
        colliderObjects.Clear();

        int numPoints = lineRenderer.positionCount;
        if (numPoints < 2) return;

        for (int i = 0; i < numPoints - 1; i++)
        {
            Vector3 start = lineRenderer.GetPosition(i);
            Vector3 end = lineRenderer.GetPosition(i + 1);

            // Create a new GameObject for the collider
            GameObject rope = new("LineCollider_" + i);
            rope.transform.parent = ropeCollidersCollection.transform;  // Parent to RopeCollidersCollection
            rope.tag = "Sling";
            rope.layer = RopeColliderLayer;

            BoxCollider2D collider = rope.AddComponent<BoxCollider2D>();
            colliderObjects.Add(rope);

            Vector3 center = (start + end) / 2;

            float length = Vector3.Distance(start, end) / 3.5f;
            collider.size = new Vector2(length, lineRenderer.endWidth); // Adjust the width as needed
            rope.transform.position = center;

            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            rope.transform.rotation = Quaternion.Euler(0, 0, angle);

            SpringJoint2D sj2d = rope.AddComponent<SpringJoint2D>();
            sj2d.autoConfigureConnectedAnchor = true;
            sj2d.dampingRatio = 0.9f;
            sj2d.frequency = 3f;
            sj2d.distance = 0.005f;
            sj2d.connectedBody = chainsRigidbodies[i];
        }
    }
}
