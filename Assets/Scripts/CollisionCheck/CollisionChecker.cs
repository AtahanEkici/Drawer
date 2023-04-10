using UnityEngine;
public class CollisionChecker : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float counter = 0f;
    [SerializeField] private float Reach = 0f;

    private static Vector3 FailStress = new(25,90f,0.15f);// frequency,angle,stress amount //

    [Header("Collider2D")]
    [SerializeField] public Collider2D col;
    private void Awake()
    {
        Reach = Time.fixedDeltaTime;
    }
    private void Update()
    {
        CheckTime();
    }
    private void CheckTime()
    {
        if (counter >= Reach)
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
        ErrorSystem.instance.SetErrorMessage(ErrorSystem.ObjectCollidedThenDestroyed);
        Destroy(transform.parent.gameObject);
        CameraShake.Instance.InduceStress(FailStress);
    }
}
