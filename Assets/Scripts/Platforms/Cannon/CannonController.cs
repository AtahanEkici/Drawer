using UnityEngine;
public class CannonController : MonoBehaviour
{
    [Header("Left Or Right")]
    [SerializeField] private bool Right = true;

    [Header("Local Componenets")]
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Vector3 InitialRotation;
    [SerializeField] private AudioSource As;

    [Header("Child Componenets")]
    [SerializeField] private CannonRammer cannonRammer;
    [SerializeField] private Transform GunBaseLocation;
    [SerializeField] private Transform RotatablePlatform;

    [Header("Ball Operations")]
    [SerializeField] public bool isBallOnBreach = false;
    [SerializeField] private float BallLaunchForce = 5f;

    [Header("Rotation Duration")]
    [SerializeField] private float RotationSpeed = 25f;
    [SerializeField] private float Rotationtime = 2.0f;
    private float InitialDuration = 0f;
    private void Awake()
    {
        Initialize();
    }
    private void Update()
    {
        AnimateCannonMoving();
        ResetCannonLocation();
    }
    private void Initialize()
    {
        InitialDuration = Rotationtime;

        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
        if(InitialRotation == null)
        {
            InitialRotation = RotatablePlatform.rotation.eulerAngles;
        }
        if(cannonRammer == null)
        {
            cannonRammer = GetComponentInChildren<CannonRammer>();
        }
        if(RotatablePlatform == null)
        {
            RotatablePlatform = transform.GetChild(0);
        }
        if(GunBaseLocation == null)
        {
            GunBaseLocation = transform.GetChild(1).GetChild(0);
        }
        if(As == null)
        {
            As = GetComponent<AudioSource>();
        }
        if(As.clip == null)
        {
            As.clip = SoundManager.CannonShot;
        }
    }
    private void AnimateCannonMoving()
    {
        if (!isBallOnBreach) { return; } // if the ball is not in the breach return //

        Rotationtime -= Time.smoothDeltaTime;

        if (Rotationtime <= 0)
        {
            LaunchBall();
            Rotationtime = InitialDuration;
        }
        else
        {
            if (Right)
            {
                RotatablePlatform.RotateAround(GunBaseLocation.position, Vector3.back, RotationSpeed * Time.smoothDeltaTime);
            }
            else
            {
                RotatablePlatform.RotateAround(GunBaseLocation.position, -Vector3.back, RotationSpeed * Time.smoothDeltaTime);
            }
        }  
    }
    private void ResetCannonLocation()
    {
        if (isBallOnBreach) { return; }

        float distance = Vector3.Distance(InitialRotation, RotatablePlatform.rotation.eulerAngles);
        
        if (distance <= 0.1f)
        {
            RotatablePlatform.Rotate(InitialRotation);
            return;
        }
        else
        {
            if(Right)
            {
                RotatablePlatform.RotateAround(GunBaseLocation.position, -Vector3.back, RotationSpeed * Time.smoothDeltaTime);
            }
            else
            {
                RotatablePlatform.RotateAround(GunBaseLocation.position, Vector3.back, RotationSpeed * Time.smoothDeltaTime);
            }
        }
    }
    private void LaunchBall()
    {
        As.Play();
        cannonRammer.LaunchBall(RotatablePlatform.up, BallLaunchForce, ForceMode2D.Impulse);
    }
}
