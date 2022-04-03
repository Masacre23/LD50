using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] public float power = 50f;
    [SerializeField] public Vector2 powerRange;
    [SerializeField] private float unPowerSpeed;
    Light spotLight;
    [SerializeField] public Vector2 mainPlayerLightDistanceRange;
    [SerializeField] public Vector2 mainPlayerLightIntensityRange;

    private void Start()
    {
        spotLight = GetComponentInChildren<Light>();
        SetPower(power);

    }
    private void Update()
    {


        AddPower(-unPowerSpeed * Time.deltaTime);
    }

    public float GetPower()
    {
        return 0;
        return power;
    }
    public void SetPower(float newPower)
    {
        AddPower(Mathf.Clamp(newPower, powerRange.x, powerRange.y) - power);
    }
    public void AddPower(float n)
    {
        return;
        power = Mathf.Clamp(power + n, powerRange.x, powerRange.y);

        spotLight.intensity = Mathf.Lerp(mainPlayerLightIntensityRange.x, mainPlayerLightIntensityRange.y, power / powerRange.y);
        spotLight.range = Mathf.Lerp(mainPlayerLightDistanceRange.x, mainPlayerLightDistanceRange.y, power / powerRange.y);
      /*  if ((power * 100f) / powerRange.y < 30f)
        {
            if (GetComponent<NavMeshObstacle>().enabled)
                GetComponent<NavMeshObstacle>().enabled = false;
        }
        else if (!GetComponent<NavMeshObstacle>().enabled)
        {
            GetComponent<NavMeshObstacle>().enabled = true;

        }     */
    }

}
