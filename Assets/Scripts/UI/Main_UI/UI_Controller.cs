using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-2200)]
public class UI_Controller : MonoBehaviour
{
    public const string ToggleLabelString = "Line Physics: ";
    public const string LinephysicsType = "LinePhysicsType";
    public const string ShowFPS = "ShowFPS";
    public const string PostProcess = "PostProcess";
    public const string FXAA = "FXAA";
    public const string AudioVolume = "VOLUME: ";

    public static UI_Controller instance = null;
    private UI_Controller() { }

    [Header("All Buttons")]
    [SerializeField] private Button[] AllButtons;

    [Header("Panels")]
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject OverlayPanel;
    [SerializeField] private GameObject ListPanel;
    [SerializeField] private GameObject ErrorPanel;
    [SerializeField] private GameObject Game_Over_Panel;

    [Header("Settings")]
    [SerializeField] public Toggle PhysicsToggle;
    [SerializeField] private TextMeshProUGUI PhysicsToggleText;
    [SerializeField] private Button MenuCloseButton;
    [SerializeField] private Toggle ShowFPSToggle;
    [SerializeField] private Toggle PostProcessToggle;
    [SerializeField] private Toggle FXAAToggle;
    [SerializeField] private Slider VolumeSlider;
    [SerializeField] private TextMeshProUGUI VolumeText;

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

    [SerializeField] private AudioListener Main_Audio_Listener;

    [Header("Game Over Cause")]
    [SerializeField] private TextMeshProUGUI Game_Over_Cause;

    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        AddSoundToButtons();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        Startup(scene);
        GetForeignReferences();
    }
    private void Start()
    {
       
        DelegateToggles();
        DelegateButtons();
    }
    private void AddSoundToButtons()
    {
        AllButtons = GetComponentsInChildren<Button>();
        SoundManager.AddButtonAudioToAllButtons(AllButtons, gameObject);
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
            if (Game_Over_Panel == null)
            {
                Game_Over_Panel = transform.GetChild(5).gameObject;
            }   
            if(Game_Over_Cause == null)
            {
                Game_Over_Cause = Game_Over_Panel.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            }
        }
        catch(Exception e)
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
            bool ShowFPSPref = false;
            bool ShowPostProcessPref = true;
            bool FXAAPref = true;
            float audio_volume = 1.0f;

            // Settings //
            if (PhysicsToggle == null)
            {
                PhysicsToggle = SettingsPanel.transform.GetChild(0).GetComponent<Toggle>();

                RestrictionSystem restrictions = RestrictionSystem.instance;

                if(restrictions.OnlyStaticDrawingsAllowed)
                {
                    //Debug.Log("Static Allowed");
                    PhysicsType = false;
                }
                else
                {
                    //Debug.Log("Dynamic Allowed");
                    PhysicsType = true;
                    PlayerPrefs.SetInt(LinephysicsType, 0);
                }
                PhysicsToggle.isOn = PhysicsType;
                PlayerPrefs.SetInt(LinephysicsType, Convert.ToInt32(PhysicsType));
                //Debug.Log("Physics Type: " + PhysicsType);
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

            if (ShowFPSToggle == null)
            {
                ShowFPSToggle = SettingsPanel.transform.GetChild(2).GetComponent<Toggle>();

                ShowFPSPref = PlayerPrefs.GetInt(ShowFPS, 0) == 1;

                ShowFPSToggle.isOn = ShowFPSPref;
            }

            if (PostProcessToggle == null)
            {
                PostProcessToggle = SettingsPanel.transform.GetChild(3).GetComponent<Toggle>();

                ShowPostProcessPref = PlayerPrefs.GetInt(PostProcess, 1) == 1;

                PostProcessToggle.isOn = ShowPostProcessPref;
            }

            if(FXAAToggle == null)
            {
                FXAAToggle = SettingsPanel.transform.GetChild(4).GetComponent<Toggle>();

                FXAAPref = PlayerPrefs.GetInt(FXAA, 1) == 1;

                FXAAToggle.isOn = FXAAPref;
            }

            if(VolumeSlider == null)
            {
                VolumeSlider = SettingsPanel.transform.GetChild(5).GetComponent<Slider>();
                VolumeText = VolumeSlider.GetComponentInChildren<TextMeshProUGUI>();
                audio_volume = PlayerPrefs.GetFloat(AudioVolume, 1.0f);
                VolumeSlider.value = audio_volume;
                AudioListener.volume = VolumeSlider.value;
                VolumeText.text = "<color=white>" + AudioVolume + "</color>" + "<color=black>" + (VolumeSlider.value * 100).ToString("F0") + "</color>";
            }
            // Settings End //

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
        catch(Exception e)
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
                Game_Over_Panel.SetActive(false);
            }

            else
            {
                OverlayPanel.SetActive(true);
                SettingsPanel.SetActive(false);
                ListPanel.SetActive(false);
                PrivacyPolicyUI.SetActive(false);
                Game_Over_Panel.SetActive(false);
            } 

            if(Main_Audio_Listener == null)
            {
                Main_Audio_Listener = FindObjectOfType<AudioListener>();
            }

            Game_Over_Panel.SetActive(false);
        }
        catch(Exception e)
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
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void DelegateToggles()
    {
        PhysicsToggle.onValueChanged.AddListener(delegate{PhysicsToggleValueChanged(PhysicsToggle);});
        ShowFPSToggle.onValueChanged.AddListener(delegate{ShowFPSToggleValueChanged(ShowFPSToggle);});
        PostProcessToggle.onValueChanged.AddListener(delegate{ShowPostProcessValueChanged(PostProcessToggle);});
        FXAAToggle.onValueChanged.AddListener(delegate{FXAAToggleValueChanged(FXAAToggle);});
        VolumeSlider.onValueChanged.AddListener(delegate{OnVolumeSliderValueChanged();});
    }
    private void OnVolumeSliderValueChanged()
    {
        float Slider_Volume = VolumeSlider.value;

        AudioListener.volume = Slider_Volume;

        VolumeText.text = "<color=white>" + AudioVolume+ "</color>" + "<color=black>" + (Slider_Volume * 100).ToString("F0")+ "</color>";

        PlayerPrefs.SetFloat(AudioVolume, Slider_Volume);
    }
    private void FXAAToggleValueChanged(Toggle toggle)
    {
        bool toggle_value = toggle.isOn;

        if(toggle_value)
        {
            PostProcessing_Manager.instance.EnableFXAA();
            PlayerPrefs.SetInt(FXAA, 1);
        }
        else
        {
            PostProcessing_Manager.instance.DisableFXAA();
            PlayerPrefs.SetInt(FXAA, 0);
        }
    }
    private void ShowPostProcessValueChanged(Toggle toggle)
    {
        bool value = toggle.isOn;

        if (value)
        {
            PostProcessing_Manager.instance.EnablePostProcess();
            PlayerPrefs.SetInt(PostProcess, 1);
        }
        else
        {
            PostProcessing_Manager.instance.DisablePostProcess();
            PlayerPrefs.SetInt(PostProcess, 0);
        }
    }
    private void PhysicsToggleValueChanged(Toggle toggle_value)
    {
        RestrictionSystem restrictions = RestrictionSystem.instance;

        if(restrictions.OnlyStaticDrawingsAllowed)
        {
            ErrorSystem.instance.SetErrorMessage(ErrorSystem.OnlyStaticDrawingsAllowed);
            toggle_value.isOn = false;
            return;
        }
        else if(restrictions.OnlyDynamicDrawingsAllowed)
        {
            ErrorSystem.instance.SetErrorMessage(ErrorSystem.OnlyDynamicDrawingsAllowed);
            toggle_value.isOn = true;
            return;
        }
        else
        {
            bool value = toggle_value.isOn;
            PhysicsToggleTextChange(value);

            if (value)
            {
                PlayerPrefs.SetInt(LinephysicsType, 1);
            }
            else
            {
                PlayerPrefs.SetInt(LinephysicsType, 0);
            }
        }  
    }
    private void ShowFPSToggleValueChanged(Toggle toggle_value)
    {
        bool value = toggle_value.isOn;

        if (value)
        {
            PlayerPrefs.SetInt(ShowFPS, 1);
        }
        else
        {
            PlayerPrefs.SetInt(ShowFPS, 0);
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
    public void OpenSettings()
    {
        if (SceneManager.GetActiveScene().name == GameManager.StartMenuScene)
        {
            OpenSettingsForStartMenu();
            return;
        }

        Draw.instance.DisableDrawing();

        LastState = GameManager.IsGamePaused();

        if (!GameManager.IsGamePaused()) { GameManager.PauseGame(); }

        OverlayPanel.SetActive(false);
        ListPanel.SetActive(false);
        Game_Over_Panel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        if(SceneManager.GetActiveScene().name == GameManager.StartMenuScene)
        {
            CloseSettingsForStartMenu();
            StartMenuController.instance.CloseSettings();
            return;
        }

        SaveSettings();

        SettingsPanel.SetActive(false);
        ListPanel.SetActive(false);
        Game_Over_Panel.SetActive(false);
        OverlayPanel.SetActive(true);

        GameManager.SetGameState(LastState);

        Draw.instance.EnableDrawing();
    }
    public void OpenSettingsForStartMenu()
    {
        OverlayPanel.SetActive(false);
        ListPanel.SetActive(false);
        Game_Over_Panel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    public void CloseSettingsForStartMenu()
    {
        SettingsPanel.SetActive(false);
        OverlayPanel.SetActive(false);
        ListPanel.SetActive(false);
        Game_Over_Panel.SetActive(false);
    }
    public void OpenList()
    {
        LastState = GameManager.IsGamePaused();
        //Debug.Log("Menu Opened at State: "+LastState+"");
        if (!GameManager.IsGamePaused()) { GameManager.PauseGame(); }

        OverlayPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        Game_Over_Panel.SetActive(false);
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
        Game_Over_Panel.SetActive(false);
        PrivacyPolicyUI.SetActive(true);
    }
    public void ClosePrivacyPolicyUI()
    {
        SettingsPanel.SetActive(false);
        ListPanel.SetActive(false);
        PrivacyPolicyUI.SetActive(false);
        Game_Over_Panel.SetActive(false);
        OverlayPanel.SetActive(true);
        GameManager.SetGameState(LastState);
    }
    public void IssueGameOverPanel(string cause)
    {
        if (!GameManager.IsGamePaused()) { GameManager.PauseGame(); }

        SettingsPanel.SetActive(false);
        ListPanel.SetActive(false);
        PrivacyPolicyUI.SetActive(false);
        OverlayPanel.SetActive(false);
        Game_Over_Cause.text = cause;
        Game_Over_Panel.SetActive(true);
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
