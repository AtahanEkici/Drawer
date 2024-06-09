using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public const string MaxLevel = "MaxLevel";
    public static LevelManager Instance { get; private set; }

    [Header("Don't Destroy On Load ?")]
    [SerializeField] private bool dontDestroy = true;

    [Header("Max Level Reached")]
    [SerializeField] private static int maxLevelReached = 1;

    private void Awake()
    {
        CheckInstance();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
            if (int.TryParse(scene.name, out int level))
            {
                CheckMaxLevel(level);
            }
            else
            {
                Debug.LogWarning("Scene name is not a valid integer: " + scene.name);
            }
    }

    private void CheckMaxLevel(int currentLevel)
    {
        maxLevelReached = PlayerPrefs.GetInt(MaxLevel, 1);

        if (maxLevelReached > currentLevel)
        {
            maxLevelReached = currentLevel;
            PlayerPrefs.SetInt(MaxLevel, maxLevelReached);
        }
        Debug.Log("Current Max Level: "+currentLevel);
    }

    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;

            if (dontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
    }

    public static void LoadLevel(string level)
    {
        try
        {
            SceneManager.LoadScene(level.Trim(), LoadSceneMode.Single);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load scene: " + level + " Error: " + e.Message);
        }
    }

    public static void LoadLevel(int level)
    {
        try
        {
            SceneManager.LoadScene(level.ToString().Trim(), LoadSceneMode.Single);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to load scene: " + level + " Error: " + e.Message);
        }
    }

    public static void LoadNextLevel()
    {
        Debug.Log(PlayerPrefs.GetInt(MaxLevel, 1));

        if (maxLevelReached == 8)
        {
            Debug.LogWarning("Max level reached. Cannot load the next level.");
            return;
        }
        else
        {
            LoadLevel(maxLevelReached + 1);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
