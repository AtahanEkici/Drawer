using UnityEngine;
using UnityEngine.UI;
public class PauseOrResume : MonoBehaviour
{
    [Header("Pause-Resume Button")]
    [SerializeField] private Button PauseOrResumeButton;

    [Header("Image Reference")]
    [SerializeField] private Image image;

    [Header("Images")]
    [SerializeField] private Sprite Pause_Image;
    [SerializeField] private Sprite Play_Image;
    private void Awake()
    {
        GetLocalReferences();
    }
    private void OnEnable()
    {
        DelegateButton();
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
        if(Time.timeScale >= 1)
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
    private void ChangeImage(Sprite sprite)
    {
        image.overrideSprite = sprite;
    }
}
