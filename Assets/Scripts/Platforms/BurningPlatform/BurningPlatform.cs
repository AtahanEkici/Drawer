using UnityEngine;
public class BurningPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.TryGetComponent(out Burn burn_reference))
        {
            collision.gameObject.AddComponent<Burn>();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Burn burn_reference))
        {
            burn_reference.BeginBurning();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        if (go.TryGetComponent(out Burn burn_reference))
        {
            burn_reference.BeginCooling();
        }
    }
}