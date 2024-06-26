using UnityEngine;
using UnityEngine.UI;
[DefaultExecutionOrder(-5000)]
public class SoundManager : MonoBehaviour
{
    public static readonly string Audio_Path = "Sounds";

    [Header("Dont Destroy On Load ? ")]
    [SerializeField] private bool Dont_DestroyOnLoad = true;

    [Header("Instance Variables")]
    public static SoundManager instance = null;

    [Header("Data Containers")]
    public static AudioClip[] AllSoundFiles;

    [Header("Pre-Built File Names")]
    private readonly string Explosion_Audio_File = "Explosion";
    private readonly string Touch_Audio_File = "Touch";
    private readonly string Pickup_Audio_File = "Pickup";
    private readonly string Hit_Audio_File = "Hit";
    private readonly string Click_Audio_File = "Click";
    private readonly string Destruction_Audio_File = "Destruction";
    private readonly string Button_Audio_File = "Button";
    private readonly string Piston_Hit_Audio_File = "PistonHit";
    private readonly string Rope_Touch = "RopeTouch";
    private readonly string Cannon_Shot = "CannonShot";

    [Header("Pre-Built Sounds")]
    public static AudioClip Explosion_Sound;
    public static AudioClip Touch_Sound;
    public static AudioClip Pickup_Sound;
    public static AudioClip Hit_Sound;
    public static AudioClip Click_Sound;
    public static AudioClip Destruction_Sound;
    public static AudioClip Button_Sound;
    public static AudioClip PistonHit;
    public static AudioClip RopeTouch;
    public static AudioClip CannonShot;

    // Main Functions Start //
    private void Awake()
    {
        CheckInstance();
    }
    private void Start()
    {
        GetSounds();
    }
    // Main Functions End //

    // Body Start //
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
            if(Dont_DestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
        }
    }
    public static void AddButtonAudioToAllButtons(Button[] buttons, GameObject go)
    {
        buttons = go.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayButtonSound);
        }
    }

    public static void PlayButtonSound()
    {
        GameObject soundObject = new("ButtonSound");

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(Button_Sound);

        Destroy(soundObject, Button_Sound.length * 2f);
    }

    private void GetSounds() // Get Sounds from the 
    {
        AllSoundFiles = Resources.LoadAll<AudioClip>(Audio_Path);

        for(int i=0;i<AllSoundFiles.Length;i++) // Could Be switch case but ah well //
        {
            AudioClip current_clip = AllSoundFiles[i];
            string file_name = current_clip.name;

            if (file_name == Explosion_Audio_File)
            {
                Explosion_Sound = current_clip;
            }
            else if(file_name == Touch_Audio_File)
            {
                Touch_Sound = current_clip;
            }
            else if(file_name == Pickup_Audio_File)
            {
                Pickup_Sound = current_clip;
            }
            else if (file_name == Hit_Audio_File)
            {
                Hit_Sound = current_clip;
            }
            else if (file_name == Click_Audio_File)
            {
                Click_Sound = current_clip;
            }
            else if (file_name == Destruction_Audio_File)
            {
                Destruction_Sound = current_clip;
            }
            else if(file_name == Button_Audio_File)
            {
                Button_Sound = current_clip;
            }
            else if (file_name == Piston_Hit_Audio_File)
            {
                PistonHit = current_clip;
            }
            else if (file_name == Rope_Touch)
            {
                RopeTouch = current_clip;
            }
            else if(file_name == Cannon_Shot)
            {
                CannonShot = current_clip;
            }
            else
            {
                Debug.LogError("Wrong File on Audio - Not Captured: "+ file_name +"");
            }
        }
    }
    // Body End //
}
