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


    //particle
    public ParticleSystem flame1;
    public ParticleSystem flame2;
    public ParticleSystem glow;
    public ParticleSystem sparks;
    [SerializeField] private Vector2 flamesRange;
    [SerializeField] private Vector2 flamesAngle;
    [SerializeField] private Vector2 flamesQty;   


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

        var sh1 = flame1.shape;                       
        sh1.radius = Mathf.LerpUnclamped(flamesRange.x, flamesRange.y, power/100);
        sh1.angle = Mathf.LerpUnclamped(flamesAngle.x, flamesAngle.y, power/100);
        var em1 = flame1.emission;
        em1.rateOverTime = Mathf.LerpUnclamped(flamesQty.x, flamesQty.y, power/100);
  
        var sh2 = flame2.shape;
        sh2.radius = Mathf.LerpUnclamped(flamesRange.x, flamesRange.y, power / 100);
        sh2.angle = Mathf.LerpUnclamped(flamesAngle.x, flamesAngle.y, power / 100);
        var em2 = flame2.emission;
        em2.rateOverTime = Mathf.LerpUnclamped(flamesQty.x, flamesQty.y, power / 100);

        var sh3 = glow.shape;
        sh3.radius = Mathf.LerpUnclamped(flamesRange.x, flamesRange.y, power / 100);
        sh3.angle = Mathf.LerpUnclamped(flamesAngle.x, flamesAngle.y, power / 100);
        var em3 = glow.emission;
        em3.rateOverTime = Mathf.LerpUnclamped(flamesQty.x, flamesQty.y, power / 100);

        var sh4 = sparks.shape;
        sh4.radius = Mathf.LerpUnclamped(flamesRange.x, flamesRange.y, power / 100);
        sh4.angle = Mathf.LerpUnclamped(flamesAngle.x, flamesAngle.y, power / 100);
        var em4 = sparks.emission;
        em4.rateOverTime = Mathf.LerpUnclamped(flamesQty.x, flamesQty.y*5f, power / 100);
    }


}
