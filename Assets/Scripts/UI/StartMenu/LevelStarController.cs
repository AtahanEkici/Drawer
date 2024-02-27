using UnityEngine;
using UnityEngine.UI;
public class LevelStarController : MonoBehaviour
{
    private static readonly string StarLocation = "UI/Star";

    public static LevelStarController instance = null;

    [Header("Sprites for Level Stars")]
    [SerializeField] private Sprite[] Stars;
    [SerializeField] private Sprite OutlineStar;
    [SerializeField] private Sprite YellowStar;

    [Header("All Buttons")]
    [SerializeField] private Button[] buttons;

    private static readonly string Out = "out";

    private void Awake()
    {
        CheckInstance();
        GetResources();
    }
    private void OnEnable()
    {
        Debug.Log("OnEnable Called On Level Star Controller");
        ChangeStarsOnLevelMenu();
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
        Stars = Resources.LoadAll<Sprite>(StarLocation);

        foreach(Sprite sprite in Stars)
        {
            if(sprite.name.Equals(Out))
            {
                OutlineStar = sprite;
            }
            else
            {
                YellowStar = sprite;
            }
        }
    }

    private void ChangeStarsOnLevelMenu() // Change star sprites accordingly with the saved level stars //
    {
        buttons = GetComponentsInChildren<Button>();


    }
}
