using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
[DefaultExecutionOrder(-5000)]
public class GameManager : MonoBehaviour
{
    public const string UITag = "UI";
    public const string StartMenuScene = "StartMenu";
    public const string EventSystemResource = "Management/Event_System/EventSystem";
    public static GameManager Instance { get; private set; }
    private GameManager() { }

    [Header("Event System")]
    [SerializeField] private GameObject Event_System_GameObject;

    [Header("Main UI")]
    [SerializeField] private static GameObject mainUI;

    [Header("ScreenShot Options")]
    [SerializeField] private string screenshotDirectory = "";

    private void Awake()
    {
        CheckInstance();
        GetResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        PauseGame(); // Pause the game on scene load // 
        GetReferences();
        InstantiateEventSystemIfNoneFound();
    }
    private void Start()
    {
        StartUpOperations();
    }
    private void Update()
    {
        ScreenShot();
    }
    private void GetReferences()
    {
        if(mainUI == null)
        {
            mainUI = GameObject.FindGameObjectWithTag(UITag);
        }
    }
    private void ScreenShot()
    {
        #if UNITY_EDITOR
            screenshotDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Screenshots";
#elif ANDROID
            screenshotDirectory = Application.persistentDataPath + "/Screenshots";
#endif

        if (!Directory.Exists(screenshotDirectory))
        {
            Directory.CreateDirectory(screenshotDirectory);
        }

        if (Input.GetKeyDown(KeyCode.Print))
        {
            string screenshotPath = screenshotDirectory + "/screenshot_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            ScreenCapture.CaptureScreenshot(screenshotPath,4);
            Debug.Log("ScreenShot saved on "+ screenshotPath + "");
        }
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetResources()
    {
        Event_System_GameObject = Resources.Load<GameObject>(EventSystemResource) as GameObject;
    }
    private void InstantiateEventSystemIfNoneFound()
    {
        EventSystem EventSystem = FindAnyObjectByType<EventSystem>();

        if(EventSystem == null)
        {
            Instantiate(Event_System_GameObject);
            //Debug.Log("Event System Minted");
        }
    }
    private void StartUpOperations()
    {
        try
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
    public static void SetGameState(bool state)
    {
        if(state)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public static bool IsGamePaused()
    {
        if(Time.timeScale <= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name,LoadSceneMode.Single);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
