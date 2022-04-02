using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{

    [SerializeField] private float blinkSpeed;
    private float lastBlink = 0f;
    private bool blinking;
    [SerializeField] private Vector2 blinkingIntensityRange;
    private Light blinkingLight;

    private void Start()
    {
        blinkingLight = GetComponent<Light>();
        blinking = true;
        StartCoroutine(Blinking());
    }


    IEnumerator Blinking()
    {

        while (blinking)
        {
            float t = Time.time;
            float tInter = 0;


            //up
            while (Time.time - t < blinkSpeed && blinking)
            {
                tInter = (Time.time - t) / blinkSpeed;

                blinkingLight.intensity = Mathf.Lerp(blinkingIntensityRange.x, blinkingIntensityRange.y, tInter);
                yield return new WaitForEndOfFrame();
            }

            t = Time.time;
            tInter = 0;


            //down
            while (Time.time - t < blinkSpeed && blinking)
            {
                tInter = 1 - ((Time.time - t) / blinkSpeed);

                blinkingLight.intensity = Mathf.Lerp(blinkingIntensityRange.x, blinkingIntensityRange.y, tInter);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
