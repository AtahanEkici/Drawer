using UnityEngine;
public class CannonRammer : MonoBehaviour
{
    [Header("Parent Components")]
    [SerializeField] private CannonController controller;

    [Header("Ball")]
    [SerializeField] private Rigidbody2D ballRigidbody;
    private void Start()
    {
        controller = GetComponentInParent<CannonController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            collision.rigidbody.velocity = Vector3.zero;
            collision.transform.parent = transform.parent;

            controller.isBallOnBreach = true;

            ballRigidbody = collision.rigidbody;
        }
    }
    public void LaunchBall(Vector3 Direction, float ForceMultiplier, ForceMode2D forceMode2d)
    {
        ballRigidbody.gameObject.transform.parent = null;
        ballRigidbody.AddForce(Direction * ForceMultiplier, forceMode2d);
        ballRigidbody = null;
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            controller.isBallOnBreach = false;
        }   
    }
}
