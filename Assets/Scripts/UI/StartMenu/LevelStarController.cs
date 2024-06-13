using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class LevelStarController : MonoBehaviour
{
    private static LevelStarController instance = null;
    private static readonly string StarLocation = "UI/Star";
    private static readonly string CoinPrefString = "Coin_";
    private static readonly string Out = "out";

    [Header("Sprites for Level Stars")]
    [SerializeField] private Sprite[] stars;
    [SerializeField] private Sprite outlineStar;
    [SerializeField] private Sprite yellowStar;

    [Header("All Stars")]
    [SerializeField] private List<GameObject> starImages = new List<GameObject>();
    [SerializeField] private int[] integerParts;

    [Header("All Buttons")]
    [SerializeField] private Button[] buttons;
    private void Awake()
    {
        CheckInstance();
        GetResources();
        GetStars();
    }
    private void OnEnable()
    {
        GetStars();
    }
    private void CheckInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void GetResources()
    {
        stars = Resources.LoadAll<Sprite>(StarLocation);

        foreach (Sprite sprite in stars)
        {
            if (sprite.name.Equals(Out))
            {
                outlineStar = sprite;
            }
            else
            {
                yellowStar = sprite;
            }
        }

        buttons = GetComponentsInChildren<Button>();
    }
    private void GetStars()
    {
        GameObject[] StarObjects = GameObject.FindGameObjectsWithTag("Star");

        starImages = StarObjects.ToList();

        integerParts = new int[starImages.Count];

        for (int i = 0; i < starImages.Count; i++)
        {
            string[] parts = starImages[i].name.Split("_");
            string rightSide = parts[1];

            if (int.TryParse(rightSide, out int intValue))
            {
                integerParts[i] = intValue;

                string temp = CoinPrefString + integerParts[i].ToString();
                //Debug.Log(temp);
                int current = PlayerPrefs.GetInt(temp, 0);

                if (current != 0)
                {
                    // Change Star to shiny one 

                    starImages[i].GetComponent<Image>().sprite = yellowStar;
                }
            }
        }
    }
}
