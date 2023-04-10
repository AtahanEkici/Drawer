using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ErrorSystem : MonoBehaviour
{
    public static ErrorSystem instance = null;
    private ErrorSystem() { }

    [Header("Rect Transform")]
    [SerializeField] private RectTransform rectrans;

    [Header("Error Received")]
    [SerializeField] private bool ErrorReceived = false;

    [Header("Retraction Controlls")]
    [SerializeField] private bool WaitForMessage = false;
    [SerializeField] private float RetractionPoint = 200f;

    [Header("Animation Controls")]
    [SerializeField] private bool isUpward = true;
    [SerializeField] private float Speed = 250f;
    [SerializeField] private float TimeCounter = 1f;

    [Header("Slider Controlls")]
    [SerializeField] private Slider TimerSlider;

    private float Timer;

    [Header("Error Text")]
    [SerializeField] private TextMeshProUGUI ErrorMessage;
    [SerializeField] private TextMeshProUGUI ErrorBaþlýk;

    private void Awake()
    {
        CheckInstance();
        GetReferences();
    }
    private void Update()
    {
        CountTime();
        AnimateTable();
    }
    private void CountTime()
    {
        if(!ErrorReceived) { return; }
        if(!WaitForMessage) { return; }
        else
        {
            TimeCounter -= Time.unscaledDeltaTime;
            TimerSlider.value = TimeCounter;

            if (TimeCounter <= 0f)
            {
                TimeCounter = Timer;
                WaitForMessage = false;
            }
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
            Destroy(gameObject);
        }
    }
    private void GetReferences()
    {
        Timer = TimeCounter;

        if (ErrorBaþlýk == null)
        {
            ErrorBaþlýk = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        if (ErrorMessage == null)
        {
            ErrorMessage = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        if(TimerSlider == null)
        {
            TimerSlider = ErrorBaþlýk.transform.GetChild(0).GetComponent<Slider>();
            TimerSlider.maxValue = TimeCounter;
            TimerSlider.minValue = 0f;
            TimerSlider.value = TimerSlider.maxValue;
        }

        if(rectrans == null)
        {
            rectrans = GetComponent<RectTransform>();
        }
    }
    public void SetErrorMessage(string Message)
    {
        ErrorMessage.text = Message;
        ErrorReceived = true;
    }
    private void AnimateTable()
    {
        if (!ErrorReceived) { return; }

        Vector2 anchors = rectrans.anchoredPosition;
       
        if (isUpward)
        {
            float height = anchors.y + (Time.unscaledDeltaTime * Speed);

            if (height >= RetractionPoint)
            {
                isUpward = false;
                WaitForMessage = true;
            }
            else
            {
                rectrans.anchoredPosition = new(anchors.x, height);
            }
        }
        else
        {
            if (WaitForMessage) { return; }

            float height = anchors.y + (Time.unscaledDeltaTime * Speed * -1);

            if (height <= -RetractionPoint)
            {
                isUpward = true;
                ErrorReceived = false;
                TimeCounter = Timer;
            }
            else
            {
                rectrans.anchoredPosition = new(anchors.x, height);
            }
        }
    }
}
