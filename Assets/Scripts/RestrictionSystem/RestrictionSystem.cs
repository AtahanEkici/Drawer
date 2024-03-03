using UnityEngine;
[DefaultExecutionOrder(-10000)]
public class RestrictionSystem : MonoBehaviour
{
    public static RestrictionSystem instance = null;
    private RestrictionSystem() {}

    public string RestrictionTag = "Restriction";

    [Header("Is Time Restricted")]
    [SerializeField] private bool isTimeRestricted = false;

    [Header("Drawing Lenght Restriction")]
    [SerializeField] private float MaxDrawingLenght = 50f;

    [Header("Drawing Type Restrictions")]
    [SerializeField] public bool OnlyDynamicDrawingsAllowed = false;
    [SerializeField] public bool OnlyStaticDrawingsAllowed = false;

    [Header("Max Amount Of Drawings")]
    [SerializeField] private int MaxDrawingCount = 10;

    [Header("Min Lenght Of Drawings")]
    [SerializeField] private int MinDrawingLenght = 1;

    [Header("Hide Scores On This Level")]
    [SerializeField] private bool Show_Score_On_This_Level = false;

    [Header("Time Limit For This Level")]
    [SerializeField] private float Time_Limit_Restriction = 600f;

    [Header("Death Walls on This Level ? ")]
    [SerializeField] private bool isDeathWalls = false;

    private void Awake()
    {
        CheckInstance();
        CheckForDynamics();
    }
    private void OnEnable()
    {
        GetTag(); 
    }
    private void Update()
    {
        CheckTime();
    }
    public bool IsDeathWallsOnThisLevel()
    {
        return isDeathWalls;
    }
    private void CheckForDynamics()
    {
        if(OnlyDynamicDrawingsAllowed && OnlyStaticDrawingsAllowed) // If Both are restricted choose one of them randomly//
        {
            Debug.LogWarning("Wrong Restriction Parameters");
            int pick = Random.Range(0, 2);

            if(pick == 0)
            {
                Debug.LogWarning("Picked Dynamic");
                OnlyDynamicDrawingsAllowed = true;
                OnlyStaticDrawingsAllowed = false;
            }
            else if(pick == 1)
            {
                Debug.LogWarning("Picked Static");
                OnlyDynamicDrawingsAllowed = false;
                OnlyStaticDrawingsAllowed = true;
            }
            else
            {
                Debug.LogWarning("Picked None");
                OnlyDynamicDrawingsAllowed = false;
                OnlyStaticDrawingsAllowed = false;
            }
        }
    }
    private void CheckTime()
    {
        if (!isTimeRestricted) { return; }

        if(TimeController.Timer >= Time_Limit_Restriction)
        {
            GameManager.Instance.GameOver(GameManager.Time_Limit_Exceeded);
            ErrorSystem.instance.SetErrorMessage(ErrorSystem.TimeLimitExceeded);
        }
    }
    private void CheckInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            try
            {
                Debug.Log("Only One Restriction Object should be remain in scene");
                Destroy(gameObject);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
                Destroy(this);
            } 
        }
    }
    /// <summary>
    /// Returns the drawing restrictions as an object array
    /// </summary>
    public object[] GetRestrictions()
    {
        object[] restrictions = new object[3];

        // Restrictions //
        restrictions[0] = MaxDrawingLenght;
        restrictions[1] = MaxDrawingCount;
        restrictions[2] = MinDrawingLenght;
        // Restrictions //

        return restrictions;
    }
    public void GetTag()
    {
        RestrictionTag = gameObject.tag;
    }
    public bool ShowScore()
    {
        return Show_Score_On_This_Level;
    }
}
