using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class TurtleBloom : MonoBehaviour
{
    [SerializeField]
    float bloomDuration = 1f;
    [SerializeField]
    bool loop = false;
    [SerializeField]
    float bloomIntensity = 1f;
    [SerializeField]
    AnimationCurve bloomIntensityShape;
    [SerializeField]
    private Gradient bloomGradient;

    [SerializeField]
    Slider durationSlider;
    [SerializeField]
    TMP_InputField durationInputField;
    [SerializeField]
    TextMeshProUGUI loopButonText;
    [SerializeField]
    Slider IntensitySlider;
    [SerializeField]
    TMP_InputField IntensityInputField;

    [SerializeField]
    private Material turtleMaterial;
    private Coroutine BloomRoutine;
    private CultureInfo en_us = CultureInfo.GetCultureInfo("en-US");
    // Start is called before the first frame update
    void Start()
    {
        InitUIValues();
    }

    public void TriggerTurtleBloom()
    {
        if (BloomRoutine != null) StopCoroutine(BloomRoutine);
        BloomRoutine = StartCoroutine(BloomingRoutine());
    }
    private IEnumerator BloomingRoutine()
    {
        float timePassed = 0f;

        do
        {
            float applyedIntensity = bloomIntensityShape.Evaluate(timePassed / bloomDuration) * bloomIntensity;
            turtleMaterial.SetFloat("_Intensity", applyedIntensity);
            turtleMaterial.SetColor("_Color", bloomGradient.Evaluate(timePassed / bloomDuration));

            timePassed += Time.deltaTime;
            yield return null;
            
        } while (timePassed < bloomDuration);
        turtleMaterial.SetFloat("_Intensity",0f);
        turtleMaterial.SetColor("_Color",Color.white);
        BloomRoutine = null;
        if(loop) { TriggerTurtleBloom(); }
    }
    //Duration
    public void OnDurationValueChanged(float duration)
    {
        UpdateDurationValue(duration);
    }
    public void OnDurationValueChanged(string duration)
    {
        UpdateDurationValue(float.Parse(duration, en_us));
    }
    private void UpdateDurationValue(float duration)
    {
        bloomDuration = duration;
        durationInputField.text = duration.ToString("F2");
        durationSlider.value = duration;
    }
    //Loop
    public void OnToggleLoop()
    {
        loop = !loop;
        loopButonText.text = loop ? "ON" : "OFF";
    }
    //Intensity
    public void OnIntensityValueChanged(float intensity)
    {
        UpdateIntensityValue(intensity);
    }
    public void OnIntensityValueChanged(string intensity)
    {
        UpdateIntensityValue(float.Parse(intensity, en_us));
    }
    private void UpdateIntensityValue(float intensity)
    {
        bloomIntensity = intensity;
        IntensityInputField.text = intensity.ToString("F2");
        IntensitySlider.value = intensity;
    }
    
    private void InitUIValues()
    {
        durationInputField.text = bloomDuration.ToString("F2");
        durationSlider.value = bloomDuration;
        IntensityInputField.text = bloomIntensity.ToString("F2");
        IntensitySlider.value = bloomIntensity;
        
    }
}
