using UnityEngine;
public class Drawing : MonoBehaviour
{
    [Header("Local Components")]
    [SerializeField] private MeshRenderer render;

    [Header("Debugging")]
    [SerializeField] private bool DebugMode = true;
    [SerializeField] private Vector3 CenterOfObject = Vector3.zero;

    private void Awake()
    {
        GetComponents();
    }
    private void Update()
    {
        Debugging();
    }
    private void GetComponents()
    {
        render = GetComponent<MeshRenderer>();
    }
    private void Debugging()
    {
        if (!DebugMode) { return; }

        CenterOfObject = render.bounds.center;

       //Debug.DrawLine(transform.position,CenterOfObject,Color.magenta,Time.deltaTime);
       Debug.DrawLine(transform.position, transform.localPosition, Color.yellow, Time.deltaTime);
    }
}
