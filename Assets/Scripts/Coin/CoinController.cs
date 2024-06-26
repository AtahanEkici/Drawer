using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CoinController : MonoBehaviour
{
    [Header("Coin Picked Up?")]
    [SerializeField] private bool isPickedUp = false;

    [Header("Local Components")]
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource Audio_Source;

    [Header("Animation Options")]
    [SerializeField] private float RotationSpeed = 500f;
    [SerializeField] private float LevitationSpeed = 1f;
    [SerializeField] private float ShrinkSpeed = 2f;
    [SerializeField] private float ShrinkThreshold = 0.0001f;
    [SerializeField] private Vector3 ShrinkVector = new(0.1f, 0.1f, 0.1f);

    [Header("PlayerPref")]
    private string CoinCheckPhrase;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Ball"))
        {
            OnCoinTaken();
        }
    }
    private void Awake()
    {
        CoinCheckPhrase = (gameObject.name + SceneManager.GetActiveScene().name).Trim();
        CheckTakenStatus();
        GetLocalComponents();
    }
    private void Update()
    {
        AnimateCoin();
    }
    private void CheckTakenStatus()
    {
        bool status;

        if(PlayerPrefs.GetInt(CoinCheckPhrase, 0) == 0)
        {
            status = false;
        }
        else
        {
            status = true;
        }

        gameObject.SetActive(!status);
    }
    private void OnCoinTaken() // Disable collider and rigidbody then Play an animation //
    {
        PlayerPrefs.SetInt(CoinCheckPhrase, 1);
        col.enabled = false;
        rb.simulated = false;
        isPickedUp = true;
        Audio_Source.Play(); 
    } 
    private void AnimateCoin()
    {
        try
        {
            RotateCoin();
            LevitateCoin();
            ShrinkCoin();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void RotateCoin()
    {
        if (!isPickedUp) { return; }
        transform.Rotate(Vector3.up, Time.smoothDeltaTime * RotationSpeed);
    }
    private void LevitateCoin()
    {
        if (!isPickedUp) { return; }
        transform.Translate(LevitationSpeed * Time.smoothDeltaTime * transform.up);
    }
    private void ShrinkCoin()
    {
        if (!isPickedUp) { return; }
        transform.localScale -= ShrinkSpeed * Time.smoothDeltaTime * ShrinkVector;

        if(transform.localScale.x < ShrinkThreshold) // after a certain threshold has been met destroy the coin //
        {
            StartCoroutine(DestroyAfterAudioFinished());
        }
    }
    private void CheckAudioSource()
    {
        if (Audio_Source.clip == null)
        {
            Audio_Source.clip = SoundManager.Pickup_Sound;
        }
    }
    public IEnumerator DestroyAfterAudioFinished()// Wait until the audio clip finishes playing and destroy the object then //
    {
        while (Audio_Source.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);// Once the audio finishes playing destroy the GameObject this script is attached to //
    }
    private void GetLocalComponents()
    {
        try
        {
            col = GetComponent<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            Audio_Source = GetComponent<AudioSource>();
            CheckAudioSource();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        PlayerPrefs.Save();
    }
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
