using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DrawingSpriteController : MonoBehaviour
{
    [Header("Memory Strings")]
    [SerializeField] public const string ToggleLabelString = "Line Physics: ";
    [SerializeField] public const string LinephysicsType = "LinePhysicsType";

    [Header("Resource File Roads")]
    [SerializeField] private readonly string White_Drawing = "UI/Drawing/White_Drawing";
    [SerializeField] private readonly string Black_Drawing = "UI/Drawing/Black_Drawing";

    [Header("Toggle")]
    [SerializeField] private bool ToggleState = true;

    [Header("Drawing Sprites")]
    [SerializeField] private Sprite WhiteSprite;
    [SerializeField] private Sprite BlackSprite;

    [Header("Local References")]
    [SerializeField] private Image imageComponent;
    [SerializeField] private Button DrawingButton;

    public static DrawingSpriteController instance = null;
    private DrawingSpriteController() { }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        if (RestrictionSystem.instance.OnlyStaticDrawingsAllowed)
        {
            //Debug.Log("Static Allowed");
            PlayerPrefs.SetInt(LinephysicsType, 0);
        }
        else if (RestrictionSystem.instance.OnlyDynamicDrawingsAllowed)
        {
            //Debug.Log("Dynamic Allowed");
            PlayerPrefs.SetInt(LinephysicsType, 1);
        }
    }
    private void Awake()
    {
        CheckInstance();
        GetReferences();
        GetResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void GetReferences()
    {
        if(imageComponent == null)
        {
            imageComponent = GetComponent<UnityEngine.UI.Image>();
        }
        if(DrawingButton == null)
        {
            DrawingButton = GetComponent<Button>();
            DrawingButton.onClick.AddListener(OnButtonPressed);
        }
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
    private void GetResources()
    {
        try
        {
            WhiteSprite = Resources.Load<Sprite>(White_Drawing);
            BlackSprite = Resources.Load<Sprite>(Black_Drawing);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void OnButtonPressed()
    {
        //Debug.Log("DrawingButton clicked");

        if (RestrictionSystem.instance.OnlyStaticDrawingsAllowed)
        {
            ErrorSystem.instance.SetErrorMessage(ErrorSystem.OnlyStaticDrawingsAllowed);
            return;
        }
        else if (RestrictionSystem.instance.OnlyDynamicDrawingsAllowed)
        {
            ErrorSystem.instance.SetErrorMessage(ErrorSystem.OnlyDynamicDrawingsAllowed);
            return;
        }
        else
        {
            try
            {
                ChangeDrawingState();
                ToggleState = !ToggleState;
                ChangeSprite();
            }
            catch(System.Exception e)
            {
                ChangeDrawingState();
            }
        }
    }
    private void ChangeDrawingState()
    {
        int type = PlayerPrefs.GetInt(LinephysicsType, 1);

        if (type == 0)
        {
            PlayerPrefs.SetInt(LinephysicsType, 1);
        }
        else
        {
            PlayerPrefs.SetInt(LinephysicsType, 0);
        }
    }
    private void ChangeSprite()
    {
        if(imageComponent.sprite == WhiteSprite)
        {
            imageComponent.sprite = BlackSprite;
        }
        else
        {
            imageComponent.sprite = WhiteSprite;
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
