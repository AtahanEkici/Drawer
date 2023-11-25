using UnityEngine;
public class BurningPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        if (!collision.gameObject.TryGetComponent(out Burn burn_reference))
        {
            go.AddComponent<Burn>();
        }
    }

    
}
