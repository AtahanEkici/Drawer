using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessing_Manager : MonoBehaviour
{
    public static PostProcessing_Manager instance = null;

    [Header("Local References")]
    [SerializeField] private PostProcessVolume Post_Process_Volume;

    [Header("External References")]
    [SerializeField] private PostProcessLayer Post_Process_Layer;

    [SerializeField] private FastApproximateAntialiasing fxaa;

    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }
    private void Start()
    {
        GetExternalReferences();
    }
    private void GetLocalReferences()
    {
        Post_Process_Volume = GetComponent<PostProcessVolume>();
    }
    private void GetExternalReferences()
    {
        Post_Process_Layer = Camera.main.GetComponent<PostProcessLayer>();
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
    public void EnablePostProcess()
    {
        try
        {
            Post_Process_Volume.enabled = true;
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }  
    }
    public void DisablePostProcess()
    {
        try
        {
            Post_Process_Volume.enabled = false;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    public void EnableFXAA()
    {
        Post_Process_Layer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
        Post_Process_Layer.fastApproximateAntialiasing.fastMode = true;
        Post_Process_Layer.fastApproximateAntialiasing.keepAlpha = true;
    }
    public void DisableFXAA()
    {
        Post_Process_Layer.antialiasingMode = PostProcessLayer.Antialiasing.None;
    }
}
