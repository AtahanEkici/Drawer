using UnityEngine;
public class BallController : MonoBehaviour
{
    public const string BallTag = "Ball";

    [Header("Burning Controlls")]
    [SerializeField] private bool isBurning = false;
    [SerializeField] private float BurnSpeed = 2f;

    [Header("Color Operations")]
    [SerializeField] private Color InitialColor = Color.black;
    [SerializeField] private Color MaxBurnColor = Color.red;

    [Header("Renderer")]
    [SerializeField] private Renderer BallRenderer;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        StartUp();
    }
    private void Update()
    {
        CoolDown();
        FireUp();
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

    public void DestroyBall()
    {
        // Game Over  // 
        // Spawn Destruction Particle //
        Destroy(gameObject);
    }
}
