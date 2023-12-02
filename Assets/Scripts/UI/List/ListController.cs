using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[DefaultExecutionOrder(-350)]
public class ListController : MonoBehaviour
{
    private const string ListObjectResourcePath = "UI/ListObject/List_Object";

    [Header("Scroll Rect")]
    [SerializeField] private ScrollRect scroll_Rect;

    [Header("Scroll Area")]
    [SerializeField] private GameObject ScrollArea;

    [Header("Scroll")]
    [SerializeField] private GameObject Scroll;

    [Header("Container")]
    [SerializeField] private GameObject Container;
    [SerializeField] private RectTransform Container_RectTransform;

    [Header("Drawing Container")]
    [SerializeField] private DrawingContainer drawings;

    [Header("List Object Options")]
    [SerializeField] private GameObject ListObject;
    [SerializeField] private float ListObject_Height = 80;
    [SerializeField] private float PaddingHeight = 10;

    [Header("Selected Objects")]
    [SerializeField] private Button DeleteSelectedButton;
    private void Awake()
    {
        GetLocalReferences();
        GetResources();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneLoadMode)
    {
        GetForeignReferences();
        DispopulateContainer();
    }
    private void OnEnable()
    {
        try
        {
            Draw.instance.DisableDrawing();
            PopulateContainer();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void OnDisable()
    {
        try
        {
            Draw.instance.EnableDrawing();
            DispopulateContainer();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void GetResources()
    {
        try
        {
            ListObject = Resources.Load<GameObject>(ListObjectResourcePath) as GameObject;
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void GetLocalReferences()
    {
        if(ScrollArea == null)
        {
            ScrollArea = transform.GetChild(0).gameObject;
        }

        if(Scroll == null)
        {
            Scroll = ScrollArea.transform.GetChild(0).gameObject;

            if(scroll_Rect == null)
            {
                scroll_Rect = Scroll.GetComponent<ScrollRect>();
            }
        }

        if(Container == null)
        {
            Container = Scroll.transform.GetChild(0).gameObject;

            if(Container_RectTransform == null)
            {
                Container_RectTransform = Container.GetComponent<RectTransform>();
            }
        }
    }
    private void GetForeignReferences()
    {
        if(drawings == null)
        {
            drawings = DrawingContainer.instance;
        }

        if(DeleteSelectedButton == null)
        {
            DeleteSelectedButton = transform.GetChild(1).GetComponent<Button>();
            DeleteSelectedButton.onClick.AddListener(DeleteSelectedGameObjects);
        }
    }
    private void PopulateContainer()
    {
        if(drawings == null) { /*ErrorSystem.instance.SetErrorMessage(ErrorSystem.NoDrawingsFound);*/ return; }

        int Child_Count = drawings.transform.childCount;

        for (int i=0;i<Child_Count;i++)
        {
            GameObject temp = Instantiate(ListObject);

            RectTransform rt = temp.GetComponent<RectTransform>();
            temp.transform.SetParent(Container.transform);
            rt.localScale = Vector3.one;

            GameObject Drawing = drawings.transform.GetChild(i).gameObject;
            
            if(Drawing == null)
            {
                return;
            }

            temp.GetComponentInChildren<TextMeshProUGUI>().text = Drawing.name;

            Toggle toggle = temp.GetComponentInChildren<Toggle>();
            toggle.onValueChanged.AddListener(delegate { ToggleValueChanged(Drawing, toggle);});

            if (Drawing.GetComponent<Glow>() != null)
            {
                toggle.isOn = true;
                toggle.transform.parent.GetComponent<Image>().color = Color.red;
            }
        }
        AdjustHegiht();
    }
    private void AdjustHegiht()
    {
        float newHeight = Container.transform.childCount * (ListObject_Height + PaddingHeight);
        Container_RectTransform.sizeDelta = new Vector2(Container_RectTransform.sizeDelta.x, newHeight);
        Vector2 anchoredPosition = Container_RectTransform.anchoredPosition; anchoredPosition.y = (newHeight / 2) * -1;
        Container_RectTransform.anchoredPosition = anchoredPosition;
    }
    private void DeleteSelectedGameObjects()
    {
        Toggle[] AllToggles = Container.GetComponentsInChildren<Toggle>();

        if (AllToggles == null || AllToggles.Length <= 0) { return; }

        try
        {
            for (int i = 0; i < AllToggles.Length; i++)
            {
                if (AllToggles[i].isOn)
                {
                    AllToggles[i].onValueChanged.RemoveAllListeners();
                    Destroy(AllToggles[i].transform.parent.gameObject);
                }
            }
            DrawingContainer.instance.DeleteAllGlowing();
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
    private void DispopulateContainer()
    {
        if(drawings == null) { return; }

        for (int i = 0; i < Container.transform.childCount; i++)
        {
            Container.transform.GetChild(i).GetComponentInChildren<Toggle>().onValueChanged.RemoveAllListeners();
            Destroy(Container.transform.GetChild(i).gameObject);
        }
    }
    private void ToggleValueChanged(GameObject go,Toggle toggle)
    {
        if(toggle.isOn)
        {
            toggle.transform.parent.GetComponent<Image>().color = Color.red;
            go.AddComponent<Glow>();
        }
        else
        {
            try
            {
                toggle.transform.parent.GetComponent<Image>().color = Color.white;
                Destroy(go.GetComponent<Glow>());
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
