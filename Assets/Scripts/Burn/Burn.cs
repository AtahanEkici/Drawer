using UnityEngine;
public class Burn : MonoBehaviour
{
    public const string DestroyParticle_Location = "Particles/BallDestroy";

    [Header("Color Operations")]
    [SerializeField] private Color InitialColor = Color.white;
    [SerializeField] private static Color WantedColor = Color.red;

    [Header("Local References")]
    [SerializeField] private Renderer render;

    [Header("Lerp Speed")]
    [SerializeField] private float LerpSpeed = 5f;

    [Header("Is On Burning Platform ?")]
    [SerializeField] private bool isOnBurningPlatform = true;

    [Header("Particles")]
    [SerializeField] private static GameObject DestroyParticle;

    private void Awake()
    {
        GetReferences();
    }
    public static bool CheckIsDrawing(GameObject go)
    {
       return go.TryGetComponent(out Drawing drawing_reference);
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
        DestroyParticle = Resources.Load<GameObject>(DestroyParticle_Location) as GameObject;
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
            PlayExplosionSound();
            Destroy(gameObject);
            SpawnDestroyedParticle(gameObject, CheckCollision(gameObject));
        }
    }
    private void PlayExplosionSound() // Destroy the GameObject after the duration of the audio clip //
    {
        AudioSource audioSource_External = new GameObject("DestroyedBallSound").AddComponent<AudioSource>();
        audioSource_External.PlayOneShot(SoundManager.Explosion_Sound);
        Destroy(audioSource_External.gameObject, SoundManager.Explosion_Sound.length * 2);
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

    public static void SpawnDestroyedParticle(GameObject go, Vector3? touch_point = null)
    {
        try
        {
            bool isDrawing = go.TryGetComponent<Drawing>(out Drawing drawing_ref);
            GameObject destructionParticle;

            //Debug.Log("Touch Point: "+touch_point);

            if (isDrawing)
            {
                destructionParticle = Instantiate(DestroyParticle, touch_point ?? drawing_ref.GetCenter(), drawing_ref.transform.rotation);
            }
            else
            {
                destructionParticle = Instantiate(DestroyParticle, go.transform.position, go.transform.rotation);
            }

            ParticleSystem particle = destructionParticle.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = particle.main;

            ParticleSystemRenderer renderer = destructionParticle.GetComponent<ParticleSystemRenderer>();
            Material particleMaterial = renderer.material;

            if (isDrawing)
            {
                mainModule.startSize = (go.transform.localScale.x) / 3;

                if (go.TryGetComponent<MeshFilter>(out var meshfilter))
                {
                    Mesh[] meshes = { meshfilter.sharedMesh };
                    renderer.SetMeshes(meshes);

                    // Set the rotation of the destruction particle to match the original object
                    destructionParticle.transform.rotation = go.transform.rotation;
                }
            }
            else
            {
                mainModule.startSize = go.transform.localScale.x / 2;
            }

            if (go.TryGetComponent<Renderer>(out var render))
            {
                particleMaterial.SetColor("_Color", render.material.color);
                renderer.material.shader = render.material.shader;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }



}
