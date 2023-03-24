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
}
