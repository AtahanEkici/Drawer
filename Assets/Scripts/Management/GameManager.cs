using UnityEngine;
public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartUpOperations();
    }
    private void StartUpOperations()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
