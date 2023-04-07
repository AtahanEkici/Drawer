using UnityEngine;
using UnityEngine.UI;
//[DefaultExecutionOrder(-60)]
public class CloseListButton : MonoBehaviour
{
    [Header("Button reference")]
    [SerializeField] private Button ExitButton;
    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        DelegateButton();
    }
    private void GetLocalReferences()
    {
        if(ExitButton == null)
        {
            ExitButton = GetComponent<Button>();
        }
    }
    private void DelegateButton()
    {
        ExitButton.onClick.AddListener(ButtonPressedAction);
    }
    private void ButtonPressedAction()
    {
        UI_Controller.instance.CloseList();
    }
}
