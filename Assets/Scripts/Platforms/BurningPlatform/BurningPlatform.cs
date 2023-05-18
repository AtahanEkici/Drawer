using UnityEngine;
public class BurningPlatform : MonoBehaviour
{
    [Header("Burn Speed")]
    [SerializeField] private float BurnSpeed = 5f;

    private void AddBurn(GameObject go)
    {
        if (go.GetComponentInParent<Burn>() == null)
        {
            go.AddComponent<Burn>().AdjustBurnSpeed(BurnSpeed);
        }

        Debug.Log("Added Burn to " + go.name + "");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        AddBurn(go);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        AddBurn(go);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;

        Burn burn = go.GetComponentInParent<Burn>();

        if(burn != null)
        {
            burn.BeginCooling(BurnSpeed);
        }
    }
}
