using TMPro;
using UnityEngine;
public class DilationController : MonoBehaviour
{
    [Header("TextMeshPro Reference")]
    [SerializeField] private TextMeshProUGUI tmp;

    [Header("Flashing Speed")]
    [SerializeField] private float DilationSpeed = 10000f;

    [Header("Time Variable")]
    [SerializeField] private Time pingpongtime;

    private void Awake()
    {
        GetLocalReferences();
    }

    private void Update()
    {
        PingPongDilation();
    }
    private void GetLocalReferences()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void PingPongDilation()
    {
        float pingPongValue = Mathf.PingPong(Time.unscaledDeltaTime * DilationSpeed, 1.0f);
        tmp.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, pingPongValue);
    }
}
