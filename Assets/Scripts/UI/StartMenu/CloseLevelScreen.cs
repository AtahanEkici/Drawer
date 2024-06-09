using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[DefaultExecutionOrder(-1000)]
public class CloseLevelScreen : MonoBehaviour
{
    public RectTransform panelRect;

    [Header("Buttons")]
    [SerializeField] private Button[] buttons;

    private void Awake()
    {
        GetReferences();
        DelegateButtonsToLevels();
    }

    private void Update()
    {
        DetectInteraction();
    }
    private void GetReferences()
    {
        panelRect = GetComponent<RectTransform>();
    }
    private void DetectInteraction()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            CheckForInput();
        }
    }
    private void CheckForInput()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // Check if there's only one result and its GameObject matches the script's GameObject
        if (results.Count == 1 && results[0].gameObject == gameObject)
        {
            SoundManager.PlayButtonSound();
            StartMenuController.instance.CloseLevelsScreen();
        }
    }
    private void DelegateButtonsToLevels()
    {
        buttons = GetComponentsInChildren<Button>();

        for(int i=0;i<buttons.Length;i++)
        {
            int index = i+1;
            buttons[i].onClick.AddListener(delegate { AssignLevelsToButtons(index);});
        }
    }
    private void AssignLevelsToButtons(int Button_Number)
    {
        //Debug.Log("Level Button: "+Button_Number+" touched");
        LevelManager.LoadLevel(Button_Number);
    }
}
