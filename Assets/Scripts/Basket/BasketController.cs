using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-100)]
public class BasketController : MonoBehaviour
{
    private const string BallTag = "Ball";

    [Header("Local Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Slider Reference")]
    [SerializeField] private Slider slider;
    [SerializeField] private Material Slider_Material;
    [SerializeField] private float SliderSpeed = 1f;

    [Header("Ball Operations")]
    [SerializeField] private bool isBallOnBasket = false;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void OnEnable()
    {
        GetForeignReferences();
    }
    private void Update()
    {
        SliderOperations();
    }
    private void GetLocalReferences()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }
    private void GetForeignReferences()
    {
        if(slider == null)
        {
            slider = UI_Controller.Instance.OnTargetSlider;
        }
        if(Slider_Material == null)
        {
            Slider_Material = slider.transform.GetChild(0).GetComponent<Image>().material;
            slider.gameObject.SetActive(false);
            //SetMaterialTransparency(0f); // Transparent on Startup //
        }
    }
    private float GetMaterialTransparency()
    {
        return Slider_Material.GetFloat("_Transparency");
    }
    private void SetMaterialTransparency(float value)
    {
        Slider_Material.SetFloat("_Transparency",value);
    }
    private void SliderOperations()
    {
        float slider_value = slider.value;
        //float slider_transparency = GetMaterialTransparency();
        //if (!isBallOnBasket) { return; }

        if(isBallOnBasket)
        {
            slider.gameObject.SetActive(true);

            slider.value += (Time.deltaTime * SliderSpeed);

            if(slider.value >= slider.maxValue)
            {
                Debug.Log("Max Value Reached");
            }
        }
        else
        {
            if(slider_value <= slider.minValue)
            {
                slider.gameObject.SetActive(false);
                Debug.Log("Min Value reached");
            }
            else
            { 
                slider.value -= (Time.deltaTime * SliderSpeed);
            } 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject go = collision.gameObject;

        if(collision.CompareTag(BallTag))
        {
            isBallOnBasket = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //GameObject go = collision.gameObject;

        if(collision.CompareTag(BallTag))
        {
            isBallOnBasket = false;
        }
    }
}
