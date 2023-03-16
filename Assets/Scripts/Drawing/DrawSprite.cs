using UnityEngine;

public class DrawSprite : MonoBehaviour
{
    private Texture2D texture;
    private Sprite sprite;
    private Color[] pixels;
    private Vector2 previousMousePosition;
    private bool isDrawing;
    private GameObject drawnObject;

    public int width = 512;
    public int height = 512;
    public float pixelSize = 0.1f;
    public Color lineColor = Color.white;

    void Start()
    {
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        pixels = new Color[width * height];
        sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
            isDrawing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
            drawnObject = CreateDrawnObject();
        }

        if (isDrawing)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2Int pixelStart = WorldToPixel(previousMousePosition);
            Vector2Int pixelEnd = WorldToPixel(currentMousePosition);
            DrawLine(pixelStart, pixelEnd, lineColor);
            previousMousePosition = currentMousePosition;
            texture.SetPixels(pixels);
            texture.Apply();
        }
    }

    void DrawLine(Vector2Int start, Vector2Int end, Color color)
    {
        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);
        int sx = start.x < end.x ? 1 : -1;
        int sy = start.y < end.y ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            pixels[start.y * width + start.x] = color;

            if (start.x == end.x && start.y == end.y)
            {
                break;
            }

            int e2 = 2 * err;

            if (e2 > -dy)
            {
                err -= dy;
                start.x += sx;
            }

            if (e2 < dx)
            {
                err += dx;
                start.y += sy;
            }
        }
    }

    Vector2Int WorldToPixel(Vector2 worldPosition)
    {
        float pixelSizeInWorldUnits = pixelSize / Camera.main.orthographicSize;
        Vector2 offset = new Vector2(width, height) * 0.5f;
        Vector2Int pixelPosition = new Vector2Int((int)((worldPosition.x + offset.x) / pixelSizeInWorldUnits), (int)((worldPosition.y + offset.y) / pixelSizeInWorldUnits));
        pixelPosition.x = Mathf.Clamp(pixelPosition.x, 0, width - 1);
        pixelPosition.y = Mathf.Clamp(pixelPosition.y, 0, height - 1);
        return pixelPosition;
    }

    private GameObject CreateDrawnObject()
    {
        // Create a new game object with a sprite renderer and a rigidbody 2D component
        GameObject drawnObject = new GameObject();
        drawnObject.AddComponent<SpriteRenderer>();
        drawnObject.AddComponent<Rigidbody2D>();

        // Assign the drawn sprite to the sprite renderer
        SpriteRenderer spriteRenderer = drawnObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        // Add a BoxCollider2D component to the game object and set its size based on the drawn sprite
        BoxCollider2D boxCollider = drawnObject.AddComponent<BoxCollider2D>();
        boxCollider.size = spriteRenderer.bounds.size;

        // Add a Rigidbody2D component to the game object
        Rigidbody2D rigidbody2D = drawnObject.GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 1f;

        // Move the drawn object to the center of the screen and reset its rotation
        drawnObject.transform.position = new Vector3(0, 0, 0);
        drawnObject.transform.rotation = Quaternion.identity;

        // Set the pixels array to transparent, so that the next drawing starts on a blank slate
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }

        // Apply the changes to the texture
        texture.SetPixels(pixels);
        texture.Apply();

        // Return the drawn object
        return drawnObject;
    }
}
