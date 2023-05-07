using System.Collections;
using UnityEngine;
public class BallController : MonoBehaviour
{
    public static BallController instance = null;
    private BallController() { }

    [Header("Coroutine Names")]
    [SerializeField] public const string CoolDown_Coroutine = "CoolDown";
    [SerializeField] public const string FireUp_Coroutine = "FireUp";

    [Header("Color Operations")]
    [SerializeField] private Color InitialColor = Color.black;

    [Header("Material")]
    [SerializeField] private Material BallMaterial;

    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            try
            {
                Destroy(gameObject);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
                Destroy(this);
            }
            
        }
    }
    private void GetLocalReferences()
    {
        try
        {
            BallMaterial = GetComponent<Renderer>().material;
            InitialColor = BallMaterial.color;
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    public void FireUp(Color targetColor,float lerpSpeed)
    {
        BallMaterial.color = Color.Lerp(BallMaterial.color, targetColor, Time.smoothDeltaTime * lerpSpeed);
    }
    public void CoolDown(float lerpSpeed)
    {
        BallMaterial.color = Color.Lerp(BallMaterial.color, InitialColor, Time.smoothDeltaTime * lerpSpeed);
    }
    public void DestroyBall()
    {
        Destroy(gameObject);
    }
}
