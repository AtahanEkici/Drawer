using UnityEngine;
[DefaultExecutionOrder(-3000)]
public class RestrictionSystem : MonoBehaviour
{
    public static RestrictionSystem instance = null;
    private RestrictionSystem() {}

    public string RestrictionTag = "Restriction";

    [Header("Drawing Lenght Restriction")]
    [SerializeField] private float MaxDrawingLenght = 50f;

    [Header("Max Amount Of Drawings")]
    [SerializeField] private int MaxDrawingCount = 10;

    [Header("Max Ink Amount")]
    [SerializeField] private float MaxInkAmount = 50f;

    private void Awake()
    {
        CheckInstance();  
    }
    private void OnEnable()
    {
        GetTag();
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
    /// returns the drawing restrictions
    /// </summary>
    public object[] GetRestrictions()
    {
        object[] restrictions = new object[4];

        // Restrictions //
        restrictions[0] = MaxDrawingLenght;
        restrictions[1] = MaxDrawingCount;
        restrictions[2] = MaxInkAmount;
        // Restrictions //

        return restrictions;
    }
    private void GetTag()
    {
        RestrictionTag = gameObject.tag;
    }
}
