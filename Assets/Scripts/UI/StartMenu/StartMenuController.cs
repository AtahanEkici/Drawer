using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-2000)]
public class StartMenuController : MonoBehaviour
{
    public static StartMenuController instance = null;

    [Header("Panels")]
    [SerializeField] private GameObject StartingScreen;
    [SerializeField] private GameObject LevelsScreen;

    [Header("Local References")]
    [SerializeField] private Button[] AllButtons;

    private void Awake()
    {
        CheckInstance();
        GetReferences();
    }
    private void Start()
    {
        Startup();
    }
    private void Startup()
    {
        AllButtons = GetComponentsInChildren<Button>();
        SoundManager.AddButtonAudioToAllButtons(AllButtons, gameObject);
        LevelsScreen.SetActive(false);
    }
    private void GetReferences()
    {
        StartingScreen = transform.GetChild(0).gameObject;
        LevelsScreen = transform.GetChild(1).gameObject;
    }
    public void OpenLevelsScreen()
    {
        StartingScreen.SetActive(false);
        LevelsScreen.SetActive(true);
    }
    public void CloseLevelsScreen()
    {
        LevelsScreen.SetActive(false);
        StartingScreen.SetActive(true);
    }
    public void OpenSettings()
    {
        StartingScreen.SetActive(false);
        LevelsScreen.SetActive(false);
        UI_Controller.instance.OpenSettings();
    }
    public void CloseSettings()
    {
        LevelsScreen.SetActive(false);
        StartingScreen.SetActive(true);
    }
    public void LoadTest()
    {
        SceneManager.LoadScene("Test", LoadSceneMode.Single);
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            try
            {
                Destroy(gameObject);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
                Destroy(this);
            }
        }
    }
}
