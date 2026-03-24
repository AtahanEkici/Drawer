using UnityEngine;
public class Rotater : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("1: "+collision.gameObject);

        if(collision.transform.CompareTag(BallController.BallTag) || collision.transform.CompareTag(Drawing.DrawingTag))
        {
            GameObject go = collision.gameObject;
            ParticleManager.SpawnDestroyedParticle(go, collision.contacts[0].point);
            Destroy(go);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("2: " + collision.gameObject);

        if (collision.transform.CompareTag(BallController.BallTag) || collision.transform.CompareTag(Drawing.DrawingTag))
        {
            GameObject go = collision.gameObject;
            ParticleManager.SpawnDestroyedParticle(go, collision.contacts[0].point);
            Destroy(go);
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Debug.Log("1: " + collision.gameObject);

        if (collision.transform.CompareTag(BallController.BallTag) || collision.transform.CompareTag(Drawing.DrawingTag))
        {
            GameObject go = collision.gameObject;
            ParticleManager.SpawnDestroyedParticle(go, gameObject.transform.position);
            Destroy(go);
        }
    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Debug.Log("2: " + other.gameObject);

        if (other.transform.CompareTag(BallController.BallTag) || other.transform.CompareTag(Drawing.DrawingTag))
        {
            GameObject go = other.gameObject;
            ParticleManager.SpawnDestroyedParticle(go, gameObject.transform.position);
            Destroy(go);
        }
    }
}
