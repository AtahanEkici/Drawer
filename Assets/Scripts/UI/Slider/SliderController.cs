using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SliderController : MonoBehaviour
{
    [Header("Slider References")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private TextMeshProUGUI SliderText;
    private void Awake()
    {
        GetLocalReferences();
    }
    private void Start()
    {
        DelegateSlider();
    }
    private void GetLocalReferences()
    {
        if(mainSlider == null)
        {
            mainSlider = GetComponent<Slider>();
        }
        if(SliderText == null)
        {
            SliderText = mainSlider.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
    private void DelegateSlider()
    {
        mainSlider.onValueChanged.AddListener(SliderOperations);
    }

    private void SliderOperations(float value)
    {
        int val = (int)value;
        SliderText.text = val.ToString() + "%";
    }
}
