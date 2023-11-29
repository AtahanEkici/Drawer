using UnityEngine;
public class BallController : MonoBehaviour
{
    public const string BallTag = "Ball";

    [Header("Momentum Calculation")]
    [SerializeField] private float SoundThreshold = 0.05f;
    [SerializeField] private float Momentum;
    [SerializeField] private float MassOfObject;
    [SerializeField] private float Velocity;

    [Header("Tags")]
    [SerializeField] private const string PlatformTag = "Platform";
    [SerializeField] private const string DrawingTag = "Drawing";
    [SerializeField] private const string StarTag = "Star";

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource Audio_Source;

    [Header("Collided Object")]
    [SerializeField] private GameObject CollidedObject;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Update()
    {
        CalculateMomentum();
    }
    private float CalculateMomentum()
    {
        Momentum = MassOfObject * rb.velocity.magnitude;
        return Momentum;
    }
    private void GetLocalReferences()
    {
        try
        {
            rb = GetComponent<Rigidbody2D>();
            Audio_Source = GetComponent<AudioSource>();
            MassOfObject = rb.mass;
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void HandleCollisionSounds(GameObject go)
    {
        try
        {
            if(go.CompareTag(DrawingTag))
            {
                PlaySoundsBasedOnMomentum(SoundManager.Click_Sound);
            }
            else if(go.CompareTag(PlatformTag))
            {
                PlaySoundsBasedOnMomentum(SoundManager.Hit_Sound);
            }
            else
            {
                PlaySoundsBasedOnMomentum(SoundManager.Touch_Sound);
            }
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void PlaySoundsBasedOnMomentum(AudioClip clip)
    {
        if(Momentum < SoundThreshold) { return; }

        else
        {
            Audio_Source.PlayOneShot(clip);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionSounds(collision.gameObject);
    }
}
