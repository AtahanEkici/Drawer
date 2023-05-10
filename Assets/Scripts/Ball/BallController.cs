using UnityEngine;
public class BallController : MonoBehaviour
{
    public const string BallTag = "Ball";

    public const string DestroyParticle_Location = "Particles/BallDestroy";

    [Header("Burning Controlls")]
    [SerializeField] private bool isBurning = false;
    [SerializeField] private float BurnSpeed = 2f;

    [Header("Color Operations")]
    [SerializeField] private Color InitialColor = Color.black;
    [SerializeField] private Color MaxBurnColor = Color.red;

    [Header("Renderer")]
    [SerializeField] private Renderer BallRenderer;

    [Header("Particles")]
    [SerializeField] private GameObject DestroyParticle;
    [SerializeField] private GameObject FlameParticle;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        GetResources();
        StartUp();
    }
    private void Update()
    {
        CoolDown();
        FireUp();
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
            BallRenderer.material.color = InitialColor;
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void StartUp()
    {
        //BallRenderer.material.color = Random.ColorHSV();
    }
    private void FireUp()
    {
        if (!isBurning) { return; }

        BallRenderer.material.color = Color.Lerp(BallRenderer.material.color, MaxBurnColor, Time.deltaTime * BurnSpeed);
        
        if(BallRenderer.material.color == MaxBurnColor)
        {
            DestroyBall();
        }
    }
    public void BurnBall(float speed)
    {
        isBurning = true;
        BurnSpeed = speed;
    }
    public void BurnBall()
    {
        isBurning = true;
    }
    private void CoolDown()
    {
        if(isBurning || BallRenderer.material.color == InitialColor) { return; }

        BallRenderer.material.color = Color.Lerp(BallRenderer.material.color, InitialColor, Time.deltaTime * BurnSpeed);
    }
    public void CoolBall()
    {
        isBurning = false;
    }
    public void CoolBall(float speed)
    {
        isBurning = false;
        BurnSpeed = speed;
    }
    private void SpawnDestroyedBallParticle()
    {
        try
        {
            GameObject DestructionParticle = Instantiate(DestroyParticle,transform.position,Quaternion.identity);

            ParticleSystem Particle = DestructionParticle.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule mainModule = Particle.main;

            ParticleSystemRenderer renderer = DestructionParticle.GetComponent<ParticleSystemRenderer>();
            Material ParticleMaterial = renderer.material;
            

            float newSize = 0.2f;
            mainModule.startSize = newSize;
            Debug.Log(newSize);

            ParticleMaterial.SetColor("_Color", BallRenderer.material.color);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            //Debug.Log("Destruction Particle could be FAILED!");
        }
    }
    public void DestroyBall()
    {
        // Game Over  // 
        SpawnDestroyedBallParticle();// Spawn Destruction Particle //
        Destroy(gameObject);
    }
}
