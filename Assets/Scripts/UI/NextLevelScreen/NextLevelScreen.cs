using UnityEngine;
using UnityEngine.UI;
public class NextLevelScreen : MonoBehaviour
{
    public static NextLevelScreen instance = null;

    [Header("Local References")]
    [SerializeField] private Button NextlevelButton;

    private void Awake()
    {
        CheckInstance();
        GetLocalReferences();
        DelegateButton();
    }
    private void GetLocalReferences()
    {
        if(NextlevelButton == null)
        {
            NextlevelButton = GetComponentInParent<Button>();
        }
    }
    private void DelegateButton()
    {
        try
        {
            NextlevelButton.onClick.AddListener(delegate{ LoadNextLevel();}); // Load Next level after the button is pressed //
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void LoadNextLevel()
    {
        LevelManager.LoadNextLevel();
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
                DestroyImmediate(gameObject);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
                DestroyImmediate(this);
            }
        }
    }
}
