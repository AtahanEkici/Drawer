using UnityEngine;
public class RandomForce : MonoBehaviour
{
    [Header("RigidBody Object")]
    [SerializeField] private Rigidbody2D rb2d;

    [Header("Min - Max Random Velocity")]
    [SerializeField] private float MinVelocity = 0.15f;
    [SerializeField] private float MaxVelocity = 0.50f;

    [Header("Ball's Direction Left or Right ? ")]
    [SerializeField] private bool ?isLeft = null;

    private void Awake()
    {
        GetComponents();
    }

    private void Start()
    {
        AddRandomForce(MinVelocity, MaxVelocity,isLeft);
    }

    private void GetComponents()
    {
        if(rb2d == null) // Get RigidBody Component of the "Ball" object //
        {
            rb2d = GetComponentInParent<Rigidbody2D>();
        }
    }
    private void AddRandomForce(float minForce, float maxForce, bool? Direction)
    {
        float force = Random.Range(minForce, maxForce);
        bool applyLeftForce = (Direction == null) ? Random.Range(0, 100) >= 50 : Direction == true;

        Vector2 forceDirection = applyLeftForce ? -transform.right : transform.right;
        rb2d.AddForce(force * rb2d.mass * forceDirection);
    }

}
