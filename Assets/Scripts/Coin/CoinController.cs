using UnityEngine;
public class CoinController : MonoBehaviour
{
    [Header("Coin Picked Up?")]
    [SerializeField] private bool isPickedUp = false;

    [Header("Local Components")]
    [SerializeField] private CircleCollider2D col;
    [SerializeField] private Rigidbody2D rb;

    [Header("Animation Options")]
    [SerializeField] private float RotationSpeed = 500f;
    [SerializeField] private float LevitationSpeed = 1f;
    [SerializeField] private float ShrinkSpeed = 0.015f;
    [SerializeField] private float ShrinkThreshold = 0.0001f;
    [SerializeField] private Vector3 ShrinkVector = new(0.1f, 0.1f, 0.1f);

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Ball"))
        {
            OnCoinTaken();
        }
    }
    private void Awake()
    {
        GetLocalComponenets();
    }
    private void Update()
    {
        RotateCoin();
        LevitateCoin();
        ShrinkCoin();
    }
    private void OnCoinTaken() // Disable collider and rigidbody then Play an animation //
    {
        col.enabled = false;
        rb.simulated = false;
        isPickedUp = true;
    }
    
    private void RotateCoin()
    {
        if (!isPickedUp) { return; }
        Debug.Log("geçti");
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
        transform.localScale -= ShrinkVector * ShrinkSpeed;

        if(transform.localScale.x < ShrinkThreshold) // after a certain threshold has been met destroy the coin //
        {
            Destroy(gameObject);
        }
    }
    private void GetLocalComponenets()
    {
        try
        {
            col = GetComponent<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
