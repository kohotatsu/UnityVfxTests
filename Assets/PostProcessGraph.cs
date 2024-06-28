using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostProcessGraph : MonoBehaviour
{
    public Material PostProcessMaterial;

    [SerializeField]
    Slider pixelSlider;
    [SerializeField]
    TMP_InputField pixelInputField;
    [SerializeField]
    Slider deformSlider;
    [SerializeField]
    TMP_InputField deformInputField;
    [SerializeField]
    Slider deformSpeedSlider;
    [SerializeField]
    TMP_InputField deformSpeedInputField;
    [SerializeField]
    TextMeshProUGUI toggleLabel;

    private bool isInverted;
    CultureInfo en_us = CultureInfo.GetCultureInfo("en-US");

    private void Start()
    {
        isInverted = PostProcessMaterial.GetInt("_Invert")==1?true:false;

        InitUIValues();
    }

    public void OnPixelChanged(float pixel)
    {
        UpdatePixelValue(pixel);
    }
    public void OnPixelChanged(string pixel)
    {
        UpdatePixelValue(float.Parse(pixel, en_us));
    }
    private void UpdatePixelValue(float pixel)
    {
        PostProcessMaterial.SetFloat("_Pixelize", pixel);
        pixelInputField.text = pixel.ToString("F2");
        pixelSlider.value = pixel;
    }

    public void OnDeformChanged(float value)
    {
        UpdateDeformValue(value);
    }
    public void OnDeformChanged(string value)
    {
        UpdateDeformValue(float.Parse(value, en_us));
    }
    private void UpdateDeformValue(float value)
    {
        PostProcessMaterial.SetFloat("_Deform", value);
        deformInputField.text = value.ToString("F2");
        deformSlider.value = value;
    }

    public void OnDeformSpeedChanged(float speed)
    {
        UpdateDeformSpeedValue(speed);
    }
    public void OnDeformSpeedChanged(string speed)
    {
        UpdateDeformSpeedValue(float.Parse(speed, en_us));
    }
    private void UpdateDeformSpeedValue(float speed)
    {
        PostProcessMaterial.SetFloat("_DefromSpeed", speed);
        deformSpeedInputField.text = speed.ToString("F2");
        deformSpeedSlider.value = speed;
    }

    public void OnToggleInvert()
    {
        isInverted = !isInverted;
        PostProcessMaterial.SetInt("_Invert", isInverted ? 1 : 0);
        toggleLabel.text = isInverted ? "ON" : "OFF";
    }

    public void OnResetValues() { InitUIValues(); }
    private void InitUIValues()
    {
        UpdatePixelValue(0f);
        UpdateDeformValue(0f);
        UpdateDeformSpeedValue(0f);
        if(isInverted) OnToggleInvert();

        deformSpeedInputField.text = PostProcessMaterial.GetFloat("_Pixelize").ToString("F2");
        deformSpeedSlider.value = PostProcessMaterial.GetFloat("_Pixelize");
        deformSpeedInputField.text = PostProcessMaterial.GetFloat("_Deform").ToString("F2");
        deformSpeedSlider.value = PostProcessMaterial.GetFloat("_Deform");
        deformSpeedInputField.text = PostProcessMaterial.GetFloat("_DefromSpeed").ToString("F2");
        deformSpeedSlider.value = PostProcessMaterial.GetFloat("_DefromSpeed");

    }
}
