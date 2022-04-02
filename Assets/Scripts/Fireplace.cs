using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{

    [SerializeField] private float power = 50f;
    [SerializeField] public Vector2 powerRange;
    [SerializeField] private float unPowerSpeed;

    //mainLight
    public Light mainLight;
    [SerializeField] private Vector2 mainLightIntensityRange;
    [SerializeField] public Vector2 mainLightDistanceRange;

    //blinkingLight
    public Light blinkingLight;
    [SerializeField] private Vector2 blinkingLightDistanceRange;


    //particle
    public ParticleSystem flame1;
    public ParticleSystem flame2;
    public ParticleSystem glow;
    public ParticleSystem sparks;
    [SerializeField] private Vector2 flamesSize;
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

        AddPower(-unPowerSpeed * Time.deltaTime);
    }
    public float GetPower()
    {
        return power;
    }
    void SetPower(float newPower)
    {
        AddPower(newPower - power);
    }
    public void AddPower(float n)
    {
        power = Mathf.Clamp(power + n, powerRange.x, powerRange.y);

        mainLight.intensity = Mathf.Lerp(mainLightIntensityRange.x, mainLightIntensityRange.y, power/ powerRange.y);
        mainLight.range = Mathf.Lerp(mainLightDistanceRange.x, mainLightDistanceRange.y, power/ powerRange.y);
        blinkingLight.range = Mathf.Lerp(blinkingLightDistanceRange.x, blinkingLightDistanceRange.y, power/ powerRange.y);

        var sh1 = flame1.shape;                       
        sh1.radius = Mathf.Lerp(flamesRange.x, flamesRange.y, power/ powerRange.y);
        sh1.angle = Mathf.Lerp(flamesAngle.x, flamesAngle.y, power/ powerRange.y);
        var em1 = flame1.emission;
        em1.rateOverTime = Mathf.Lerp(flamesQty.x, flamesQty.y, power/ powerRange.y);
        var m1 = flame1.main;
        m1.startSize = Mathf.Lerp(flamesSize.x, flamesSize.y, power / powerRange.y);

        var sh2 = flame2.shape;
        sh2.radius = Mathf.Lerp(flamesRange.x, flamesRange.y, power / powerRange.y);
        sh2.angle = Mathf.Lerp(flamesAngle.x, flamesAngle.y, power / powerRange.y);
        var em2 = flame2.emission;
        em2.rateOverTime = Mathf.Lerp(flamesQty.x, flamesQty.y, power / powerRange.y);
        var m2 = flame2.main;
        m2.startSize = Mathf.Lerp(flamesSize.x*0.75f, flamesSize.y * 0.75f, power / powerRange.y);

        var sh3 = glow.shape;
        sh3.radius = Mathf.Lerp(flamesRange.x, flamesRange.y, power / powerRange.y);
        sh3.angle = Mathf.Lerp(flamesAngle.x, flamesAngle.y, power / powerRange.y);
        var em3 = glow.emission;
        em3.rateOverTime = Mathf.Lerp(flamesQty.x, flamesQty.y, power / powerRange.y);

        var sh4 = sparks.shape;
        sh4.radius = Mathf.Lerp(flamesRange.x, flamesRange.y, power / powerRange.y);
        sh4.angle = Mathf.Lerp(flamesAngle.x, flamesAngle.y, power / powerRange.y);
        var em4 = sparks.emission;
        em4.rateOverTime = Mathf.Lerp(flamesQty.x, flamesQty.y*5f, power / powerRange.y);
    }


    public float GetLightDistance()
    {
        return Mathf.Lerp(mainLightDistanceRange.x, mainLightDistanceRange.y, power / powerRange.y)*0.5f;  
    }
}
