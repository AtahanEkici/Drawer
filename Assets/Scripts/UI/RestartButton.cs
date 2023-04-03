using UnityEngine;
using UnityEngine.UI;
public class RestartButton : MonoBehaviour
{
    [Header("Pause-Resume Button")]
    [SerializeField] private Button Restart_Button;
    private void Awake()
    {
        GetLocalReferences();
        DelegateButton();
    }
    private void GetLocalReferences()
    {
        if (Restart_Button == null)
        {
            Restart_Button = GetComponent<Button>();
        }
    }
    private void DelegateButton()
    {
        Restart_Button.onClick.AddListener(OnButtonPressed);
    }
    private void OnButtonPressed()
    {
        //Debug.Log("Restart button Pressed");
        GameManager.RestartGame();
    }
}
