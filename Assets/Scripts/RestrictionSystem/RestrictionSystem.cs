using UnityEngine;
[DefaultExecutionOrder(-3000)]
public class RestrictionSystem : MonoBehaviour
{
    public static RestrictionSystem instance = null;
    private RestrictionSystem() {}

    [Header("Drawing Lenght Restriction")]
    [SerializeField] private float MinDrawingLenght = 10f;

    [Header("Max Amount Of Drawings")]
    [SerializeField] private int MaxDrawingAmount = 10;

    private void Awake()
    {
        CheckInstance();
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
    public int GetMaxDrawingCount()
    {
        return MaxDrawingAmount;
    }
    public float GetMinDrawinglenght()
    {
        return MinDrawingLenght;
    }
}