using UnityEngine;
using UnityEditor;
public class NewlyOpened : MonoBehaviour
{
    [Header("Scene to load after the operation")]
    [SerializeField] private string Level = "Test";

    private void Awake()
    {
        try
        {
            PlayerSettings.SplashScreen.backgroundColor = Random.ColorHSV();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void Start()
    {
        GameManager.LoadScene(Level);
    }
}
