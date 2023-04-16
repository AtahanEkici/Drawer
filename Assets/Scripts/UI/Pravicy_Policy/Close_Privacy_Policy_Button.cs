using UnityEngine;
using UnityEngine.UI;
public class Close_Privacy_Policy_Button : MonoBehaviour
{
    public static Close_Privacy_Policy_Button instance = null;
    private Close_Privacy_Policy_Button() { }

    [Header("Button Reference")]
    [SerializeField] private Button Close_Button;
    private void Awake()
    {
        CheckInstance();
        GetReferences();
        DelegateButton();
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Gameobject: "+gameObject.name+" destroyed because of the duplication");
        }
    }
    private void DelegateButton()
    {
        Close_Button.onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        Destroy(gameObject);
    }
    private void GetReferences()
    {
        Close_Button = GetComponentInChildren<Button>();
    }
    private void OnDestroy()
    {
        Close_Button.onClick.RemoveAllListeners();
    }
}