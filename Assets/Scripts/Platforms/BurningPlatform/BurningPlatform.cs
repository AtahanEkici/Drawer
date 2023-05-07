using System.Collections;
using UnityEngine;
public class BurningPlatform : MonoBehaviour
{
    private string BallTag;

    [Header("Burn Color")]
    [SerializeField] private Color WantedColor = Color.red;

    [Header("Burn Speed")]
    [SerializeField] private float BurnSpeed = 2f;

    private void Start()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        BallTag = BallController.instance.tag;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject EnteredObject = collision.gameObject;

        Debug.Log("Entered: "+ EnteredObject.name);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject ExitedObject = collision.gameObject;

        Debug.Log("Exited: "+ ExitedObject.name);
    }

    public IEnumerator FireUp(GameObject CollidedObject)
    {
        Material mat = CollidedObject.GetComponent<Renderer>().material;

        while(mat.color != WantedColor)
        {
            mat.color = Color.Lerp(mat.color, WantedColor, Time.smoothDeltaTime * BurnSpeed);
            yield return null;
        }
        
        if(mat.color == WantedColor)
        {
            Destroy(CollidedObject);
        }
    }
    /*
    public void CoolDown(float lerpSpeed)
    {
        BallMaterial.color = Color.Lerp(BallMaterial.color, InitialColor, Time.smoothDeltaTime * lerpSpeed);
    }
    */
}
