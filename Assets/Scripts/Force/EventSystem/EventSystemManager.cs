using UnityEngine;
public class EventSystemManager : MonoBehaviour
{
    private static EventSystemManager instance = null;
    private EventSystemManager() { }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            try
            {
                Destroy(gameObject);
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
                Destroy(this);
            }
        }
    }
}
