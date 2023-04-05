using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-80)]
public class TimeController : MonoBehaviour
{
    [Header("Timer Text")]
    [SerializeField] private static TextMeshProUGUI TimerUI;
    [SerializeField] private const string StandaloneText = "Time: ";

    [Header("Time Options")]
    [SerializeField] private static bool isTimerStopped = false;
    [SerializeField] private static float Timer = 0f;
    private void Awake()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    private void OnSceneUnloaded(Scene scene)
    {
        ResetTimer();
    }
    private void Start()
    {
        GetForeignReferences();
        UpdateTimerUI();
    }
    private void Update()
    {
        CountTime();
    }
    private void GetForeignReferences()
    {
        if(TimerUI == null)
        {
            TimerUI = GetComponent<TextMeshProUGUI>();
        }
    }
    private void CountTime()
    {
        if (GameManager.IsGamePaused()) { return; }
        else if (isTimerStopped) { return; }
        else
        {  
            Timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }
    public static void StopTimer()
    {
        isTimerStopped = true;
        Debug.Log("Timer Stopped");
    }
    public static void StartTimer()
    {
        isTimerStopped = false;
        Debug.Log("Timer Started");
    }
    public static void ResetTimer()
    {
        Timer = 0f;
        UpdateTimerUI();
        Debug.Log("Timer Reset");
    }
    private static void UpdateTimerUI()
    {
        TimerUI.text = StandaloneText + Timer.ToString("00.00");
    }
    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}
