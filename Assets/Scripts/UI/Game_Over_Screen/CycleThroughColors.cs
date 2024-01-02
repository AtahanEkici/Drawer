using TMPro;
using UnityEngine;

public class CycleThroughColors : MonoBehaviour
{
    [Header("Randomize Colors")]
    [SerializeField] private bool isRandom = false;

    [Header("TextMesh Object Reference")]
    [SerializeField] private TextMeshProUGUI textMesh;

    [Header("Color Options")]
    [SerializeField] private Color[] colors;

    [Header("Cycle Speed")]
    [SerializeField] private float speed = 0.50f;
    [SerializeField] private float speedCounter = 0f;

    [Header("Text")]
    private string initialText;

    private int colorIndex = 0;

    private void Awake()
    {
        GetLocalReferences();
        CheckColors();
    }
    private void GetLocalReferences()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        initialText = textMesh.text;
    }

    private void CheckColors()
    {
        if (colors.Length <= 0)
        {
            colors = new Color[2];
            colors[0] = Color.white;
            colors[1] = Color.black;
        }
    }
    private void Update()
    {
        FormatTextColor();
    }
    private void FormatTextColor()
    {
        if (speedCounter >= speed)
        {
            if(isRandom)
            {
                RandomizeColors();
            }
            else
            {
                ShiftColors();
            }
            
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
            Color selectedColor = colors[colorIndex++ % colors.Length]; // Pick a color from the array
            coloredText += $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}>{initialText[i]}</color>";
            //colorIndex++;
        }

        textMesh.SetText(coloredText);
    }
    private void RandomizeColors()
    {
        string coloredText = "";

        for (int i = 0; i < initialText.Length; i++)
        {
            Color selectedColor = Random.ColorHSV();
            coloredText += $"<color=#{ColorUtility.ToHtmlStringRGB(selectedColor)}>{initialText[i]}</color>";
        }

        textMesh.SetText(coloredText);
    }
}