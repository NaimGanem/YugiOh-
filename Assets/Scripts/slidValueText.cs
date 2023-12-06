using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class slidValueText : MonoBehaviour
{
    public Slider slider;
    public TMP_Text textComp;
    void Awake()
    {
        slider = GetComponentInParent<Slider>();
        

    }
    void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }
    void UpdateText(float val)
    {
        textComp.text = slider.value.ToString();
    }
}
