using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingSettings : MonoBehaviour
{
    public PostProcessVolume volume;
    public Slider contrastSlider;
    public Slider saturationSlider;
    public Slider brightnessSlider;
    ColorGrading colorGrading;

    void Start()
    {
        if (!volume.profile.TryGetSettings(out colorGrading))
        {
            Debug.LogError("Color Grading not found in the volume profile.");
        }

        contrastSlider.value = PlayerPrefs.GetFloat("Contrast", .33f);
        saturationSlider.value = PlayerPrefs.GetFloat("Saturation", 0.5f);
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 0.5f);
    }

    void Update()
    {

    }

    public void SetContrast()
    {
        colorGrading.contrast.value = Mathf.Lerp(-50f, 100f, contrastSlider.value);
        PlayerPrefs.SetFloat("Contrast", contrastSlider.value);
    }
    public void SetSaturation()
    {
        colorGrading.saturation.value = Mathf.Lerp(-100f, 100f, saturationSlider.value);
        PlayerPrefs.SetFloat("Saturation", saturationSlider.value);
    }
    public void SetBrightness()
    {
        colorGrading.postExposure.value = Mathf.Lerp(-5f, 5f, brightnessSlider.value);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
    }

}
