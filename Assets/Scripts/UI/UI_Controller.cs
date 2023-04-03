using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-200)]
public class UI_Controller : MonoBehaviour
{
    private const string ToggleLabelString = "Line Physics: ";
    public const string LinephysicsType = "LinePhysicsType";

    public static UI_Controller instance = null;

    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject OverlayPanel;

    [Header("Settings")]
    [SerializeField] private Toggle PhysicsToggle;
    [SerializeField] private TextMeshProUGUI PhysicsToggleText;
    [SerializeField] private Button MenuCloseButton;

    [Header("Overlay")]
    [SerializeField] private Button MenuOpenButton;
    [SerializeField] public Slider OnTargetSlider;

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
        Startup();
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
        if(SettingsPanel == null)
        {
            SettingsPanel = transform.GetChild(0).gameObject;
        }
        if (OverlayPanel == null)
        {
            OverlayPanel = transform.GetChild(1).gameObject;
        }
    }
    private void GetForeignReferences()
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

        if(PhysicsToggleText == null)
        {
            PhysicsToggleText = PhysicsToggle.GetComponentInChildren<TextMeshProUGUI>();

            PhysicsToggleTextChange(PhysicsType);
        }
        if(MenuCloseButton == null)
        {
            MenuCloseButton = SettingsPanel.transform.GetChild(1).GetComponent<Button>();
        }

        // Overlay //
        if(MenuOpenButton == null)
        {
            MenuOpenButton = OverlayPanel.transform.GetChild(0).GetComponent<Button>();
        }
        if(OnTargetSlider == null)
        {
            OnTargetSlider = OverlayPanel.transform.GetChild(1).GetComponent<Slider>();
        }    
    }
    private void Startup()
    {
        try
        {
            if(OverlayPanel == null || SettingsPanel == null)
            {
                GetLocalReferences();
            }

            OverlayPanel.SetActive(true);
            SettingsPanel.SetActive(false);
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void DelegateButtons()
    {
        MenuOpenButton.onClick.AddListener(OpenSettings);
        MenuCloseButton.onClick.AddListener(CloseSettings);
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
        LastState = GameManager.IsGamePaused();
        Debug.Log("Menu Opened at State: "+LastState+"");
        if (!GameManager.IsGamePaused()) { GameManager.PauseGame(); }

        OverlayPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    private void CloseSettings()
    {
        SaveSettings();
        OverlayPanel.SetActive(true);
        SettingsPanel.SetActive(false);
        Debug.Log("Menu closed at State: "+LastState);
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
