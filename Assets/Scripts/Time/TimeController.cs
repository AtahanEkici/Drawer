using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
[DefaultExecutionOrder(-80)]
public class TimeController : MonoBehaviour
{
    public static TimeController instance = null;
    private TimeController() { }

    [Header("Timer Text")]
    [SerializeField] private static TextMeshProUGUI TimerUI;
    [SerializeField] private const string StandaloneText = "Time: ";

    [Header("Time Options")]
    [SerializeField] private static bool isTimerStopped = false;
    [SerializeField] public static float Timer = 0f;
    private void Awake()
    {
        CheckInstance();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode lsm)
    {
        if(scene.name == GameManager.StartMenuScene)
        {
            return;
        }
        else
        {
            GetForeignReferences();
            ResetTimer();
        } 
    }
    private void Update()
    {
        CountTime();
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
        //Debug.Log("Timer Stopped");
    }
    public static void StartTimer()
    {
        isTimerStopped = false;
        //Debug.Log("Timer Started");
    }
    public static void ResetTimer()
    {
        Timer = 0f;
        UpdateTimerUI();
        isTimerStopped = false;
        //Debug.Log("Timer Reset");
    }
    public float GetCurrentTimer()
    {
        return Timer;
    }
    private static void UpdateTimerUI()
    {
        TimerUI.text = StandaloneText + Timer.ToString("00.00");
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
