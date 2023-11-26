using TMPro;
using UnityEngine;
public class ShowFPS : MonoBehaviour
{
    private const string FPSSign = "FPS: ";

    [Header("Local TxtMeshPro Reference")]
    [SerializeField] private TextMeshProUGUI FPSText;

    [Header("Update Interval")]
    [SerializeField] private int UpdateInterval = 10;
    [SerializeField] private int counter = 0;
    private void Start()
    {
        FPSText = GetComponent<TextMeshProUGUI>();
    }
    private bool CheckFPSShow()
    {
        return PlayerPrefs.GetInt(UI_Controller.ShowFPS, 0) == 1;
    }
    private void ShowFPSOnUI()
    {
        if (!CheckFPSShow()) { FPSText.text = ""; return; }
        else if (counter < UpdateInterval) { counter++; return; }
        else
        {
            counter = 0;
            FPSText.text = FPSSign + (1f / Time.unscaledDeltaTime).ToString("F2") + "";
        }   
    }
    private void Update()
    {
        ShowFPSOnUI();
    }
}
