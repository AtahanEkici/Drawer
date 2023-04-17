using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-1200)]
public class UI_Controller : MonoBehaviour
{
    public const string ToggleLabelString = "Line Physics: ";
    public const string LinephysicsType = "LinePhysicsType";

    public static UI_Controller instance = null;
    private UI_Controller() { }

    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject OverlayPanel;
    [SerializeField] private GameObject ListPanel;
    [SerializeField] private GameObject ErrorPanel;

    [Header("Settings")]
    [SerializeField] private Toggle PhysicsToggle;
    [SerializeField] private TextMeshProUGUI PhysicsToggleText;
    [SerializeField] private Button MenuCloseButton;

    [Header("Overlay")]
    [SerializeField] public TextMeshProUGUI ScoreBoard;
    [SerializeField] private Button MenuOpenButton;
    [SerializeField] public Slider OnTargetSlider;

    [Header("List View")]
    [SerializeField] private Button ListViewButton;

    [Header("Privacy Policy")]
    [SerializeField] private GameObject PrivacyPolicyUI;

    [Header("Last State")]
    [SerializeField] private bool LastState;

    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        GetForeignReferences();
        DelegateToggles();
        DelegateButtons();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        //GetForeignReferences();
        Startup(scene);
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetLocalReferences()
    {
        try
        {
            if (SettingsPanel == null)
            {
                SettingsPanel = transform.GetChild(0).gameObject;
            }
            if (OverlayPanel == null)
            {
                OverlayPanel = transform.GetChild(1).gameObject;
            }
            if (ListPanel == null)
            {
                ListPanel = transform.GetChild(2).gameObject;
            }
            if(ErrorPanel == null)
            {
                ErrorPanel = transform.GetChild(3).gameObject;
            }
            if(PrivacyPolicyUI == null)
            {
                PrivacyPolicyUI = transform.GetChild(4).gameObject;
            }
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }  
    }
    private void GetForeignReferences()
    {
        try
        {
            // variables //
            bool PhysicsType = true;

            // Settings //
            if (PhysicsToggle == null)
            {
                PhysicsToggle = SettingsPanel.transform.GetChild(0).GetComponent<Toggle>();

                if (PlayerPrefs.GetInt(LinephysicsType, 1) == 1)
                {
                    PhysicsType = true;
                }
                else
                {
                    PhysicsType = false;
                }
                PhysicsToggle.isOn = PhysicsType;
            }

            if (PhysicsToggleText == null)
            {
                PhysicsToggleText = PhysicsToggle.GetComponentInChildren<TextMeshProUGUI>();

                PhysicsToggleTextChange(PhysicsType);
            }
            if (MenuCloseButton == null)
            {
                MenuCloseButton = SettingsPanel.transform.GetChild(1).GetComponent<Button>();
            }

            // Overlay //
            if (MenuOpenButton == null)
            {
                MenuOpenButton = OverlayPanel.transform.GetChild(0).GetComponent<Button>();
            }
            if (OnTargetSlider == null)
            {
                OnTargetSlider = OverlayPanel.transform.GetChild(1).GetComponent<Slider>();
            }
            if(ScoreBoard == null)
            {
                ScoreBoard = OverlayPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            }

            //ListView //
            if(ListViewButton == null)
            {
                ListViewButton = OverlayPanel.transform.GetChild(6).GetComponent<Button>();
            }
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void Startup(Scene scene)
    {
        try
        {
            if(OverlayPanel == null || SettingsPanel == null || ListPanel == null || PrivacyPolicyUI == null)
            {
                GetLocalReferences();
            }

            if (scene.name == GameManager.StartMenuScene)
            {
                OverlayPanel.SetActive(false);
                SettingsPanel.SetActive(false);
                ListPanel.SetActive(false);
                PrivacyPolicyUI.SetActive(false);
            }

            else
            {
                OverlayPanel.SetActive(true);
                SettingsPanel.SetActive(false);
                ListPanel.SetActive(false);
                PrivacyPolicyUI.SetActive(false);
            } 
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void DelegateButtons()
    {
        try
        {
            MenuOpenButton.onClick.AddListener(OpenSettings);
            MenuCloseButton.onClick.AddListener(CloseSettings);
            ListViewButton.onClick.AddListener(OpenList);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void DelegateToggles()
    {
        PhysicsToggle.onValueChanged.AddListener(delegate{PhysicsToggleValueChanged(PhysicsToggle);});
    }
    private void PhysicsToggleValueChanged(Toggle toggle_value)
    {
        bool value = toggle_value.isOn;
        PhysicsToggleTextChange(value);

        if(value)
        {
            PlayerPrefs.SetInt(LinephysicsType,1);
        }
        else
        {
            PlayerPrefs.SetInt(LinephysicsType, 0);
        } 
    }
    private void PhysicsToggleTextChange(bool value)
    {
        if (value)
        {
            PhysicsToggleText.text = ToggleLabelString + "Dynamic";
        }
        else
        {
            PhysicsToggleText.text = ToggleLabelString + "Static";
        }
    }
    private void OpenSettings()
    {
        Draw.instance.DisableDrawing();

        LastState = GameManager.IsGamePaused();

        if (!GameManager.IsGamePaused()) { GameManager.PauseGame(); }

        OverlayPanel.SetActive(false);
        ListPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    private void CloseSettings()
    {
        SaveSettings();

        OverlayPanel.SetActive(true);
        SettingsPanel.SetActive(false);
        ListPanel.SetActive(false);

        GameManager.SetGameState(LastState);

        Draw.instance.EnableDrawing();
    }
    public void OpenList()
    {
        LastState = GameManager.IsGamePaused();
        //Debug.Log("Menu Opened at State: "+LastState+"");
        if (!GameManager.IsGamePaused()) { GameManager.PauseGame(); }

        OverlayPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        ListPanel.SetActive(true);
    }
    public void CloseList()
    {
        SettingsPanel.SetActive(false);
        ListPanel.SetActive(false);
        OverlayPanel.SetActive(true);
        //Debug.Log("Menu closed at State: "+LastState);
        GameManager.SetGameState(LastState);
    }
    public void OpenPrivacyPolicyUI()
    {
        LastState = GameManager.IsGamePaused();
        if (!GameManager.IsGamePaused()) { GameManager.PauseGame(); }

        OverlayPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        ListPanel.SetActive(false);
        PrivacyPolicyUI.SetActive(true);
    }
    public void ClosePrivacyPolicyUI()
    {
        SettingsPanel.SetActive(false);
        ListPanel.SetActive(false);
        PrivacyPolicyUI.SetActive(false);
        OverlayPanel.SetActive(true);
        GameManager.SetGameState(LastState);
    }
    private void SaveSettings()
    {
        PlayerPrefs.Save();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
