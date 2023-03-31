using UnityEngine;

public class Bottom_Basket : MonoBehaviour
{
    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void GetLocalReferences()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        Debug.Log(go.name);
    }
}
