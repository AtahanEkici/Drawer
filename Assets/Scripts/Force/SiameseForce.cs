using UnityEngine;
public class SiameseForce : MonoBehaviour
{
    [Header("Attached Rigidbody")]
    [SerializeField] public Rigidbody2D rb2d;

    private void OnTrigerEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);

        Vector2 collisionForce = collision.relativeVelocity;

        collision.rigidbody.AddForce(-collisionForce * 6f, ForceMode2D.Impulse);

        ApplyForceToOtherObject(collisionForce);
    }
    private void ApplyForceToOtherObject(Vector2 force)
    {
        if (rb2d != null)
        {
            // Apply the same force to the other object
            rb2d.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
