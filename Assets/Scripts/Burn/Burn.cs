using UnityEngine;
public class Burn : MonoBehaviour
{
    [Header("Color Operations")]
    [SerializeField] private Color InitialColor = Color.white;
    [SerializeField] private Color WantedColor = Color.red;

    [Header("Local References")]
    [SerializeField] private Renderer render;

    [Header("Lerp Speed")]
    [SerializeField] private float LerpSpeed = 1f;

    [Header("Is On Burning Platform ?")]
    [SerializeField] private bool isOnBurningPlatform = true;

    private void Awake()
    {
        GetReferences();
    }
    private void Update()
    {
        FireUp();
        CoolDown();
    }
    private void GetReferences()
    {
        render = GetComponentInParent<Renderer>();
        InitialColor = render.material.color;
    }
    public void AdjustBurnSpeed(float adjustedSpeed)
    {
        LerpSpeed = adjustedSpeed;
    }
    public void BeginCooling()
    {
        isOnBurningPlatform = false;
    }
    private void FireUp()
    {
        if (!isOnBurningPlatform) { return; }

        render.material.color = Color.Lerp(render.material.color, WantedColor, LerpSpeed * Time.smoothDeltaTime);

        if(render.material.color == WantedColor)
        {
            Destroy(gameObject);
        }
    }
    private void CoolDown()
    {
        if (isOnBurningPlatform) { return; }

        render.material.color = Color.Lerp(render.material.color, InitialColor, LerpSpeed * Time.smoothDeltaTime);

        if (render.material.color == InitialColor && !isOnBurningPlatform)
        {
            Destroy(this);
        }
    }
}
