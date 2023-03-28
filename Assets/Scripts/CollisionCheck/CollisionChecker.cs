using UnityEngine;
public class CollisionChecker : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float counter = 0f;
    [SerializeField] private float Reach = 0f; 

    [Header("Collider2D")]
    [SerializeField] public Collider2D col;
    private void Awake()
    {
        Reach = Time.fixedDeltaTime;
    }
    private void Update()
    {
        if(counter >= Reach)
        {
            col.isTrigger = false;
            Destroy(this);
        }
        else
        {
            counter += Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("OnTriggerEnterCalled Called: "+collision.gameObject.name+"");
        Destroy(transform.parent.gameObject);
    }
}
