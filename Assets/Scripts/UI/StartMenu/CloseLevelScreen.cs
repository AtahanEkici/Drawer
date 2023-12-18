using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CloseLevelScreen : MonoBehaviour
{
    public RectTransform panelRect;

    private void Awake()
    {
        panelRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        DetectInteraction();
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

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == gameObject)
            {
                SoundManager.PlayButtonSound();
                StartMenuController.instance.CloseLevelsScreen();
                break;
            }
        }
    }
}
