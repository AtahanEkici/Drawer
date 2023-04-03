using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-90)]
public class PauseOrResume : MonoBehaviour
{
    public static PauseOrResume Instance { get; private set; }
    private PauseOrResume() { }

    [Header("Pause-Resume Button")]
    [SerializeField] private Button PauseOrResumeButton;

    [Header("Image Reference")]
    [SerializeField] private Image image;

    [Header("Images")]
    [SerializeField] private Sprite Pause_Image;
    [SerializeField] private Sprite Play_Image;
    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        DelegateButton();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode SceneLoadMode)
    {
        OnStart();
    }
    private void CheckInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetLocalReferences()
    {
        if(PauseOrResumeButton == null)
        {
            PauseOrResumeButton = GetComponent<Button>();
        }

        if(image == null)
        {
            image = GetComponent<Image>();
        }
    }
    private void DelegateButton()
    {
        PauseOrResumeButton.onClick.AddListener(OnButtonPressed);
    }
    private void OnButtonPressed()
    {
        //Debug.Log("Pause button Pressed");

        if(!GameManager.IsGamePaused())
        {
            GameManager.PauseGame();
            ChangeImage(Play_Image);
        }
        else
        {
            GameManager.ResumeGame();
            ChangeImage(Pause_Image);
        }
    }
    public void OnStart()
    {
        if(GameManager.IsGamePaused())
        {
            ChangeImage(Play_Image);
        }
        else
        {
            ChangeImage(Pause_Image);
        }
    }
    private void ChangeImage(Sprite sprite)
    {
        image.overrideSprite = sprite;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
