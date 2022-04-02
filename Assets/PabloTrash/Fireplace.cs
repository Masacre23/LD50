using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{

    private float power = 50f;
    [SerializeField] private Vector2 powerRange;

    //mainLight
    public Light mainLight;
    [SerializeField] private Vector2 mainLightIntensityRange;
    [SerializeField] private Vector2 mainLightDistanceRange;

    //blinkingLight
    public Light blinkingLight;
    [SerializeField] private Vector2 blinkingLightDistanceRange;

    private void Start()
    {
        SetPower(power);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O)) 
        {
            AddPower(-10);
        }
        if (Input.GetKeyUp(KeyCode.P)) 
        {
            AddPower(10);
        }  
    }

    void SetPower(float newPower)
    {
        AddPower(newPower - power);
    }
    void AddPower(float n)
    {
        power = Mathf.Clamp(power + n, powerRange.x, powerRange.y);

        mainLight.intensity = Mathf.LerpUnclamped(mainLightIntensityRange.x, mainLightIntensityRange.y, power/100);
        mainLight.range = Mathf.LerpUnclamped(mainLightDistanceRange.x, mainLightDistanceRange.y, power/100);
        blinkingLight.range = Mathf.LerpUnclamped(blinkingLightDistanceRange.x, blinkingLightDistanceRange.y, power/100);

    }
    

}
