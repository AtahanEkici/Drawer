using UnityEngine;
public class BallController : MonoBehaviour
{
    public const string BallTag = "Ball";

    [Header("Tags")]
    [SerializeField] private const string PlatformTag = "Platform";
    [SerializeField] private const string DrawingTag = "Drawing";
    [SerializeField] private const string StarTag = "Star";

    [Header("Audio Source")]
    [SerializeField] private AudioSource Audio_Source;

    [Header("Collided Object")]
    [SerializeField] private GameObject CollidedObject;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void GetLocalReferences()
    {
        try
        {
            Audio_Source = GetComponent<AudioSource>();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void OnDestroy() // Play an audioclip after the p
    {
        AudioSource audioSource_External = new GameObject("DestroyedBallSound").AddComponent<AudioSource>();

        // Play the AudioClip as a one-shot
        audioSource_External.PlayOneShot(SoundManager.Explosion_Sound);

        // Destroy the GameObject after the audio clip ends
        float clipLength = SoundManager.Explosion_Sound.length; // Get the length of the audio clip
        Destroy(audioSource_External.gameObject, clipLength); // Destroy the GameObject after the duration of the audio clip
    }
    private void HandleCollisionSounds(GameObject go)
    {
        try
        {
            if(go.CompareTag(DrawingTag))
            {
                Audio_Source.PlayOneShot(SoundManager.Hit_Sound);
            }
            else if (go.CompareTag(StarTag))
            {
                Audio_Source.PlayOneShot(SoundManager.Pickup_Sound);
            }
            else
            {
                Audio_Source.PlayOneShot(SoundManager.Touch_Sound);
            }
            Audio_Source.Play();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollidedObject = collision.gameObject;

        HandleCollisionSounds(CollidedObject);
    }
}
