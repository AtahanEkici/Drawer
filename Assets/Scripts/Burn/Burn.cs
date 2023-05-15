using UnityEngine;
public class Burn : MonoBehaviour
{
    public const string DestroyParticle_Location = "Particles/BallDestroy";

    [Header("Color Operations")]
    [SerializeField] private Color InitialColor = Color.white;
    [SerializeField] private Color WantedColor = Color.red;

    [Header("Local References")]
    [SerializeField] private Renderer render;

    [Header("Lerp Speed")]
    [SerializeField] private float LerpSpeed = 1f;

    [Header("Is On Burning Platform ?")]
    [SerializeField] private bool isOnBurningPlatform = true;

    [Header("Particles")]
    [SerializeField] private static GameObject DestroyParticle;

    private void Awake()
    {
        GetReferences();
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
    }
    public void BeginCooling()
    {
        isOnBurningPlatform = false;
    }
    private void FireUp()
    {
        if (!isOnBurningPlatform) { return; }

        render.material.color = Color.Lerp(render.material.color, WantedColor, LerpSpeed * Time.smoothDeltaTime);

        if(render.material.color == WantedColor)
        {
            Destroy(gameObject);
            SpawnDestroyedParticle();
        }
    }
    private void CoolDown()
    {
        if (isOnBurningPlatform) { return; }

        render.material.color = Color.Lerp(render.material.color, InitialColor, LerpSpeed * Time.smoothDeltaTime);

        if (render.material.color == InitialColor && !isOnBurningPlatform)
        {
            Destroy(this);
        }
    }

    private void SpawnDestroyedParticle()
    {
        try
        {
            GameObject DestructionParticle = Instantiate(DestroyParticle, gameObject.transform.position, transform.rotation);
            DestructionParticle.transform.localScale = transform.localScale;

            ParticleSystem Particle = DestructionParticle.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = Particle.main;

            ParticleSystemRenderer renderer = DestructionParticle.GetComponent<ParticleSystemRenderer>();
            Material ParticleMaterial = renderer.sharedMaterial;
            MeshFilter meshfilter = GetComponentInParent<MeshFilter>();

            Shader shader = render.material.shader;
            Material particleMaterial = new(shader);

            if (meshfilter == null)
            {
                particleMaterial.SetTexture("_BaseMap", renderer.material.mainTexture);
                renderer.material = particleMaterial;
            }
            else
            {
                Mesh[] mesh = { meshfilter.sharedMesh };
                renderer.SetMeshes(mesh);
            }

            float newSize = (transform.localScale.z == 0 ? (transform.localScale.x + transform.localScale.y) / 4 : (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 6);
            mainModule.startSize = newSize;

            ParticleMaterial.SetColor("_Color", render.material.color);
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}