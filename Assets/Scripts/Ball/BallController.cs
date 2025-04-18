using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private const string SlingTag = "Sling";

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
    private void CalculateMomentum()
    {
        Momentum = rb.mass * rb.linearVelocity.magnitude;
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
            switch (go.tag)
            {
                case DrawingTag:
                    PlaySoundsBasedOnMomentum(SoundManager.Click_Sound);
                    break;
                case PlatformTag:
                    PlaySoundsBasedOnMomentum(SoundManager.Hit_Sound);
                    break;
                case SlingTag:
                    PlaySoundsBasedOnMomentum(SoundManager.RopeTouch);
                    break;
                default:
                    PlaySoundsBasedOnMomentum(SoundManager.Touch_Sound);
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void PlaySoundsBasedOnMomentum(AudioClip clip)
    {
        if(Momentum < SoundThreshold) { return; }

        else if(Audio_Source.isActiveAndEnabled)
        {
            Audio_Source.PlayOneShot(clip);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionSounds(collision.gameObject);
        //Debug.Log("Touched: "+collision.gameObject);
    }

    private void OnDestroy()
    {
        try
        {
            GameManager.Instance.GameOver(GameManager.Ball_Is_Destroyed);
            Handheld.Vibrate();
        }
        catch(Exception e)
        {
            Debug.LogWarning("Hata: "+e);
        }
        
    }
         
}
