using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeDuration = 1f;
    [SerializeField]
    private float shakeStrength = 1f;
    [SerializeField]
    private float shakeSpeed = 0f;
    [SerializeField]
    private float shakeRotation = 0f;
    [SerializeField]
    private float shakeZoom = 0f;
    [SerializeField]
    private AnimationCurve shakeShape;
    [SerializeField]
    private Vector3 biases = Vector3.zero;
    [SerializeField]
    private float biaseStrength = 1f;

    [SerializeField]
    Slider durationSlider;
    [SerializeField]
    Slider strengthSlider;
    [SerializeField]
    Slider speedSlider;
    [SerializeField]
    Slider rotationSlider;
    [SerializeField]
    Slider zoomSlider;
    [SerializeField]
    Slider[] biasesSliders;
    [SerializeField]
    Slider biaseStrengthSlider;
    [SerializeField]
    TMP_InputField durationInputField;
    [SerializeField]
    TMP_InputField strengthInputField;
    [SerializeField]
    TMP_InputField speedInputField;
    [SerializeField]
    TMP_InputField rotationInputField;
    [SerializeField]
    TMP_InputField zoomInputField;
    [SerializeField]
    TMP_InputField[] biasesInputFields;
    [SerializeField]
    TMP_InputField biaseStrengthInputField;

    private Camera mainCamera;
    private Coroutine ShakeRoutine;
    private float defaultZoom;
    // Start is called before the first frame update
    CultureInfo en_us = CultureInfo.GetCultureInfo("en-US");
    void Start()
    {
        mainCamera = Camera.main;
        defaultZoom = mainCamera.orthographicSize;

        InitUIValues();
    }

    public void TriggerScreenShake()
    {
        if(ShakeRoutine != null)StopCoroutine(ShakeRoutine);
        ShakeRoutine = StartCoroutine(SquareShake());
    }
    private IEnumerator SquareShake()
    {
        float timePassed = 0f;
        
        do
        {
            float applyedStrength = shakeShape.Evaluate(timePassed/shakeDuration)*shakeStrength;
            float applyedRotation = shakeShape.Evaluate(timePassed/shakeDuration)*shakeRotation;
            float applyedZoom = shakeShape.Evaluate(timePassed/shakeDuration)*shakeZoom;
            Vector3 applyedBias = biases * shakeShape.Evaluate(timePassed/shakeDuration)*biaseStrength;
            mainCamera.transform.localPosition = 
                new Vector3(UnityEngine.Random.Range(-applyedStrength, applyedStrength) + applyedBias.x, 
                UnityEngine.Random.Range(-applyedStrength, applyedStrength) + applyedBias.y, 
                mainCamera.transform.localPosition.z);

            mainCamera.transform.localRotation = Quaternion.Euler(0f,0f,
                UnityEngine.Random.Range(-applyedRotation, applyedRotation));

            mainCamera.orthographicSize = defaultZoom + 
                UnityEngine.Random.Range(-applyedZoom, applyedZoom) - applyedBias.z;

            timePassed += shakeSpeed == 0f? Time.deltaTime: Mathf.Max(Time.deltaTime, 1f / shakeSpeed);
            if(shakeSpeed == 0f) yield return null;
            else yield return new WaitForSeconds(1f/shakeSpeed);

        } while (timePassed < shakeDuration);
        mainCamera.transform.localPosition = new Vector3(0f,0f, mainCamera.transform.localPosition.z);
        mainCamera.transform.localRotation = Quaternion.Euler(0f,0f,0f);
        mainCamera.orthographicSize = defaultZoom;
        ShakeRoutine = null;
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
        shakeDuration = duration;
        durationInputField.text = duration.ToString("F2");
        durationSlider.value = duration;
    }
    //Strength
    public void OnStrengthValueChanged(float strength)
    {
        UpdateStrengthValue(strength);
    }
    public void OnStrengthValueChanged(string strength)
    {
        UpdateStrengthValue(float.Parse(strength, en_us));
    }
    private void UpdateStrengthValue(float strength)
    {
        shakeStrength = strength;
        strengthInputField.text = strength.ToString("F2");
        strengthSlider.value = strength;
    }
    //Speed
    public void OnSpeedValueChanged(float speed)
    {
        UpdateSpeedValue(speed);
    }
    public void OnSpeedValueChanged(string speed)
    {
        UpdateSpeedValue(float.Parse(speed, en_us));
    }
    private void UpdateSpeedValue(float speed)
    {
        shakeSpeed = speed;
        speedInputField.text = speed.ToString("F2");
        speedSlider.value = speed;
    }
    //Rotation
    public void OnRotationValueChanged(float rotation)
    {
        UpdateRotationValue(rotation);
    }
    public void OnRotationValueChanged(string rotation)
    {
        UpdateRotationValue(float.Parse(rotation, en_us));
    }
    private void UpdateRotationValue(float rotation)
    {
        shakeRotation = rotation;
        rotationInputField.text = rotation.ToString("F2");
        rotationSlider.value = rotation;
    }
    //Zoom
    public void OnZoomValueChanged(float zoom)
    {
        UpdateZoomValue(zoom);
    }
    public void OnZoomValueChanged(string zoom)
    {
        UpdateZoomValue(float.Parse(zoom, en_us));
    }
    private void UpdateZoomValue(float zoom)
    {
        shakeZoom = zoom;
        zoomInputField.text = zoom.ToString("F2");
        zoomSlider.value = zoom;
    }
    //Bias-
    //x
    public void OnBiasXValueChanged(float value)
    {
        UpdateBiasXValue(value);
    }
    public void OnBiasXValueChanged(string value)
    {
        UpdateBiasXValue(float.Parse(value, en_us));
    }
    private void UpdateBiasXValue(float value)
    {
        biases.x = value;
        biasesInputFields[0].text = value.ToString("F2");
        biasesSliders[0].value = value;
    }
    //y
    public void OnBiasYValueChanged(float value)
    {
        UpdateBiasYValue(value);
    }
    public void OnBiasYValueChanged(string value)
    {
        UpdateBiasYValue(float.Parse(value, en_us));
    }
    private void UpdateBiasYValue(float value)
    {
        biases.y = value;
        biasesInputFields[1].text = value.ToString("F2");
        biasesSliders[1].value = value;
    }
    //z
    public void OnBiasZValueChanged(float value)
    {
        UpdateBiasZValue(value);
    }
    public void OnBiasZValueChanged(string value)
    {
        UpdateBiasZValue(float.Parse(value, en_us));
    }
    private void UpdateBiasZValue(float value)
    {
        biases.z = value;
        biasesInputFields[2].text = value.ToString("F2");
        biasesSliders[2].value = value;
    }
    //BiasStrength
    public void OnBiasStrengthValueChanged(float biasStrength)
    {
        UpdateBiasStrengthValue(biasStrength);
    }
    public void OnBiasStrengthValueChanged(string biasStrength)
    {
        UpdateBiasStrengthValue(float.Parse(biasStrength, en_us));
    }
    private void UpdateBiasStrengthValue(float _biasStrength)
    {
        biaseStrength = _biasStrength;
        biaseStrengthInputField.text = _biasStrength.ToString("F2");
        biaseStrengthSlider.value = _biasStrength;
    }

    private void InitUIValues()
    {
        durationInputField.text = shakeDuration.ToString("F2");
        durationSlider.value = shakeDuration;
        strengthInputField.text = shakeStrength.ToString("F2");
        strengthSlider.value = shakeStrength;
        speedInputField.text = shakeSpeed.ToString("F2");
        speedSlider.value = shakeSpeed;
        rotationInputField.text = shakeRotation.ToString("F2");
        rotationSlider.value = shakeRotation;
        zoomInputField.text = shakeZoom.ToString("F2");
        zoomSlider.value = shakeZoom;
        biasesInputFields[0].text = biases.x.ToString("F2");
        biasesSliders[0].value = biases.x;
        biasesInputFields[1].text = biases.y.ToString("F2");
        biasesSliders[1].value = biases.y;
        biasesInputFields[2].text = biases.z.ToString("F2");
        biasesSliders[2].value = biases.z;
        biaseStrengthInputField.text = biaseStrength.ToString("F2");
        biaseStrengthSlider.value = biaseStrength;
    }
}
