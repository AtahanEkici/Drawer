using UnityEngine;
public class BallController : MonoBehaviour
{
    public const string BallTag = "Ball";

    [Header("Renderer")]
    [SerializeField] private Renderer BallRenderer;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void GetLocalReferences()
    {
        try
        {
            BallRenderer = GetComponent<Renderer>();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
