using UnityEngine;
public class Burn : MonoBehaviour
{ 
    [Header("Color Operations")]
    [SerializeField] private Color InitialColor = Color.white;
    [SerializeField] private static Color WantedColor = Color.red;

    [Header("Local References")]
    [SerializeField] private Renderer render;

    [Header("Lerp Speed")]
    [SerializeField] private float LerpSpeed = 5f;

    [Header("Is On Burning Platform ?")]
    [SerializeField] private bool isOnBurningPlatform = true;

    private void Awake()
    {
        GetReferences();
    }
    public static bool CheckIsDrawing(GameObject go)
    {
        return go.TryGetComponent(out Drawing _);
    }
    private void Update()
    {
        FireUp();
        CoolDown();
        CheckCollision(gameObject);
    }
    private void GetReferences()
    {
        render = GetComponentInParent<Renderer>();
        InitialColor = render.material.color;
    }
    public void AdjustBurnSpeed(float adjustedSpeed)
    {
        LerpSpeed = adjustedSpeed;
        isOnBurningPlatform = true;
    }
    public void BeginBurning()
    {
        isOnBurningPlatform = true;
    }
    public void BeginCooling(float adjustedSpeed)
    {
        LerpSpeed = adjustedSpeed;
        isOnBurningPlatform = false;
    }
    public void BeginCooling()
    {
        isOnBurningPlatform = false;
    }
    private void FireUp()
    {
        if (!isOnBurningPlatform) { return; }

        //Debug.Log(gameObject.name + " is burning up");

        render.material.color = Color.Lerp(render.material.color, WantedColor, LerpSpeed * Time.deltaTime);

        // Check the approximate color difference instead of direct comparison
        if (ColorDifference(render.material.color, WantedColor) < 0.001f)
        {
            ParticleManager.SpawnDestroyedParticle(gameObject, CheckCollision(gameObject));
        }
    }
    private void CoolDown()
    {
        if (isOnBurningPlatform) { return; }

        //Debug.Log(gameObject.name + " is cooling down");

        render.material.color = Color.Lerp(render.material.color, InitialColor, LerpSpeed * Time.deltaTime);

        // Check the approximate color difference instead of direct comparison
        if (ColorDifference(render.material.color, InitialColor) < 0.001f)
        {
            Destroy(this);
        }
    }
    private float ColorDifference(Color colorA, Color colorB)
    {
        return Mathf.Abs(colorA.r - colorB.r) + Mathf.Abs(colorA.g - colorB.g) + Mathf.Abs(colorA.b - colorB.b) + Mathf.Abs(colorA.a - colorB.a);
    }
    public Vector3? CheckCollision(GameObject go)
    {
        float raycastLength = go.transform.localScale.magnitude;
        
        if (go.TryGetComponent<Collider2D>(out var originCollider))
        {
            Ray2D hitRay = new(transform.position, -transform.up * raycastLength);

            //Debug.DrawRay(hitRay.origin, hitRay.direction, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(hitRay.origin, hitRay.direction, raycastLength);

            //Debug.Log("Hit: " + hit.collider.gameObject);

            if (hit.collider != originCollider)
            {
                return hit.point;
            }
            else
            {
                return null;
            }
        }
        return null;
    }
}
