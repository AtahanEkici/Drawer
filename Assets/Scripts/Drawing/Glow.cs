using UnityEngine;
public class Glow : MonoBehaviour
{
    [Header("Returning Color")]
    [SerializeField] private bool isReturning = false;

    [Header("Material")]
    [SerializeField] private Material Color_Material;

    [Header("Color Options")]
    [SerializeField] private Color InitialColor;
    [SerializeField] private Color TargetColor = Color.red;

    [Header("Glow Options")]
    [SerializeField] private float GlowTimer = 0.5f;
    [SerializeField] private float InitialTimer;
    [SerializeField] private float GlowSpeed = 2f;

    private void Awake()
    {
        GetLocalReferences();
    }
    private void Update()
    {
        CountTime();
    }
    private void GetLocalReferences()
    {
        InitialTimer = GlowTimer;
        Color_Material = GetComponent<Renderer>().material;
        InitialColor = Color_Material.color;
    }
    private void CountTime()
    {
        if(GlowTimer <= 0)
        {
            if(isReturning)
            {
                isReturning = false;
            }
            else
            {
                isReturning = true;
            }

            GlowTimer = InitialTimer;
        }
        else
        {
            GlowTimer -= Time.unscaledDeltaTime;
        }

        if (isReturning)
        {
            Color_Material.color = Color.Lerp(Color_Material.color, TargetColor, Time.unscaledDeltaTime * GlowSpeed);
        }
        else
        {
            Color_Material.color = Color.Lerp(Color_Material.color, InitialColor, Time.unscaledDeltaTime * GlowSpeed);
        }
    }
    private void OnDestroy()
    {
        Color_Material.color = InitialColor;
    }
}
