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
    private void Start()
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
        try
        {
            if (slider == null)
            {
                slider = UI_Controller.instance.OnTargetSlider;
            }
            if (Slider_Material == null)
            {
                Slider_Material = slider.transform.GetChild(0).GetComponent<Image>().material;
                slider.gameObject.SetActive(false);
            }
        }
        catch(System.Exception e)
        {
            Debug.LogWarning("Hata: "+e);
        }
    }
    private void SliderOperations()
    {
        if(slider == null) { return; }
        if (isBallOnBasket && slider.value >= slider.maxValue) { return; }
        else if(!isBallOnBasket && slider.value <= slider.minValue) { return; }

        if(isBallOnBasket)
        {
            slider.gameObject.SetActive(true);

            slider.value += (Time.smoothDeltaTime * SliderSpeed);

            if(slider.value >= slider.maxValue)
            {
                Debug.Log("Max Value Reached");
                Debug.LogWarning("Level Should End in Success After this prompt!!");
                TimeController.StopTimer();
                GameManager.PauseGame();
                UI_Controller.instance.OpenNextLevelScreen();
                // Call New Level transaction //
            }
        }
        else
        {
            slider.value -= (Time.smoothDeltaTime * SliderSpeed);

            if (slider.value <= slider.minValue)
            {
                slider.gameObject.SetActive(false);
                //Debug.Log("Min Value reached");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(BallTag))
        {
            isBallOnBasket = true;
        }
        else if(collision.CompareTag(Drawing.DrawingTag))
        {
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(BallTag))
        {
            isBallOnBasket = false;
        }
    }
}
