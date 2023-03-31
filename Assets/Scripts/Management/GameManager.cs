using UnityEngine;
[DefaultExecutionOrder(-1000)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameManager() { }
    private void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        StartUpOperations();
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
    private void StartUpOperations()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
