using UnityEngine;
[DefaultExecutionOrder(-10000)]
public class RestrictionSystem : MonoBehaviour
{
    public static RestrictionSystem instance = null;
    private RestrictionSystem() {}

    public string RestrictionTag = "Restriction";

    [Header("Drawing Lenght Restriction")]
    [SerializeField] private float MaxDrawingLenght = 50f;

    [Header("Drawing Type Restrictions")]
    [SerializeField] public bool OnlyDynamicDrawingsAllowed = false;
    [SerializeField] public bool OnlyStaticDrawingsAllowed = false;

    [Header("Max Amount Of Drawings")]
    [SerializeField] private int MaxDrawingCount = 10;

    [Header("Hide Scores On This Level")]
    [SerializeField] private bool Show_Score_On_This_Level = false;

    private void Awake()
    {
        CheckInstance();
        CheckForDynamics();
    }
    private void OnEnable()
    {
        GetTag(); 
    }
    private void CheckForDynamics()
    {
        if(OnlyDynamicDrawingsAllowed && OnlyStaticDrawingsAllowed) // If Both are restricted choose one of them randomly//
        {
            int pick = Random.Range(0, 1);

            if(pick == 0)
            {
                OnlyDynamicDrawingsAllowed = true;
                OnlyStaticDrawingsAllowed = false;
            }
            else
            {
                OnlyDynamicDrawingsAllowed = false;
                OnlyStaticDrawingsAllowed = true;
            }
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
                Destroy(this);
                Debug.LogException(e);
            } 
        }
    }
    /// <summary>
    /// Returns the drawing restrictions as an object array
    /// </summary>
    public object[] GetRestrictions()
    {
        object[] restrictions = new object[4];

        // Restrictions //
        restrictions[0] = MaxDrawingLenght;
        restrictions[1] = MaxDrawingCount;
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
