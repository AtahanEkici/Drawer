using UnityEngine;
public class LevelStarController : MonoBehaviour
{
    private static readonly string StarLocation = "UI/Star";

    public static LevelStarController instance = null;

    [Header("Images")]
    [SerializeField] private Sprite[] Stars;
    [SerializeField] private Sprite OutlineStar;
    [SerializeField] private Sprite YellowStar;

    [Header("Resource Names")]
    [SerializeField] private static readonly string Out = "out";
    [SerializeField] private static readonly string In = "in";

    private void Awake()
    {
        CheckInstance();
        GetResources();
    }
    private void OnEnable()
    {
        Debug.Log("OnEnable Called On Level Star Controller");
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
                Destroy(gameObject);
            }
            catch(System.Exception e)
            {
                Destroy(this);
                Debug.LogException(e);
            }
        }
    }
    private void GetResources()
    {
        Stars = Resources.LoadAll(StarLocation) as Sprite[];

        for(int i=0;i<Stars.Length;i++)
        {
            if (Stars[i].name.ToLower().Equals(Out))
            {
                OutlineStar = Stars[i];
            }
            else if(Stars[i].name.ToLower().Equals(In))
            {
                YellowStar = Stars[i];
            }
        }
    }
}
