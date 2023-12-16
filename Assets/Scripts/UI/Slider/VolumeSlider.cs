using UnityEngine;
using UnityEngine.UI;
public class VolumeSlider : MonoBehaviour
{
    public static VolumeSlider instance = null;

    [Header("Local References")]
    [SerializeField] private Slider Volume_Slider;
    [SerializeField] private Image FillImage;
    [SerializeField] private Image HandleImage;

    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
    }

    private void Start()
    {
        //SubscribeToListener();
        ChangeFillColor(Color.grey);
    }

    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void GetLocalReferences()
    {
        if (Volume_Slider == null)
        {
            Volume_Slider = GetComponentInChildren<Slider>();
        }

        if (FillImage == null)
        {
            FillImage = transform.GetChild(1).GetComponentInChildren<Image>();
        }

        if (HandleImage == null)
        {
            HandleImage = transform.GetChild(2).GetComponentInChildren<Image>();
        }
    }
    public void ChangeFillColor(Color WantedColor)
    {
        FillImage.color = WantedColor;
    }
    /*
     * Not Needed For Now //
     * 
    private void SubscribeToListener()
    {
        Volume_Slider.onValueChanged.AddListener(delegate { Handle_Color_Change(); });
    }

    private void Handle_Color_Change()
    {
        float sliderValue = Volume_Slider.value;

        Color fillColor = FillImage.color;

        float fillR = fillColor.r;
        float fillG = fillColor.g;
        float fillB = fillColor.b;

        Color handleColor = new(fillR * sliderValue, fillG * sliderValue, fillB * sliderValue);

        HandleImage.color = handleColor;
    }
    */
}
