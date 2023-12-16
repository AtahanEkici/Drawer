using TMPro;
using UnityEngine;

public class CycleThroughColors : MonoBehaviour
{
    [Header("TextMesh Object Reference")]
    [SerializeField] private TextMeshProUGUI textMesh;

    [Header("Color Options")]
    [SerializeField] private Color[] colors;

    [Header("Cycle Speed")]
    [SerializeField] private float speed = 1.0f;
    private float speedCounter = 0f;

    [Header("Text")]
    [SerializeField] private string initialText = "Sample Text";

    private int colorIndex = 0;

    private void Awake()
    {
        GetLocalReferences();
    }

    private void GetLocalReferences()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = initialText;
    }

    private void Update()
    {
        FormatTextColor();
    }

    private void FormatTextColor()
    {
        if (speedCounter >= speed)
        {
            ShiftColors();
            speedCounter = 0f;
        }
        else
        {
            speedCounter += Time.unscaledDeltaTime;
        }
    }

    private void ShiftColors()
    {
        string coloredText = "";

        for (int i = 0; i < initialText.Length; i++)
        {
            Color selectedColor = colors[colorIndex % colors.Length]; // Pick a color from the array
            coloredText += $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}>{initialText[i]}</color>";
            colorIndex++;
        }

        textMesh.text = coloredText;
    }
}
