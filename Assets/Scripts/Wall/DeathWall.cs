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
    private void HandleDestruction(GameObject go)
    {
        Debug.Log("Geçti");
        Audio_Source.PlayOneShot(SoundManager.Destruction_Sound);
        Destroy(go);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleDestruction(collision.gameObject);
    }
}
