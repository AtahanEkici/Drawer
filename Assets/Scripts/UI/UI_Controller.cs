using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-200)]
public class UI_Controller : MonoBehaviour
{
    private const string ToggleLabelString = "Line Physics: ";
    public const string LinephysicsType = "LinePhysicsType";
    public static UI_Controller Instance { get; private set; }
    private UI_Controller() { }

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

    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnEnable()
    {
        GetForeignReferences();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        Startup();
    }
    private void CheckInstance()
    {
        if(Instance == null)
        {
            Instance = this;
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

        // Delegations //
        DelegateToggles();
        DelegateButtons();
    }
    private void Startup()
    {
        OverlayPanel.SetActive(true);
        SettingsPanel.SetActive(false);
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
        Debug.Log("Opened Settings");
        GameManager.PauseGame();
        OverlayPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    private void CloseSettings()
    {
        Debug.Log("Closed Settings");
        GameManager.ResumeGame();
        OverlayPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
    private void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}
