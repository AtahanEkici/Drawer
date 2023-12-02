using UnityEngine;
public class DeathWall : MonoBehaviour
{
    [Header("Death Speaker")]
    [SerializeField] private AudioSource Audio_Source;

    private void Awake()
    {
        StartUp();
    }
    private void StartUp()
    {
        Audio_Source = gameObject.AddComponent<AudioSource>();
    }
    private void HandleDestruction(GameObject go, Vector3 Touch_Point)
    {
        Audio_Source.PlayOneShot(SoundManager.Destruction_Sound);
        Destroy(go);
        ParticleManager.SpawnDestroyedParticle(go, Touch_Point);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleDestruction(collision.gameObject, collision.contacts[0].point);
    }
}
