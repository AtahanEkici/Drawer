using UnityEngine;
using static UnityEditor.PlayerSettings;
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

    [Header("IsDrawing")]
    [SerializeField] private bool isDrawing = false;
    [SerializeField] private Drawing drawing_reference = null;

    private void Awake()
    {
        GetReferences();
        CheckIsDrawing();
    }
    private void CheckIsDrawing()
    {
        drawing_reference = GetComponent<Drawing>();

        if (drawing_reference != null)
        {
            isDrawing = true;
        }
    }
    private void Update()
    {
        FireUp();
        CoolDown();
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

        Debug.Log(gameObject.name + " is burning up");

        render.material.color = Color.Lerp(render.material.color, WantedColor, LerpSpeed * Time.deltaTime);

        // Check the approximate color difference instead of direct comparison
        if (ColorDifference(render.material.color, WantedColor) < 0.001f)
        {
            SpawnDestroyedParticle();
            Destroy(gameObject);
        }
    }

    private void CoolDown()
    {
        if (isOnBurningPlatform) { return; }

        Debug.Log(gameObject.name + " is cooling down");

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
    private void SpawnDestroyedParticle()
    {
        try
        {
                GameObject destructionParticle;

                if (isDrawing)
                {
                    destructionParticle = Instantiate(DestroyParticle, GetComponent<Drawing>().GetCenter(), transform.rotation);
                }
                else
                {
                    destructionParticle = Instantiate(DestroyParticle, transform.position, transform.rotation);
                }

                ParticleSystem particle = destructionParticle.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule mainModule = particle.main;

                ParticleSystemRenderer renderer = destructionParticle.GetComponent<ParticleSystemRenderer>();
                Material particleMaterial = renderer.material;

                if (isDrawing)
                {
                    mainModule.startSize = transform.localScale.x / 3;

                    if (TryGetComponent<MeshFilter>(out var meshfilter))
                    {
                        Mesh[] meshes = { meshfilter.sharedMesh };
                        renderer.SetMeshes(meshes);

                        // Set the rotation of the destruction particle to match the original object
                        destructionParticle.transform.rotation = transform.rotation;
                    }
                }
                else
                {
                    mainModule.startSize = transform.localScale.x / 2;
                }

                if (TryGetComponent<Renderer>(out var render))
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
