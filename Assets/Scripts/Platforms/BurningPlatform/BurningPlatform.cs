using UnityEngine;
public class BurningPlatform : MonoBehaviour
{
    private string BallTag = "Ball";

    [Header("Burn Speed")]
    [SerializeField] private float BurnSpeed = 8f;

    private void Start()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        BallTag = BallController.BallTag;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject EnteredObject = collision.gameObject;

        if (EnteredObject.CompareTag(BallTag))
        {
            if (EnteredObject.TryGetComponent<BallController>(out var bc))
            {
                bc.BurnBall(BurnSpeed);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject ExitedObject = collision.gameObject;

        if(ExitedObject.CompareTag(BallTag))
        {
            if (ExitedObject.TryGetComponent<BallController>(out var bc))
            {
                bc.CoolBall(BurnSpeed);
            }
        }  
    }
}
