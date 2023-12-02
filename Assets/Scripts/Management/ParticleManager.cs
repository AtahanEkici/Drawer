using UnityEngine;
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance = null;

    public const string DestroyParticle_Location = "Particles/BallDestroy";

    [Header("Particles")]
    [SerializeField] private static GameObject DestroyParticle;

    [SerializeField] private bool DontDestroyOn_Load = true;

    private void Awake()
    {
        CheckInstance();
        GetResources();
    }

    private void CheckInstance()
    {
        try
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                Destroy(this);
            }
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        } 

        if(DontDestroyOn_Load)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void GetResources()
    {
        DestroyParticle = Resources.Load<GameObject>(DestroyParticle_Location) as GameObject;
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
                destructionParticle = Instantiate(DestroyParticle, touch_point ?? go.transform.position, go.transform.rotation);
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
