using UnityEngine;
public class Rotater : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag(BallController.BallTag) || collision.transform.CompareTag(Drawing.DrawingTag))
        {
            GameObject go = collision.gameObject;
            ParticleManager.SpawnDestroyedParticle(go, collision.contacts[0].point);
            Destroy(go);
        }
    }
}
