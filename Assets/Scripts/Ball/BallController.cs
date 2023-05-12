using UnityEngine;
public class BallController : MonoBehaviour
{
    public const string BallTag = "Ball";
    public const string DestroyParticle_Location = "Particles/BallDestroy";

    [Header("Renderer")]
    [SerializeField] private Renderer BallRenderer;

    [Header("Particles")]
    [SerializeField] private static GameObject DestroyParticle;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        GetResources();
    }
    private void GetResources()
    {
        DestroyParticle = Resources.Load<GameObject>(DestroyParticle_Location) as GameObject;
    }
    private void GetLocalReferences()
    {
        try
        {
            BallRenderer = GetComponent<Renderer>();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void SpawnDestroyedParticle()
    {
        try
        {
            GameObject DestructionParticle = Instantiate(DestroyParticle,transform.position,Quaternion.identity);
            DestructionParticle.transform.localScale = transform.localScale;

            ParticleSystem Particle = DestructionParticle.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = Particle.main;

            ParticleSystemRenderer renderer = DestructionParticle.GetComponent<ParticleSystemRenderer>();
            Material ParticleMaterial = renderer.sharedMaterial;
            MeshFilter meshfilter = GetComponentInParent<MeshFilter>();

            Shader shader = BallRenderer.material.shader;
            Material particleMaterial = new Material(shader);

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

            ParticleMaterial.SetColor("_Color", BallRenderer.material.color);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void OnDestroy()
    {
        if (gameObject.scene.isLoaded) //Was Deleted
        {
            DestroyBall();
        }
    }
    private void DestroyBall()
    {
        // Game Over  // 
        SpawnDestroyedParticle(); // Spawn Destruction Particle //
    }
}
