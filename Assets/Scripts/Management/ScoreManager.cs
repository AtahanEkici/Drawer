using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;
    private ScoreManager() { }

    public const string Score_PlayerPrefs = "Score:";
    public const string ScoreTag = "ScoreBoard";

    [Header("Score")]
    [SerializeField] private int Score = 0;

    [Header("Scene Info")]
    [SerializeField] private string SceneName = string.Empty;

    [Header("Text Reference")]
    [SerializeField] private TextMeshProUGUI tmpro;

    private void Awake()
    {
        CheckInstance();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        try
        {
            GetReferences();
            UpdateSceneName(scene);
            UpdateScoreBoard();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void GetReferences()
    {
        try
        {
            if (tmpro == null)
            {
                tmpro = UI_Controller.instance.ScoreBoard;
            }
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);

            if(tmpro == null)
            {
                tmpro = GameObject.FindGameObjectWithTag(ScoreTag).GetComponent<TextMeshProUGUI>();
            }
        }
    }
    private void UpdateScoreBoard()
    {
        if(tmpro == null)
        {
            GetReferences();
        }

        Score = GetScore();
        tmpro.text = Score_PlayerPrefs + Score.ToString();
    }
    private int GetScore()
    {
        return PlayerPrefs.GetInt((Score_PlayerPrefs + SceneName), 0);
    }
    public void SetScore(int score)
    {
        PlayerPrefs.SetInt((Score_PlayerPrefs + SceneName), (GetScore()+score));
        Score = GetScore();
    }
    private void UpdateSceneName(Scene scene)
    {
        SceneName = scene.name;
    }
    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            try
            {
                Destroy(gameObject);
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerPrefs.Save();
    }
}
