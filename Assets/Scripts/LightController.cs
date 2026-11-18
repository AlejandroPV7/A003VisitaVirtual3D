using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public static LightController Instance;

    [SerializeField] private Material lightMaterial;
    [SerializeField] private VolumeProfile volumeProfile;

    private float initialPostExposure = 1f;
    private float targetPostExposure = -2f;

    private Color initialLightColor;

    bool isLightOn = true;

    ColorAdjustments colorAdjustments;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        initialLightColor = lightMaterial.GetColor("_EmissionColor");


        volumeProfile.TryGet(out ColorAdjustments _colorAdjustments);
        colorAdjustments = _colorAdjustments;
    }

    void Update()
    {

    }

    public void SetLights(bool lightsOn)
    {
        isLightOn = lightsOn;
        lightMaterial.SetColor("_EmissionColor", isLightOn ? initialLightColor : (new Vector4(0f, 0f, 0f, 1f) * 0));
        colorAdjustments.postExposure.value = isLightOn ? initialPostExposure : targetPostExposure;
    }

    private void OnApplicationQuit()
    {
        lightMaterial.SetColor("_EmissionColor", initialLightColor);
        colorAdjustments.postExposure.value = initialPostExposure;
    }
}
