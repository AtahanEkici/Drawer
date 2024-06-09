using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ErrorSystem : MonoBehaviour
{
    // Fixed Error Messages - Start //
    public const string DynamicLineWhileGamePaused = "Can not Place Dynamic Line while Game is Paused!";

    public const string ObjectCollidedThenDestroyed = "Can not draw colliding dynamic objects!";

    public const string ReachedMaxDrawingCount = "You have reached max drawing count!";

    public const string DrawingTooSmall = "The Drawing is too small!";
    public const string DrawingTooLarge = "The Drawing is too LARGE!";

    public const string NoDrawingsFound = "No Drawings Found The List is Empty";

    public const string OnlyStaticDrawingsAllowed = "Only Static Drawings are allowed on this level";
    public const string OnlyDynamicDrawingsAllowed = "Only Dynamic Drawings are allowed on this level";

    public const string TimeLimitExceeded = " Time Limit Exceeded";
    // Fixed Error Messages - END //

    public static ErrorSystem instance = null;
    private ErrorSystem() { }

    [Header("Rect Transform")]
    [SerializeField] private RectTransform rectrans;

    [Header("Error Received")]
    [SerializeField] private bool ErrorReceived = false;

    [Header("Retraction Controlls")]
    private bool WaitForMessage = false;
    [SerializeField] private float RetractionPoint = 200f;

    [Header("Animation Controls")]
    private bool isUpward = true;
    [SerializeField] private float Speed = 400f;
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
    public void SetErrorMessage(string Message, string Baþlýk)
    {
        ErrorMessage.text = Message;
        ErrorBaþlýk.text = Baþlýk;
        ErrorReceived = true;
    }
    private void AnimateTable()
    {
        if (!ErrorReceived) { return; }
        if(SceneManager.GetActiveScene().name == GameManager.StartMenuScene) { return; } // Do not call Error System on Start Menu //

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
