using UnityEngine;
public class CameraShake : MonoBehaviour
{
    [Header("Do Not Destroy On Load ?")]
    [SerializeField] private bool Do_Not_Destroy = true;

    [SerializeField]
    Vector3 maximumTranslationShake = Vector3.one;
    private float ang;
    private Vector3 maximumAngularShake;
    [SerializeField]
    float traumaExponent = 1;
    [SerializeField]
    float recoverySpeed = 1;

    private float trauma;
    private float seed;
    private float frequency = 25;

    public static CameraShake _instance;
    public static CameraShake Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        CheckInstance();
        CheckDestruction();
    }
    private void Start()
    {
        seed = Random.value;
        maximumAngularShake = Vector3.one * ang;
    }
    private void Update()
    {
        float shake = Mathf.Pow(trauma, traumaExponent);
        
        transform.SetLocalPositionAndRotation(new Vector3(
        maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.unscaledTime * frequency) * 2 - 1),
        maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.unscaledTime * frequency) * 2 - 1),
        maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.unscaledTime * frequency) * 2 - 1)) * shake, 
        Quaternion.Euler(new Vector3(
        maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.unscaledTime * frequency) * 2 - 1),
        maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.unscaledTime * frequency) * 2 - 1),
        maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.unscaledTime * frequency) * 2 - 1)) * shake));
        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.unscaledDeltaTime);
    }
    public void InduceStress(float frequency,float ang,float stress)
    {
        this.frequency = frequency;
        this.ang = ang;
        trauma = Mathf.Clamp01(trauma + stress);
    }
    public void InduceStress(Vector3 values)
    {
        this.frequency = values.x;
        this.ang = values.y;
        trauma = Mathf.Clamp01(trauma + values.z);
    }
    private void CheckInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    private void CheckDestruction()
    {
        if(Do_Not_Destroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}