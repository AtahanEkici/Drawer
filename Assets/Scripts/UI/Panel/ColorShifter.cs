using UnityEngine;
using UnityEngine.UI;
public class ColorShifter : MonoBehaviour
{
    [Header("Local References")]
    [SerializeField] private Image image;

    [Header("Color Target")]
    [SerializeField] private float ColorAlpha = 0.5f;
    [SerializeField] private Color TargetColor;

    [Header("Timer Control")]
    [SerializeField] private float Timer = 2f;
    [SerializeField] private float TempTimer;

    private void Awake()
    {
        GetLocalReferences();
        TempTimer = Timer;
    }
    private void Update()
    {
        TimerEvent();
        LerpColor();
    }
    private void GetLocalReferences()
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
    }
    private void TimerEvent()
    {
        if(Timer <= 0f)
        {
            TargetColor = Random.ColorHSV();
            Timer = TempTimer;
            TargetColor.a = ColorAlpha;
        }
        else
        {
            Timer -= Time.unscaledDeltaTime;
        }
    }
    private void LerpColor()
    {
        image.color = Color.Lerp(image.color,TargetColor,Time.unscaledDeltaTime);
    }
}
