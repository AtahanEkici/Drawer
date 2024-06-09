using UnityEngine;
public class PistonController : MonoBehaviour
{
    [Header("Piston Parts")]
    [SerializeField] private GameObject pistonUpper;

     [Header("Max Piston Travel")]
     [SerializeField] private float PistonSpeed = 5f;
     private bool isPushing = false;
     private float MaxPistonDistance = 1.0f;
     private Vector3 PushedPistonPosition;

     [Header("Local Components")]
     private Vector3 initialPistonPosition;
     private BoxCollider2D pistonStartTrigger;
    private void Start()
    {
        GetReferences();
    }
    private void Update()
    {
        if (isPushing)
        {
            PistonPush();
        }
        else
        {
            PistonPull();
        }
    }
    private void GetReferences()
    {
        pistonStartTrigger = GetComponent<BoxCollider2D>();
        pistonUpper = transform.GetChild(0).gameObject;
        initialPistonPosition = pistonUpper.GetComponent<Transform>().localPosition;
        PushedPistonPosition = new Vector3(initialPistonPosition.x, initialPistonPosition.y + MaxPistonDistance, initialPistonPosition.z);
    }
    private void PistonPush()
    {
        if (Vector3.Distance(PushedPistonPosition, pistonUpper.transform.localPosition) <= 0.001f)
        {
            isPushing = false;
            //Debug.Log("Push Complete");
            return;
        }
        else
        {
            //Debug.Log("Pushing");
            pistonUpper.transform.localPosition = Vector3.MoveTowards(pistonUpper.transform.localPosition, PushedPistonPosition, PistonSpeed * Time.deltaTime);
        }
    }
    private void PistonPull()
    {
        if (Vector3.Distance(initialPistonPosition, pistonUpper.transform.localPosition) <= 0.001f)
        {
            //Debug.Log("Pull Complete");
            return;
        }
        else
        {
            //Debug.Log("Pulling");
            pistonUpper.transform.localPosition = Vector3.MoveTowards(pistonUpper.transform.localPosition, initialPistonPosition, PistonSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPushing = true;
    }
}
